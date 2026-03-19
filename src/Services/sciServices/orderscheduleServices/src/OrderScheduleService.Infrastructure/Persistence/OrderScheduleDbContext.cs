namespace OrderScheduleService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderScheduleService.Domain.Aggregates;
using OrderScheduleService.Domain.Entities;

public class OrderScheduleDbContext : DbContext
{
    public OrderScheduleDbContext(DbContextOptions<OrderScheduleDbContext> options) : base(options)
    {
    }

    public DbSet<TiedOrderAggregate> TiedOrders { get; set; } = null!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
    public DbSet<ScheduleAggregate> Schedules { get; set; } = null!;
    public DbSet<ScheduleDetail> ScheduleDetails { get; set; } = null!;
    public DbSet<OrderActual> OrderActuals { get; set; } = null!;
    public DbSet<Shift> Shifts { get; set; } = null!;
    public DbSet<CapacityChange> CapacityChanges { get; set; } = null!;
    public DbSet<EmptiesOrder> EmptiesOrders { get; set; } = null!;
    public DbSet<ScheduleConfirm> ScheduleConfirms { get; set; } = null!;
    public DbSet<ActualOrderSchedule> ActualOrderSchedules { get; set; } = null!;
    public DbSet<ActualOrderMap> ActualOrderMaps { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<OrderScheduleService.Domain.Common.DomainEvent>();

        modelBuilder.ApplyConfiguration(new TiedOrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
        modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new ScheduleDetailConfiguration());
        modelBuilder.ApplyConfiguration(new OrderActualConfiguration());
        modelBuilder.ApplyConfiguration(new ShiftConfiguration());
        modelBuilder.ApplyConfiguration(new CapacityChangeConfiguration());
        modelBuilder.ApplyConfiguration(new EmptiesOrderConfiguration());
        modelBuilder.ApplyConfiguration(new ScheduleConfirmConfiguration());
        modelBuilder.ApplyConfiguration(new ActualOrderScheduleConfiguration());
        modelBuilder.ApplyConfiguration(new ActualOrderMapConfiguration());
    }
}

public class TiedOrderConfiguration : IEntityTypeConfiguration<TiedOrderAggregate>
{
    public void Configure(EntityTypeBuilder<TiedOrderAggregate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.CustomerCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.OrderedDate).IsRequired();
        builder.Property(x => x.CompanyUnitId).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.RecordStatus).HasColumnType("char(1)").IsRequired();
        builder.Property(x => x.ModifiedSciUserId).HasMaxLength(255);
        builder.Property(x => x.OrderNumberCode).HasMaxLength(10);
        builder.Property(x => x.LcNumber).HasMaxLength(255);

        builder.HasMany(x => x.Details)
            .WithOne()
            .HasForeignKey(x => x.TiedOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("OS_TIED_ORDER_HEADER");
    }
}

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.TiedOrderId).IsRequired();
        builder.Property(x => x.ItemId).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.ItemName).HasMaxLength(2000);
        builder.Property(x => x.OrderQuantity).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(19,4)");

        builder.ToTable("OS_TIED_ORDER_DETAILS");
    }
}

public class ScheduleConfiguration : IEntityTypeConfiguration<ScheduleAggregate>
{
    public void Configure(EntityTypeBuilder<ScheduleAggregate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.FillingPointGroupId).IsRequired();
        builder.Property(x => x.ItemId).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.OrderType).HasMaxLength(1);
        builder.Property(x => x.RequiredDate).IsRequired();
        builder.Property(x => x.OrderQuantity).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.ShiftCapacity).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.TotalAllocatedQuantity).HasColumnType("decimal(38,0)");

        builder.HasMany(x => x.ScheduleDetails)
            .WithOne()
            .HasForeignKey(x => x.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("OS_SCHEDULE_MASTER");
    }
}

public class ScheduleDetailConfiguration : IEntityTypeConfiguration<ScheduleDetail>
{
    public void Configure(EntityTypeBuilder<ScheduleDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ScheduleId).IsRequired();
        builder.Property(x => x.FillingShift).HasColumnType("char(1)").HasMaxLength(1);
        builder.Property(x => x.StartTime).HasMaxLength(5);
        builder.Property(x => x.EndTime).HasMaxLength(5);
        builder.Property(x => x.FillQuantity).HasColumnType("decimal(38,0)");
        builder.Property(x => x.FillingPointGroupId).HasColumnType("decimal(38,0)");

        builder.ToTable("OS_SCHEDULE_DETAILS");
    }
}

