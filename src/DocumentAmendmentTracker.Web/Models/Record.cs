using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class Record
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string RecordNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public RecordStatus Status { get; set; } = RecordStatus.Active;

    public List<Amendment> Amendments { get; set; } = new();
}
