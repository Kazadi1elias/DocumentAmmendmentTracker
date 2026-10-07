using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class DiaryEvent
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string EventNo { get; set; } = string.Empty;

    public int MatterId { get; set; }

    [Required, StringLength(100)]
    public string EventType { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    [StringLength(20)]
    public string Time { get; set; } = string.Empty;

    [StringLength(300)]
    public string VenueRegistry { get; set; } = string.Empty;

    [StringLength(200)]
    public string Judge { get; set; } = string.Empty;

    [StringLength(200)]
    public string ArbitratorMediatorConciliator { get; set; } = string.Empty;

    [StringLength(500)]
    public string PartiesRequiredToAttend { get; set; } = string.Empty;

    [StringLength(200)]
    public string InternalAttendee { get; set; } = string.Empty;

    [StringLength(200)]
    public string ExternalCounselAttending { get; set; } = string.Empty;

    [StringLength(1000)]
    public string PurposeIssue { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string EventStatus { get; set; } = "Scheduled";

    [StringLength(2000)]
    public string OutcomeRulingDirection { get; set; } = string.Empty;

    [StringLength(1000)]
    public string NextActionArising { get; set; } = string.Empty;

    [StringLength(200)]
    public string ActionOwner { get; set; } = string.Empty;

    public DateTime? ActionDueDate { get; set; }

    [StringLength(2000)]
    public string Remarks { get; set; } = string.Empty;

    // --- Derived (computed server-side, never stored) ---

    public int DaysToEvent { get; set; }

    public string Alert { get; set; } = string.Empty;
}
