using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Infrastructure.Data.Configurations;

public class CompetencyIndConfiguration : IEntityTypeConfiguration<CompetencyInd>
{
    public void Configure(EntityTypeBuilder<CompetencyInd> builder)
    {
        builder.ToTable("DD_COMPETENCY_IND");
        builder.HasNoKey();
        builder.Property(x => x.SrlNo).HasColumnName("SRL_NO").HasColumnType("decimal(38,0)");
        builder.Property(x => x.Band).HasColumnName("BAND").HasMaxLength(50);
        builder.Property(x => x.CompNum).HasColumnName("COMP_NUM");
        builder.Property(x => x.IndFlag).HasColumnName("IND_FLAG").HasMaxLength(1);
        builder.Property(x => x.IndDefn).HasColumnName("IND_DEFN").HasMaxLength(4000);

        builder.HasIndex(x => x.CompNum).HasDatabaseName("IDX_DD_COMPETENCY_COMPNUM");
    }
}
