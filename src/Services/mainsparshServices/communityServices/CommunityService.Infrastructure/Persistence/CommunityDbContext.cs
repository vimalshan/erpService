namespace CommunityService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;

public class CommunityDbContext : DbContext
{
    public DbSet<Community> Communities => Set<Community>();
    public DbSet<CommunityMember> CommunityMembers => Set<CommunityMember>();

    public CommunityDbContext(DbContextOptions<CommunityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Community entity
        modelBuilder.Entity<Community>(builder =>
        {
            builder.ToTable("COMMUNITY_MAST");
            builder.HasKey(c => c.CommunityId);

            builder.Property(c => c.CommunityId)
                .HasColumnName("COMMUNITY_ID")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.CommunityCode)
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.CommunityCode(v))
                .HasColumnName("COMMUNITY_CODE")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.CommunityName)
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.CommunityName(v))
                .HasColumnName("COMMUNITY_NAME")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.CommunityDescription)
                .HasColumnName("COMMUNITY_DESC");

            builder.Property(c => c.CommunityType)
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.CommunityType(v))
                .HasColumnName("COMMUNITY_TYPE")
                .HasMaxLength(50);

            builder.Property(c => c.CommunityIcon)
                .HasColumnName("COMMUNITY_ICON")
                .HasMaxLength(500);

            builder.Property(c => c.CommunityBanner)
                .HasColumnName("COMMUNITY_BANNER")
                .HasMaxLength(500);

            builder.Property(c => c.PrivacyLevel)
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.PrivacyLevel(v))
                .HasColumnName("PRIVACY_LEVEL")
                .HasMaxLength(20)
                .HasDefaultValueSql("'PUBLIC'");

            builder.Property(c => c.OwnerId)
                .HasColumnName("OWNER_ID");

            builder.Property(c => c.ApproverId)
                .HasColumnName("APPROVER_ID");

            builder.Property(c => c.CommunityStatus)
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.CommunityStatus(v))
                .HasColumnName("COMMUNITY_STATUS")
                .HasMaxLength(20)
                .HasDefaultValueSql("'ACTIVE'");

            builder.Property(c => c.MemberCount)
                .HasColumnName("MEMBER_COUNT")
                .HasDefaultValue(0);

            builder.OwnsOne(c => c.AuditInfo, b =>
            {
                b.Property(a => a.CreatedBy).HasColumnName("CREATED_BY");
                b.Property(a => a.CreatedOn).HasColumnName("CREATED_ON").HasDefaultValueSql("GETDATE()");
                b.Property(a => a.UpdatedBy).HasColumnName("UPDATED_BY");
                b.Property(a => a.UpdatedOn).HasColumnName("UPDATED_ON");
            });

            builder.HasIndex(c => c.CommunityCode).IsUnique();
            builder.HasMany(c => c.Members)
                .WithOne()
                .HasForeignKey("COMMUNITY_ID");
        });

        // Configure CommunityMember entity
        modelBuilder.Entity<CommunityMember>(builder =>
        {
            builder.ToTable("COMMUNITY_MEMBERS");
            builder.HasKey(m => m.MemberId);

            builder.Property(m => m.MemberId)
                .HasColumnName("MEMBER_ID")
                .ValueGeneratedOnAdd();

            builder.Property(m => m.CommunityId)
                .HasColumnName("COMMUNITY_ID");

            builder.Property(m => m.UserSysId)
                .HasColumnName("USER_SYSID");

            builder.Property(m => m.MemberRole)
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.MemberRole(v))
                .HasColumnName("MEMBER_ROLE")
                .HasMaxLength(50)
                .HasDefaultValueSql("'MEMBER'");

            builder.Property(m => m.JoinDate)
                .HasColumnName("JOIN_DATE")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(m => m.LeaveDate)
                .HasColumnName("LEAVE_DATE");

            builder.Property(m => m.MemberStatus)
                .HasConversion(v => v.Value, v => new Domain.ValueObjects.MemberStatus(v))
                .HasColumnName("MEMBER_STATUS")
                .HasMaxLength(20)
                .HasDefaultValueSql("'ACTIVE'");

            builder.Property(m => m.ContributionCount)
                .HasColumnName("CONTRIBUTION_COUNT")
                .HasDefaultValue(0);

            builder.OwnsOne(m => m.AuditInfo, b =>
            {
                b.Property(a => a.CreatedBy).HasColumnName("CREATED_BY");
                b.Property(a => a.CreatedOn).HasColumnName("CREATED_ON").HasDefaultValueSql("GETDATE()");
                b.Property(a => a.UpdatedBy).HasColumnName("UPDATED_BY");
                b.Property(a => a.UpdatedOn).HasColumnName("UPDATED_ON");
            });

            builder.HasIndex(m => new { m.CommunityId, m.UserSysId })
                .IsUnique()
                .HasDatabaseName("UC_COMMUNITY_USER");
            builder.HasIndex(m => m.CommunityId).HasDatabaseName("IX_COMMUNITY_MEMBERS_COMMUNITY_ID");
            builder.HasIndex(m => m.UserSysId).HasDatabaseName("IX_COMMUNITY_MEMBERS_USER_SYSID");
            builder.HasIndex(m => m.MemberRole).HasDatabaseName("IX_COMMUNITY_MEMBERS_ROLE");
            builder.HasIndex(m => m.MemberStatus).HasDatabaseName("IX_COMMUNITY_MEMBERS_STATUS");
        });

        // Create indexes on Community
        modelBuilder.Entity<Community>()
            .HasIndex(c => c.CommunityCode).HasDatabaseName("IX_COMMUNITY_MAST_CODE");

        modelBuilder.Entity<Community>()
            .HasIndex(c => c.CommunityType).HasDatabaseName("IX_COMMUNITY_MAST_TYPE");

        modelBuilder.Entity<Community>()
            .HasIndex(c => c.CommunityStatus).HasDatabaseName("IX_COMMUNITY_MAST_STATUS");

        modelBuilder.Entity<Community>()
            .HasIndex(c => c.OwnerId).HasDatabaseName("IX_COMMUNITY_MAST_OWNER");

        modelBuilder.Entity<Community>()
            .HasIndex(c => c.PrivacyLevel).HasDatabaseName("IX_COMMUNITY_MAST_PRIVACY");
    }
}
