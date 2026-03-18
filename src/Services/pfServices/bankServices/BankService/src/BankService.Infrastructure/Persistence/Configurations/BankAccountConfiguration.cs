using BankService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankService.Infrastructure.Persistence.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("BANK_ACCOUNT");
        builder.HasKey(e => e.AccountId);

        builder.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID").UseIdentityColumn();
        builder.Property(e => e.AccountNumber).HasColumnName("ACCOUNT_NUMBER").HasMaxLength(30);
        builder.Property(e => e.AccountTitle).HasColumnName("ACCOUNT_TITLE").HasMaxLength(100);
        builder.Property(e => e.BankCode).HasColumnName("BANK_CODE").HasMaxLength(6).IsFixedLength();
        builder.Property(e => e.TrustCode).HasColumnName("TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.AccountType).HasColumnName("ACCOUNT_TYPE").HasMaxLength(20);
        builder.Property(e => e.AccountBalance).HasColumnName("ACCOUNT_BALANCE").HasColumnType("decimal(19,0)").HasDefaultValue(0m);
        builder.Property(e => e.AccountStatus).HasColumnName("ACCOUNT_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");
        builder.Property(e => e.OpeningDate).HasColumnName("OPENING_DATE").HasPrecision(3);
        builder.Property(e => e.ClosingDate).HasColumnName("CLOSING_DATE").HasPrecision(3);

        builder.HasIndex(e => e.AccountNumber).IsUnique();
        builder.HasIndex(e => new { e.TrustCode, e.AccountStatus }).HasDatabaseName("IDX_BANK_ACCOUNT_TRUST");

        builder.Ignore(e => e.DomainEvents);
    }
}
