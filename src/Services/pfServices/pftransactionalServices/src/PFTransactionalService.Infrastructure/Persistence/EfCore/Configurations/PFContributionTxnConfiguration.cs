using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore.Configurations;

public class PFContributionTxnConfiguration : IEntityTypeConfiguration<PFContributionTxn>
{
    public void Configure(EntityTypeBuilder<PFContributionTxn> builder)
    {
        builder.ToTable("PF_CONTRIBUTION_TXN");
        builder.HasKey(e => e.PfTxnId);

        builder.Property(e => e.PfTxnId).HasColumnName("PF_TXN_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.EmpSysId).HasColumnName("EMP_SYS_ID");
        builder.Property(e => e.PfEmpContribution).HasColumnName("PF_EMP_CONTRIBUTION").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfErContribution).HasColumnName("PF_ER_CONTRIBUTION").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfVolContribution).HasColumnName("PF_VOL_CONTRIBUTION").HasColumnType("decimal(19,0)");
        builder.Property(e => e.PfTxnDate).HasColumnName("PF_TXN_DATE").HasPrecision(3);
        builder.Property(e => e.PfTxnMonth).HasColumnName("PF_TXN_MONTH").HasPrecision(3);
        builder.Property(e => e.PfTxnStatus).HasColumnName("PF_TXN_STATUS")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (TransactionStatus)v[0])
            .HasMaxLength(1)
            .HasDefaultValue(TransactionStatus.Posted)
            .HasSentinel(TransactionStatus.Posted);
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("CREATED_ON").HasPrecision(3);

        builder.HasIndex(e => e.EmpSysId).HasDatabaseName("IDX_PF_CONTRIBUTION_TXN_EMPSYSID");
        builder.HasIndex(e => e.PfTxnMonth).HasDatabaseName("IDX_PF_CONTRIBUTION_TXN_MONTH");

        builder.Ignore(e => e.DomainEvents);
    }
}
