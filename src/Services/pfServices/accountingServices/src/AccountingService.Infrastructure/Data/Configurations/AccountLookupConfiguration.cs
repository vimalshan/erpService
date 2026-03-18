using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.Infrastructure.Data.Configurations;

public class AccountLookupConfiguration : IEntityTypeConfiguration<AccountLookup>
{
    public void Configure(EntityTypeBuilder<AccountLookup> builder)
    {
        builder.ToTable("ACC_LOOKUP");
        builder.HasKey(x => x.ConTyp);
        builder.Property(x => x.ConTyp).HasColumnName("CON_TYP").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(x => x.EdCod).HasColumnName("ED_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(x => x.AccCod).HasColumnName("ACC_COD");
        builder.Property(x => x.TrnTyp).HasColumnName("TRN_TYP").HasColumnType("CHAR(1)");
    }
}
