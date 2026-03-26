using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class CompOffConfiguration : IEntityTypeConfiguration<CompOffAggregate>
{
    public void Configure(EntityTypeBuilder<CompOffAggregate> builder)
    {
        builder.ToTable("COMPOFF_ADJUST");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("COA_SYSID").ValueGeneratedNever();

        builder.Property(c => c.EmployeeSysId)
            .HasColumnName("COA_EMPSYSID")
            .IsRequired();

        builder.Property(c => c.HoursRequested)
            .HasColumnName("COA_HOURSREQUESTED")
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(c => c.Status)
            .HasColumnName("COA_STATUS")
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(c => c.RequestedOn)
            .HasColumnName("COA_REQUESTEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(c => c.RequestedBy)
            .HasColumnName("COA_REQUESTEDBY")
            .IsRequired();

        builder.Property(c => c.ApprovedOn)
            .HasColumnName("COA_APPROVEDON")
            .HasColumnType("datetime2(3)");

        builder.Property(c => c.ApprovedBy)
            .HasColumnName("COA_APPROVEDBY");

        builder.Ignore(c => c.DomainEvents);

        builder.HasIndex(c => c.EmployeeSysId).HasDatabaseName("IX_COMPOFF_ADJUST_EMPSYSID");
        builder.HasIndex(c => c.Status).HasDatabaseName("IX_COMPOFF_ADJUST_STATUS");
    }
}
