using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class Attachment
{
    public int Id { get; set; }

    public int AmendmentId { get; set; }
    public Amendment? Amendment { get; set; }

    [Required, StringLength(260)]
    public string FileName { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string StoredPath { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    [Required, StringLength(200)]
    public string ContentType { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string UploadedBy { get; set; } = string.Empty;

    public DateTime UploadedDate { get; set; }
}
