using FaqServices.Domain.Common;
using FaqServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaqServices.Infrastructure.Data;

public class FaqDbContext : DbContext
{
    public DbSet<FaqGrade> FaqGrades { get; set; } = null!;
    public DbSet<FaqQuestion> FaqQuestions { get; set; } = null!;
    public DbSet<FaqAnswer> FaqAnswers { get; set; } = null!;

    public FaqDbContext(DbContextOptions<FaqDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // FAQ_GRADE Configuration
        modelBuilder.Entity<FaqGrade>(entity =>
        {
            entity.ToTable("FAQ_GRADE");
            entity.HasKey(e => e.PK);
            entity.Property(e => e.PK).HasColumnType("varchar(255)").ValueGeneratedNever().IsRequired();
            entity.Property(e => e.GradeName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.SortOrder).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedBy).HasColumnType("varchar(255)").HasMaxLength(255);
            entity.Property(e => e.UpdatedBy).HasColumnType("varchar(255)").HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.DeletedAt);
            
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.SortOrder);
            
            // Navigation
            entity.HasMany(e => e.Questions)
                .WithOne(q => q.Grade)
                .HasForeignKey(q => q.GradeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // FAQ_QUESTION Configuration
        modelBuilder.Entity<FaqQuestion>(entity =>
        {
            entity.ToTable("FAQ_QUESTION");
            entity.HasKey(e => e.PK);
            entity.Property(e => e.PK).HasColumnType("varchar(255)").ValueGeneratedNever().IsRequired();
            entity.Property(e => e.GradeId).HasColumnType("varchar(255)").IsRequired().HasMaxLength(255);
            entity.Property(e => e.QuestionText).IsRequired().HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.QuestionTextAr).HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.ImageBlobUrl).HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.SortOrder).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedBy).HasColumnType("varchar(255)").HasMaxLength(255);
            entity.Property(e => e.UpdatedBy).HasColumnType("varchar(255)").HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.DeletedAt);
            
            entity.HasIndex(e => e.GradeId);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.SortOrder);
            
            // Navigation
            entity.HasOne(e => e.Grade)
                .WithMany(g => g.Questions)
                .HasForeignKey(e => e.GradeId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasMany(e => e.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // FAQ_ANSWER Configuration
        modelBuilder.Entity<FaqAnswer>(entity =>
        {
            entity.ToTable("FAQ_ANSWER");
            entity.HasKey(e => e.PK);
            entity.Property(e => e.PK).HasColumnType("varchar(255)").ValueGeneratedNever().IsRequired();
            entity.Property(e => e.QuestionId).HasColumnType("varchar(255)").IsRequired().HasMaxLength(255);
            entity.Property(e => e.AnswerText).IsRequired().HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.AnswerTextAr).HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.ImageBlobUrl).HasColumnType("NVARCHAR(MAX)");
            entity.Property(e => e.IsCorrect).HasDefaultValue(false);
            entity.Property(e => e.SortOrder).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedBy).HasColumnType("varchar(255)").HasMaxLength(255);
            entity.Property(e => e.UpdatedBy).HasColumnType("varchar(255)").HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.DeletedAt);
            
            entity.HasIndex(e => e.QuestionId);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.IsCorrect);
            entity.HasIndex(e => e.SortOrder);
            
            // Navigation
            entity.HasOne(e => e.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkUpdated();
            }
        }
    }
}
