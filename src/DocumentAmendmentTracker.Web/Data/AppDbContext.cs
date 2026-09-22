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
    }
}
