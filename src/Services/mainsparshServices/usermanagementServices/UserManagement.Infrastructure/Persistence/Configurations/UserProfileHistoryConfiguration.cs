using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class UserProfileHistoryConfiguration : IEntityTypeConfiguration<UserProfileHistory>
{
    public void Configure(EntityTypeBuilder<UserProfileHistory> builder)
    {
        builder.ToTable("USER_PROFILEHIST");

        builder.HasKey(h => h.HistId);
        builder.Property(h => h.HistId).HasColumnName("HIST_ID").UseIdentityColumn();
        builder.Property(h => h.PolicyId).HasColumnName("POLICY_ID").IsRequired();
        builder.Property(h => h.UserSysId).HasColumnName("USER_SYSID").IsRequired();
        builder.Property(h => h.ProfileField).HasColumnName("PROFILE_FIELD").HasColumnType("VARCHAR(100)");
        builder.Property(h => h.OldValue).HasColumnName("OLD_VALUE").HasColumnType("NVARCHAR(500)");
        builder.Property(h => h.NewValue).HasColumnName("NEW_VALUE").HasColumnType("NVARCHAR(500)");
        builder.Property(h => h.ChangeReason).HasColumnName("CHAR_REASON").HasColumnType("VARCHAR(500)");
        builder.Property(h => h.ChangedBy).HasColumnName("CHANGED_BY").IsRequired();
        builder.Property(h => h.ChangedOn).HasColumnName("CHANGED_ON").HasColumnType("DATETIME2(3)").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(h => h.PolicyId).HasDatabaseName("IX_USER_PROFILEHIST_POLICY_ID");
        builder.HasIndex(h => h.UserSysId).HasDatabaseName("IX_USER_PROFILEHIST_USER_SYSID");
        builder.HasIndex(h => h.ChangedOn).HasDatabaseName("IX_USER_PROFILEHIST_DATE");

        builder.Ignore(h => h.DomainEvents);
    }
}
