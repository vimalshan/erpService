using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class TravelExpenseAllocationConfiguration : IEntityTypeConfiguration<TravelExpenseAllocation>
{
    public void Configure(EntityTypeBuilder<TravelExpenseAllocation> builder)
    {
        builder.ToTable("TRAVEL_EXPENSEALL");
        builder.HasKey(e => new { e.RequestNumber, e.AllocationSerialNumber });

        builder.Property(e => e.RequestNumber).HasColumnName("TR_REQ_NUM");
        builder.Property(e => e.AllocationSerialNumber).HasColumnName("TR_SRL_NUM");
        builder.Property(e => e.ExpenseSerialNumber).HasColumnName("TR_EXP_SRL");
        builder.Property(e => e.UnitCode).HasColumnName("TR_UNT_COD").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.CostCentreCode).HasColumnName("TR_CST_COD").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.AllocationType).HasColumnName("TR_ALL_TYP").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.AllocationPercentage).HasColumnName("TR_ALL_PER").HasColumnType("decimal(19,0)");

        builder.Ignore(e => e.DomainEvents);
    }
}
