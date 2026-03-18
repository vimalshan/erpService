using LovService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LovService.Infrastructure.Data.Configurations;

public class ItemDataConfiguration : IEntityTypeConfiguration<ItemData>
{
    public void Configure(EntityTypeBuilder<ItemData> builder)
    {
        builder.ToTable("ITEMDATA");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(x => x.CatName).HasColumnName("CATNAME").HasMaxLength(40);
        builder.Property(x => x.ItemName).HasColumnName("ITEMNAME").HasMaxLength(60);
        builder.Property(x => x.Make).HasColumnName("MAKE").HasMaxLength(30);
        builder.Property(x => x.Uom).HasColumnName("UOM").HasMaxLength(20);
        builder.Property(x => x.Price).HasColumnName("PRICE");
    }
}
