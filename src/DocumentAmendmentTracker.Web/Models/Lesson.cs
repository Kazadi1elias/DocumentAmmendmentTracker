using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class Lesson
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string LessonNo { get; set; } = string.Empty;

    public int MatterId { get; set; }

    public DateTime DateLogged { get; set; }

    [Required, StringLength(200)]
    public string LoggedBy { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LessonType { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string StageInLifecycle { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string PrimaryRootCause { get; set; } = string.Empty;

    [StringLength(200)]
    public string SecondaryRootCause { get; set; } = string.Empty;

    [Required, StringLength(4000)]
    public string WhatHappened { get; set; } = string.Empty;

    [StringLength(2000)]
    public string ConsequenceWhyItMattered { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Avoidability { get; set; } = string.Empty;

    public decimal CostAttributable { get; set; }

    [StringLength(100)]
    public string InstrumentToChange { get; set; } = string.Empty;

    [StringLength(300)]
    public string SpecificPolicyAffected { get; set; } = string.Empty;

    [StringLength(2000)]
    public string RecommendedChange { get; set; } = string.Empty;

    [StringLength(200)]
    public string ActionOwnerFunction { get; set; } = string.Empty;

    [StringLength(200)]
    public string ActionOwnerName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Priority { get; set; } = string.Empty;

    public DateTime? TargetImplementationDate { get; set; }

    [Required, StringLength(50)]
    public string ActionStatus { get; set; } = "Logged - not yet reviewed";

    public DateTime? DateImplemented { get; set; }

    [StringLength(1000)]
    public string EvidenceOfImplementation { get; set; } = string.Empty;

    public DateTime? EffectivenessReviewDate { get; set; }

    [StringLength(50)]
    public string EffectivenessVerdict { get; set; } = string.Empty;

    [StringLength(10)]
    public string HasIssueRecurred { get; set; } = "No";

    [StringLength(500)]
    public string OtherMattersShowingSameIssue { get; set; } = string.Empty;

    [StringLength(200)]
    public string ApprovedByGovernanceForum { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Confidentiality { get; set; } = "Internal";

    [StringLength(2000)]
    public string Remarks { get; set; } = string.Empty;

    // --- Derived (computed server-side, never stored) ---

    public int? DaysOverdue { get; set; }
}
