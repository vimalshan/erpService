using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.Infrastructure.Data.Configurations;

public class PfSubAccountConfiguration : IEntityTypeConfiguration<PfSubAccount>
{
    public void Configure(EntityTypeBuilder<PfSubAccount> builder)
    {
        builder.ToTable("PF_SUB_ACCOUNT");
        builder.HasKey(x => x.SubAccCod);
        builder.Property(x => x.SubAccCod).HasColumnName("SUB_ACC_COD").ValueGeneratedNever();
        builder.Property(x => x.SubAccNam).HasColumnName("SUB_ACC_NAM").HasMaxLength(255).IsRequired();
    }
}
