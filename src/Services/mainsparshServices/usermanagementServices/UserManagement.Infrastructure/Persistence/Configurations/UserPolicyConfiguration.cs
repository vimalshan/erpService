using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class UserPolicyConfiguration : IEntityTypeConfiguration<UserPolicy>
{
    public void Configure(EntityTypeBuilder<UserPolicy> builder)
    {
        builder.ToTable("USER_POLICY");

        builder.HasKey(p => p.PolicyId);
        builder.Property(p => p.PolicyId).HasColumnName("POLICY_ID").UseIdentityColumn();
        builder.Property(p => p.UserSysId).HasColumnName("USER_SYSID").IsRequired();
        builder.Property(p => p.PolicyCode).HasColumnName("POLICY_CODE").HasColumnType("VARCHAR(50)").IsRequired();
        builder.Property(p => p.PolicyType).HasColumnName("POLICY_TYPE").HasColumnType("VARCHAR(100)");
        builder.Property(p => p.DataRetentionDays).HasColumnName("DATA_RETENTION_DAYS");
        builder.Property(p => p.SessionTimeoutMins).HasColumnName("SESSION_TIMEOUT_MINS");
        builder.Property(p => p.MaxLoginAttempts).HasColumnName("MAX_LOGIN_ATTEMPTS");
        builder.Property(p => p.PolicyStatus).HasColumnName("POLICY_STATUS").HasColumnType("CHAR(1)").HasDefaultValue('A');
        builder.Property(p => p.EffectiveFrom).HasColumnName("EFFECTIVE_FROM").IsRequired();
        builder.Property(p => p.EffectiveTo).HasColumnName("EFFECTIVE_TO");
        builder.Property(p => p.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(p => p.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("DATETIME2(3)").HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(p => p.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("DATETIME2(3)");

        builder.HasIndex(p => p.UserSysId).IsUnique().HasDatabaseName("UQ_USER_POLICY_USER_SYSID");
        builder.HasIndex(p => p.PolicyStatus).HasDatabaseName("IX_USER_POLICY_STATUS");
        builder.HasIndex(p => p.PolicyType).HasDatabaseName("IX_USER_POLICY_TYPE");

        builder.HasMany(p => p.ProfileHistories)
               .WithOne(h => h.Policy)
               .HasForeignKey(h => h.PolicyId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(p => p.DomainEvents);
    }
}
