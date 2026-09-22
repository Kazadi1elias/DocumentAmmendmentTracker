namespace DocumentAmendmentTracker.Web.Services;

/// <summary>An in-flight file selected in the UI, not yet persisted.</summary>
public sealed class AttachmentUpload : IDisposable
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required long SizeBytes { get; init; }
    public required Stream Content { get; init; }

    public void Dispose() => Content.Dispose();
}
