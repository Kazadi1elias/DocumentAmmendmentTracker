using DocumentAmendmentTracker.Web.Data;
using DocumentAmendmentTracker.Web.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DocumentAmendmentTracker.Web.Services;

public class RecordService : IRecordService
{
    private const int MaxSequenceConflictRetries = 3;

    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger<RecordService> _logger;

    public RecordService(
        IDbContextFactory<AppDbContext> contextFactory,
        IFileStorageService fileStorage,
        ILogger<RecordService> logger)
    {
        _contextFactory = contextFactory;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<List<Record>> GetRecordsAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Records
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Record?> GetRecordWithHistoryAsync(int recordId, CancellationToken cancellationToken = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await db.Records
            .AsNoTracking()
            .Include(r => r.Amendments.OrderBy(a => a.SequenceNumber))
                .ThenInclude(a => a.Attachments)
            .FirstOrDefaultAsync(r => r.Id == recordId, cancellationToken);
    }

    public async Task<Record> CreateRecordAsync(
        string recordNumber,
        string title,
        string createdBy,
        string initialNotes,
        IReadOnlyList<AttachmentUpload> uploads,
        CancellationToken cancellationToken = default)
    {
        foreach (var upload in uploads)
        {
            _fileStorage.ValidateFile(upload.FileName, upload.SizeBytes);
        }

        await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var now = DateTime.UtcNow;
        var record = new Record
        {
            RecordNumber = recordNumber,
            Title = title,
            CreatedBy = createdBy,
            CreatedDate = now,
            Status = RecordStatus.Active,
        };

        var amendmentZero = new Amendment
        {
            SequenceNumber = 0,
            Description = string.IsNullOrWhiteSpace(initialNotes) ? "Record created." : initialNotes,
            AmendedBy = createdBy,
            AmendedDate = now,
        };
        record.Amendments.Add(amendmentZero);

        db.Records.Add(record);
        await db.SaveChangesAsync(cancellationToken);

        var copiedPaths = new List<string>();
        try
        {
            await CopyAndAttachUploadsAsync(db, record.Id, amendmentZero, uploads, createdBy, copiedPaths, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return record;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            foreach (var path in copiedPaths)
            {
                _fileStorage.TryDeleteFile(path);
            }
            throw;
        }
    }

    public async Task<Amendment> AddAmendmentAsync(
        int recordId,
        string description,
        string amendedBy,
        IReadOnlyList<AttachmentUpload> uploads,
        CancellationToken cancellationToken = default)
    {
        foreach (var upload in uploads)
        {
            _fileStorage.ValidateFile(upload.FileName, upload.SizeBytes);
        }

        for (var attempt = 1; attempt <= MaxSequenceConflictRetries; attempt++)
        {
            await using var db = await _contextFactory.CreateDbContextAsync(cancellationToken);
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

            var recordExists = await db.Records.AnyAsync(r => r.Id == recordId, cancellationToken);
            if (!recordExists)
            {
                throw new InvalidOperationException($"Record {recordId} was not found.");
            }

            var nextSequence = 1 + await db.Amendments
                .Where(a => a.RecordId == recordId)
                .Select(a => (int?)a.SequenceNumber)
                .MaxAsync(cancellationToken) ?? 0;

            var amendment = new Amendment
            {
                RecordId = recordId,
                SequenceNumber = nextSequence,
                Description = description,
                AmendedBy = amendedBy,
                AmendedDate = DateTime.UtcNow,
            };
            db.Amendments.Add(amendment);

            try
            {
                await db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex) && attempt < MaxSequenceConflictRetries)
            {
                _logger.LogInformation(
                    "Sequence number {Sequence} for record {RecordId} was claimed concurrently; retrying (attempt {Attempt}).",
                    nextSequence, recordId, attempt);
                await transaction.RollbackAsync(cancellationToken);
                continue;
            }

            var copiedPaths = new List<string>();
            try
            {
                await CopyAndAttachUploadsAsync(db, recordId, amendment, uploads, amendedBy, copiedPaths, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return amendment;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                foreach (var path in copiedPaths)
                {
                    _fileStorage.TryDeleteFile(path);
                }
                throw;
            }
        }

        throw new InvalidOperationException(
            $"Could not add amendment to record {recordId} after {MaxSequenceConflictRetries} attempts due to concurrent updates. Please try again.");
    }

    private async Task CopyAndAttachUploadsAsync(
        AppDbContext db,
        int recordId,
        Amendment amendment,
        IReadOnlyList<AttachmentUpload> uploads,
        string uploadedBy,
        List<string> copiedPaths,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        foreach (var upload in uploads)
        {
            var storedPath = _fileStorage.BuildStoredPath(recordId, amendment.Id, upload.FileName);
            await _fileStorage.SaveFileAsync(upload.Content, storedPath, cancellationToken);
            copiedPaths.Add(storedPath);

            db.Attachments.Add(new Attachment
            {
                AmendmentId = amendment.Id,
                FileName = upload.FileName,
                StoredPath = storedPath,
                FileSizeBytes = upload.SizeBytes,
                ContentType = upload.ContentType,
                UploadedBy = uploadedBy,
                UploadedDate = now,
            });
        }
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
        ex.InnerException is SqlException { Number: 2601 or 2627 };
}
