using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GroupManagementService.Domain.Entities;
using GroupManagementService.Domain.ValueObjects;

namespace GroupManagementService.Infrastructure.Persistence.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("GROUP_MAST");

            builder.HasKey(x => x.Id)
                .HasName("PK_GROUP_MAST");

            builder.Property(x => x.Id)
                .HasColumnName("GROUP_ID")
                .IsRequired();

            builder.Property(x => x.Code)
                .HasColumnName("GROUP_CODE")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("GROUP_NAME")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("GROUP_DESC")
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.Status)
                .HasColumnName("GROUP_STATUS")
                .HasConversion(
                    v => v == GroupStatus.Active ? 'A' : 'I',
                    v => v == 'A' ? GroupStatus.Active : GroupStatus.Inactive)
                .HasMaxLength(1)
                .IsRequired();

            builder.Property(x => x.IsAdmin)
                .HasColumnName("IS_ADMIN")
                .HasConversion(
                    v => v ? 'Y' : 'N',
                    v => v == 'Y')
                .HasColumnType("char(1)")
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CREATED_BY")
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasColumnName("CREATED_ON")
                .HasColumnType("datetime2(3)")
                .IsRequired();

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            builder.Property(x => x.UpdatedOn)
                .HasColumnName("UPDATED_ON")
                .HasColumnType("datetime2(3)");

            builder.HasIndex(x => x.Code)
                .HasDatabaseName("IX_GROUP_MAST_CODE")
                .IsUnique();

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_GROUP_MAST_STATUS");

            builder.HasIndex(x => x.IsAdmin)
                .HasDatabaseName("IX_GROUP_MAST_IS_ADMIN");

            builder.HasMany<GroupMenuMap>()
                .WithOne()
                .HasForeignKey("GroupId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class GroupMenuMapConfiguration : IEntityTypeConfiguration<GroupMenuMap>
    {
        public void Configure(EntityTypeBuilder<GroupMenuMap> builder)
        {
            builder.ToTable("GROUP_MENUMAP");

            builder.HasKey(x => x.Id)
                .HasName("PK_GROUP_MENUMAP");

            builder.Property(x => x.Id)
                .HasColumnName("MENUMAP_ID")
                .IsRequired();

            builder.Property(x => x.GroupId)
                .HasColumnName("GROUP_ID")
                .IsRequired();

            builder.Property(x => x.MenuCode)
                .HasColumnName("MENU_CODE")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.MenuName)
                .HasColumnName("MENU_NAME")
                .HasMaxLength(255)
                .IsRequired();

            builder.OwnsOne(x => x.Permissions, pb =>
            {
                pb.Property(p => p.CanView)
                    .HasColumnName("CAN_VIEW")
                    .HasDefaultValue(true);

                pb.Property(p => p.CanCreate)
                    .HasColumnName("CAN_CREATE")
                    .HasDefaultValue(false);

                pb.Property(p => p.CanEdit)
                    .HasColumnName("CAN_EDIT")
                    .HasDefaultValue(false);

                pb.Property(p => p.CanDelete)
                    .HasColumnName("CAN_DELETE")
                    .HasDefaultValue(false);

                pb.Property(p => p.CanApprove)
                    .HasColumnName("CAN_APPROVE")
                    .HasDefaultValue(false);
            });

            builder.Property(x => x.MenuSequence)
                .HasColumnName("MENU_SEQUENCE");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CREATED_BY")
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasColumnName("CREATED_ON")
                .HasColumnType("datetime2(3)")
                .IsRequired();

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            builder.Property(x => x.UpdatedOn)
                .HasColumnName("UPDATED_ON")
                .HasColumnType("datetime2(3)");

            builder.HasIndex(x => x.GroupId)
                .HasDatabaseName("IX_GROUP_MENUMAP_GROUP_ID");

            builder.HasIndex(x => x.MenuCode)
                .HasDatabaseName("IX_GROUP_MENUMAP_MENU");

            builder.HasIndex(x => new { x.GroupId, x.MenuCode })
                .HasDatabaseName("UC_GROUP_MENU")
                .IsUnique();

            builder.HasOne<Group>()
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .HasConstraintName("FK_GROUP_MENUMAP_GROUP");
        }
    }
}
