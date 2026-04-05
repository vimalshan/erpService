namespace WebsiteContentService.Infrastructure.Persistence.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteContentService.Domain.Entities;
using WebsiteContentService.Domain.ValueObjects;

public class WebsitePageConfiguration : IEntityTypeConfiguration<WebsitePage>
{
    public void Configure(EntityTypeBuilder<WebsitePage> builder)
    {
        builder.ToTable("WEBSITE_PAGES");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("PAGE_ID")
            .UseIdentityColumn();

        builder.OwnsOne(e => e.PageCode, pc =>
        {
            pc.Property(p => p.Value)
                .HasColumnName("PAGE_CODE")
                .HasMaxLength(100)
                .IsRequired();

            pc.HasIndex(p => p.Value).IsUnique();
        });

        builder.Property(e => e.PageTitle)
            .HasColumnName("PAGE_TITLE")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.PageContent)
            .HasColumnName("PAGE_CONTENT");

        builder.Property(e => e.MetaDescription)
            .HasColumnName("META_DESCRIPTION")
            .HasMaxLength(500);

        builder.Property(e => e.MetaKeywords)
            .HasColumnName("META_KEYWORDS")
            .HasMaxLength(500);

        builder.Property(e => e.PageOrder)
            .HasColumnName("PAGE_ORDER");

        builder.Property(e => e.ParentPageId)
            .HasColumnName("PARENT_PAGE_ID");

        builder.OwnsOne(e => e.IsPublished, ip =>
        {
            ip.Property(p => p.Value)
                .HasColumnName("IS_PUBLISHED")
                .HasMaxLength(1)
                .HasDefaultValue('N')
                .IsRequired();
        });

        builder.Property(e => e.PublishedDate)
            .HasColumnName("PUBLISHED_DATE")
            .HasPrecision(3);

        builder.OwnsOne(e => e.PageStatus, ps =>
        {
            ps.Property(p => p.Value)
                .HasColumnName("PAGE_STATUS")
                .HasMaxLength(20)
                .HasDefaultValue("ACTIVE")
                .IsRequired();
        });

        builder.Property(e => e.CreatedBy)
            .HasColumnName("CREATED_BY")
            .IsRequired();

        builder.Property(e => e.CreatedOn)
            .HasColumnName("CREATED_ON")
            .HasPrecision(3)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("UPDATED_BY");

        builder.Property(e => e.UpdatedOn)
            .HasColumnName("UPDATED_ON")
            .HasPrecision(3);

        builder.Property(e => e.Version)
            .HasColumnName("VERSION")
            .IsConcurrencyToken();

        builder.HasIndex(e => e.ParentPageId).HasDatabaseName("IX_WEBSITE_PAGES_PARENT");
    }
}
