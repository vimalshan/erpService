using Microsoft.EntityFrameworkCore;

namespace EmailNotification.Infrastructure.Data;

/// <summary>
/// Entity Framework Core DbContext for EmailNotification service
/// </summary>
public class EmailNotificationDbContext : DbContext
{
    /// <summary>
    /// Email type master table
    /// </summary>
    public DbSet<Domain.Aggregates.EmailTypeAggregate> EmailTypes { get; set; } = null!;

    /// <summary>
    /// Mail access (recipients) table
    /// </summary>
    public DbSet<Domain.Entities.MailAccess> MailAccesses { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the EmailNotificationDbContext class
    /// </summary>
    /// <param name="options">DbContext options</param>
    public EmailNotificationDbContext(DbContextOptions<EmailNotificationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Configures the model on model creation
    /// </summary>
    /// <param name="modelBuilder">The model builder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure EmailTypeAggregate
        modelBuilder.Entity<Domain.Aggregates.EmailTypeAggregate>(entity =>
        {
            entity.ToTable("EMAIL_TYPEMAST");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("EMAIL_TYPEID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.EmailName)
                .HasColumnName("EMAIL_NAME")
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.EmailType)
                .HasColumnName("EMAIL_TYPE")
                .HasConversion(
                    x => ((char)x).ToString(),
                    x => x == "D" ? Domain.ValueObjects.EmailTypeEnum.Daily : Domain.ValueObjects.EmailTypeEnum.Event)
                .HasMaxLength(1)
                .IsRequired();

            entity.Property(e => e.EmailProcName)
                .HasColumnName("EMAIL_PRCNAME")
                .HasMaxLength(100)
                .IsRequired();

            // Map audit fields to EMAIL_MODIFIEDBY and EMAIL_MODIFIEDON
            // The domain model's CreatedBy/CreatedAt will be synced from ModifiedBy/ModifiedAt on creation
            entity.Property(e => e.ModifiedBy)
                .HasColumnName("EMAIL_MODIFIEDBY")
                .IsRequired();

            entity.Property(e => e.ModifiedAt)
                .HasColumnName("EMAIL_MODIFIEDON")
                .IsRequired();

            // Do not map CreatedBy and CreatedAt - they'll be handled by value generators
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.CreatedAt);

            // Configure navigation to MailAccess
            entity.HasMany<Domain.Entities.MailAccess>()
                .WithOne()
                .HasForeignKey(m => m.MailTypeId);

            // Create index
            entity.HasIndex(e => e.EmailType)
                .HasDatabaseName("IX_EMAIL_TYPEMAST_TYPE");
        });

        // Configure MailAccess
        modelBuilder.Entity<Domain.Entities.MailAccess>(entity =>
        {
            entity.ToTable("MAIL_ACCESS");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("MAIL_ACCESSID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.MailTypeId)
                .HasColumnName("MAIL_TYPEID")
                .IsRequired();

            entity.Property(e => e.MailOrgId)
                .HasColumnName("MAIL_ORGID");

            entity.Property(e => e.MailBusinessId)
                .HasColumnName("MAIL_BUSINESSID");

            entity.Property(e => e.MailEmpSysId)
                .HasColumnName("MAIL_EMPSYSID");

            entity.Property(e => e.MailEmail)
                .HasColumnName("MAIL_EMAILID")
                .HasMaxLength(200)
                .IsRequired()
                .HasConversion(
                    x => x.Value,
                    x => new Domain.ValueObjects.EmailAddress(x));

            entity.Property(e => e.MailName)
                .HasColumnName("MAIL_NAME")
                .HasMaxLength(100);

            entity.Property(e => e.ModifiedBy)
                .HasColumnName("MAIL_MODIFIEDBY")
                .IsRequired();

            entity.Property(e => e.ModifiedAt)
                .HasColumnName("MAIL_MODIFIEDON")
                .IsRequired();

            // Do not map CreatedBy and CreatedAt - they'll be handled by value generators
            entity.Ignore(e => e.CreatedBy);
            entity.Ignore(e => e.CreatedAt);

            // Create indexes
            entity.HasIndex(e => e.MailTypeId)
                .HasDatabaseName("IX_MAIL_ACCESS_TYPEID");

            entity.HasIndex(e => e.MailEmail)
                .HasDatabaseName("IX_MAIL_ACCESS_EMAILID");

            entity.HasIndex(e => e.MailOrgId)
                .HasDatabaseName("IX_MAIL_ACCESS_ORGID");

            entity.HasIndex(e => e.MailEmpSysId)
                .HasDatabaseName("IX_MAIL_ACCESS_EMPSYSID");

            // Foreign key constraint
            entity.HasOne<Domain.Aggregates.EmailTypeAggregate>()
                .WithMany()
                .HasForeignKey(m => m.MailTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
