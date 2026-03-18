using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class WebsiteContactEmailConfiguration : IEntityTypeConfiguration<WebsiteContactEmail>
{
    public void Configure(EntityTypeBuilder<WebsiteContactEmail> builder)
    {
        builder.ToTable("WEBSITE_CON_MAILID");

        builder.HasKey(c => c.ContactId);
        builder.Property(c => c.ContactId).HasColumnName("CONTACT_ID").UseIdentityColumn();
        builder.Property(c => c.UserSysId).HasColumnName("USER_SYSID").IsRequired();
        builder.Property(c => c.PrimaryEmail).HasColumnName("PRIMARY_EMAIL").HasColumnType("VARCHAR(255)").IsRequired();
        builder.Property(c => c.SecondaryEmail).HasColumnName("SECONDARY_EMAIL").HasColumnType("VARCHAR(255)");
        builder.Property(c => c.Phone).HasColumnName("PHONE").HasColumnType("VARCHAR(20)");
        builder.Property(c => c.Mobile).HasColumnName("MOBILE").HasColumnType("VARCHAR(20)");
        builder.Property(c => c.Website).HasColumnName("WEBSITE").HasColumnType("VARCHAR(255)");
        builder.Property(c => c.SocialMedia).HasColumnName("SOCIAL_MEDIA").HasColumnType("VARCHAR(500)");
        builder.Property(c => c.NewsletterOptIn).HasColumnName("NEWSLETTER_OPT_IN").HasColumnType("CHAR(1)").HasDefaultValue('Y');
        builder.Property(c => c.ContactStatus).HasColumnName("CONTACT_STATUS").HasColumnType("CHAR(1)").HasDefaultValue('A');
        builder.Property(c => c.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(c => c.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("DATETIME2(3)").HasDefaultValueSql("GETDATE()");
        builder.Property(c => c.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(c => c.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("DATETIME2(3)");

        builder.HasIndex(c => c.UserSysId).HasDatabaseName("IX_WEBSITE_CON_MAILID_USER_SYSID");
        builder.HasIndex(c => c.PrimaryEmail).HasDatabaseName("IX_WEBSITE_CON_MAILID_EMAIL");
        builder.HasIndex(c => c.ContactStatus).HasDatabaseName("IX_WEBSITE_CON_MAILID_STATUS");

        builder.Ignore(c => c.DomainEvents);
    }
}
