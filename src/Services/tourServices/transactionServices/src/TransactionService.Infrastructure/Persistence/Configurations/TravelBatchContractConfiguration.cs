using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class TravelBatchContractConfiguration : IEntityTypeConfiguration<TravelBatchContract>
{
    public void Configure(EntityTypeBuilder<TravelBatchContract> builder)
    {
        builder.ToTable("TRAVEL_BATCHCONTRACT");
        builder.HasKey(x => x.ContractNum);

        builder.Property(x => x.ContractNum).HasColumnName("BATCHCON_NUM").HasColumnType("DECIMAL(38)").ValueGeneratedNever();
        builder.Property(x => x.BatchMainNum).HasColumnName("BATCHCON_MAINNO").HasColumnType("DECIMAL(38)");
        builder.Property(x => x.BookCnfNo).HasColumnName("BATCHCON_BOOKCNFNO").HasColumnType("DECIMAL(38)");
        builder.Property(x => x.TicketCost).HasColumnName("BATCHCON_TKTCOST").HasMaxLength(255);
        builder.Property(x => x.TicketCostAdj).HasColumnName("BATCHCON_TKTCOSTADJ").HasMaxLength(255);
        builder.Property(x => x.BasicTax).HasColumnName("BATCHCON_BASTAX").HasMaxLength(255);
        builder.Property(x => x.TotalPayAmt).HasColumnName("BATCHCON_TOTPAYAMT").HasMaxLength(255);
        builder.Property(x => x.ApprovedAmt).HasColumnName("BATCHCON_APPROVEDAMT").HasMaxLength(255);
        builder.Property(x => x.ServiceTax).HasColumnName("BATCHCON_SERVICE TAX").HasMaxLength(255);
        builder.Property(x => x.CessTax).HasColumnName("BATCHCON_CESSTAX").HasMaxLength(255);
        builder.Property(x => x.AdditionalTax).HasColumnName("BATCHCON_ADLTAX").HasMaxLength(255);
        builder.Property(x => x.Remarks).HasColumnName("BATCHCON_REMARKS").HasMaxLength(255);
        builder.Property(x => x.Remarks1).HasColumnName("BATCHCON_REMARKS1").HasMaxLength(255);
        builder.Property(x => x.Remarks2).HasColumnName("BATCHCON_REMARKS2").HasMaxLength(255);

        builder.Ignore(x => x.DomainEvents);
    }
}
