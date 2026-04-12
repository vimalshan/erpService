using AgencyService.Domain.Common;
using AgencyService.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgencyService.Infrastructure.Data;

public class AgencyDbContext : DbContext
{
    private readonly ILogger<AgencyDbContext> _logger;
    private readonly IEventPublisher _eventPublisher;
    
    public AgencyDbContext(DbContextOptions<AgencyDbContext> options, ILogger<AgencyDbContext> logger, IEventPublisher eventPublisher)
        : base(options)
    {
        _logger = logger;
        _eventPublisher = eventPublisher;
    }
    
    public DbSet<Domain.Entities.Agency> Agencies { get; set; }
    public DbSet<Domain.Entities.Vendor> Vendors { get; set; }
    public DbSet<Domain.Entities.Airline> Airlines { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Agency configuration
        modelBuilder.Entity<Domain.Entities.Agency>(entity =>
        {
            entity.HasKey(a => a.Id);
            
            entity.Property(a => a.AgencyCode)
                .IsRequired();
            
            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.OwnsOne(a => a.ContactInfo, contact =>
            {
                contact.Property(c => c.Email).HasColumnName("Email");
                contact.Property(c => c.Phone).HasColumnName("Phone");
                contact.Property(c => c.AlternatePhone).HasColumnName("AlternatePhone");
            });
            
            entity.OwnsOne(a => a.Address, address =>
            {
                address.Property(a => a.AddressLine1).HasColumnName("AddressLine1");
                address.Property(a => a.AddressLine2).HasColumnName("AddressLine2");
                address.Property(a => a.AddressLine3).HasColumnName("AddressLine3");
                address.Property(a => a.AddressLine4).HasColumnName("AddressLine4");
            });
            
            entity.OwnsOne(a => a.Type, type =>
            {
                type.Property(t => t.Code).HasColumnName("AgencyTypeCode");
                type.Property(t => t.Name).HasColumnName("AgencyTypeName");
            });
            
            entity.Property(a => a.CreatedOn).IsRequired();
            
            entity.ToTable("AGENCY_MASTER");
        });
        
        // Vendor configuration
        modelBuilder.Entity<Domain.Entities.Vendor>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Id).ValueGeneratedNever();
            
            entity.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(65);
            
            entity.Property(v => v.CategoryType)
                .IsRequired()
                .HasMaxLength(1);
            
            entity.ToTable("VENDOR_MASTER");
        });
        
        // Airline configuration
        modelBuilder.Entity<Domain.Entities.Airline>(entity =>
        {
            entity.HasKey(a => a.Id);
            
            entity.Property(a => a.Code)
                .IsRequired()
                .HasMaxLength(3);
            
            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.ToTable("AIRLINE_MAST");
        });
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Publish domain events via IEventPublisher
        foreach (var domainEvent in domainEvents)
        {
            try
            {
                await _eventPublisher.PublishAsync(domainEvent);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to publish domain event {EventType}", domainEvent.GetType().Name);
            }
        }

        // Clear domain events
        foreach (var entity in domainEntities)
        {
            entity.Entity.ClearDomainEvents();
        }

        return result;
    }
}
