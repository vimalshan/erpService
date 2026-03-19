using MenuAndSecurityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuAndSecurityService.Infrastructure.Persistence.Configurations;

public class RoleMenuAccessConfiguration : IEntityTypeConfiguration<RoleMenuAccess>
{
    public void Configure(EntityTypeBuilder<RoleMenuAccess> builder)
    {
        builder.ToTable("ROLE_MENUACCESS");

        builder.HasKey(r => r.MenuAccessId);

        builder.Property(r => r.MenuAccessId).HasColumnName("MENU_ACCESSID").ValueGeneratedNever();
        builder.Property(r => r.MenuId).HasColumnName("MENU_ID").IsRequired();
        builder.Property(r => r.MenuRoleId).HasColumnName("MENU_ROLEID").IsRequired();
        builder.Property(r => r.RoleModifiedBy).HasColumnName("ROLE_MODIFIEDBY");
        builder.Property(r => r.RoleModifiedOn).HasColumnName("ROLE_MODIFIEDON").HasPrecision(3);

        builder.HasOne(r => r.Menu)
            .WithMany(m => m.RoleMenuAccesses)
            .HasForeignKey(r => r.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(r => r.DomainEvents);
    }
}
