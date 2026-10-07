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
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DiaryEvent> DiaryEvents => Set<DiaryEvent>();
    public DbSet<FeeNote> FeeNotes => Set<FeeNote>();
    public DbSet<Lesson> Lessons => Set<Lesson>();

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

        // Document/DiaryEvent/FeeNote/Lesson: schema only, same stored-procedure-only pattern.
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasIndex(d => d.DocNo).IsUnique();
            entity.Ignore(d => d.DaysToDeadline);
            entity.Ignore(d => d.DeadlineFlag);

            entity.HasOne<Matter>()
                .WithMany()
                .HasForeignKey(d => d.MatterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DiaryEvent>(entity =>
        {
            entity.HasIndex(e => e.EventNo).IsUnique();
            entity.Ignore(e => e.DaysToEvent);
            entity.Ignore(e => e.Alert);

            entity.HasOne<Matter>()
                .WithMany()
                .HasForeignKey(e => e.MatterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FeeNote>(entity =>
        {
            entity.HasIndex(f => f.RefNo).IsUnique();

            entity.Property(f => f.ProfessionalFees).HasPrecision(18, 2);
            entity.Property(f => f.Disbursements).HasPrecision(18, 2);
            entity.Property(f => f.Vat).HasPrecision(18, 2);
            entity.Property(f => f.WithholdingTax).HasPrecision(18, 2);
            entity.Property(f => f.ApprovedBudgetForStage).HasPrecision(18, 2);

            entity.Ignore(f => f.GrossInvoiceValue);
            entity.Ignore(f => f.NetPayableToFirm);
            entity.Ignore(f => f.VarianceToBudget);

            entity.HasOne<Matter>()
                .WithMany()
                .HasForeignKey(f => f.MatterId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasIndex(l => l.LessonNo).IsUnique();
            entity.Property(l => l.CostAttributable).HasPrecision(18, 2);
            entity.Ignore(l => l.DaysOverdue);

            entity.HasOne<Matter>()
                .WithMany()
                .HasForeignKey(l => l.MatterId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
