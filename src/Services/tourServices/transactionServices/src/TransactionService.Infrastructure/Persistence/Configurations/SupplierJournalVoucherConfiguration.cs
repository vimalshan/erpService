using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Aggregates;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class SupplierJournalVoucherConfiguration : IEntityTypeConfiguration<SupplierJournalVoucher>
{
    public void Configure(EntityTypeBuilder<SupplierJournalVoucher> builder)
    {
        builder.ToTable("JVSUP_MAIN");
        builder.HasKey(x => x.JvId);

        builder.Property(x => x.JvId).HasColumnName("JV_ID").ValueGeneratedNever();
        builder.Property(x => x.JvType).HasColumnName("JV_TYPE").HasMaxLength(10).IsRequired();
        builder.Property(x => x.JvDate).HasColumnName("JV_DATE").IsRequired();
        builder.Property(x => x.JvVendorId).HasColumnName("JV_VENDORID");
        builder.Property(x => x.JvOraRefNo).HasColumnName("JV_ORAREFNO").HasMaxLength(50);
        builder.Property(x => x.JvStatus).HasColumnName("JV_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.JvPayUnitId).HasColumnName("JV_PAYUNITID");
        builder.Property(x => x.JvRefInvNo).HasColumnName("JV_REFINVNO").HasMaxLength(25);
        builder.Property(x => x.JvNetAmt).HasColumnName("JV_NETAMT").HasColumnType("DECIMAL(19,0)");
        builder.Property(x => x.JvTrnType).HasColumnName("JV_TRNTYPE").HasMaxLength(3);
        builder.Property(x => x.JvOraVendorId).HasColumnName("JV_ORAVENDORID");
        builder.Property(x => x.JvAdminId).HasColumnName("JV_ADMINID");
        builder.Property(x => x.JvInvBatchId).HasColumnName("JV_INVBATCHID");
        builder.Property(x => x.JvOraSiteId).HasColumnName("JV_ORASITEID");
        builder.Property(x => x.JvCenvatApplicable).HasColumnName("JV_CENVATAPPLICABLE").HasMaxLength(1);
        builder.Property(x => x.JvDocKeyNo).HasColumnName("JV_DOCKEYNO").HasMaxLength(100);
        builder.Property(x => x.CreatedBy).HasColumnName("JV_CREATEDBY");
        builder.Property(x => x.CreatedOn).HasColumnName("JV_CREATEDON");

        builder.HasMany(x => x.Lines)
            .WithOne()
            .HasForeignKey(x => x.JvId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.ModifiedBy);
        builder.Ignore(x => x.ModifiedOn);
    }
}
