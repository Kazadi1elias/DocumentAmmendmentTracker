namespace DocumentAmendmentTracker.Web.Services;

/// <summary>An in-flight file selected in the UI, not yet persisted.</summary>
public sealed class AttachmentUpload : IDisposable
{
    public AttachmentUpload(string fileName, string contentType, long sizeBytes, Stream content)
    {
        FileName = fileName;
        ContentType = contentType;
        SizeBytes = sizeBytes;
        Content = content;
    }

    public string FileName { get; }
    public string ContentType { get; }
    public long SizeBytes { get; }
    public Stream Content { get; }

    public void Dispose() => Content.Dispose();
}
