using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Domain.Enums;

namespace AimsTransactionService.Infrastructure.Data.Configurations;

public class SwipeConfiguration : IEntityTypeConfiguration<SwipeAggregate>
{
    public void Configure(EntityTypeBuilder<SwipeAggregate> builder)
    {
        builder.ToTable("SWIPE_RAWPUNCH");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("SRP_SYSID").ValueGeneratedNever();

        builder.Property(s => s.EmployeeSysId)
            .HasColumnName("SRP_EMPSYSID")
            .IsRequired();

        builder.OwnsOne(s => s.PunchInfo, pi =>
        {
            pi.Property(p => p.GateNo)
                .HasColumnName("SRP_GATENO")
                .IsRequired();

            pi.Property(p => p.PunchStatus)
                .HasColumnName("SRP_INOUTSTATUS")
                .HasMaxLength(1)
                .IsRequired()
                .HasConversion(
                    v => ((char)(int)v).ToString(),
                    s => (PunchStatus)s[0]);

            pi.Property(p => p.MachineNo)
                .HasColumnName("SRP_MACHINENO");

            pi.Property(p => p.ReferenceNo)
                .HasColumnName("SRP_REFERENCENO")
                .HasMaxLength(50);
        });

        builder.Property(s => s.PunchTime)
            .HasColumnName("SRP_PUNCHTIME")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(s => s.PullStatus)
            .HasColumnName("SRP_PULLSTATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => ((char)(int)v).ToString(),
                s => (PullStatus)s[0]);

        builder.Property(s => s.UpdatedBy)
            .HasColumnName("SRP_UPDATEDBY")
            .IsRequired();

        builder.Property(s => s.UpdatedOn)
            .HasColumnName("SRP_UPDATEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(s => s.DomainEvents);

        builder.HasIndex(s => s.EmployeeSysId).HasDatabaseName("IX_SWIPE_RAWPUNCH_EMPSYSID");
        builder.HasIndex(s => s.PunchTime).HasDatabaseName("IX_SWIPE_RAWPUNCH_PUNCHTIME");
    }
}
