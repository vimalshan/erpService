using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class AttendanceLopDetailConfiguration : IEntityTypeConfiguration<AttendanceLopDetail>
{
    public void Configure(EntityTypeBuilder<AttendanceLopDetail> builder)
    {
        builder.ToTable("ATTENDANCE_LOPDET");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("ALD_SYSID").ValueGeneratedNever();

        builder.Property(d => d.LopMainId)
            .HasColumnName("ALD_LOPMAINID")
            .IsRequired();

        builder.Property(d => d.LopDate)
            .HasColumnName("ALD_LOPDATE")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(d => d.LopHours)
            .HasColumnName("ALD_LOPHOURS")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(d => d.LopReason)
            .HasColumnName("ALD_LOPREASON")
            .HasMaxLength(255);

        builder.Ignore(d => d.DomainEvents);

        builder.HasIndex(d => d.LopMainId).HasDatabaseName("IX_ATTENDANCE_LOPDET_LOPMAINID");
    }
}
