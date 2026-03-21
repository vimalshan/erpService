using FleetManagement.Domain.Common;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Data;

public class FleetDbContext(DbContextOptions<FleetDbContext> options, IMediator mediator) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<TripStop> TripStops => Set<TripStop>();
    public DbSet<MaintenanceLog> MaintenanceLogs => Set<MaintenanceLog>();
    public DbSet<FuelLog> FuelLogs => Set<FuelLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Vehicle
        modelBuilder.Entity<Vehicle>(e =>
        {
            e.ToTable("Vehicle");
            e.HasKey(v => v.VehicleId);
            e.Property(v => v.VehicleId).HasColumnName("vehicle_id");
            e.Property(v => v.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
            e.HasIndex(v => v.Code).IsUnique();
            e.Property(v => v.LicensePlate).HasColumnName("license_plate").HasMaxLength(20).IsRequired();
            e.HasIndex(v => v.LicensePlate).IsUnique();
            e.Property(v => v.VehicleType).HasColumnName("vehicle_type").HasMaxLength(30)
                .HasConversion<string>().IsRequired();
            e.Property(v => v.Make).HasColumnName("make").HasMaxLength(50);
            e.Property(v => v.Model).HasColumnName("model").HasMaxLength(50);
            e.Property(v => v.Year).HasColumnName("year");
            e.Property(v => v.CapacityWeight).HasColumnName("capacity_weight").HasColumnType("decimal(18,3)");
            e.Property(v => v.CapacityVolume).HasColumnName("capacity_volume").HasColumnType("decimal(18,3)");
            e.Property(v => v.Status).HasColumnName("status").HasMaxLength(20)
                .HasConversion<string>().HasDefaultValue(VehicleStatus.AVAILABLE);
            e.Property(v => v.WarehouseId).HasColumnName("warehouse_id");
            e.Property(v => v.Notes).HasColumnName("notes");
            e.Property(v => v.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(v => v.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
            e.Property(v => v.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");
            e.HasIndex(v => v.WarehouseId).HasDatabaseName("IX_Vehicle_Warehouse");
            e.HasIndex(v => v.Status).HasDatabaseName("IX_Vehicle_Status");
            e.Ignore(v => v.DomainEvents);
        });

        // Driver
        modelBuilder.Entity<Driver>(e =>
        {
            e.ToTable("Driver");
            e.HasKey(d => d.DriverId);
            e.Property(d => d.DriverId).HasColumnName("driver_id");
            e.Property(d => d.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
            e.HasIndex(d => d.Code).IsUnique();
            e.Property(d => d.EmployeeId).HasColumnName("employee_id");
            e.HasIndex(d => d.EmployeeId).IsUnique().HasFilter("[employee_id] IS NOT NULL");
            e.Property(d => d.FullName).HasColumnName("full_name").HasMaxLength(100).IsRequired();
            e.Property(d => d.LicenseNumber).HasColumnName("license_number").HasMaxLength(50).IsRequired();
            e.Property(d => d.LicenseExpiry).HasColumnName("license_expiry");
            e.Property(d => d.Phone).HasColumnName("phone").HasMaxLength(30);
            e.Property(d => d.Email).HasColumnName("email").HasMaxLength(100);
            e.Property(d => d.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(d => d.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
            e.Property(d => d.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");
            e.HasIndex(d => d.EmployeeId).HasDatabaseName("IX_Driver_Employee");
            e.Ignore(d => d.DomainEvents);
        });

        // Route
        modelBuilder.Entity<Route>(e =>
        {
            e.ToTable("Route");
            e.HasKey(r => r.RouteId);
            e.Property(r => r.RouteId).HasColumnName("route_id");
            e.Property(r => r.RouteName).HasColumnName("route_name").HasMaxLength(50).IsRequired();
            e.HasIndex(r => r.RouteName).IsUnique();
            e.Property(r => r.Description).HasColumnName("description").HasMaxLength(255);
            e.Property(r => r.StartLocation).HasColumnName("start_location").HasMaxLength(100);
            e.Property(r => r.EndLocation).HasColumnName("end_location").HasMaxLength(100);
            e.Property(r => r.EstimatedDuration).HasColumnName("estimated_duration");
            e.Property(r => r.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            e.Property(r => r.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
            e.Ignore(r => r.DomainEvents);
        });

        // Trip
        modelBuilder.Entity<Trip>(e =>
        {
            e.ToTable("Trip");
            e.HasKey(t => t.TripId);
            e.Property(t => t.TripId).HasColumnName("trip_id");
            e.Property(t => t.TripNumber).HasColumnName("trip_number").HasMaxLength(50).IsRequired();
            e.HasIndex(t => t.TripNumber).IsUnique();
            e.Property(t => t.RouteId).HasColumnName("route_id");
            e.Property(t => t.VehicleId).HasColumnName("vehicle_id");
            e.Property(t => t.DriverId).HasColumnName("driver_id");
            e.Property(t => t.TripDate).HasColumnName("trip_date");
            e.Property(t => t.StartTime).HasColumnName("start_time");
            e.Property(t => t.EndTime).HasColumnName("end_time");
            e.Property(t => t.OriginType).HasColumnName("origin_type").HasMaxLength(30);
            e.Property(t => t.OriginId).HasColumnName("origin_id");
            e.Property(t => t.DestinationType).HasColumnName("destination_type").HasMaxLength(30);
            e.Property(t => t.DestinationId).HasColumnName("destination_id");
            e.Property(t => t.Status).HasColumnName("status").HasMaxLength(30)
                .HasConversion<string>().HasDefaultValue(TripStatus.PLANNED);
            e.Property(t => t.Notes).HasColumnName("notes");
            e.Property(t => t.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
            e.Property(t => t.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
            e.Property(t => t.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");
            e.HasOne(t => t.Vehicle).WithMany(v => v.Trips).HasForeignKey(t => t.VehicleId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(t => t.Driver).WithMany(d => d.Trips).HasForeignKey(t => t.DriverId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(t => t.Route).WithMany(r => r.Trips).HasForeignKey(t => t.RouteId).OnDelete(DeleteBehavior.SetNull);
            e.HasIndex(t => t.VehicleId).HasDatabaseName("IX_Trip_Vehicle");
            e.HasIndex(t => t.DriverId).HasDatabaseName("IX_Trip_Driver");
            e.HasIndex(t => t.RouteId).HasDatabaseName("IX_Trip_Route");
            e.HasIndex(t => t.Status).HasDatabaseName("IX_Trip_Status");
            e.Ignore(t => t.DomainEvents);
        });

        // TripStop
        modelBuilder.Entity<TripStop>(e =>
        {
            e.ToTable("TripStop");
            e.HasKey(s => s.StopId);
            e.Property(s => s.StopId).HasColumnName("stop_id");
            e.Property(s => s.TripId).HasColumnName("trip_id");
            e.Property(s => s.StopSequence).HasColumnName("stop_sequence");
            e.Property(s => s.StopType).HasColumnName("stop_type").HasMaxLength(30);
            e.Property(s => s.LocationType).HasColumnName("location_type").HasMaxLength(30);
            e.Property(s => s.LocationId).HasColumnName("location_id");
            e.Property(s => s.Address).HasColumnName("address").HasMaxLength(200);
            e.Property(s => s.PlannedArrival).HasColumnName("planned_arrival");
            e.Property(s => s.ActualArrival).HasColumnName("actual_arrival");
            e.Property(s => s.PlannedDeparture).HasColumnName("planned_departure");
            e.Property(s => s.ActualDeparture).HasColumnName("actual_departure");
            e.Property(s => s.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("PENDING");
            e.Property(s => s.Notes).HasColumnName("notes");
            e.HasOne(s => s.Trip).WithMany(t => t.Stops).HasForeignKey(s => s.TripId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(s => new { s.TripId, s.StopSequence }).IsUnique().HasDatabaseName("UQ_TripStop_Sequence");
            e.HasIndex(s => s.TripId).HasDatabaseName("IX_TripStop_Trip");
            e.Ignore(s => s.DomainEvents);
        });

        // MaintenanceLog
        modelBuilder.Entity<MaintenanceLog>(e =>
        {
            e.ToTable("MaintenanceLog");
            e.HasKey(m => m.LogId);
            e.Property(m => m.LogId).HasColumnName("log_id");
            e.Property(m => m.VehicleId).HasColumnName("vehicle_id");
            e.Property(m => m.MaintenanceDate).HasColumnName("maintenance_date");
            e.Property(m => m.MaintenanceType).HasColumnName("maintenance_type").HasMaxLength(50).IsRequired();
            e.Property(m => m.Description).HasColumnName("description");
            e.Property(m => m.Cost).HasColumnName("cost").HasColumnType("decimal(18,2)");
            e.Property(m => m.OdometerReading).HasColumnName("odometer_reading");
            e.Property(m => m.NextDueDate).HasColumnName("next_due_date");
            e.Property(m => m.PerformedBy).HasColumnName("performed_by").HasMaxLength(100);
            e.Property(m => m.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
            e.HasOne(m => m.Vehicle).WithMany(v => v.MaintenanceLogs).HasForeignKey(m => m.VehicleId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(m => m.VehicleId).HasDatabaseName("IX_MaintenanceLog_Vehicle");
            e.Ignore(m => m.DomainEvents);
        });

        // FuelLog
        modelBuilder.Entity<FuelLog>(e =>
        {
            e.ToTable("FuelLog");
            e.HasKey(f => f.FuelLogId);
            e.Property(f => f.FuelLogId).HasColumnName("fuel_log_id");
            e.Property(f => f.VehicleId).HasColumnName("vehicle_id");
            e.Property(f => f.FuelDate).HasColumnName("fuel_date").HasDefaultValueSql("GETDATE()");
            e.Property(f => f.Gallons).HasColumnName("gallons").HasColumnType("decimal(18,3)");
            e.Property(f => f.Cost).HasColumnName("cost").HasColumnType("decimal(18,2)");
            e.Property(f => f.OdometerReading).HasColumnName("odometer_reading");
            e.Property(f => f.Notes).HasColumnName("notes");
            e.HasOne(f => f.Vehicle).WithMany(v => v.FuelLogs).HasForeignKey(f => f.VehicleId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(f => f.VehicleId).HasDatabaseName("IX_FuelLog_Vehicle");
            e.Ignore(f => f.DomainEvents);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, ct);

        return result;
    }
}
