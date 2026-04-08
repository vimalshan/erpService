using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Infrastructure.Persistence.Configurations;

public sealed class AutoMailStatusConfiguration : IEntityTypeConfiguration<AutoMailStatusEntity>
{
    public void Configure(EntityTypeBuilder<AutoMailStatusEntity> builder)
    {
        builder.ToTable("AUTO_MAIL_STATUS");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("AUTO_MAIL_STATUS_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.MailType).HasColumnName("MAIL_TYPE").HasMaxLength(25).IsRequired();
        builder.Property(e => e.MailDate).HasColumnName("MAIL_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.MailStatus).HasColumnName("MAIL_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(e => e.MailRemarks).HasColumnName("MAIL_REMARKS").HasMaxLength(1000);

        builder.Ignore(e => e.DomainEvents);

        builder.HasData(
            new { Id = 1, MailType = "DAILY_REPORT", MailDate = new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc),
                MailStatus = "S", MailRemarks = "Sent successfully" },
            new { Id = 2, MailType = "WEEKLY_SUMMARY", MailDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                MailStatus = "S", MailRemarks = (string?)null },
            new { Id = 3, MailType = "ALERT_DISPATCH", MailDate = new DateTime(2026, 3, 19, 0, 0, 0, DateTimeKind.Utc),
                MailStatus = "F", MailRemarks = "SMTP server unreachable" }
        );
    }
}

public sealed class AutoMailIdConfiguration : IEntityTypeConfiguration<AutoMailIdEntity>
{
    public void Configure(EntityTypeBuilder<AutoMailIdEntity> builder)
    {
        builder.ToTable("AUTO_MAILID");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("AUTO_MAILID_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.IdType).HasColumnName("ID_TYPE").HasMaxLength(25);
        builder.Property(e => e.MailId).HasColumnName("MAILID").HasMaxLength(40);
        builder.Property(e => e.StartDate).HasColumnName("STARTDATE").HasPrecision(3);
        builder.Property(e => e.EndDate).HasColumnName("ENDDATE").HasPrecision(3);
        builder.Property(e => e.MailType).HasColumnName("MAIL_TYPE").HasMaxLength(3);

        builder.Ignore(e => e.DomainEvents);

        builder.HasData(
            new { Id = 1, IdType = "DISPATCH", MailId = "dispatch@sci.com",
                StartDate = new DateTime?(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = (DateTime?)null, MailType = "TO" },
            new { Id = 2, IdType = "DISPATCH", MailId = "mgr@sci.com",
                StartDate = new DateTime?(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = (DateTime?)null, MailType = "CC" },
            new { Id = 3, IdType = "ALERT", MailId = "alerts@sci.com",
                StartDate = new DateTime?(new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc)),
                EndDate = new DateTime?(new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)),
                MailType = "TO" }
        );
    }
}
