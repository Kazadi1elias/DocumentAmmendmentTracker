using DocumentAmendmentTracker.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentAmendmentTracker.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Record> Records => Set<Record>();
    public DbSet<Amendment> Amendments => Set<Amendment>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Matter> Matters => Set<Matter>();
    public DbSet<Party> Parties => Set<Party>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasIndex(r => r.RecordNumber).IsUnique();

            entity.HasMany(r => r.Amendments)
                .WithOne(a => a.Record)
                .HasForeignKey(a => a.RecordId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Amendment>(entity =>
        {
            entity.HasIndex(a => new { a.RecordId, a.SequenceNumber }).IsUnique();

            entity.HasMany(a => a.Attachments)
                .WithOne(f => f.Amendment)
                .HasForeignKey(f => f.AmendmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Matter/Party: schema only. All reads/writes go through stored procedures -
        // EF Core here exists purely to generate migrations.
        modelBuilder.Entity<Matter>(entity =>
        {
            entity.HasIndex(m => m.MatterId).IsUnique();

            entity.Property(m => m.SumClaimed).HasPrecision(18, 2);
            entity.Property(m => m.ProvisionRecommended).HasPrecision(18, 2);
            entity.Property(m => m.EstimatedLegalFeesToFinalisation).HasPrecision(18, 2);
            entity.Property(m => m.LegalFeesIncurredToDate).HasPrecision(18, 2);

            entity.Ignore(m => m.Parties);
            entity.Ignore(m => m.CauseTitle);
            entity.Ignore(m => m.NoOfClaimants);
            entity.Ignore(m => m.NoOfRespondents);
            entity.Ignore(m => m.LeadClaimant);
            entity.Ignore(m => m.LeadRespondent);
            entity.Ignore(m => m.PartyStructure);
            entity.Ignore(m => m.SumClaimedPerPartiesRegister);
            entity.Ignore(m => m.ClaimsReconciliation);
            entity.Ignore(m => m.TotalEstimatedLegalCost);
            entity.Ignore(m => m.TotalFinancialExposure);
            entity.Ignore(m => m.DaysOpen);
            entity.Ignore(m => m.DaysToNextDate);
            entity.Ignore(m => m.DaysSinceLastActivity);
            entity.Ignore(m => m.PartiesOnRecord);

            entity.HasMany<Party>()
                .WithOne()
                .HasForeignKey(p => p.MatterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Party>(entity =>
        {
            entity.HasIndex(p => p.PartyNo).IsUnique();
            entity.Property(p => p.IndividualSumClaimed).HasPrecision(18, 2);
            entity.Ignore(p => p.Designation);
        });
    }
}
