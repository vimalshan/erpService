namespace WebsiteContentService.Infrastructure.Persistence.EntityConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteContentService.Domain.Entities;
using WebsiteContentService.Domain.ValueObjects;

public class WebsiteNewsConfiguration : IEntityTypeConfiguration<WebsiteNews>
{
    public void Configure(EntityTypeBuilder<WebsiteNews> builder)
    {
        builder.ToTable("WEBSITE_NEWS");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("NEWS_ID")
            .UseIdentityColumn();

        builder.Property(e => e.NewsTitle)
            .HasColumnName("NEWS_TITLE")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.NewsContent)
            .HasColumnName("NEWS_CONTENT")
            .IsRequired();

        builder.Property(e => e.NewsSummary)
            .HasColumnName("NEWS_SUMMARY")
            .HasMaxLength(500);

        builder.Property(e => e.NewsCategory)
            .HasColumnName("NEWS_CATEGORY")
            .HasMaxLength(100);

        builder.Property(e => e.FeaturedImage)
            .HasColumnName("FEATURED_IMAGE")
            .HasMaxLength(500);

        builder.OwnsOne(e => e.IsFeatured, f =>
        {
            f.Property(p => p.Value)
                .HasColumnName("IS_FEATURED")
                .HasMaxLength(1)
                .HasDefaultValue('N')
                .IsRequired();
        });

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

        builder.Property(e => e.PublishStartDate)
            .HasColumnName("PUBLISH_START_DATE")
            .HasPrecision(3);

        builder.Property(e => e.PublishEndDate)
            .HasColumnName("PUBLISH_END_DATE")
            .HasPrecision(3);

        builder.OwnsOne(e => e.NewsStatus, ns =>
        {
            ns.Property(p => p.Value)
                .HasColumnName("NEWS_STATUS")
                .HasMaxLength(20)
                .HasDefaultValue("DRAFT")
                .IsRequired();
        });

        builder.Property(e => e.ViewCount)
            .HasColumnName("VIEW_COUNT")
            .HasDefaultValue(0);

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

        builder.HasIndex(e => e.NewsCategory).HasDatabaseName("IX_WEBSITE_NEWS_CATEGORY");
        builder.HasIndex(e => e.PublishedDate).HasDatabaseName("IX_WEBSITE_NEWS_DATE");
    }
}
