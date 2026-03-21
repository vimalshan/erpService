using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelService.Domain.Entities.Forex;

namespace TravelService.Infrastructure.Persistence.Configurations;

public class ForexMainConfiguration : IEntityTypeConfiguration<ForexMain>
{
    public void Configure(EntityTypeBuilder<ForexMain> builder)
    {
        builder.ToTable("TOURPLAN_FOREXMAIN");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("FORREQ_ID").HasMaxLength(255);
        builder.Property(x => x.TourPlanId).HasColumnName("FORREQ_TPID").HasMaxLength(255).IsRequired();
        builder.Property(x => x.PassportNo).HasColumnName("FORREQ_PASSNO").HasMaxLength(255).IsRequired();
        builder.Property(x => x.PassportName).HasColumnName("FORREQ_PASSNAME").HasMaxLength(255).IsRequired();
        builder.Property(x => x.PassportLocation).HasColumnName("FORREQ_PASSLOCATION").HasMaxLength(255).IsRequired();
        builder.Property(x => x.PassportExpiryDate).HasColumnName("FORREQ_PASSEXPDATE").IsRequired();
        builder.Property(x => x.Destination).HasColumnName("FORREQ_DESTINATION").HasMaxLength(255);
        builder.Property(x => x.Status).HasColumnName("FORREQ_STATUS").HasMaxLength(255);
        builder.Property(x => x.LastModifiedBy).HasColumnName("FORREQ_LASTMODIFIEDBY").HasMaxLength(255).IsRequired();
        builder.Property(x => x.LastModifiedOn).HasColumnName("FORREQ_LASTMODIFIEDON").IsRequired();
        builder.Property(x => x.ReceivedOn).HasColumnName("FORREQ_RECEIVEDON");
        builder.Property(x => x.ReferenceNo).HasColumnName("FORREQ_REFNO").HasMaxLength(255);
        builder.Property(x => x.Charges).HasColumnName("FORREQ_TAX1").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.ServiceTax).HasColumnName("FORREQ_TAX2").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.EduCess).HasColumnName("FORREQ_TAX3").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.HeEduCess).HasColumnName("FORREQ_TAX4").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.RoundingAmount).HasColumnName("FORREQ_TAX5").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.VendorId).HasColumnName("FORREQ_VENDORID").HasMaxLength(255);
        builder.Property(x => x.Currency).HasColumnName("FORREQ_CURRENCY").HasMaxLength(255);
        builder.Property(x => x.TotalValue).HasColumnName("FORREQ_TOTVALUE").HasConversion<string>().HasMaxLength(255);
        builder.Property(x => x.RecommendedBy).HasColumnName("FORREQ_RECBY").HasMaxLength(255);
        builder.Property(x => x.RequestType).HasColumnName("FORREQ_TYPE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdditionalRemarks).HasColumnName("FORREQ_ADLREMARKS").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdvanceRefNo).HasColumnName("FORREQ_ADVREFNO").HasMaxLength(255).IsRequired();
        builder.HasMany(x => x.Details).WithOne().HasForeignKey(d => d.ForexRequestId);
        builder.HasMany(x => x.ChequeDetails).WithOne().HasForeignKey(d => d.ForexRequestId);
        builder.Ignore(x => x.DomainEvents);
    }
}
