namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

public sealed class CategoryDefaultConfiguration : IEntityTypeConfiguration<CategoryDefault>
{
    public void Configure(EntityTypeBuilder<CategoryDefault> builder)
    {
        builder.ToTable("SP_CATEGORY_DEFAULT");
        builder.HasKey(c => new { c.StationeryId, c.CategoryId, c.LocationId });
        builder.Property(c => c.StationeryId).HasColumnName("CD_STATIONERYID");
        builder.Property(c => c.CategoryId).HasColumnName("CD_CATEGORYID");
        builder.Property(c => c.LocationId).HasColumnName("CD_LOCATIONID");
        builder.Property(c => c.ModifiedBy).HasColumnName("CD_MODIFIEDBY");
        builder.Property(c => c.ModifiedOn).HasColumnName("CD_MODIFIEDON");

        builder.Ignore(c => c.DomainEvents);
    }
}
