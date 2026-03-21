using CustomerService.Domain.Common;
using CustomerService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence;

public class CustomerDbContext : DbContext
{
    private readonly IMediator _mediator;

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");
            entity.HasKey(e => e.CustomerId);
            entity.Property(e => e.CustomerId).HasColumnName("customer_id").UseIdentityColumn();
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(30).IsRequired();
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.CompanyName).HasColumnName("company_name").HasMaxLength(100);
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasColumnName("created_date").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.ModifiedDate).HasColumnName("modified_date").HasDefaultValueSql("GETDATE()");

            // Value Object: ContactInfo (owned type mapped to columns)
            entity.OwnsOne(e => e.Contact, contact =>
            {
                contact.Property(c => c.ContactPerson).HasColumnName("contact_person").HasMaxLength(100);
                contact.Property(c => c.ContactTitle).HasColumnName("contact_title").HasMaxLength(50);
                contact.Property(c => c.Email).HasColumnName("email").HasMaxLength(100);
                contact.Property(c => c.Phone).HasColumnName("phone").HasMaxLength(30);
            });

            // Value Object: Address (owned type mapped to columns)
            entity.OwnsOne(e => e.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("address").HasMaxLength(200);
                address.Property(a => a.City).HasColumnName("city").HasMaxLength(50);
                address.Property(a => a.State).HasColumnName("state").HasMaxLength(50);
                address.Property(a => a.Country).HasColumnName("country").HasMaxLength(50);
                address.Property(a => a.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
            });

            entity.Ignore(e => e.DomainEvents);
        });

        // Seed data
        modelBuilder.Entity<Customer>().HasData(
            new
            {
                CustomerId = 1,
                Code = "CUST001",
                Name = "Acme Corporation",
                CompanyName = "Acme Corp",
                IsActive = true,
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifiedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                CustomerId = 2,
                Code = "CUST002",
                Name = "Globex Industries",
                CompanyName = "Globex Inc",
                IsActive = true,
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifiedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                CustomerId = 3,
                Code = "CUST003",
                Name = "Wayne Enterprises",
                CompanyName = "Wayne Corp",
                IsActive = true,
                CreatedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifiedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Owned type seed data must be seeded separately
        modelBuilder.Entity<Customer>().OwnsOne(e => e.Contact).HasData(
            new { CustomerId = 1, ContactPerson = "John Doe", ContactTitle = "Sales Manager", Email = "john@acme.com", Phone = "+1-555-0101" },
            new { CustomerId = 2, ContactPerson = "Jane Smith", ContactTitle = "Director", Email = "jane@globex.com", Phone = "+1-555-0102" },
            new { CustomerId = 3, ContactPerson = "Bruce Wayne", ContactTitle = "CEO", Email = "bruce@wayne.com", Phone = "+1-555-0103" }
        );

        modelBuilder.Entity<Customer>().OwnsOne(e => e.Address).HasData(
            new { CustomerId = 1, Street = "123 Main St", City = "Springfield", State = "IL", Country = "USA", PostalCode = "62701" },
            new { CustomerId = 2, Street = "456 Industrial Blvd", City = "Shelbyville", State = "IL", Country = "USA", PostalCode = "62565" },
            new { CustomerId = 3, Street = "1007 Mountain Dr", City = "Gotham", State = "NJ", Country = "USA", PostalCode = "07001" }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        // Clear domain events after publishing
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
