# DocumentAmmendmentTracker

A Blazor Server app for tracking a record's amendment and attachment history over time.
Every record is created with an initial "Amendment 0"; subsequent amendments are added
sequentially, each optionally carrying one or more attachments stored on a network share
(UNC path) — the database holds only metadata and file paths.

## Stack

- **Blazor Server** (.NET 8, unified `Blazor Web App` template, Server interactivity)
- **EF Core 8 + SQL Server**, Code First, migrations in `src/DocumentAmendmentTracker.Web/Data/Migrations`
- **Radzen.Blazor** for UI components (data grid, timeline, forms, notifications)
- File uploads use `InputFile.OpenReadStream()` streamed directly to the network share —
  never fully buffered in memory

## Running locally

1. Point `ConnectionStrings:AppDbContext` in `appsettings.json` (or `appsettings.Development.json`)
   at a reachable SQL Server instance.
2. Point `FileStorage:RootPath` at a UNC path the app server can write to.
3. Apply migrations:
   ```
   cd src/DocumentAmendmentTracker.Web
   dotnet ef database update
   ```
4. `dotnet run`

## Key design decisions (spec's open items)

The spec left several decisions for build time. Choices made, and why:

1. **Concurrency** — the `Amendment` unique index on `(RecordId, SequenceNumber)` is the
   real guard: `RecordService.AddAmendmentAsync` computes the next sequence number and
   retries (bounded) if two users race and collide on the same number. `Amendment.RowVersion`
   (SQL Server `rowversion`) is carried as a concurrency token for future edit/update scenarios.
2. **Network share reliability** — `FileStorageService.SaveFileAsync` retries transient
   `IOException`s a configurable number of times (`FileStorage:CopyRetryCount`) before giving up;
   no queue/background retry was built for this pass.
3. **Amendments as snapshots vs. additive notes** — additive notes. The `Record`'s own fields
   are not versioned; only the amendment history (description + attachments) accumulates.
4. **Max attachments per amendment** — multiple attachments are supported per amendment.
5. **Permissions** — no ownership/role restriction; any user can amend any record. No auth
   system was in scope, so "who" is just a free-text `AmendedBy`/`CreatedBy` field.
6. **Failed file copy** — the whole amendment insert (and any attachments already copied in
   that batch) is rolled back: `RecordService` wraps the amendment + attachment inserts and the
   file copy in one DB transaction, and deletes any partially-copied files if the transaction
   is rolled back. No orphaned DB rows or partial files are left behind.

## Verification

The service layer (record creation, sequential amendments, attachment placement, and
transactional rollback on a simulated network share failure) was exercised end-to-end
against a real relational database with ACID transactions as part of development; all
checks passed. A live UI walkthrough against a real SQL Server + UNC share is still
recommended before production use.
