using System.Data;
using DocumentAmendmentTracker.Web.Data;
using DocumentAmendmentTracker.Web.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DocumentAmendmentTracker.Web.Services;

/// <summary>
/// All Matter/Party reads and writes go through stored procedures via raw ADO.NET, sharing the
/// EF Core connection/transaction. EF Core itself is used only for schema/migrations.
/// </summary>
public class MatterService : IMatterService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public MatterService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Matter>> GetMattersAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await db.Database.OpenConnectionAsync(cancellationToken);
        var connection = (SqlConnection)db.Database.GetDbConnection();

        var matters = new List<Matter>();
        await using (var command = new SqlCommand("dbo.sp_Matters_GetAll", connection) { CommandType = CommandType.StoredProcedure })
        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                matters.Add(ReadMatter(reader));
            }
        }

        var partiesByMatter = new Dictionary<int, List<Party>>();
        await using (var command = new SqlCommand("dbo.sp_Parties_GetAll", connection) { CommandType = CommandType.StoredProcedure })
        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                var party = ReadParty(reader);
                if (!partiesByMatter.TryGetValue(party.MatterId, out var list))
                {
                    list = new List<Party>();
                    partiesByMatter[party.MatterId] = list;
                }

                list.Add(party);
            }
        }

        foreach (var matter in matters)
        {
            ApplyDerivedFields(matter, partiesByMatter.TryGetValue(matter.Id, out var parties) ? parties : new List<Party>());
        }

        return matters;
    }

    public async Task<Matter?> GetMatterByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await db.Database.OpenConnectionAsync(cancellationToken);
        var connection = (SqlConnection)db.Database.GetDbConnection();

        await using var command = new SqlCommand("dbo.sp_Matters_GetById", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.AddWithValue("@Id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        var matter = ReadMatter(reader);

        var parties = new List<Party>();
        await reader.NextResultAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            parties.Add(ReadParty(reader));
        }

        ApplyDerivedFields(matter, parties);
        return matter;
    }

    public async Task<Matter> CreateMatterAsync(Matter input, string createdBy, CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await db.Database.OpenConnectionAsync(cancellationToken);
        var connection = (SqlConnection)db.Database.GetDbConnection();

        var prefix = PrefixForForumType(input.ForumType);
        var year = input.DateOfCommencement.Year.ToString();

        await using var command = new SqlCommand("dbo.sp_Matters_Create", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.AddWithValue("@Prefix", prefix);
        command.Parameters.AddWithValue("@Year", year);
        command.Parameters.AddWithValue("@EntityEmployer", input.EntityEmployer);
        command.Parameters.AddWithValue("@ForumType", input.ForumType);
        command.Parameters.AddWithValue("@CourtTribunal", input.CourtTribunal);
        command.Parameters.AddWithValue("@CauseNo", input.CauseNo);
        command.Parameters.AddWithValue("@NamesOfParties", input.NamesOfParties);
        command.Parameters.AddWithValue("@OurRole", input.OurRole);
        command.Parameters.AddWithValue("@NatureOfMatter", input.NatureOfMatter);
        command.Parameters.AddWithValue("@DateOfCommencement", input.DateOfCommencement);
        command.Parameters.AddWithValue("@EstimatedDateOfCompletion", (object?)input.EstimatedDateOfCompletion ?? DBNull.Value);
        command.Parameters.AddWithValue("@LawFirmWithConduct", input.LawFirmWithConduct);
        command.Parameters.AddWithValue("@InternalLawyerResponsible", input.InternalLawyerResponsible);
        command.Parameters.AddWithValue("@BriefOutline", input.BriefOutline);
        command.Parameters.AddWithValue("@Currency", input.Currency);
        command.Parameters.AddWithValue("@SumClaimed", input.SumClaimed);
        command.Parameters.AddWithValue("@LikelihoodOfAdverseOutcome", input.LikelihoodOfAdverseOutcome);
        command.Parameters.AddWithValue("@ProvisionRecommended", input.ProvisionRecommended);
        command.Parameters.AddWithValue("@StatusOfMatter", input.StatusOfMatter);
        command.Parameters.AddWithValue("@RiskRating", input.RiskRating);
        command.Parameters.AddWithValue("@PrivilegedConfidential", input.PrivilegedConfidential);
        command.Parameters.AddWithValue("@CreatedBy", createdBy);

        var idParam = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var matterIdParam = new SqlParameter("@MatterId", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
        command.Parameters.Add(idParam);
        command.Parameters.Add(matterIdParam);

        await command.ExecuteNonQueryAsync(cancellationToken);

        var newId = (int)idParam.Value;
        return await GetMatterByIdAsync(newId, cancellationToken)
            ?? throw new InvalidOperationException($"Matter {newId} was created but could not be re-read.");
    }

    public async Task UpdateMatterAsync(Matter input, CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await db.Database.OpenConnectionAsync(cancellationToken);
        var connection = (SqlConnection)db.Database.GetDbConnection();

        await using var command = new SqlCommand("dbo.sp_Matters_Update", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.AddWithValue("@Id", input.Id);
        command.Parameters.AddWithValue("@EntityEmployer", input.EntityEmployer);
        command.Parameters.AddWithValue("@ForumType", input.ForumType);
        command.Parameters.AddWithValue("@CourtTribunal", input.CourtTribunal);
        command.Parameters.AddWithValue("@CauseNo", input.CauseNo);
        command.Parameters.AddWithValue("@NamesOfParties", input.NamesOfParties);
        command.Parameters.AddWithValue("@OurRole", input.OurRole);
        command.Parameters.AddWithValue("@NatureOfMatter", input.NatureOfMatter);
        command.Parameters.AddWithValue("@DateOfCommencement", input.DateOfCommencement);
        command.Parameters.AddWithValue("@EstimatedDateOfCompletion", (object?)input.EstimatedDateOfCompletion ?? DBNull.Value);
        command.Parameters.AddWithValue("@LawFirmWithConduct", input.LawFirmWithConduct);
        command.Parameters.AddWithValue("@InternalLawyerResponsible", input.InternalLawyerResponsible);
        command.Parameters.AddWithValue("@BriefOutline", input.BriefOutline);
        command.Parameters.AddWithValue("@Currency", input.Currency);
        command.Parameters.AddWithValue("@SumClaimed", input.SumClaimed);
        command.Parameters.AddWithValue("@LikelihoodOfAdverseOutcome", input.LikelihoodOfAdverseOutcome);
        command.Parameters.AddWithValue("@ProvisionRecommended", input.ProvisionRecommended);
        command.Parameters.AddWithValue("@StatusOfMatter", input.StatusOfMatter);
        command.Parameters.AddWithValue("@NextDate", (object?)input.NextDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@NextStepAction", input.NextStepAction);
        command.Parameters.AddWithValue("@Judge", input.Judge);
        command.Parameters.AddWithValue("@ArbitratorMediatorConciliator", input.ArbitratorMediatorConciliator);
        command.Parameters.AddWithValue("@EstimatedLegalFeesToFinalisation", input.EstimatedLegalFeesToFinalisation);
        command.Parameters.AddWithValue("@LegalFeesIncurredToDate", input.LegalFeesIncurredToDate);
        command.Parameters.AddWithValue("@RiskRating", input.RiskRating);
        command.Parameters.AddWithValue("@LastActivityDate", input.LastActivityDate);
        command.Parameters.AddWithValue("@RelatedConsolidatedWith", input.RelatedConsolidatedWith);
        command.Parameters.AddWithValue("@LeadMatterInGroup", input.LeadMatterInGroup);
        command.Parameters.AddWithValue("@PrivilegedConfidential", input.PrivilegedConfidential);
        command.Parameters.AddWithValue("@DateClosed", (object?)input.DateClosed ?? DBNull.Value);
        command.Parameters.AddWithValue("@OutcomeResult", input.OutcomeResult);
        command.Parameters.AddWithValue("@PostMatterReviewStatus", input.PostMatterReviewStatus);
        command.Parameters.AddWithValue("@PostMatterReviewDate", (object?)input.PostMatterReviewDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@PreventabilityAssessment", input.PreventabilityAssessment);
        command.Parameters.AddWithValue("@Remarks", input.Remarks);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<Party> AddPartyAsync(int matterId, Party input, CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        var connection = (SqlConnection)db.Database.GetDbConnection();

        await using var command = new SqlCommand("dbo.sp_Parties_Add", connection) { CommandType = CommandType.StoredProcedure };
        command.Transaction = (SqlTransaction)transaction.GetDbTransaction();
        command.Parameters.AddWithValue("@MatterId", matterId);
        command.Parameters.AddWithValue("@Side", input.Side);
        command.Parameters.AddWithValue("@PartyRoleTitle", input.PartyRoleTitle);
        command.Parameters.AddWithValue("@PartyName", input.PartyName);
        command.Parameters.AddWithValue("@PartyType", input.PartyType);
        command.Parameters.AddWithValue("@IsGroupEntity", input.IsGroupEntity);
        command.Parameters.AddWithValue("@EmployeePayrollNo", input.EmployeePayrollNo);
        command.Parameters.AddWithValue("@NrcPassportRegNo", input.NrcPassportRegNo);
        command.Parameters.AddWithValue("@RepresentedBy", input.RepresentedBy);
        command.Parameters.AddWithValue("@IndividualSumClaimed", input.IndividualSumClaimed);
        command.Parameters.AddWithValue("@DateJoined", input.DateJoined);
        command.Parameters.AddWithValue("@PartyStatus", input.PartyStatus);
        command.Parameters.AddWithValue("@ServiceAddress", input.ServiceAddress);
        command.Parameters.AddWithValue("@Remarks", input.Remarks);

        var idParam = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };
        var partyNoParam = new SqlParameter("@PartyNo", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
        var partyOrderParam = new SqlParameter("@PartyOrder", SqlDbType.Int) { Direction = ParameterDirection.Output };
        command.Parameters.Add(idParam);
        command.Parameters.Add(partyNoParam);
        command.Parameters.Add(partyOrderParam);

        await command.ExecuteNonQueryAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        input.Id = (int)idParam.Value;
        input.MatterId = matterId;
        input.PartyNo = (string)partyNoParam.Value;
        input.PartyOrder = (int)partyOrderParam.Value;
        return input;
    }

    public async Task UpdatePartyAsync(Party input, CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await db.Database.OpenConnectionAsync(cancellationToken);
        var connection = (SqlConnection)db.Database.GetDbConnection();

        await using var command = new SqlCommand("dbo.sp_Parties_Update", connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.AddWithValue("@Id", input.Id);
        command.Parameters.AddWithValue("@Side", input.Side);
        command.Parameters.AddWithValue("@PartyRoleTitle", input.PartyRoleTitle);
        command.Parameters.AddWithValue("@PartyName", input.PartyName);
        command.Parameters.AddWithValue("@PartyType", input.PartyType);
        command.Parameters.AddWithValue("@IsGroupEntity", input.IsGroupEntity);
        command.Parameters.AddWithValue("@EmployeePayrollNo", input.EmployeePayrollNo);
        command.Parameters.AddWithValue("@NrcPassportRegNo", input.NrcPassportRegNo);
        command.Parameters.AddWithValue("@RepresentedBy", input.RepresentedBy);
        command.Parameters.AddWithValue("@IndividualSumClaimed", input.IndividualSumClaimed);
        command.Parameters.AddWithValue("@DateCeased", (object?)input.DateCeased ?? DBNull.Value);
        command.Parameters.AddWithValue("@PartyStatus", input.PartyStatus);
        command.Parameters.AddWithValue("@ServiceAddress", input.ServiceAddress);
        command.Parameters.AddWithValue("@Remarks", input.Remarks);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    internal static void ApplyDerivedFields(Matter matter, List<Party> parties)
    {
        matter.Parties = parties;
        foreach (var party in parties)
        {
            party.Designation = $"{Ordinal(party.PartyOrder)} {party.PartyRoleTitle}".Trim();
        }

        var activeParties = parties.Where(p => !MatterLists.InactivePartyStatuses.Contains(p.PartyStatus)).ToList();
        var claimants = activeParties.Where(p => p.Side == "Claimant / Plaintiff").OrderBy(p => p.PartyOrder).ToList();
        var respondents = activeParties.Where(p => p.Side == "Defendant / Respondent").OrderBy(p => p.PartyOrder).ToList();

        matter.NoOfClaimants = claimants.Count;
        matter.NoOfRespondents = respondents.Count;
        matter.LeadClaimant = claimants.FirstOrDefault()?.PartyName ?? string.Empty;
        matter.LeadRespondent = respondents.FirstOrDefault()?.PartyName ?? string.Empty;
        matter.PartyStructure = matter.NoOfClaimants > 1 || matter.NoOfRespondents > 1 ? "Multi-party" : "Single Party Each Side";
        matter.CauseTitle = $"{PartyGroupLabel(claimants)} v {PartyGroupLabel(respondents)}";

        matter.SumClaimedPerPartiesRegister = claimants.Sum(p => p.IndividualSumClaimed);
        matter.ClaimsReconciliation = matter.SumClaimed == matter.SumClaimedPerPartiesRegister ? "Agrees" : "Variance";

        matter.TotalEstimatedLegalCost = matter.EstimatedLegalFeesToFinalisation + matter.LegalFeesIncurredToDate;
        matter.TotalFinancialExposure = matter.SumClaimed + matter.TotalEstimatedLegalCost;

        var today = DateTime.UtcNow.Date;
        matter.DaysOpen = (today - matter.DateOfCommencement.Date).Days;
        matter.DaysToNextDate = matter.NextDate.HasValue ? (matter.NextDate.Value.Date - today).Days : null;
        matter.DaysSinceLastActivity = (today - matter.LastActivityDate.Date).Days;
        matter.PartiesOnRecord = parties.Count;
    }

    private static string PartyGroupLabel(List<Party> sideParties)
    {
        if (sideParties.Count == 0)
        {
            return "[No party on record]";
        }

        var lead = sideParties[0].PartyName;
        var others = sideParties.Count - 1;
        return others switch
        {
            0 => lead,
            1 => $"{lead} & Another",
            _ => $"{lead} & {others} Others",
        };
    }

    private static string Ordinal(int n)
    {
        if (n % 100 is 11 or 12 or 13)
        {
            return $"{n}th";
        }

        return (n % 10) switch
        {
            1 => $"{n}st",
            2 => $"{n}nd",
            3 => $"{n}rd",
            _ => $"{n}th",
        };
    }

    private static string PrefixForForumType(string forumType) => forumType switch
    {
        "Arbitration" => "ARB",
        "Conciliation" or "Mediation" => "CON",
        _ => "LIT",
    };

    private static Matter ReadMatter(SqlDataReader reader)
    {
        return new Matter
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            MatterId = reader.GetString(reader.GetOrdinal("MatterId")),
            EntityEmployer = reader.GetString(reader.GetOrdinal("EntityEmployer")),
            ForumType = reader.GetString(reader.GetOrdinal("ForumType")),
            CourtTribunal = reader.GetString(reader.GetOrdinal("CourtTribunal")),
            CauseNo = reader.GetString(reader.GetOrdinal("CauseNo")),
            NamesOfParties = reader.GetString(reader.GetOrdinal("NamesOfParties")),
            OurRole = reader.GetString(reader.GetOrdinal("OurRole")),
            NatureOfMatter = reader.GetString(reader.GetOrdinal("NatureOfMatter")),
            DateOfCommencement = reader.GetDateTime(reader.GetOrdinal("DateOfCommencement")),
            EstimatedDateOfCompletion = reader.IsDBNull(reader.GetOrdinal("EstimatedDateOfCompletion")) ? null : reader.GetDateTime(reader.GetOrdinal("EstimatedDateOfCompletion")),
            LawFirmWithConduct = reader.GetString(reader.GetOrdinal("LawFirmWithConduct")),
            InternalLawyerResponsible = reader.GetString(reader.GetOrdinal("InternalLawyerResponsible")),
            BriefOutline = reader.GetString(reader.GetOrdinal("BriefOutline")),
            Currency = reader.GetString(reader.GetOrdinal("Currency")),
            SumClaimed = reader.GetDecimal(reader.GetOrdinal("SumClaimed")),
            LikelihoodOfAdverseOutcome = reader.GetString(reader.GetOrdinal("LikelihoodOfAdverseOutcome")),
            ProvisionRecommended = reader.GetDecimal(reader.GetOrdinal("ProvisionRecommended")),
            StatusOfMatter = reader.GetString(reader.GetOrdinal("StatusOfMatter")),
            NextDate = reader.IsDBNull(reader.GetOrdinal("NextDate")) ? null : reader.GetDateTime(reader.GetOrdinal("NextDate")),
            NextStepAction = reader.GetString(reader.GetOrdinal("NextStepAction")),
            Judge = reader.GetString(reader.GetOrdinal("Judge")),
            ArbitratorMediatorConciliator = reader.GetString(reader.GetOrdinal("ArbitratorMediatorConciliator")),
            EstimatedLegalFeesToFinalisation = reader.GetDecimal(reader.GetOrdinal("EstimatedLegalFeesToFinalisation")),
            LegalFeesIncurredToDate = reader.GetDecimal(reader.GetOrdinal("LegalFeesIncurredToDate")),
            RiskRating = reader.GetString(reader.GetOrdinal("RiskRating")),
            LastActivityDate = reader.GetDateTime(reader.GetOrdinal("LastActivityDate")),
            RelatedConsolidatedWith = reader.GetString(reader.GetOrdinal("RelatedConsolidatedWith")),
            LeadMatterInGroup = reader.GetBoolean(reader.GetOrdinal("LeadMatterInGroup")),
            PrivilegedConfidential = reader.GetBoolean(reader.GetOrdinal("PrivilegedConfidential")),
            DateClosed = reader.IsDBNull(reader.GetOrdinal("DateClosed")) ? null : reader.GetDateTime(reader.GetOrdinal("DateClosed")),
            OutcomeResult = reader.GetString(reader.GetOrdinal("OutcomeResult")),
            PostMatterReviewStatus = reader.GetString(reader.GetOrdinal("PostMatterReviewStatus")),
            PostMatterReviewDate = reader.IsDBNull(reader.GetOrdinal("PostMatterReviewDate")) ? null : reader.GetDateTime(reader.GetOrdinal("PostMatterReviewDate")),
            PreventabilityAssessment = reader.GetString(reader.GetOrdinal("PreventabilityAssessment")),
            Remarks = reader.GetString(reader.GetOrdinal("Remarks")),
            CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
        };
    }

    private static Party ReadParty(SqlDataReader reader)
    {
        return new Party
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            PartyNo = reader.GetString(reader.GetOrdinal("PartyNo")),
            MatterId = reader.GetInt32(reader.GetOrdinal("MatterId")),
            Side = reader.GetString(reader.GetOrdinal("Side")),
            PartyOrder = reader.GetInt32(reader.GetOrdinal("PartyOrder")),
            PartyRoleTitle = reader.GetString(reader.GetOrdinal("PartyRoleTitle")),
            PartyName = reader.GetString(reader.GetOrdinal("PartyName")),
            PartyType = reader.GetString(reader.GetOrdinal("PartyType")),
            IsGroupEntity = reader.GetBoolean(reader.GetOrdinal("IsGroupEntity")),
            EmployeePayrollNo = reader.GetString(reader.GetOrdinal("EmployeePayrollNo")),
            NrcPassportRegNo = reader.GetString(reader.GetOrdinal("NrcPassportRegNo")),
            RepresentedBy = reader.GetString(reader.GetOrdinal("RepresentedBy")),
            IndividualSumClaimed = reader.GetDecimal(reader.GetOrdinal("IndividualSumClaimed")),
            DateJoined = reader.GetDateTime(reader.GetOrdinal("DateJoined")),
            DateCeased = reader.IsDBNull(reader.GetOrdinal("DateCeased")) ? null : reader.GetDateTime(reader.GetOrdinal("DateCeased")),
            PartyStatus = reader.GetString(reader.GetOrdinal("PartyStatus")),
            ServiceAddress = reader.GetString(reader.GetOrdinal("ServiceAddress")),
            Remarks = reader.GetString(reader.GetOrdinal("Remarks")),
        };
    }
}
