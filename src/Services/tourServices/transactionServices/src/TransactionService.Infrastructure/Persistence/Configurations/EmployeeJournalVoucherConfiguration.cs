using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Aggregates;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class EmployeeJournalVoucherConfiguration : IEntityTypeConfiguration<EmployeeJournalVoucher>
{
    public void Configure(EntityTypeBuilder<EmployeeJournalVoucher> builder)
    {
        builder.ToTable("JVEMP_MAIN");
        builder.HasKey(x => x.JvBatchId);

        builder.Property(x => x.JvBatchId).HasColumnName("JV_BATCHID").ValueGeneratedNever();
        builder.Property(x => x.JvTpId).HasColumnName("JV_TPID");
        builder.Property(x => x.JvType).HasColumnName("JV_TYPE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.JvDate).HasColumnName("JV_DATE").IsRequired();
        builder.Property(x => x.JvEmpSysId).HasColumnName("JV_EMPSYSID").IsRequired();
        builder.Property(x => x.JvStatus).HasColumnName("JV_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.JvTrnType).HasColumnName("JV_TRNTYPE").HasMaxLength(3).IsRequired();
        builder.Property(x => x.JvOraRefNo).HasColumnName("JV_ORAREFNO").HasMaxLength(50);
        builder.Property(x => x.JvNetAmt).HasColumnName("JV_NETAMT").HasColumnType("DECIMAL(19,0)");
        builder.Property(x => x.JvPayUnitId).HasColumnName("JV_PAYUNITID");
        builder.Property(x => x.JvTrnRefNo).HasColumnName("JV_TRNREFNO");
        builder.Property(x => x.CreatedBy).HasColumnName("JV_CREATEDBY");
        builder.Property(x => x.CreatedOn).HasColumnName("JV_CREATEDON");

        builder.HasMany(x => x.Lines)
            .WithOne()
            .HasForeignKey(x => x.JvBatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.ModifiedBy);
        builder.Ignore(x => x.ModifiedOn);
    }
}
