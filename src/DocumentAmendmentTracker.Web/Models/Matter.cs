using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class Matter
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string MatterId { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string EntityEmployer { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string ForumType { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string CourtTribunal { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string CauseNo { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string NamesOfParties { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string OurRole { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string NatureOfMatter { get; set; } = string.Empty;

    public DateTime DateOfCommencement { get; set; }

    public DateTime? EstimatedDateOfCompletion { get; set; }

    [StringLength(200)]
    public string LawFirmWithConduct { get; set; } = string.Empty;

    [StringLength(200)]
    public string InternalLawyerResponsible { get; set; } = string.Empty;

    [StringLength(4000)]
    public string BriefOutline { get; set; } = string.Empty;

    [Required, StringLength(10)]
    public string Currency { get; set; } = "ZMW";

    public decimal SumClaimed { get; set; }

    [Required, StringLength(50)]
    public string LikelihoodOfAdverseOutcome { get; set; } = string.Empty;

    public decimal ProvisionRecommended { get; set; }

    [Required, StringLength(50)]
    public string StatusOfMatter { get; set; } = "New";

    public DateTime? NextDate { get; set; }

    [StringLength(1000)]
    public string NextStepAction { get; set; } = string.Empty;

    [StringLength(200)]
    public string Judge { get; set; } = string.Empty;

    [StringLength(200)]
    public string ArbitratorMediatorConciliator { get; set; } = string.Empty;

    public decimal EstimatedLegalFeesToFinalisation { get; set; }

    public decimal LegalFeesIncurredToDate { get; set; }

    [Required, StringLength(50)]
    public string RiskRating { get; set; } = string.Empty;

    public DateTime LastActivityDate { get; set; }

    [StringLength(200)]
    public string RelatedConsolidatedWith { get; set; } = string.Empty;

    public bool LeadMatterInGroup { get; set; }

    public bool PrivilegedConfidential { get; set; }

    public DateTime? DateClosed { get; set; }

    [StringLength(2000)]
    public string OutcomeResult { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string PostMatterReviewStatus { get; set; } = "Not yet due";

    public DateTime? PostMatterReviewDate { get; set; }

    [StringLength(50)]
    public string PreventabilityAssessment { get; set; } = string.Empty;

    [StringLength(4000)]
    public string Remarks { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public List<Party> Parties { get; set; } = new();

    // --- Derived (computed server-side from Parties register, never stored) ---

    public string CauseTitle { get; set; } = string.Empty;

    public int NoOfClaimants { get; set; }

    public int NoOfRespondents { get; set; }

    public string LeadClaimant { get; set; } = string.Empty;

    public string LeadRespondent { get; set; } = string.Empty;

    public string PartyStructure { get; set; } = string.Empty;

    public decimal SumClaimedPerPartiesRegister { get; set; }

    public string ClaimsReconciliation { get; set; } = string.Empty;

    public decimal TotalEstimatedLegalCost { get; set; }

    public decimal TotalFinancialExposure { get; set; }

    public int DaysOpen { get; set; }

    public int? DaysToNextDate { get; set; }

    public int DaysSinceLastActivity { get; set; }

    public int PartiesOnRecord { get; set; }
}
