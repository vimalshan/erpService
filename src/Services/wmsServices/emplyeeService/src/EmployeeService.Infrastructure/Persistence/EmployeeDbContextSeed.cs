using EmployeeService.Domain.Entities;
using EmployeeService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Infrastructure.Persistence;

public static class EmployeeDbContextSeed
{
    public static async Task SeedAsync(EmployeeDbContext context)
    {
        // Apply pending migrations first
        await context.Database.MigrateAsync();

        if (await context.Employees.AnyAsync())
            return;

        var employees = new List<Employee>
        {
            Employee.Create("John", "Doe", "EMP-001", new DateTime(2020, 1, 15),
                "Warehouse Manager", "Operations", null, null, "+1234567890", "john.doe@company.com"),
            Employee.Create("Jane", "Smith", "EMP-002", new DateTime(2021, 3, 20),
                "Inventory Analyst", "Inventory", null, null, "+0987654321", "jane.smith@company.com"),
            Employee.Create("Bob", "Johnson", "EMP-003", new DateTime(2022, 6, 10),
                "Picker", "Warehouse", null, null, "+1122334455", "bob.johnson@company.com"),
            Employee.Create("Alice", "Williams", "EMP-004", new DateTime(2023, 2, 1),
                "Shipping Coordinator", "Logistics", null, null, "+5566778899", "alice.williams@company.com"),
            Employee.Create("Charlie", "Brown", "EMP-005", new DateTime(2019, 11, 30),
                "Quality Inspector", "Quality", null, null, "+6677889900", "charlie.brown@company.com"),
        };

        // Clear domain events from seed data
        foreach (var employee in employees)
        {
            employee.ClearDomainEvents();
        }

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();
    }
}
