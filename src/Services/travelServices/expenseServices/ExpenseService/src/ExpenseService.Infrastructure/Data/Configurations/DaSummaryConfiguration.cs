using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseService.Infrastructure.Data.Configurations;

public class DaSummaryConfiguration : IEntityTypeConfiguration<DaSummary>
{
    public void Configure(EntityTypeBuilder<DaSummary> builder)
    {
        builder.ToTable("DA_SUMMARY");
        builder.HasKey(e => e.RequestId);

        builder.Property(e => e.RequestId).HasColumnName("DA_REQID").ValueGeneratedNever();
        builder.Property(e => e.AdminHours).HasColumnName("DA_ADMHRS").HasColumnType("decimal(19,0)");
        builder.Property(e => e.AdminDays).HasColumnName("DA_ADMDYS").HasColumnType("decimal(19,0)");
        builder.Property(e => e.AdminRate).HasColumnName("DA_ADMRAT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.AdminAmount).HasColumnName("DA_ADMAMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SelfHours).HasColumnName("DA_SLFHRS").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SelfDays).HasColumnName("DA_SLFDYS").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SelfRate).HasColumnName("DA_SLFRAT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.SelfAmount).HasColumnName("DA_SLFAMT").HasColumnType("decimal(19,0)");

        builder.Ignore(e => e.DomainEvents);
    }
}
