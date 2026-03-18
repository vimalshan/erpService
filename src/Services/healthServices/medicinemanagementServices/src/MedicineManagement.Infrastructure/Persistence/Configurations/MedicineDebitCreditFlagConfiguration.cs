using MedicineManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicineManagement.Infrastructure.Persistence.Configurations;

public class MedicineDebitCreditFlagConfiguration : IEntityTypeConfiguration<MedicineDebitCreditFlag>
{
    public void Configure(EntityTypeBuilder<MedicineDebitCreditFlag> builder)
    {
        builder.ToTable("MED_DRCRFLG");
        builder.HasNoKey();
        builder.Property(e => e.Flag).HasColumnName("MED_FLG").HasColumnType("CHAR(1)");
        builder.Property(e => e.DebitCredit).HasColumnName("MED_DRCR");
        builder.Ignore(e => e.DomainEvents);
    }
}
