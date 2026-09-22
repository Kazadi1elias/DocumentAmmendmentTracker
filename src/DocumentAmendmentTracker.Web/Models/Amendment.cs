using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class Amendment
{
    public int Id { get; set; }

    public int RecordId { get; set; }
    public Record? Record { get; set; }

    /// <summary>0 = the record's initial creation; 1, 2, 3... for each amendment thereafter.</summary>
    public int SequenceNumber { get; set; }

    [Required, StringLength(4000)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string AmendedBy { get; set; } = string.Empty;

    public DateTime AmendedDate { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public List<Attachment> Attachments { get; set; } = new();
}