public class OrderActualConfiguration : IEntityTypeConfiguration<OrderActual>
{
    public void Configure(EntityTypeBuilder<OrderActual> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.OrderNumber).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.HeaderId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.LineId).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.OrderedItem).HasMaxLength(2000);
        builder.Property(x => x.OrderedItemId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.OrderedQuantity).HasColumnType("decimal(38,0)");
        builder.Property(x => x.OrderQuantityUom).HasMaxLength(3);
        builder.Property(x => x.CancelledQuantity).HasColumnType("decimal(38,0)");
        builder.Property(x => x.FulfilledQuantity).HasColumnType("decimal(38,0)");
        builder.Property(x => x.ShippingQuantity).HasColumnType("decimal(38,0)");
        builder.Property(x => x.ShippingQuantityUom).HasMaxLength(3);
        builder.Property(x => x.InvoicedQuantity).HasColumnType("decimal(38,0)");
        builder.Property(x => x.ShipFromOrgId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.SoldFromOrgId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.SoldToOrgId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.ShipToOrgId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.OrderSourceId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.CustomerName).HasMaxLength(50);
        builder.Property(x => x.CustPoNumber).HasMaxLength(50);
        builder.Property(x => x.ConsigneeName).HasMaxLength(50);

        builder.ToTable("OS_ACTUAL_ORDER");
    }
}

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.HasKey(x => new { x.ShiftCode, x.CompanyUnitId });
        builder.Ignore(x => x.Id);

        builder.Property(x => x.ShiftCode).HasColumnType("char(1)").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ShiftDescription).HasMaxLength(20).IsRequired();
        builder.Property(x => x.CompanyUnitId).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.StartTime).HasMaxLength(5).IsRequired();
        builder.Property(x => x.EndTime).HasMaxLength(5).IsRequired();

        builder.ToTable("OS_SHIFT_MASTER");

        // Seed reference data
        builder.HasData(
            new { ShiftCode = 'A', ShiftDescription = "Morning Shift", CompanyUnitId = 1m, StartTime = "06:00", StartDay = 0, EndTime = "14:00", EndDay = 0 },
            new { ShiftCode = 'B', ShiftDescription = "Afternoon Shift", CompanyUnitId = 1m, StartTime = "14:00", StartDay = 0, EndTime = "22:00", EndDay = 0 },
            new { ShiftCode = 'C', ShiftDescription = "Night Shift", CompanyUnitId = 1m, StartTime = "22:00", StartDay = 0, EndTime = "06:00", EndDay = 1 }
        );
    }
}

public class CapacityChangeConfiguration : IEntityTypeConfiguration<CapacityChange>
{
    public void Configure(EntityTypeBuilder<CapacityChange> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ChangeDate).IsRequired();
        builder.Property(x => x.RerunStatus).HasMaxLength(1).IsRequired();

        builder.ToTable("OS_CAPACITY_CHANGES");
    }
}

public class EmptiesOrderConfiguration : IEntityTypeConfiguration<EmptiesOrder>
{
    public void Configure(EntityTypeBuilder<EmptiesOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.SciItemId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.ItemId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.OrderQuantity).HasColumnType("decimal(38,0)");

        builder.ToTable("OS_EMPTIES_ORDER");
    }
}

public class ScheduleConfirmConfiguration : IEntityTypeConfiguration<ScheduleConfirm>
{
    public void Configure(EntityTypeBuilder<ScheduleConfirm> builder)
    {
        builder.HasKey(x => x.ScheduleDate);

        builder.Property(x => x.ScheduleDate).IsRequired();
        builder.Property(x => x.ScheduleStatus).HasMaxLength(1).IsRequired();
        builder.Property(x => x.ModifiedDate).IsRequired();

        builder.ToTable("OS_SCHEDULE_CONFIRM");
    }
}

public class ActualOrderScheduleConfiguration : IEntityTypeConfiguration<ActualOrderSchedule>
{
    public void Configure(EntityTypeBuilder<ActualOrderSchedule> builder)
    {
        builder.HasKey(x => new { x.CtOrderId, x.LineId });

        builder.Property(x => x.CtOrderId).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LineId).HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.OrderedItemId).HasColumnType("decimal(38,0)");

        builder.ToTable("OS_ACTUAL_ORDER_SCHEDULE");
    }
}

public class ActualOrderMapConfiguration : IEntityTypeConfiguration<ActualOrderMap>
{
    public void Configure(EntityTypeBuilder<ActualOrderMap> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.TiedOrderDetailId).HasColumnType("decimal(38,0)");
        builder.Property(x => x.ActualLineId).HasColumnType("decimal(38,0)");

        builder.ToTable("ACTUAL_ORDER_MAP");
    }
}
