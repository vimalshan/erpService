using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class PaymentTermConfiguration : IEntityTypeConfiguration<PaymentTerm>
{
    public void Configure(EntityTypeBuilder<PaymentTerm> builder)
    {
        builder.ToTable("AP_TERMS_TL");
        builder.HasKey(e => e.TermId);
        builder.Property(e => e.TermId).HasColumnName("TERM_ID").ValueGeneratedNever();
        builder.Property(e => e.LastUpdateDate).HasColumnName("LAST_UPDATE_DATE");
        builder.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
        builder.Property(e => e.CreationDate).HasColumnName("CREATION_DATE");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(50);
        builder.Property(e => e.EnabledFlag).HasColumnName("ENABLED_FLAG").HasMaxLength(1);
        builder.Property(e => e.DueCutoffDay).HasColumnName("DUE_CUTOFF_DAY").HasColumnType("decimal(38,0)");
        builder.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(240);
    }
}
