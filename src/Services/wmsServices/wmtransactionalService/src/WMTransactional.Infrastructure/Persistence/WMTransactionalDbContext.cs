using Microsoft.EntityFrameworkCore;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence;

public class WMTransactionalDbContext : DbContext
{
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();
    public DbSet<Receiving> Receivings => Set<Receiving>();
    public DbSet<ReceivingLine> ReceivingLines => Set<ReceivingLine>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderLine> SalesOrderLines => Set<SalesOrderLine>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<ShipmentLine> ShipmentLines => Set<ShipmentLine>();

    public WMTransactionalDbContext(DbContextOptions<WMTransactionalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WMTransactionalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
