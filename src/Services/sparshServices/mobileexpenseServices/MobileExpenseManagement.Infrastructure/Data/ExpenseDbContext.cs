namespace MobileExpenseManagement.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using MobileExpenseManagement.Domain.Entities;

/// <summary>
/// Entity Framework DbContext for Mobile Expense Management
/// </summary>
public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<ExpenseFile> ExpenseFiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Expense entity
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("MOBEXP_DET");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("MOBEXP_ID")
                .HasDefaultValueSql("NEXT VALUE FOR dbo.seq_MOBEXP_Id");

            entity.Property(e => e.TripId)
                .HasColumnName("MOBEXP_TPID")
                .IsRequired();

            entity.Property(e => e.CategoryId)
                .HasColumnName("MOBEXP_CATID")
                .IsRequired();

            entity.Property(e => e.ExpenseDate)
                .HasColumnName("MOBEXP_DATE")
                .HasColumnType("datetime2(3)");

            entity.Property(e => e.Comment)
                .HasColumnName("MOBEXP_COMMENT")
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.Amount)
                .HasColumnName("MOBEXP_AMOUNT")
                .HasColumnType("decimal(19,2)");

            entity.Property(e => e.CurrencyId)
                .HasColumnName("MOBEXP_CURRID");

            entity.Property(e => e.EnteredBy)
                .HasColumnName("MOBEXP_ENTEREDBY")
                .IsRequired();

            entity.Property(e => e.EnteredOn)
                .HasColumnName("MOBEXP_ENTEREDON")
                .HasColumnType("datetime2(3)")
                .IsRequired();

            entity.Property(e => e.ModifiedOn)
                .HasColumnName("MOBEXP_MODIFIEDON")
                .HasColumnType("datetime2(3)");

            entity.Property(e => e.ModifiedBy)
                .HasColumnName("MOBEXP_MODIFIEDBY");

            entity.Property(e => e.DeletedOn)
                .HasColumnName("MOBEXP_DELETEDON")
                .HasColumnType("datetime2(3)");

            entity.Property(e => e.DeletedBy)
                .HasColumnName("MOBEXP_DELETEDBY");

            entity.Property(e => e.IsDeleted)
                .HasColumnName("MOBEXP_ISDELETED")
                .HasDefaultValue(false);

            entity.HasMany(e => e.Files)
                .WithOne(f => f.Expense)
                .HasForeignKey(f => f.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.TripId).HasDatabaseName("IX_MOBEXP_TPID");
            entity.HasIndex(e => e.CategoryId).HasDatabaseName("IX_MOBEXP_CATID");
            entity.HasIndex(e => e.ExpenseDate).HasDatabaseName("IX_MOBEXP_DATE");
            entity.HasIndex(e => e.EnteredBy).HasDatabaseName("IX_MOBEXP_ENTEREDBY");
            entity.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_MOBEXP_ISDELETED");
        });

        // Configure ExpenseFile entity
        modelBuilder.Entity<ExpenseFile>(entity =>
        {
            entity.ToTable("MOBEXP_FILE");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("MOBEXPPHT_ID")
                .HasDefaultValueSql("NEXT VALUE FOR dbo.seq_MOBEXP_File_Id");

            entity.Property(e => e.ExpenseId)
                .HasColumnName("MOBEXPPHT_EXPID")
                .IsRequired();

            entity.Property(e => e.FileName)
                .HasColumnName("MOBEXPPHT_FILENAME")
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.FileData)
                .HasColumnName("MOBEXPPHT_FILEDATA")
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.FileSize)
                .HasColumnName("MOBEXPPHT_FILESIZE");

            entity.Property(e => e.ContentType)
                .HasColumnName("MOBEXPPHT_CONTENTTYPE")
                .HasMaxLength(100);

            entity.Property(e => e.UploadedOn)
                .HasColumnName("MOBEXPPHT_UPLOADEDON")
                .HasColumnType("datetime2(3)");

            entity.Property(e => e.UploadedBy)
                .HasColumnName("MOBEXPPHT_UPLOADEDBY");

            entity.Property(e => e.BlobStoragePath)
                .HasColumnName("MOBEXPPHT_BLOBPATH")
                .HasMaxLength(500);

            entity.Property(e => e.IsDeleted)
                .HasColumnName("MOBEXPPHT_ISDELETED")
                .HasDefaultValue(false);

            entity.HasIndex(e => e.ExpenseId).HasDatabaseName("IX_MOBEXP_FILE_EXPID");
            entity.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_MOBEXP_FILE_ISDELETED");
        });

        // Exclude domain events from the model
        modelBuilder.Ignore<MobileExpenseManagement.Domain.Entities.DomainEvent>();
        modelBuilder.Ignore<MobileExpenseManagement.Domain.Entities.ExpenseCreatedDomainEvent>();
        modelBuilder.Ignore<MobileExpenseManagement.Domain.Entities.ExpenseUpdatedDomainEvent>();
        modelBuilder.Ignore<MobileExpenseManagement.Domain.Entities.ExpenseDeletedDomainEvent>();
    }
}
