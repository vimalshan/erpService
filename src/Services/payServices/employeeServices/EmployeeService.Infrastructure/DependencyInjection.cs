using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmployeeService.Domain.Repositories;
using EmployeeService.Infrastructure.Persistence;
using EmployeeService.Infrastructure.Repositories;

namespace EmployeeService.Infrastructure;

/// <summary>
/// Extension method for registering infrastructure services
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<EmployeeDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions => 
            {
                sqlOptions.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.GetName().Name);
                sqlOptions.CommandTimeout(30);
            });
        });

        // Register Repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ISalaryIncrementLogRepository, SalaryIncrementLogRepository>();

        // Register DbContext Factory (useful for background jobs)
        services.AddScoped<IDbContextFactory<EmployeeDbContext>>(provider =>
            new PooledDbContextFactory(provider.GetRequiredService<EmployeeDbContext>()));

        return services;
    }

    /// <summary>
    /// Apply pending migrations and seed initial data
    /// </summary>
    public static async Task MigrateAndSeedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
        
        // Apply migrations
        await dbContext.Database.MigrateAsync();
        
        // Seed initial data
        await SeedInitialData(dbContext);
    }

    private static async Task SeedInitialData(EmployeeDbContext context)
    {
        // Check if data already exists
        if (await context.Employees.AnyAsync())
            return;

        // Seed sample employees
        var employees = new List<Domain.Entities.Employee>
        {
            new Domain.Entities.Employee(
                employeeSystemId: 1001,
                firstName: "Rajesh",
                lastName: "Kumar",
                email: "rajesh.kumar@example.com",
                employeeCode: "EMP001",
                joiningDate: new DateTime(2020, 01, 15))
            {
                MiddleName = "Singh",
                PhoneNumber = "+91 9876543210",
                CostCenterId = "CC001"
            },
            new Domain.Entities.Employee(
                employeeSystemId: 1002,
                firstName: "Priya",
                lastName: "Sharma",
                email: "priya.sharma@example.com",
                employeeCode: "EMP002",
                joiningDate: new DateTime(2021, 03, 22))
            {
                MiddleName = "Tanvi",
                PhoneNumber = "+91 9876543211",
                CostCenterId = "CC002"
            },
            new Domain.Entities.Employee(
                employeeSystemId: 1003,
                firstName: "Amit",
                lastName: "Patel",
                email: "amit.patel@example.com",
                employeeCode: "EMP003",
                joiningDate: new DateTime(2019, 06, 10))
            {
                MiddleName = "Kumar",
                PhoneNumber = "+91 9876543212",
                CostCenterId = "CC001"
            },
            new Domain.Entities.Employee(
                employeeSystemId: 1004,
                firstName: "Neha",
                lastName: "Gupta",
                email: "neha.gupta@example.com",
                employeeCode: "EMP004",
                joiningDate: new DateTime(2022, 02, 14))
            {
                MiddleName = "Rani",
                PhoneNumber = "+91 9876543213",
                CostCenterId = "CC003"
            },
            new Domain.Entities.Employee(
                employeeSystemId: 1005,
                firstName: "Vikram",
                lastName: "Singh",
                email: "vikram.singh@example.com",
                employeeCode: "EMP005",
                joiningDate: new DateTime(2021, 09, 01))
            {
                MiddleName = "Rajendra",
                PhoneNumber = "+91 9876543214",
                CostCenterId = "CC002"
            }
        };

        // Initialize CTC for each employee
        employees[0].InitializeCTC(
            new Domain.ValueObjects.Money(600000),
            new Domain.ValueObjects.Money(300000),
            new DateTime(2020, 01, 15));

        employees[1].InitializeCTC(
            new Domain.ValueObjects.Money(550000),
            new Domain.ValueObjects.Money(275000),
            new DateTime(2021, 03, 22));

        employees[2].InitializeCTC(
            new Domain.ValueObjects.Money(750000),
            new Domain.ValueObjects.Money(375000),
            new DateTime(2019, 06, 10));

        employees[3].InitializeCTC(
            new Domain.ValueObjects.Money(500000),
            new Domain.ValueObjects.Money(250000),
            new DateTime(2022, 02, 14));

        employees[4].InitializeCTC(
            new Domain.ValueObjects.Money(650000),
            new Domain.ValueObjects.Money(325000),
            new DateTime(2021, 09, 01));

        await context.Employees.AddRangeAsync(employees);

        // Seed sample salary increment logs
        var incrementLogs = new List<Domain.Entities.SalaryIncrementLog>
        {
            new Domain.Entities.SalaryIncrementLog(
                employeeSystemId: 1001,
                oldCTC: new Domain.ValueObjects.Money(600000),
                newCTC: new Domain.ValueObjects.Money(660000),
                incrementPercentage: new Domain.ValueObjects.Percentage(10),
                effectiveDate: new DateTime(2023, 04, 01),
                approvedBy: 5001,
                approvalComments: "Annual increment 2023"),
            new Domain.Entities.SalaryIncrementLog(
                employeeSystemId: 1002,
                oldCTC: new Domain.ValueObjects.Money(550000),
                newCTC: new Domain.ValueObjects.Money(605000),
                incrementPercentage: new Domain.ValueObjects.Percentage(10),
                effectiveDate: new DateTime(2023, 04, 01),
                approvedBy: 5001,
                approvalComments: "Annual increment 2023"),
            new Domain.Entities.SalaryIncrementLog(
                employeeSystemId: 1003,
                oldCTC: new Domain.ValueObjects.Money(750000),
                newCTC: new Domain.ValueObjects.Money(825000),
                incrementPercentage: new Domain.ValueObjects.Percentage(10),
                effectiveDate: new DateTime(2023, 04, 01),
                approvedBy: 5001,
                approvalComments: "Annual increment 2023"),
            new Domain.Entities.SalaryIncrementLog(
                employeeSystemId: 1001,
                oldCTC: new Domain.ValueObjects.Money(660000),
                newCTC: new Domain.ValueObjects.Money(726000),
                incrementPercentage: new Domain.ValueObjects.Percentage(10),
                effectiveDate: new DateTime(2024, 04, 01),
                approvedBy: 5001,
                approvalComments: "Annual increment 2024")
        };

        await context.SalaryIncrementLogs.AddRangeAsync(incrementLogs);
        await context.SaveChangesAsync();
    }
}

/// <summary>
/// Pooled DbContext Factory wrapper
/// </summary>
public class PooledDbContextFactory : IDbContextFactory<EmployeeDbContext>
{
    private readonly EmployeeDbContext _dbContext;

    public PooledDbContextFactory(EmployeeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public EmployeeDbContext CreateDbContext()
    {
        return _dbContext;
    }
}
