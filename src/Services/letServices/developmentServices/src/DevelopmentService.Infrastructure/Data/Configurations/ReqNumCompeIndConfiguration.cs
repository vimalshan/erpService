using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Infrastructure.Data.Configurations;

public class ReqNumCompeIndConfiguration : IEntityTypeConfiguration<ReqNumCompeInd>
{
    public void Configure(EntityTypeBuilder<ReqNumCompeInd> builder)
    {
        builder.ToTable("DD_REQNUM_COMPE_IND");
        builder.HasNoKey();
        builder.Property(x => x.ReqNum).HasColumnName("REQNUM");
        builder.Property(x => x.CompNum).HasColumnName("COMPNUM").HasColumnType("decimal(38,0)");
        builder.Property(x => x.IndNum).HasColumnName("INDNUM").HasColumnType("decimal(38,0)");
        builder.Property(x => x.Flag).HasColumnName("FLAG").HasMaxLength(1);
        builder.Property(x => x.PinNum).HasColumnName("PINNUM").HasColumnType("decimal(38,0)");
    }
}
