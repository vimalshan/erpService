using ComplaintService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintService.Infrastructure.Persistence.Configurations;

public class ComplaintGroupConfiguration : IEntityTypeConfiguration<ComplaintGroup>
{
    public void Configure(EntityTypeBuilder<ComplaintGroup> builder)
    {
        builder.ToTable("COMPL_MAIN");
        builder.HasKey(x => x.GroupId);

        builder.Property(x => x.UnitCode).HasColumnName("CM_UNIT_CODE").HasMaxLength(3).IsRequired().IsFixedLength();
        builder.Property(x => x.GroupId).HasColumnName("CM_GROUPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.GroupName).HasColumnName("CM_GROUP_NAME").HasMaxLength(2000).IsRequired();
        builder.Property(x => x.GroupDesc).HasColumnName("CM_GROUP_DESC").HasMaxLength(2000);
        builder.Property(x => x.GroupSrc).HasColumnName("CM_GROUP_SRC").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.BehalfFlag).HasColumnName("CM_BEHALF_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(x => x.BehalfPin).HasColumnName("CM_BEHALF_PIN").HasColumnType("decimal(38,0)");
        builder.Property(x => x.RegPin).HasColumnName("CM_REG_PIN").HasColumnType("decimal(38,0)");
        builder.Property(x => x.Shift).HasColumnName("CM_SHIFT").HasMaxLength(255);
        builder.Property(x => x.Mail).HasColumnName("CM_MAIL").HasMaxLength(255);
        builder.Property(x => x.Submit).HasColumnName("CM_SUBMIT").HasMaxLength(255);
        builder.Property(x => x.RegDate).HasColumnName("CM_REG_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.UpdatedBy).HasColumnName("CM_UPDATEDBY").HasMaxLength(255);
        builder.Property(x => x.UpdatedOn).HasColumnName("CM_UPDATEDON").HasColumnType("datetime2(3)");

        builder.Ignore(x => x.DomainEvents);
        builder.HasMany(x => x.Tickets).WithOne().HasForeignKey("GroupId").HasPrincipalKey(x => x.GroupSrc);
    }
}
