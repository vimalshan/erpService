using AccountingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.Infrastructure.Data.Configurations;

public class MainAccountConfiguration : IEntityTypeConfiguration<MainAccount>
{
    public void Configure(EntityTypeBuilder<MainAccount> builder)
    {
        builder.ToTable("MAINACCOUNT_MASTER");
        builder.HasKey(x => x.MainAccountCode);
        builder.Property(x => x.MainAccountCode).HasColumnName("MAIN_ACCOUNT_CODE").HasMaxLength(10).IsRequired();
        builder.Property(x => x.MainAccountName).HasColumnName("MAIN_ACCOUNT_NAME").HasMaxLength(200);
        builder.Property(x => x.MainAccountShrtName).HasColumnName("MAIN_ACCOUNT_SHRT_NAME").HasMaxLength(30);
        builder.Property(x => x.MainClosureFlag).HasColumnName("MAIN_CLOSURE_FLAG").HasColumnType("CHAR(1)");
    }
}
