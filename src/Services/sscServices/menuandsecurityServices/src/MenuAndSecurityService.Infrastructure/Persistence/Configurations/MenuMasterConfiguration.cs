using MenuAndSecurityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuAndSecurityService.Infrastructure.Persistence.Configurations;

public class MenuMasterConfiguration : IEntityTypeConfiguration<MenuMaster>
{
    public void Configure(EntityTypeBuilder<MenuMaster> builder)
    {
        builder.ToTable("MENU_MASTER");

        builder.HasKey(m => m.MenuId);

        builder.Property(m => m.MenuId).HasColumnName("MENU_ID").ValueGeneratedNever();
        builder.Property(m => m.MenuName).HasColumnName("MENU_NAME").HasMaxLength(100).IsRequired();
        builder.Property(m => m.MenuPageName).HasColumnName("MENU_PAGENAME").HasMaxLength(200).IsRequired();
        builder.Property(m => m.MenuParentId).HasColumnName("MENU_PARENTID");
        builder.Property(m => m.MenuDisplayOrder).HasColumnName("MENU_DISPLAYORDER").IsRequired();
        builder.Property(m => m.ModifiedBy).HasColumnName("MENU_MODIFIEDBY").IsRequired();
        builder.Property(m => m.ModifiedOn).HasColumnName("MENU_MODIFIEDON").HasPrecision(3).IsRequired();

        builder.HasOne(m => m.Parent)
            .WithMany(m => m.Children)
            .HasForeignKey(m => m.MenuParentId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder.Ignore(m => m.DomainEvents);
    }
}
