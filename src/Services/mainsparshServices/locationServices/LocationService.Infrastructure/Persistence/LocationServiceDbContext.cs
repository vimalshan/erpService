using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LocationService.Domain.Aggregates;
using LocationService.Domain.Entities;
using LocationService.Domain.ValueObjects;

namespace LocationService.Infrastructure.Persistence
{
    /// <summary>
    /// Entity Framework DbContext for Location Service
    /// </summary>
    public class LocationServiceDbContext : DbContext
    {
        public LocationServiceDbContext(DbContextOptions<LocationServiceDbContext> options) : base(options)
        {
        }

        public DbSet<LocationAggregate> Locations { get; set; }
        public DbSet<RoomAggregate> Rooms { get; set; }
        public DbSet<RoomResourceAggregate> RoomResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exclude DomainEvent from EF Core mapping
            modelBuilder.Ignore<DomainEvent>();

            // Configure Location Aggregate
            ConfigureLocationAggregate(modelBuilder);

            // Configure Room Aggregate
            ConfigureRoomAggregate(modelBuilder);

            // Configure RoomResource Aggregate
            ConfigureRoomResourceAggregate(modelBuilder);
        }

        private void ConfigureLocationAggregate(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<LocationAggregate>();

            builder.ToTable("LOCATION_CONTACT");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("LOCATION_ID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LocationCode)
                .HasColumnName("LOCATION_CODE")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.LocationName)
                .HasColumnName("LOCATION_NAME")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.LocationStatus)
                .HasColumnName("LOCATION_STATUS")
                .HasMaxLength(1)
                .HasConversion(
                    v => v.Value,
                    v => new Status(v)
                )
                .HasDefaultValueSql("'A'");

            builder.OwnsOne(x => x.Address, ad =>
            {
                ad.Property(a => a.StreetAddress)
                    .HasColumnName("LOCATION_ADDRESS")
                    .HasColumnType("NVARCHAR(MAX)");
                ad.Property(a => a.City)
                    .HasColumnName("CITY")
                    .HasMaxLength(100);
                ad.Property(a => a.State)
                    .HasColumnName("STATE")
                    .HasMaxLength(100);
                ad.Property(a => a.PostalCode)
                    .HasColumnName("PIN_CODE")
                    .HasMaxLength(10);
                ad.Property(a => a.Country)
                    .HasColumnName("COUNTRY")
                    .HasMaxLength(100);
            });

            builder.OwnsOne(x => x.Contact, c =>
            {
                c.Property(a => a.Phone)
                    .HasColumnName("PHONE")
                    .HasMaxLength(20);
                c.Property(a => a.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(255);
                c.Property(a => a.ContactPerson)
                    .HasColumnName("CONTACT_PERSON")
                    .HasMaxLength(255);
            });

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CREATED_BY")
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasColumnName("CREATED_ON")
                .HasColumnType("DATETIME2(3)")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            builder.Property(x => x.UpdatedOn)
                .HasColumnName("UPDATED_ON")
                .HasColumnType("DATETIME2(3)");

            builder.HasIndex(x => x.LocationCode).HasDatabaseName("IX_LOCATION_CONTACT_CODE");
            builder.HasIndex(x => x.LocationStatus).HasDatabaseName("IX_LOCATION_CONTACT_STATUS");

            builder.HasMany(x => x.Rooms)
                .WithOne()
                .HasForeignKey(r => r.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureRoomAggregate(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<RoomAggregate>();

            builder.ToTable("ROOM_MAST");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("ROOM_ID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LocationId)
                .HasColumnName("LOCATION_ID")
                .IsRequired();

            builder.Property(x => x.RoomCode)
                .HasColumnName("ROOM_CODE")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.RoomName)
                .HasColumnName("ROOM_NAME")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.RoomCapacity)
                .HasColumnName("ROOM_CAPACITY");

            builder.Property(x => x.RoomType)
                .HasColumnName("ROOM_TYPE")
                .HasMaxLength(50);

            builder.Property(x => x.FloorNumber)
                .HasColumnName("FLOOR_NUMBER");

            builder.Property(x => x.RoomStatus)
                .HasColumnName("ROOM_STATUS")
                .HasMaxLength(1)
                .HasConversion(
                    v => v.Value,
                    v => new Status(v)
                )
                .HasDefaultValueSql("'A'");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CREATED_BY")
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasColumnName("CREATED_ON")
                .HasColumnType("DATETIME2(3)")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            builder.Property(x => x.UpdatedOn)
                .HasColumnName("UPDATED_ON")
                .HasColumnType("DATETIME2(3)");

            builder.HasIndex(x => x.LocationId).HasDatabaseName("IX_ROOM_MAST_LOCATION_ID");
            builder.HasIndex(x => x.RoomCode).HasDatabaseName("IX_ROOM_MAST_CODE");
            builder.HasIndex(x => x.RoomType).HasDatabaseName("IX_ROOM_MAST_TYPE");

            builder.HasIndex(x => new { x.LocationId, x.RoomCode })
                .IsUnique()
                .HasDatabaseName("UC_ROOM_CODE_LOCATION");

            builder.HasMany(x => x.Resources)
                .WithOne()
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureRoomResourceAggregate(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<RoomResourceAggregate>();

            builder.ToTable("ROOM_RESOURCE");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("RESOURCE_ID")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.RoomId)
                .HasColumnName("ROOM_ID")
                .IsRequired();

            builder.Property(x => x.LocationId)
                .HasColumnName("LOCATION_ID")
                .IsRequired();

            builder.Property(x => x.ResourceCode)
                .HasColumnName("RESOURCE_CODE")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ResourceName)
                .HasColumnName("RESOURCE_NAME")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.ResourceType)
                .HasColumnName("RESOURCE_TYPE")
                .HasMaxLength(100);

            builder.Property(x => x.ResourceQuantity)
                .HasColumnName("RESOURCE_QUANTITY");

            builder.Property(x => x.ResourceStatus)
                .HasColumnName("RESOURCE_STATUS")
                .HasMaxLength(1)
                .HasConversion(
                    v => v.Value,
                    v => new Status(v)
                )
                .HasDefaultValueSql("'A'");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CREATED_BY")
                .IsRequired();

            builder.Property(x => x.CreatedOn)
                .HasColumnName("CREATED_ON")
                .HasColumnType("DATETIME2(3)")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("UPDATED_BY");

            builder.Property(x => x.UpdatedOn)
                .HasColumnName("UPDATED_ON")
                .HasColumnType("DATETIME2(3)");

            builder.HasIndex(x => x.RoomId).HasDatabaseName("IX_ROOM_RESOURCE_ROOM_ID");
            builder.HasIndex(x => x.LocationId).HasDatabaseName("IX_ROOM_RESOURCE_LOCATION_ID");
            builder.HasIndex(x => x.ResourceType).HasDatabaseName("IX_ROOM_RESOURCE_TYPE");
        }
    }
}
