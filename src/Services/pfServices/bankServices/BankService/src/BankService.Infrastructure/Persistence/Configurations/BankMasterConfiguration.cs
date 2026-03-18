using BankService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankService.Infrastructure.Persistence.Configurations;

public class BankMasterConfiguration : IEntityTypeConfiguration<BankMaster>
{
    public void Configure(EntityTypeBuilder<BankMaster> builder)
    {
        builder.ToTable("BANK_MASTER");
        builder.HasKey(e => new { e.BankTrustCode, e.BankCode });

        builder.Property(e => e.BankTrustCode).HasColumnName("BANK_TRUST_CODE").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.BankCode).HasColumnName("BANK_CODE").HasMaxLength(6).IsFixedLength();
        builder.Property(e => e.BankName).HasColumnName("BANK_NAME").HasMaxLength(65).IsFixedLength();
        builder.Property(e => e.MicrCode).HasColumnName("MICR_CODE").HasMaxLength(9).IsFixedLength();
        builder.Property(e => e.BranchName).HasColumnName("BRANCH_NAME").HasMaxLength(65);
        builder.Property(e => e.BranchAddressLine1).HasColumnName("BRANCH_ADDRESS_LINE_1").HasMaxLength(200);
        builder.Property(e => e.BranchAddressLine2).HasColumnName("BRANCH_ADDRESS_LINE_2").HasMaxLength(200);
        builder.Property(e => e.BranchAddressLine3).HasColumnName("BRANCH_ADDRESS_LINE_3").HasMaxLength(200);
        builder.Property(e => e.BranchAddressLine4).HasColumnName("BRANCH_ADDRESS_LINE_4").HasMaxLength(200);
        builder.Property(e => e.BranchPhoneNo).HasColumnName("BRANCH_PHONE_NO").HasMaxLength(200);
        builder.Property(e => e.BranchFaxNo).HasColumnName("BRANCH_FAX_NO").HasMaxLength(200);
        builder.Property(e => e.BranchEffDate).HasColumnName("BRANCH_EFF_DATE").HasPrecision(3);
        builder.Property(e => e.BranchClsDate).HasColumnName("BRANCH_CLS_DATE").HasPrecision(3);
        builder.Property(e => e.BranchStatus).HasColumnName("BRANCH_STATUS").HasMaxLength(1).IsFixedLength().HasDefaultValue("A");

        builder.HasIndex(e => new { e.BankTrustCode, e.BranchStatus }).HasDatabaseName("IDX_BANK_MASTER_TRUST");

        builder.Ignore(e => e.DomainEvents);
    }
}
