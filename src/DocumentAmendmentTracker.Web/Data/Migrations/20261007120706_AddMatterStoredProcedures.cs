using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentAmendmentTracker.Web.Data.Migrations
{
    public partial class AddMatterStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Matters_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, MatterId, EntityEmployer, ForumType, CourtTribunal, CauseNo, NamesOfParties, OurRole,
           NatureOfMatter, DateOfCommencement, EstimatedDateOfCompletion, LawFirmWithConduct,
           InternalLawyerResponsible, BriefOutline, Currency, SumClaimed, LikelihoodOfAdverseOutcome,
           ProvisionRecommended, StatusOfMatter, NextDate, NextStepAction, Judge,
           ArbitratorMediatorConciliator, EstimatedLegalFeesToFinalisation, LegalFeesIncurredToDate,
           RiskRating, LastActivityDate, RelatedConsolidatedWith, LeadMatterInGroup, PrivilegedConfidential,
           DateClosed, OutcomeResult, PostMatterReviewStatus, PostMatterReviewDate, PreventabilityAssessment,
           Remarks, CreatedBy, CreatedDate
    FROM dbo.Matters
    ORDER BY CreatedDate DESC;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Parties_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, PartyNo, MatterId, Side, PartyOrder, PartyRoleTitle, PartyName, PartyType, IsGroupEntity,
           EmployeePayrollNo, NrcPassportRegNo, RepresentedBy, IndividualSumClaimed, DateJoined, DateCeased,
           PartyStatus, ServiceAddress, Remarks
    FROM dbo.Parties
    ORDER BY MatterId, Side, PartyOrder;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Matters_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, MatterId, EntityEmployer, ForumType, CourtTribunal, CauseNo, NamesOfParties, OurRole,
           NatureOfMatter, DateOfCommencement, EstimatedDateOfCompletion, LawFirmWithConduct,
           InternalLawyerResponsible, BriefOutline, Currency, SumClaimed, LikelihoodOfAdverseOutcome,
           ProvisionRecommended, StatusOfMatter, NextDate, NextStepAction, Judge,
           ArbitratorMediatorConciliator, EstimatedLegalFeesToFinalisation, LegalFeesIncurredToDate,
           RiskRating, LastActivityDate, RelatedConsolidatedWith, LeadMatterInGroup, PrivilegedConfidential,
           DateClosed, OutcomeResult, PostMatterReviewStatus, PostMatterReviewDate, PreventabilityAssessment,
           Remarks, CreatedBy, CreatedDate
    FROM dbo.Matters
    WHERE Id = @Id;

    SELECT Id, PartyNo, MatterId, Side, PartyOrder, PartyRoleTitle, PartyName, PartyType, IsGroupEntity,
           EmployeePayrollNo, NrcPassportRegNo, RepresentedBy, IndividualSumClaimed, DateJoined, DateCeased,
           PartyStatus, ServiceAddress, Remarks
    FROM dbo.Parties
    WHERE MatterId = @Id
    ORDER BY Side, PartyOrder;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Matters_Create
    @Prefix NVARCHAR(10),
    @Year NVARCHAR(4),
    @EntityEmployer NVARCHAR(200),
    @ForumType NVARCHAR(100),
    @CourtTribunal NVARCHAR(200),
    @CauseNo NVARCHAR(100),
    @NamesOfParties NVARCHAR(1000),
    @OurRole NVARCHAR(100),
    @NatureOfMatter NVARCHAR(100),
    @DateOfCommencement DATETIME2,
    @EstimatedDateOfCompletion DATETIME2 = NULL,
    @LawFirmWithConduct NVARCHAR(200),
    @InternalLawyerResponsible NVARCHAR(200),
    @BriefOutline NVARCHAR(MAX),
    @Currency NVARCHAR(10),
    @SumClaimed DECIMAL(18,2),
    @LikelihoodOfAdverseOutcome NVARCHAR(50),
    @ProvisionRecommended DECIMAL(18,2),
    @StatusOfMatter NVARCHAR(50),
    @RiskRating NVARCHAR(50),
    @PrivilegedConfidential BIT,
    @CreatedBy NVARCHAR(200),
    @Id INT OUTPUT,
    @MatterId NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DECLARE @NextSeq INT;
    SELECT @NextSeq = ISNULL(MAX(CAST(RIGHT(MatterId, 3) AS INT)), 0) + 1
    FROM dbo.Matters WITH (UPDLOCK, HOLDLOCK)
    WHERE MatterId LIKE @Prefix + '-' + @Year + '-%';

    SET @MatterId = @Prefix + '-' + @Year + '-' + RIGHT('000' + CAST(@NextSeq AS NVARCHAR(3)), 3);

    DECLARE @Now DATETIME2 = SYSUTCDATETIME();

    INSERT INTO dbo.Matters
        (MatterId, EntityEmployer, ForumType, CourtTribunal, CauseNo, NamesOfParties, OurRole,
         NatureOfMatter, DateOfCommencement, EstimatedDateOfCompletion, LawFirmWithConduct,
         InternalLawyerResponsible, BriefOutline, Currency, SumClaimed, LikelihoodOfAdverseOutcome,
         ProvisionRecommended, StatusOfMatter, NextDate, NextStepAction, Judge,
         ArbitratorMediatorConciliator, EstimatedLegalFeesToFinalisation, LegalFeesIncurredToDate,
         RiskRating, LastActivityDate, RelatedConsolidatedWith, LeadMatterInGroup, PrivilegedConfidential,
         DateClosed, OutcomeResult, PostMatterReviewStatus, PostMatterReviewDate, PreventabilityAssessment,
         Remarks, CreatedBy, CreatedDate)
    VALUES
        (@MatterId, @EntityEmployer, @ForumType, @CourtTribunal, @CauseNo, @NamesOfParties, @OurRole,
         @NatureOfMatter, @DateOfCommencement, @EstimatedDateOfCompletion, @LawFirmWithConduct,
         @InternalLawyerResponsible, @BriefOutline, @Currency, @SumClaimed, @LikelihoodOfAdverseOutcome,
         @ProvisionRecommended, @StatusOfMatter, NULL, '', '',
         '', 0, 0,
         @RiskRating, @DateOfCommencement, '', 0, @PrivilegedConfidential,
         NULL, '', 'Not yet due', NULL, '',
         '', @CreatedBy, @Now);

    SET @Id = SCOPE_IDENTITY();

    COMMIT TRANSACTION;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Matters_Update
    @Id INT,
    @EntityEmployer NVARCHAR(200),
    @ForumType NVARCHAR(100),
    @CourtTribunal NVARCHAR(200),
    @CauseNo NVARCHAR(100),
    @NamesOfParties NVARCHAR(1000),
    @OurRole NVARCHAR(100),
    @NatureOfMatter NVARCHAR(100),
    @DateOfCommencement DATETIME2,
    @EstimatedDateOfCompletion DATETIME2 = NULL,
    @LawFirmWithConduct NVARCHAR(200),
    @InternalLawyerResponsible NVARCHAR(200),
    @BriefOutline NVARCHAR(MAX),
    @Currency NVARCHAR(10),
    @SumClaimed DECIMAL(18,2),
    @LikelihoodOfAdverseOutcome NVARCHAR(50),
    @ProvisionRecommended DECIMAL(18,2),
    @StatusOfMatter NVARCHAR(50),
    @NextDate DATETIME2 = NULL,
    @NextStepAction NVARCHAR(1000),
    @Judge NVARCHAR(200),
    @ArbitratorMediatorConciliator NVARCHAR(200),
    @EstimatedLegalFeesToFinalisation DECIMAL(18,2),
    @LegalFeesIncurredToDate DECIMAL(18,2),
    @RiskRating NVARCHAR(50),
    @LastActivityDate DATETIME2,
    @RelatedConsolidatedWith NVARCHAR(200),
    @LeadMatterInGroup BIT,
    @PrivilegedConfidential BIT,
    @DateClosed DATETIME2 = NULL,
    @OutcomeResult NVARCHAR(2000),
    @PostMatterReviewStatus NVARCHAR(50),
    @PostMatterReviewDate DATETIME2 = NULL,
    @PreventabilityAssessment NVARCHAR(50),
    @Remarks NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Matters
    SET EntityEmployer = @EntityEmployer,
        ForumType = @ForumType,
        CourtTribunal = @CourtTribunal,
        CauseNo = @CauseNo,
        NamesOfParties = @NamesOfParties,
        OurRole = @OurRole,
        NatureOfMatter = @NatureOfMatter,
        DateOfCommencement = @DateOfCommencement,
        EstimatedDateOfCompletion = @EstimatedDateOfCompletion,
        LawFirmWithConduct = @LawFirmWithConduct,
        InternalLawyerResponsible = @InternalLawyerResponsible,
        BriefOutline = @BriefOutline,
        Currency = @Currency,
        SumClaimed = @SumClaimed,
        LikelihoodOfAdverseOutcome = @LikelihoodOfAdverseOutcome,
        ProvisionRecommended = @ProvisionRecommended,
        StatusOfMatter = @StatusOfMatter,
        NextDate = @NextDate,
        NextStepAction = @NextStepAction,
        Judge = @Judge,
        ArbitratorMediatorConciliator = @ArbitratorMediatorConciliator,
        EstimatedLegalFeesToFinalisation = @EstimatedLegalFeesToFinalisation,
        LegalFeesIncurredToDate = @LegalFeesIncurredToDate,
        RiskRating = @RiskRating,
        LastActivityDate = @LastActivityDate,
        RelatedConsolidatedWith = @RelatedConsolidatedWith,
        LeadMatterInGroup = @LeadMatterInGroup,
        PrivilegedConfidential = @PrivilegedConfidential,
        DateClosed = @DateClosed,
        OutcomeResult = @OutcomeResult,
        PostMatterReviewStatus = @PostMatterReviewStatus,
        PostMatterReviewDate = @PostMatterReviewDate,
        PreventabilityAssessment = @PreventabilityAssessment,
        Remarks = @Remarks
    WHERE Id = @Id;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Parties_Add
    @MatterId INT,
    @Side NVARCHAR(100),
    @PartyRoleTitle NVARCHAR(100),
    @PartyName NVARCHAR(300),
    @PartyType NVARCHAR(100),
    @IsGroupEntity BIT,
    @EmployeePayrollNo NVARCHAR(100),
    @NrcPassportRegNo NVARCHAR(100),
    @RepresentedBy NVARCHAR(300),
    @IndividualSumClaimed DECIMAL(18,2),
    @DateJoined DATETIME2,
    @PartyStatus NVARCHAR(100),
    @ServiceAddress NVARCHAR(500),
    @Remarks NVARCHAR(2000),
    @Id INT OUTPUT,
    @PartyNo NVARCHAR(100) OUTPUT,
    @PartyOrder INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DECLARE @NextNo INT;
    SELECT @NextNo = ISNULL(MAX(CAST(SUBSTRING(PartyNo, 3, 10) AS INT)), 0) + 1
    FROM dbo.Parties WITH (UPDLOCK, HOLDLOCK);
    SET @PartyNo = 'P-' + RIGHT('0000' + CAST(@NextNo AS NVARCHAR(4)), 4);

    SELECT @PartyOrder = ISNULL(MAX(PartyOrder), 0) + 1
    FROM dbo.Parties WITH (UPDLOCK, HOLDLOCK)
    WHERE MatterId = @MatterId AND Side = @Side;

    INSERT INTO dbo.Parties
        (PartyNo, MatterId, Side, PartyOrder, PartyRoleTitle, PartyName, PartyType, IsGroupEntity,
         EmployeePayrollNo, NrcPassportRegNo, RepresentedBy, IndividualSumClaimed, DateJoined, DateCeased,
         PartyStatus, ServiceAddress, Remarks)
    VALUES
        (@PartyNo, @MatterId, @Side, @PartyOrder, @PartyRoleTitle, @PartyName, @PartyType, @IsGroupEntity,
         @EmployeePayrollNo, @NrcPassportRegNo, @RepresentedBy, @IndividualSumClaimed, @DateJoined, NULL,
         @PartyStatus, @ServiceAddress, @Remarks);

    SET @Id = SCOPE_IDENTITY();

    COMMIT TRANSACTION;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Parties_Update
    @Id INT,
    @Side NVARCHAR(100),
    @PartyRoleTitle NVARCHAR(100),
    @PartyName NVARCHAR(300),
    @PartyType NVARCHAR(100),
    @IsGroupEntity BIT,
    @EmployeePayrollNo NVARCHAR(100),
    @NrcPassportRegNo NVARCHAR(100),
    @RepresentedBy NVARCHAR(300),
    @IndividualSumClaimed DECIMAL(18,2),
    @DateCeased DATETIME2 = NULL,
    @PartyStatus NVARCHAR(100),
    @ServiceAddress NVARCHAR(500),
    @Remarks NVARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Parties
    SET Side = @Side,
        PartyRoleTitle = @PartyRoleTitle,
        PartyName = @PartyName,
        PartyType = @PartyType,
        IsGroupEntity = @IsGroupEntity,
        EmployeePayrollNo = @EmployeePayrollNo,
        NrcPassportRegNo = @NrcPassportRegNo,
        RepresentedBy = @RepresentedBy,
        IndividualSumClaimed = @IndividualSumClaimed,
        DateCeased = @DateCeased,
        PartyStatus = @PartyStatus,
        ServiceAddress = @ServiceAddress,
        Remarks = @Remarks
    WHERE Id = @Id;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Parties_Update;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Parties_Add;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Matters_Update;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Matters_Create;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Matters_GetById;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Parties_GetAll;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Matters_GetAll;");
        }
    }
}
