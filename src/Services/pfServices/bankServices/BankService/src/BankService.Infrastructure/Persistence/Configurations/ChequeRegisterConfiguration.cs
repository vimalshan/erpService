using BankService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankService.Infrastructure.Persistence.Configurations;

public class ChequeRegisterConfiguration : IEntityTypeConfiguration<ChequeRegister>
{
    public void Configure(EntityTypeBuilder<ChequeRegister> builder)
    {
        builder.ToTable("CHEQUE_REGISTER");
        builder.HasKey(e => e.RegisterId);

        builder.Property(e => e.RegisterId).HasColumnName("REGISTER_ID").UseIdentityColumn();
        builder.Property(e => e.ChequeNoFrom).HasColumnName("CHEQUE_NO_FROM").HasColumnType("decimal(20,0)");
        builder.Property(e => e.ChequeNoTo).HasColumnName("CHEQUE_NO_TO").HasColumnType("decimal(20,0)");
        builder.Property(e => e.ChequeBookId).HasColumnName("CHEQUE_BOOK_ID").HasMaxLength(50);
        builder.Property(e => e.AccountId).HasColumnName("ACCOUNT_ID");
        builder.Property(e => e.IssuedDate).HasColumnName("ISSUED_DATE").HasPrecision(3);
        builder.Property(e => e.RegisterStatus).HasColumnName("REGISTER_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");

        builder.HasOne(e => e.Account)
            .WithMany(a => a.ChequeRegisters)
            .HasForeignKey(e => e.AccountId)
            .HasConstraintName("FK_CHEQUE_REGISTER_ACCOUNT");

        builder.HasIndex(e => new { e.AccountId, e.RegisterStatus }).HasDatabaseName("IDX_CHEQUE_REGISTER_ACCOUNT");

        builder.Ignore(e => e.DomainEvents);
    }
}
