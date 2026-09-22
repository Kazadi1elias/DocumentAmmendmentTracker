namespace DocumentAmendmentTracker.Web.Services;

public interface IFileStorageService
{
    /// <summary>Validates a candidate upload against size and extension rules. Throws <see cref="InvalidOperationException"/> if invalid.</summary>
    void ValidateFile(string fileName, long sizeBytes);

    /// <summary>Builds the UNC storage path for an attachment: {root}\{RecordId}\{AmendmentId}\{Guid}_{OriginalFileName}</summary>
    string BuildStoredPath(int recordId, int amendmentId, string fileName);

    /// <summary>Streams <paramref name="content"/> to <paramref name="storedPath"/> on the network share, retrying transient I/O failures.</summary>
    Task SaveFileAsync(Stream content, string storedPath, CancellationToken cancellationToken = default);

    /// <summary>Best-effort delete used to clean up partially-copied files when a transaction is rolled back.</summary>
    void TryDeleteFile(string storedPath);
}
