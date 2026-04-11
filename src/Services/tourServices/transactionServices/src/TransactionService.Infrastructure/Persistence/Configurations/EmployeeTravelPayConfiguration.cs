using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class EmployeeTravelPayConfiguration : IEntityTypeConfiguration<EmployeeTravelPay>
{
    public void Configure(EntityTypeBuilder<EmployeeTravelPay> builder)
    {
        builder.ToTable("TRAVEL_EMPPAYDET");
        builder.HasNoKey();

        builder.Property(x => x.EmpPayId).HasColumnName("EMPPAY_ID").HasMaxLength(255);
        builder.Property(x => x.EmpPayEmpSysId).HasColumnName("EMPPAY_EMPSYSID").HasMaxLength(255);
        builder.Property(x => x.EmpPayType).HasColumnName("EMPPAY_TYPE").HasMaxLength(255);
        builder.Property(x => x.EmpPayMode).HasColumnName("EMPPAY_MODE").HasMaxLength(255);
        builder.Property(x => x.EmpPayTrnDate).HasColumnName("EMPPAY_TRNDATE");
        builder.Property(x => x.EmpPayAmount).HasColumnName("EMPPAY_AMOUNT").HasMaxLength(255);
        builder.Property(x => x.EmpPaySource).HasColumnName("EMPPAY_SOURCE").HasMaxLength(255);
        builder.Property(x => x.EmpPayTrnType).HasColumnName("EMPPAY_TRNTYPE").HasMaxLength(255);
        builder.Property(x => x.EmpPayDate).HasColumnName("EMPPAY_DATE");
        builder.Property(x => x.EmpPayRefId).HasColumnName("EMPPAY_REFID").HasMaxLength(255);
        builder.Property(x => x.EmpPayAccType).HasColumnName("EMPPAY_ACCTYPE").HasMaxLength(255);
        builder.Property(x => x.EmpPayAccRefNo).HasColumnName("EMPPAY_ACCREFNO").HasMaxLength(255);
        builder.Property(x => x.EmpPayTpId).HasColumnName("EMPPAY_TPID").HasMaxLength(255);

        builder.Ignore(x => x.DomainEvents);
    }
}
