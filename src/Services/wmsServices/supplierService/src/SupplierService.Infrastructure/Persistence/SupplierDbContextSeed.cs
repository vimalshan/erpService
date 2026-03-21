using Microsoft.EntityFrameworkCore;

namespace SupplierService.Infrastructure.Persistence;

public static class SupplierDbContextSeed
{
    public static async Task SeedAsync(SupplierDbContext context)
    {
        if (await context.Suppliers.AnyAsync())
            return;

        await context.Database.ExecuteSqlRawAsync(@"
            SET IDENTITY_INSERT [Supplier] ON;

            INSERT INTO [Supplier] (supplier_id, code, name, contact_person, email, phone, address, city, state, country, postal_code, is_active, created_date, modified_date)
            VALUES
            (1, 'SUP-001', 'Acme Corp', 'John Doe', 'john@acmecorp.com', '+1-555-0100', '123 Main St', 'New York', 'NY', 'USA', '10001', 1, GETDATE(), GETDATE()),
            (2, 'SUP-002', 'Global Supplies Ltd', 'Jane Smith', 'jane@globalsupplies.com', '+1-555-0200', '456 Oak Ave', 'Los Angeles', 'CA', 'USA', '90001', 1, GETDATE(), GETDATE()),
            (3, 'SUP-003', 'TechParts Inc', 'Bob Wilson', 'bob@techparts.com', '+1-555-0300', '789 Pine Rd', 'Chicago', 'IL', 'USA', '60601', 1, GETDATE(), GETDATE()),
            (4, 'SUP-004', 'Quality Materials Co', 'Alice Brown', 'alice@qualitymaterials.com', '+1-555-0400', '321 Elm Blvd', 'Houston', 'TX', 'USA', '77001', 0, GETDATE(), GETDATE()),
            (5, 'SUP-005', 'FastShip Logistics', 'Charlie Davis', 'charlie@fastship.com', '+1-555-0500', '654 Maple Dr', 'Phoenix', 'AZ', 'USA', '85001', 1, GETDATE(), GETDATE());

            SET IDENTITY_INSERT [Supplier] OFF;
        ");
    }
}
