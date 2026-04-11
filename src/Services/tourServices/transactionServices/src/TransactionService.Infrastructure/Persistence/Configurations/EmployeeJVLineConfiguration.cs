using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class EmployeeJVLineConfiguration : IEntityTypeConfiguration<EmployeeJVLine>
{
    public void Configure(EntityTypeBuilder<EmployeeJVLine> builder)
    {
        builder.ToTable("JVEMP_SUB");
        builder.HasKey(x => x.JvSubId);

        builder.Property(x => x.JvSubId).HasColumnName("JV_SUBID").ValueGeneratedNever();
        builder.Property(x => x.JvBatchId).HasColumnName("JV_BATCHID");
        builder.Property(x => x.JvBu).HasColumnName("JV_BU").HasMaxLength(25);
        builder.Property(x => x.JvAcCode).HasColumnName("JV_ACCODE").HasMaxLength(25);
        builder.Property(x => x.JvSubAcc).HasColumnName("JV_SUBACC").HasMaxLength(25);
        builder.Property(x => x.JvCcCode).HasColumnName("JV_CCCODE").HasMaxLength(25);
        builder.Property(x => x.JvProduct).HasColumnName("JV_PRODUCT").HasMaxLength(25);
        builder.Property(x => x.JvDcFlag).HasColumnName("JV_DCFLAG").HasMaxLength(25);
        builder.Property(x => x.JvTrnAmt).HasColumnName("JV_TRNAMT").HasMaxLength(25);
        builder.Property(x => x.JvIutaBu).HasColumnName("JV_IUTABU").HasMaxLength(25);
        builder.Property(x => x.JvLoc).HasColumnName("JV_LOC").HasMaxLength(25);
        builder.Property(x => x.JvRemarks).HasColumnName("JV_REMARKS").HasMaxLength(100);
        builder.Property(x => x.JvLineFlag).HasColumnName("JV_LINEFLAG").HasMaxLength(1);
        builder.Property(x => x.JvCombinationId).HasColumnName("JV_COMBINATIONID").HasMaxLength(200);
        builder.Property(x => x.JvSubType).HasColumnName("JV_SUBTYPE").HasMaxLength(3);
        builder.Property(x => x.JvCombinationCode).HasColumnName("JV_COMBINATIONCODE").HasMaxLength(207);

        builder.Ignore(x => x.DomainEvents);
    }
}
