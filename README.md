# DocumentAmmendmentTracker

A Blazor Server app for tracking a record's amendment and attachment history over time.
Every record is created with an initial "Amendment 0"; subsequent amendments are added
sequentially, each optionally carrying one or more attachments stored on a network share
(UNC path) — the database holds only metadata and file paths.

## Stack

- **Blazor Server** (.NET 6, classic template: `Pages/_Host.cshtml` + `App.razor` router)
- **EF Core 6 + SQL Server**, Code First, migrations in `src/DocumentAmendmentTracker.Web/Data/Migrations`
- **Windows/AD authentication** via `Microsoft.AspNetCore.Authentication.Negotiate` — every
  page requires a signed-in user (`AddAuthorization` fallback policy); `CreatedBy`/`AmendedBy`
  are taken from the authenticated identity, not typed in
- **Radzen.Blazor** (4.32.2) for data display: the records grid, the amendment timeline, and form inputs
- **SweetAlert2** (`CurrieTechnologies.Razor.SweetAlert2`) for success/error popups after
  creating a record or saving an amendment
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

Negotiate (Windows) authentication needs a domain-joined host (IIS or Kestrel on a
domain-joined Windows server, or Kerberos/keytab-configured Linux) to actually authenticate
a user — it can't be exercised from an arbitrary dev box. Locally, requests without
Windows credentials get a `401` + `WWW-Authenticate: Negotiate` challenge, which is the
expected behavior, not a bug.

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
5. **Permissions** — no ownership/role restriction; any authenticated Windows/AD user can
   amend any record. `CreatedBy`/`AmendedBy` come from `HttpContext.User.Identity.Name`
   (Negotiate), not a free-text field.
6. **Failed file copy** — the whole amendment insert (and any attachments already copied in
   that batch) is rolled back: `RecordService` wraps the amendment + attachment inserts and the
   file copy in one DB transaction, and deletes any partially-copied files if the transaction
   is rolled back. No orphaned DB rows or partial files are left behind.

## Verification

The service layer (record creation, sequential amendments, attachment placement, and
transactional rollback on a simulated network share failure) was exercised end-to-end
against a real relational database with ACID transactions as part of development; all
checks passed. The app was also started locally to confirm the Negotiate auth pipeline
correctly challenges unauthenticated requests. A live UI walkthrough against a real
SQL Server + UNC share on a domain-joined host is still recommended before production use.
