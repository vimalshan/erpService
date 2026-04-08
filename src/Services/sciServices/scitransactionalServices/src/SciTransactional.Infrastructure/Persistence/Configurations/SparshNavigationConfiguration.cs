using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Infrastructure.Persistence.Configurations;

public sealed class SparshNavigationConfiguration : IEntityTypeConfiguration<SparshNavigationEntity>
{
    public void Configure(EntityTypeBuilder<SparshNavigationEntity> builder)
    {
        builder.ToTable("SPARSH_NAVIGATION");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("SN_REQ_NUM").ValueGeneratedNever();
        builder.Property(e => e.UserId).HasColumnName("SN_USR_ID").HasMaxLength(25).IsRequired();
        builder.Property(e => e.UserNum).HasColumnName("SN_USR_NUM").IsRequired();
        builder.Property(e => e.RandomNum).HasColumnName("SN_RAN_NUM").HasMaxLength(25);
        builder.Property(e => e.UpdatedDate).HasColumnName("SN_UPD_DAT").HasPrecision(3).IsRequired();
        builder.Property(e => e.SciId).HasColumnName("SN_SCI_ID").HasMaxLength(1).IsRequired();
        builder.Property(e => e.StatusFlag).HasColumnName("SN_STS_FLG").HasMaxLength(1);

        builder.Ignore(e => e.DomainEvents);

        builder.HasData(
            new { Id = 1L, UserId = "ADMIN", UserNum = 1L, RandomNum = "RND001",
                UpdatedDate = new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc),
                SciId = "Y", StatusFlag = "A" },
            new { Id = 2L, UserId = "USER01", UserNum = 2L, RandomNum = "RND002",
                UpdatedDate = new DateTime(2026, 3, 18, 0, 0, 0, DateTimeKind.Utc),
                SciId = "N", StatusFlag = "P" },
            new { Id = 3L, UserId = "ADMIN", UserNum = 1L, RandomNum = "RND003",
                UpdatedDate = new DateTime(2026, 3, 19, 0, 0, 0, DateTimeKind.Utc),
                SciId = "Y", StatusFlag = "C" }
        );
    }
}
