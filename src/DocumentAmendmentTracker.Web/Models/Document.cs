using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class Document
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string DocNo { get; set; } = string.Empty;

    public int MatterId { get; set; }

    [Required, StringLength(100)]
    public string DocumentType { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Direction { get; set; } = string.Empty;

    [StringLength(200)]
    public string PartyReference { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string DocumentTitle { get; set; } = string.Empty;

    public DateTime DateOfDocument { get; set; }

    public DateTime? DateFiledServed { get; set; }

    public DateTime? FilingResponseDeadline { get; set; }

    [StringLength(200)]
    public string PreparedFiledBy { get; set; } = string.Empty;

    [StringLength(200)]
    public string ServedOn { get; set; } = string.Empty;

    [StringLength(100)]
    public string ModeOfService { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string DocumentStatus { get; set; } = "Draft";

    [StringLength(50)]
    public string Version { get; set; } = string.Empty;

    [StringLength(200)]
    public string PhysicalFileBoxRef { get; set; } = string.Empty;

    [StringLength(500)]
    public string ElectronicFolderLink { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Confidentiality { get; set; } = "Internal";

    [StringLength(200)]
    public string OriginalHeldBy { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Remarks { get; set; } = string.Empty;

    // --- Derived (computed server-side, never stored) ---

    public int? DaysToDeadline { get; set; }

    public string DeadlineFlag { get; set; } = string.Empty;
}
