using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SciTransactional.Domain.Entities;

namespace SciTransactional.Infrastructure.Persistence.Configurations;

public sealed class NormsMainConfiguration : IEntityTypeConfiguration<NormsMainEntity>
{
    public void Configure(EntityTypeBuilder<NormsMainEntity> builder)
    {
        builder.ToTable("NORMS_MAIN");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("NORM_NO").ValueGeneratedOnAdd();
        builder.Property(e => e.EffectiveDate).HasColumnName("NORM_EFF_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.ClosureDate).HasColumnName("NORM_CLS_DATE").HasPrecision(3);

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Details);

        builder.HasData(
            new { Id = 1L, EffectiveDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ClosureDate = (DateTime?)null },
            new { Id = 2L, EffectiveDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                ClosureDate = new DateTime?(new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc)) },
            new { Id = 3L, EffectiveDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                ClosureDate = (DateTime?)null }
        );
    }
}

public sealed class NormsMasterConfiguration : IEntityTypeConfiguration<NormsMasterEntity>
{
    public void Configure(EntityTypeBuilder<NormsMasterEntity> builder)
    {
        builder.ToTable("NORMS_MASTER");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("NORM_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.InputCode).HasColumnName("NORM_INPUT_CODE");
        builder.Property(e => e.OutputCode).HasColumnName("NORM_OUTPUT_CODE");
        builder.Property(e => e.Rate).HasColumnName("NORM_RATE");
        builder.Property(e => e.NormNo).HasColumnName("NORM_NO");

        builder.Ignore(e => e.DomainEvents);

        builder.HasData(
            new { Id = 101L, InputCode = (int?)1001, OutputCode = (int?)2001, Rate = (int?)100, NormNo = (long?)1L },
            new { Id = 102L, InputCode = (int?)1002, OutputCode = (int?)2002, Rate = (int?)150, NormNo = (long?)1L },
            new { Id = 201L, InputCode = (int?)1003, OutputCode = (int?)2003, Rate = (int?)200, NormNo = (long?)2L },
            new { Id = 301L, InputCode = (int?)1004, OutputCode = (int?)2004, Rate = (int?)175, NormNo = (long?)3L }
        );
    }
}
