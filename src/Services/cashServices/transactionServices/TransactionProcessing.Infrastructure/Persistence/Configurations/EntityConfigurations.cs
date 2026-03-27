using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionProcessing.Domain.Entities;

namespace TransactionProcessing.Infrastructure.Persistence.Configurations;

public sealed class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.ToTable("FINANCIAL_TRANSACTION");
        builder.HasKey(e => e.TxnId);
        builder.Property(e => e.TxnId).HasColumnName("TXN_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.TxnBatchId).HasColumnName("TXN_BATCH_ID");
        builder.Property(e => e.TxnType).HasColumnName("TXN_TYPE").HasMaxLength(50).IsRequired();
        builder.Property(e => e.TxnSubType).HasColumnName("TXN_SUB_TYPE").HasMaxLength(50);
        builder.Property(e => e.TxnAmount).HasColumnName("TXN_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.TxnCurrencyId).HasColumnName("TXN_CURRENCY_ID");
        builder.Property(e => e.TxnExchangeRate).HasColumnName("TXN_EXCHANGE_RATE").HasColumnType("decimal(18,8)");
        builder.Property(e => e.TxnBaseAmount).HasColumnName("TXN_BASE_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.TxnReference).HasColumnName("TXN_REFERENCE").HasMaxLength(100);
        builder.Property(e => e.TxnSourceService).HasColumnName("TXN_SOURCE_SERVICE").HasMaxLength(100).IsRequired();
        builder.Property(e => e.TxnSourceId).HasColumnName("TXN_SOURCE_ID");
        builder.Property(e => e.TxnStatus).HasColumnName("TXN_STATUS").HasMaxLength(20).IsRequired();
        builder.Property(e => e.TxnRemarks).HasColumnName("TXN_REMARKS").HasMaxLength(500);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
        builder.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(e => e.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasOne(e => e.Batch).WithMany(b => b.Transactions).HasForeignKey(e => e.TxnBatchId);
        builder.HasMany(e => e.Audits).WithOne().HasForeignKey(a => a.TxnId);

        builder.HasIndex(e => e.TxnBatchId).HasDatabaseName("IX_FINANCIAL_TRANSACTION_BATCH");
        builder.HasIndex(e => e.TxnStatus).HasDatabaseName("IX_FINANCIAL_TRANSACTION_STATUS");
        builder.HasIndex(e => e.CreatedOn).HasDatabaseName("IX_FINANCIAL_TRANSACTION_DATE");

        builder.Ignore(e => e.Id);
        builder.Ignore(e => e.DomainEvents);
    }
}

public sealed class TransactionBatchConfiguration : IEntityTypeConfiguration<TransactionBatch>
{
    public void Configure(EntityTypeBuilder<TransactionBatch> builder)
    {
        builder.ToTable("TRANSACTION_BATCH");
        builder.HasKey(e => e.BatchId);
        builder.Property(e => e.BatchId).HasColumnName("BATCH_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.BatchType).HasColumnName("BATCH_TYPE").HasMaxLength(50).IsRequired();
        builder.Property(e => e.BatchDate).HasColumnName("BATCH_DATE");
        builder.Property(e => e.BatchStatus).HasColumnName("BATCH_STATUS").HasMaxLength(20).IsRequired();
        builder.Property(e => e.BatchTotalCount).HasColumnName("BATCH_TOTAL_COUNT");
        builder.Property(e => e.BatchSuccessCount).HasColumnName("BATCH_SUCCESS_COUNT");
        builder.Property(e => e.BatchFailureCount).HasColumnName("BATCH_FAILURE_COUNT");
        builder.Property(e => e.BatchTotalAmount).HasColumnName("BATCH_TOTAL_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
        builder.Property(e => e.CompletedOn).HasColumnName("COMPLETED_ON");

        builder.HasIndex(e => e.BatchStatus).HasDatabaseName("IX_TRANSACTION_BATCH_STATUS");

        builder.Ignore(e => e.Id);
        builder.Ignore(e => e.DomainEvents);
    }
}

public sealed class DealSettlementConfiguration : IEntityTypeConfiguration<DealSettlement>
{
    public void Configure(EntityTypeBuilder<DealSettlement> builder)
    {
        builder.ToTable("DEAL_SETTLEMENT_PROC");
        builder.HasKey(e => e.SettlementId);
        builder.Property(e => e.SettlementId).HasColumnName("SETTLEMENT_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.TxnId).HasColumnName("TXN_ID");
        builder.Property(e => e.DealId).HasColumnName("DEAL_ID");
        builder.Property(e => e.SetId).HasColumnName("SET_ID");
        builder.Property(e => e.SettlementType).HasColumnName("SETTLEMENT_TYPE").HasMaxLength(1).IsRequired();
        builder.Property(e => e.SpotRate).HasColumnName("SPOT_RATE").HasColumnType("decimal(18,8)");
        builder.Property(e => e.ExchangeRate).HasColumnName("EXCHANGE_RATE").HasColumnType("decimal(18,8)");
        builder.Property(e => e.SettlementAmount).HasColumnName("SETTLEMENT_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.GainLossAmount).HasColumnName("GAIN_LOSS_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.PremiumAmount).HasColumnName("PREMIUM_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.WindingFee).HasColumnName("WINDING_FEE").HasColumnType("decimal(18,4)");
        builder.Property(e => e.NetAmount).HasColumnName("NET_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.BankAccountId).HasColumnName("BANK_ACCOUNT_ID");
        builder.Property(e => e.ProcessingStatus).HasColumnName("PROCESSING_STATUS").HasMaxLength(20).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");

        builder.HasOne(e => e.Transaction).WithOne(t => t.DealSettlement).HasForeignKey<DealSettlement>(e => e.TxnId);

        builder.HasIndex(e => e.DealId).HasDatabaseName("IX_DEAL_SETTLEMENT_PROC_DEAL");

        builder.Ignore(e => e.DomainEvents);
    }
}

public sealed class LoanDisbursementConfiguration : IEntityTypeConfiguration<LoanDisbursement>
{
    public void Configure(EntityTypeBuilder<LoanDisbursement> builder)
    {
        builder.ToTable("LOAN_DISBURSEMENT_PROC");
        builder.HasKey(e => e.DisbProcId);
        builder.Property(e => e.DisbProcId).HasColumnName("DISB_PROC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.TxnId).HasColumnName("TXN_ID");
        builder.Property(e => e.LoanId).HasColumnName("LOAN_ID");
        builder.Property(e => e.DisbId).HasColumnName("DISB_ID");
        builder.Property(e => e.DisbAmount).HasColumnName("DISB_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.ExchangeRate).HasColumnName("EXCHANGE_RATE").HasColumnType("decimal(18,8)");
        builder.Property(e => e.ConvertedAmount).HasColumnName("CONVERTED_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.BankAccountId).HasColumnName("BANK_ACCOUNT_ID");
        builder.Property(e => e.ProcessingStatus).HasColumnName("PROCESSING_STATUS").HasMaxLength(20).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");

        builder.HasOne(e => e.Transaction).WithOne(t => t.LoanDisbursement).HasForeignKey<LoanDisbursement>(e => e.TxnId);

        builder.HasIndex(e => e.LoanId).HasDatabaseName("IX_LOAN_DISBURSEMENT_PROC_LOAN");

        builder.Ignore(e => e.DomainEvents);
    }
}

public sealed class LoanRepaymentConfiguration : IEntityTypeConfiguration<LoanRepayment>
{
    public void Configure(EntityTypeBuilder<LoanRepayment> builder)
    {
        builder.ToTable("LOAN_REPAYMENT_PROC");
        builder.HasKey(e => e.RepayProcId);
        builder.Property(e => e.RepayProcId).HasColumnName("REPAY_PROC_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.TxnId).HasColumnName("TXN_ID");
        builder.Property(e => e.LoanId).HasColumnName("LOAN_ID");
        builder.Property(e => e.RepayId).HasColumnName("REPAY_ID");
        builder.Property(e => e.RepayAmount).HasColumnName("REPAY_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.ExchangeRate).HasColumnName("EXCHANGE_RATE").HasColumnType("decimal(18,8)");
        builder.Property(e => e.ConvertedAmount).HasColumnName("CONVERTED_AMOUNT").HasColumnType("decimal(18,4)");
        builder.Property(e => e.BankAccountId).HasColumnName("BANK_ACCOUNT_ID");
        builder.Property(e => e.ProcessingStatus).HasColumnName("PROCESSING_STATUS").HasMaxLength(20).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");

        builder.HasOne(e => e.Transaction).WithOne(t => t.LoanRepayment).HasForeignKey<LoanRepayment>(e => e.TxnId);

        builder.HasIndex(e => e.LoanId).HasDatabaseName("IX_LOAN_REPAYMENT_PROC_LOAN");

        builder.Ignore(e => e.DomainEvents);
    }
}

public sealed class TransactionAuditConfiguration : IEntityTypeConfiguration<TransactionAudit>
{
    public void Configure(EntityTypeBuilder<TransactionAudit> builder)
    {
        builder.ToTable("TRANSACTION_AUDIT");
        builder.HasKey(e => e.AuditId);
        builder.Property(e => e.AuditId).HasColumnName("AUDIT_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.TxnId).HasColumnName("TXN_ID");
        builder.Property(e => e.PreviousStatus).HasColumnName("PREVIOUS_STATUS").HasMaxLength(20).IsRequired();
        builder.Property(e => e.NewStatus).HasColumnName("NEW_STATUS").HasMaxLength(20).IsRequired();
        builder.Property(e => e.AuditAction).HasColumnName("AUDIT_ACTION").HasMaxLength(200).IsRequired();
        builder.Property(e => e.AuditRemarks).HasColumnName("AUDIT_REMARKS").HasMaxLength(500);
        builder.Property(e => e.AuditBy).HasColumnName("AUDIT_BY");
        builder.Property(e => e.AuditOn).HasColumnName("AUDIT_ON");

        builder.HasIndex(e => e.TxnId).HasDatabaseName("IX_TRANSACTION_AUDIT_TXN");

        builder.Ignore(e => e.DomainEvents);
    }
}
