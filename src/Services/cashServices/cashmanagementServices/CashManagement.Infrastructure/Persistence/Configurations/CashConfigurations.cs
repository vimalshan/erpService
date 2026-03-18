using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CashManagement.Domain.Entities;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Infrastructure.Persistence.Configurations;

public class CashUnitConfiguration : IEntityTypeConfiguration<CashUnit>
{
    public void Configure(EntityTypeBuilder<CashUnit> builder)
    {
        builder.ToTable("CASH_UNIT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CASH_UNIT_ID").ValueGeneratedNever();
        builder.Property(x => x.Name).HasColumnName("CASH_UNIT_NAME").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Code).HasColumnName("CASH_UNIT_CODE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Location).HasColumnName("CASH_UNIT_LOCATION").HasMaxLength(100);
        builder.Property(x => x.InChargeEmployeeId).HasColumnName("CASH_UNIT_INCHARGE");
        builder.Property(x => x.OpeningBalance).HasColumnName("CASH_UNIT_OPENINGBAL").HasColumnType("decimal(19,0)");
        builder.Property(x => x.Status).HasColumnName("CASH_UNIT_STATUS")
            .HasConversion(v => ((char)(int)v).ToString(), v => (EntityStatus)(int)v[0])
            .HasColumnType("char(1)");
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(x => x.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasMany(x => x.Transactions).WithOne()
            .HasForeignKey(t => t.CashUnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CashTransactionConfiguration : IEntityTypeConfiguration<CashTransaction>
{
    public void Configure(EntityTypeBuilder<CashTransaction> builder)
    {
        builder.ToTable("CASH_TRANSACTION");
        builder.HasKey(x => x.CashTxnId);
        builder.Property(x => x.CashTxnId).HasColumnName("CASH_TXN_ID").UseIdentityColumn();
        builder.Property(x => x.CashUnitId).HasColumnName("CASH_UNIT_ID").IsRequired();
        builder.Property(x => x.TxnType).HasColumnName("CASH_TXN_TYPE")
            .HasConversion(v => ((char)(int)v).ToString(), v => (CashTransactionType)(int)v[0])
            .HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.Amount).HasColumnName("CASH_TXN_AMOUNT").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.Source).HasColumnName("CASH_TXN_SOURCE").HasMaxLength(100);
        builder.Property(x => x.PayeeId).HasColumnName("CASH_TXN_PAYEE_ID");
        builder.Property(x => x.RefNo).HasColumnName("CASH_TXN_REF_NO").HasMaxLength(50);
        builder.Property(x => x.TxnDate).HasColumnName("CASH_TXN_DATE").IsRequired();
        builder.Property(x => x.Remarks).HasColumnName("CASH_TXN_REMARKS").HasMaxLength(500);
        builder.Property(x => x.Status).HasColumnName("CASH_TXN_STATUS")
            .HasConversion(v => ((char)(int)v).ToString(), v => (TransactionStatus)(int)v[0])
            .HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.AuthorizedBy).HasColumnName("AUTHORIZED_BY");
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();

        builder.HasIndex(x => new { x.CashUnitId, x.TxnDate }).HasDatabaseName("IX_CASH_TRANSACTION_UNIT_DATE");
        builder.HasIndex(x => x.TxnType).HasDatabaseName("IX_CASH_TRANSACTION_TYPE");
    }
}
