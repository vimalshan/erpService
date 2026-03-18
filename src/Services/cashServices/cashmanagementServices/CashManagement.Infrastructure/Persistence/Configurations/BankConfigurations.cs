using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CashManagement.Domain.Entities;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Infrastructure.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("BANK_ACCOUNT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BANK_ACCOUNT_ID").ValueGeneratedNever();
        builder.Property(x => x.BankName).HasColumnName("BANK_NAME").HasMaxLength(100).IsRequired();
        builder.Property(x => x.AccountNo).HasColumnName("BANK_ACCOUNT_NO").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Branch).HasColumnName("BANK_BRANCH").HasMaxLength(100);
        builder.Property(x => x.AccountType).HasColumnName("BANK_ACCOUNT_TYPE").HasMaxLength(20);
        builder.Property(x => x.Status).HasColumnName("BANK_ACCOUNT_STATUS")
            .HasConversion(v => ((char)(int)v).ToString(), v => (EntityStatus)(int)v[0])
            .HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(x => x.UpdatedOn).HasColumnName("UPDATED_ON");
    }
}

public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
{
    public void Configure(EntityTypeBuilder<BankTransaction> builder)
    {
        builder.ToTable("BANK_TRANSACTION");
        builder.HasKey(x => x.BankTxnId);
        builder.Property(x => x.BankTxnId).HasColumnName("BANK_TXN_ID").UseIdentityColumn();
        builder.Property(x => x.BankAccountId).HasColumnName("BANK_ACCOUNT_ID").IsRequired();
        builder.Property(x => x.TxnType).HasColumnName("BANK_TXN_TYPE")
            .HasConversion(v => ((char)(int)v).ToString(), v => (BankTransactionType)(int)v[0])
            .HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.Amount).HasColumnName("BANK_TXN_AMOUNT").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.TxnDate).HasColumnName("BANK_TXN_DATE").IsRequired();
        builder.Property(x => x.Reference).HasColumnName("BANK_TXN_REFERENCE").HasMaxLength(50);
        builder.Property(x => x.Remarks).HasColumnName("BANK_TXN_REMARKS").HasMaxLength(500);
        builder.Property(x => x.Status).HasColumnName("BANK_TXN_STATUS")
            .HasConversion(v => ((char)(int)v).ToString(), v => (TransactionStatus)(int)v[0])
            .HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();

        builder.HasIndex(x => new { x.BankAccountId, x.TxnDate }).HasDatabaseName("IX_BANK_TRANSACTION_ACCOUNT_DATE");
        builder.HasIndex(x => x.TxnType).HasDatabaseName("IX_BANK_TRANSACTION_TYPE");
    }
}

public class ChequeRegisterConfiguration : IEntityTypeConfiguration<ChequeRegister>
{
    public void Configure(EntityTypeBuilder<ChequeRegister> builder)
    {
        builder.ToTable("CHEQUE_REGISTER");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CHEQUE_ID").UseIdentityColumn();
        builder.Property(x => x.BankAccountId).HasColumnName("BANK_ACCOUNT_ID").IsRequired();
        builder.Property(x => x.ChequeNumber).HasColumnName("CHEQUE_NUMBER").HasMaxLength(20).IsRequired();
        builder.Property(x => x.PayeeName).HasColumnName("PAYEE_NAME").HasMaxLength(100).IsRequired();
        builder.Property(x => x.ChequeAmount).HasColumnName("CHEQUE_AMOUNT").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.IssueDate).HasColumnName("CHEQUE_ISSUE_DATE").IsRequired();
        builder.Property(x => x.ChequeDate).HasColumnName("CHEQUE_DATE").IsRequired();
        builder.Property(x => x.Reference).HasColumnName("CHEQUE_REFERENCE").HasMaxLength(100);
        builder.Property(x => x.Status).HasColumnName("CHEQUE_STATUS")
            .HasConversion(v => ((char)(int)v).ToString(), v => (ChequeStatus)(int)v[0])
            .HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.BounceReason).HasColumnName("CHEQUE_BOUNCE_REASON").HasMaxLength(200);
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(x => x.UpdatedOn).HasColumnName("UPDATED_ON");

        builder.HasIndex(x => x.BankAccountId).HasDatabaseName("IX_CHEQUE_REGISTER_ACCOUNT");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_CHEQUE_REGISTER_STATUS");
        builder.HasIndex(x => new { x.BankAccountId, x.ChequeNumber }).IsUnique().HasDatabaseName("UQ_CHEQUE_REGISTER");
    }
}

public class BankReconciliationConfiguration : IEntityTypeConfiguration<BankReconciliation>
{
    public void Configure(EntityTypeBuilder<BankReconciliation> builder)
    {
        builder.ToTable("BANK_RECONCILIATION");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("RECON_ID").UseIdentityColumn();
        builder.Property(x => x.BankAccountId).HasColumnName("BANK_ACCOUNT_ID").IsRequired();
        builder.Property(x => x.BankStatementBalance).HasColumnName("BANK_STATEMENT_BALANCE").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.LedgerBalance).HasColumnName("LEDGER_BALANCE").HasColumnType("decimal(19,0)").IsRequired();
        builder.Property(x => x.UnclearedCheques).HasColumnName("UNCLEARED_CHEQUES").HasColumnType("decimal(19,0)");
        builder.Property(x => x.DifferenceAmount).HasColumnName("DIFFERENCE_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(x => x.Status).HasColumnName("RECONCILIATION_STATUS").HasConversion<string?>().HasMaxLength(10);
        builder.Property(x => x.ReconciliationDate).HasColumnName("RECONCILIATION_DATE").IsRequired();
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();

        builder.HasIndex(x => x.BankAccountId).HasDatabaseName("IX_BANK_RECONCILIATION_ACCOUNT");
    }
}

public class ChequeRegisterAuditConfiguration : IEntityTypeConfiguration<ChequeRegisterAudit>
{
    public void Configure(EntityTypeBuilder<ChequeRegisterAudit> builder)
    {
        builder.ToTable("CHEQUE_REGISTER_AUDIT");
        builder.HasKey(x => x.AuditId);
        builder.Property(x => x.AuditId).HasColumnName("AUDIT_ID").UseIdentityColumn();
        builder.Property(x => x.ChequeId).HasColumnName("CHEQUE_ID").IsRequired();
        builder.Property(x => x.BankAccountId).HasColumnName("BANK_ACCOUNT_ID").IsRequired();
        builder.Property(x => x.ChequeNumber).HasColumnName("CHEQUE_NUMBER").HasMaxLength(20).IsRequired();
        builder.Property(x => x.PreviousStatus).HasColumnName("PREVIOUS_STATUS").HasMaxLength(10);
        builder.Property(x => x.NewStatus).HasColumnName("NEW_STATUS").HasMaxLength(10).IsRequired();
        builder.Property(x => x.AuditAction).HasColumnName("AUDIT_ACTION").HasMaxLength(10).IsRequired();
        builder.Property(x => x.AuditDate).HasColumnName("AUDIT_DATE").IsRequired();
    }
}
