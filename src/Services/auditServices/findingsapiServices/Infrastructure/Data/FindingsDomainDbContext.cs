using FindingsAPI.Gateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindingsAPI.Gateway.Infrastructure.Data;

public class FindingsDomainDbContext : DbContext
{
    public FindingsDomainDbContext(DbContextOptions<FindingsDomainDbContext> options) : base(options) { }

    public DbSet<FindingEntity> Findings => Set<FindingEntity>();
    public DbSet<FindingStatusEntity> FindingStatuses => Set<FindingStatusEntity>();
    public DbSet<FindingCategoryEntity> FindingCategories => Set<FindingCategoryEntity>();
    public DbSet<FindingClauseEntity> FindingClauses => Set<FindingClauseEntity>();
    public DbSet<FindingFocusAreaEntity> FindingFocusAreas => Set<FindingFocusAreaEntity>();
    public DbSet<FindingResponseEntity> FindingResponses => Set<FindingResponseEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Finding
        modelBuilder.Entity<FindingEntity>(e =>
        {
            e.ToTable("Findings");
            e.HasKey(x => x.FindingId);
            e.Property(x => x.FindingNumber).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.FindingNumber).IsUnique();
            e.Property(x => x.Title).HasMaxLength(500).IsRequired();
            e.Property(x => x.Description).IsRequired();
            e.Property(x => x.FindingType).HasMaxLength(50).IsRequired();
            e.Property(x => x.Severity).HasMaxLength(50);
            e.Property(x => x.Evidence);
            e.Property(x => x.RootCause);
            e.Property(x => x.CorrectiveAction);
            e.Property(x => x.PreventiveAction);
            e.Property(x => x.VerificationMethod);
            e.HasIndex(x => x.AuditId);
            e.HasIndex(x => x.SiteId);
            e.HasIndex(x => x.FindingStatusId);
            e.HasIndex(x => x.FindingType);
            e.HasIndex(x => x.Severity);
            e.HasIndex(x => x.DueDate);
            e.HasIndex(x => x.AssignedTo);
            e.HasIndex(x => x.IsActive);
            e.HasOne(x => x.FindingStatus).WithMany().HasForeignKey(x => x.FindingStatusId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.FindingCategory).WithMany().HasForeignKey(x => x.FindingCategoryId).OnDelete(DeleteBehavior.SetNull);
            e.HasMany(x => x.Responses).WithOne(x => x.Finding).HasForeignKey(x => x.FindingId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Clauses).WithOne(x => x.Finding).HasForeignKey(x => x.FindingId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.FocusAreas).WithOne(x => x.Finding).HasForeignKey(x => x.FindingId).OnDelete(DeleteBehavior.Cascade);
            e.Ignore(x => x.DomainEvents);
        });

        // FindingStatus
        modelBuilder.Entity<FindingStatusEntity>(e =>
        {
            e.ToTable("FindingStatuses");
            e.HasKey(x => x.FindingStatusId);
            e.Property(x => x.StatusName).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.StatusName).IsUnique();
            e.Property(x => x.StatusCode).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.StatusCode).IsUnique();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.Color).HasMaxLength(7);
        });

        // FindingCategory
        modelBuilder.Entity<FindingCategoryEntity>(e =>
        {
            e.ToTable("FindingCategories");
            e.HasKey(x => x.FindingCategoryId);
            e.Property(x => x.CategoryName).HasMaxLength(200).IsRequired();
            e.HasIndex(x => x.CategoryName).IsUnique();
            e.Property(x => x.CategoryCode).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.CategoryCode).IsUnique();
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.Color).HasMaxLength(7);
            e.HasOne(x => x.ParentCategory).WithMany(x => x.ChildCategories).HasForeignKey(x => x.ParentCategoryId).OnDelete(DeleteBehavior.Restrict);
        });

        // FindingClause
        modelBuilder.Entity<FindingClauseEntity>(e =>
        {
            e.ToTable("FindingClauses");
            e.HasKey(x => x.FindingClauseId);
            e.HasIndex(x => new { x.FindingId, x.ClauseId }).IsUnique();
            e.Property(x => x.Notes).HasMaxLength(500);
        });

        // FindingFocusArea
        modelBuilder.Entity<FindingFocusAreaEntity>(e =>
        {
            e.ToTable("FindingFocusAreas");
            e.HasKey(x => x.FindingFocusAreaId);
            e.HasIndex(x => new { x.FindingId, x.FocusAreaId }).IsUnique();
            e.Property(x => x.Notes).HasMaxLength(500);
        });

        // FindingResponse
        modelBuilder.Entity<FindingResponseEntity>(e =>
        {
            e.ToTable("FindingResponses");
            e.HasKey(x => x.FindingResponseId);
            e.Property(x => x.ResponseText).IsRequired();
            e.Property(x => x.ResponseType).HasMaxLength(50).IsRequired();
            e.Property(x => x.Status).HasMaxLength(50);
            e.Property(x => x.AttachmentPath).HasMaxLength(500);
            e.Property(x => x.ReviewComments);
            e.HasIndex(x => x.FindingId);
            e.HasIndex(x => x.RespondedBy);
            e.HasIndex(x => x.ResponseDate);
            e.HasIndex(x => x.IsSubmittedToDNV);
            e.HasIndex(x => x.Status);
            e.HasIndex(x => x.IsActive);
        });
    }
}
