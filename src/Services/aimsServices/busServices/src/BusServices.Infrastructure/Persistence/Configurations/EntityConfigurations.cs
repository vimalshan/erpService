using BusServices.Domain.Entities;
using BusServices.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusServices.Infrastructure.Persistence.Configurations;

public sealed class BusRouteConfiguration : IEntityTypeConfiguration<BusRoute>
{
    public void Configure(EntityTypeBuilder<BusRoute> builder)
    {
        builder.ToTable("BUSROUTE_MASTER");
        builder.HasKey(r => r.RouteId);
        builder.Property(r => r.RouteId).HasColumnName("ROUTE_ID").ValueGeneratedNever();
        builder.Property(r => r.BusId).HasColumnName("ROUTE_BUS_ID").IsRequired();
        builder.Property(r => r.Name).HasColumnName("ROUTE_NAME").HasMaxLength(100).IsRequired();
        builder.Property(r => r.Description).HasColumnName("ROUTE_DESCRIPTION").HasMaxLength(255);
        builder.Property(r => r.LastModifiedBy).HasColumnName("ROUTE_LASTMODIFIEDBY");
        builder.Property(r => r.LastModifiedOn).HasColumnName("ROUTE_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Property(r => r.Status)
            .HasConversion(
                rs => rs.Value.ToString(),
                v => RouteStatus.Create(v[0]))
            .HasColumnName("ROUTE_STATUS")
            .HasMaxLength(1)
            .IsRequired();

        builder.Ignore(r => r.DomainEvents);
    }
}

public sealed class BusArrivalConfiguration : IEntityTypeConfiguration<BusArrival>
{
    public void Configure(EntityTypeBuilder<BusArrival> builder)
    {
        builder.ToTable("BUS_ARRIVALDET");
        builder.HasKey(a => a.ArrivalId);
        builder.Property(a => a.ArrivalId).HasColumnName("ARRIVAL_ID").ValueGeneratedNever();
        builder.Property(a => a.BusId).HasColumnName("ARRIVAL_BUS_ID").IsRequired();
        builder.Property(a => a.ArrivalDate).HasColumnName("ARRIVAL_DATE").HasColumnType("datetime2(3)");
        builder.Property(a => a.ArrivalTime).HasColumnName("ARRIVAL_TIME").HasColumnType("time");
        builder.Property(a => a.Remarks).HasColumnName("ARRIVAL_REMARKS").HasMaxLength(255);
        builder.Property(a => a.LastModifiedBy).HasColumnName("ARRIVAL_LASTMODIFIEDBY");
        builder.Property(a => a.LastModifiedOn).HasColumnName("ARRIVAL_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Property(a => a.Status)
            .HasConversion(
                s => s.Value.ToString(),
                v => ArrivalStatus.Create(v[0]))
            .HasColumnName("ARRIVAL_STATUS")
            .HasMaxLength(1)
            .IsRequired();

        builder.Ignore(a => a.DomainEvents);
    }
}

public sealed class EmployeeBusConfiguration : IEntityTypeConfiguration<EmployeeBus>
{
    public void Configure(EntityTypeBuilder<EmployeeBus> builder)
    {
        builder.ToTable("EMPLOYEE_BUS");
        builder.HasKey(e => e.EmpBusId);
        builder.Property(e => e.EmpBusId).HasColumnName("EMPBUS_ID").ValueGeneratedNever();
        builder.Property(e => e.EmpSysId).HasColumnName("EMPBUS_EMPSYSID").IsRequired();
        builder.Property(e => e.BusId).HasColumnName("EMPBUS_BUSID").IsRequired();
        builder.Property(e => e.RouteId).HasColumnName("EMPBUS_ROUTEID").IsRequired();
        builder.Property(e => e.EffectiveDate).HasColumnName("EMPBUS_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ClosingDate).HasColumnName("EMPBUS_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.LastModifiedBy).HasColumnName("EMPBUS_LASTMODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("EMPBUS_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}

public sealed class BusDeductionRateConfiguration : IEntityTypeConfiguration<BusDeductionRate>
{
    public void Configure(EntityTypeBuilder<BusDeductionRate> builder)
    {
        builder.ToTable("BUSDEDUCTION_RATEMAST");
        builder.HasKey(d => d.DeductId);
        builder.Property(d => d.DeductId).HasColumnName("DEDUCT_ID").ValueGeneratedNever();
        builder.Property(d => d.BusId).HasColumnName("DEDUCT_BUSID").IsRequired();
        builder.Property(d => d.Amount).HasColumnName("DEDUCT_AMOUNT").HasColumnType("decimal(10,2)").IsRequired();
        builder.Property(d => d.EffectiveDate).HasColumnName("DEDUCT_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(d => d.ClosingDate).HasColumnName("DEDUCT_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(d => d.LastModifiedBy).HasColumnName("DEDUCT_LASTMODIFIEDBY");
        builder.Property(d => d.LastModifiedOn).HasColumnName("DEDUCT_LASTMODIFIEDON").HasColumnType("datetime2(3)");

        builder.Ignore(d => d.DomainEvents);
    }
}
