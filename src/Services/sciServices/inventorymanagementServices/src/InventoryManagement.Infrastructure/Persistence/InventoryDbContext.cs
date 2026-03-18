using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<MainProductMaster> MainProductMasters => Set<MainProductMaster>();
    public DbSet<ProductMaster> ProductMasters => Set<ProductMaster>();
    public DbSet<ProductTypeMaster> ProductTypeMasters => Set<ProductTypeMaster>();
    public DbSet<ItemMaster> ItemMasters => Set<ItemMaster>();
    public DbSet<PackageType> PackageTypes => Set<PackageType>();
    public DbSet<ItemCapacity> ItemCapacities => Set<ItemCapacity>();
    public DbSet<ItemGrade> ItemGrades => Set<ItemGrade>();
    public DbSet<GradeMaster> GradeMasters => Set<GradeMaster>();
    public DbSet<ItemMap> ItemMaps => Set<ItemMap>();
    public DbSet<MaterialTaxClass> MaterialTaxClasses => Set<MaterialTaxClass>();
    public DbSet<UnitOfMeasure> UnitsOfMeasure => Set<UnitOfMeasure>();
    public DbSet<UnitsClass> UnitsClasses => Set<UnitsClass>();
    public DbSet<AdvanceLicenseMaster> AdvanceLicenseMasters => Set<AdvanceLicenseMaster>();
    public DbSet<AdvanceLicenseEntitlement> AdvanceLicenseEntitlements => Set<AdvanceLicenseEntitlement>();
    public DbSet<ItemType> ItemTypes => Set<ItemType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
    }
}
