namespace DocumentAmendmentTracker.Web.Options;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";

    /// <summary>UNC root path, e.g. \\server\share\Records</summary>
    public string RootPath { get; set; } = string.Empty;

    public long MaxFileSizeBytes { get; set; } = 50 * 1024 * 1024;

    public string[] AllowedExtensions { get; set; } =
        [".pdf", ".docx", ".xlsx", ".jpg", ".jpeg", ".png"];

    public int CopyRetryCount { get; set; } = 3;

    public int CopyRetryDelayMilliseconds { get; set; } = 500;
}
