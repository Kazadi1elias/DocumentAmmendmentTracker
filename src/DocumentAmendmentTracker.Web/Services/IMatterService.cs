using DocumentAmendmentTracker.Web.Models;

namespace DocumentAmendmentTracker.Web.Services;

public interface IMatterService
{
    /// <summary>All matters, with derived fields (cause title, party counts, totals, day-counts) computed from the full Parties register.</summary>
    Task<List<Matter>> GetMattersAsync(CancellationToken cancellationToken = default);

    Task<Matter?> GetMatterByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates a matter, auto-generating its Matter ID (e.g. LIT-2026-001) from forum type and commencement year.</summary>
    Task<Matter> CreateMatterAsync(Matter input, string createdBy, CancellationToken cancellationToken = default);

    Task UpdateMatterAsync(Matter input, CancellationToken cancellationToken = default);

    /// <summary>Adds a party, auto-assigning its Party No. (sequential across the register) and Party Order (sequential per matter+side).</summary>
    Task<Party> AddPartyAsync(int matterId, Party input, CancellationToken cancellationToken = default);

    Task UpdatePartyAsync(Party input, CancellationToken cancellationToken = default);
}
