using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore.Configurations;

public class PFWithdrawalCertificateConfiguration : IEntityTypeConfiguration<PFWithdrawalCertificate>
{
    public void Configure(EntityTypeBuilder<PFWithdrawalCertificate> builder)
    {
        builder.ToTable("PF_WITHDRAWAL_CERTIFICATE");
        builder.HasKey(e => e.CertificateId);

        builder.Property(e => e.CertificateId).HasColumnName("CERTIFICATE_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.PfSettlementId).HasColumnName("PF_SETTLEMENT_ID");
        builder.Property(e => e.EmpSysId).HasColumnName("EMP_SYS_ID");
        builder.Property(e => e.CertificateAmount).HasColumnName("CERTIFICATE_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.CertificateDate).HasColumnName("CERTIFICATE_DATE").HasPrecision(3);
        builder.Property(e => e.CertificateStatus).HasColumnName("CERTIFICATE_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (CertificateStatus)v[0])
            .HasMaxLength(1)
            .HasDefaultValue(CertificateStatus.Generated)
            .HasSentinel(CertificateStatus.Generated);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);

        builder.Ignore(e => e.DomainEvents);
    }
}
