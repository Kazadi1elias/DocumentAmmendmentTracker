using DocumentAmendmentTracker.Web.Models;

namespace DocumentAmendmentTracker.Web.Services;

public interface IRecordService
{
    Task<List<Record>> GetRecordsAsync(CancellationToken cancellationToken = default);

    Task<Record?> GetRecordWithHistoryAsync(int recordId, CancellationToken cancellationToken = default);

    /// <summary>Creates a new record together with its Amendment 0 (initial state) and any initial attachments, as a single atomic operation.</summary>
    Task<Record> CreateRecordAsync(
        string recordNumber,
        string title,
        string createdBy,
        string initialNotes,
        IReadOnlyList<AttachmentUpload> uploads,
        CancellationToken cancellationToken = default);

    /// <summary>Adds the next sequential amendment to an existing record, with any attachments, as a single atomic operation.</summary>
    Task<Amendment> AddAmendmentAsync(
        int recordId,
        string description,
        string amendedBy,
        IReadOnlyList<AttachmentUpload> uploads,
        CancellationToken cancellationToken = default);
}
