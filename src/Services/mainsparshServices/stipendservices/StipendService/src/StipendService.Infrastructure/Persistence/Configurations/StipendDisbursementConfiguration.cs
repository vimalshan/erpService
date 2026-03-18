using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StipendService.Domain.Entities;

namespace StipendService.Infrastructure.Persistence.Configurations;

public class StipendDisbursementConfiguration : IEntityTypeConfiguration<StipendDisbursement>
{
    public void Configure(EntityTypeBuilder<StipendDisbursement> builder)
    {
        builder.ToTable("SRF_STIPEND_DISBURSEMENT");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("DISBURSEMENT_ID")
            .UseIdentityColumn(1, 1);

        builder.Property(x => x.SrfId).HasColumnName("SRF_ID").IsRequired();
        builder.Property(x => x.StipendId).HasColumnName("STIPEND_ID").IsRequired();
        builder.Property(x => x.DisbursementDate).HasColumnName("DISBURSEMENT_DATE").HasColumnType("date").IsRequired();
        builder.Property(x => x.DisbursementAmount).HasColumnName("DISBURSEMENT_AMOUNT").HasColumnType("decimal(19,2)").IsRequired();
        builder.Property(x => x.DisbursementStatus).HasColumnName("DISBURSEMENT_STATUS").HasMaxLength(20).HasDefaultValue("D");
        builder.Property(x => x.MonthYear).HasColumnName("MONTH_YEAR").HasMaxLength(7);
        builder.Property(x => x.BankReference).HasColumnName("BANK_REFERENCE").HasMaxLength(100);
        builder.Property(x => x.ReferenceNo).HasColumnName("REFERENCE_NO").HasMaxLength(100);
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(x => x.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("datetime2(3)");

        builder.HasIndex(x => x.SrfId).HasDatabaseName("IX_SRF_STIPEND_DISBURSEMENT_SRF_ID");
        builder.HasIndex(x => x.StipendId).HasDatabaseName("IX_SRF_STIPEND_DISBURSEMENT_STIPEND_ID");
        builder.HasIndex(x => x.DisbursementDate).HasDatabaseName("IX_SRF_STIPEND_DISBURSEMENT_DATE");
        builder.HasIndex(x => x.DisbursementStatus).HasDatabaseName("IX_SRF_STIPEND_DISBURSEMENT_STATUS");
        builder.HasIndex(x => x.MonthYear).HasDatabaseName("IX_SRF_STIPEND_DISBURSEMENT_MONTH");
    }
}
