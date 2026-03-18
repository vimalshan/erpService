using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeService.Infrastructure.Data
{
    /// <summary>
    /// Database initializer for creating and seeding the database schema
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Initializes the database by creating all tables if they don't exist
        /// </summary>
        public static async Task InitializeAsync(EmployeeServiceDbContext context, ILogger? logger = null)
        {
            try
            {
                logger?.LogInformation("Starting database initialization...");

                // Delete existing database if it exists to ensure clean schema with updated configuration
                await context.Database.EnsureDeletedAsync();

                // Create database and tables with current schema
                await context.Database.EnsureCreatedAsync();
                
                logger?.LogInformation("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error during database initialization: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Seeds initial data into the database
        /// </summary>
        public static async Task SeedDataAsync(EmployeeServiceDbContext context, ILogger? logger = null)
        {
            try
            {
                logger?.LogInformation("Starting database seeding...");

                // Check if employees already exist to avoid duplicate seeding
                if (context.Employees.Any())
                {
                    logger?.LogInformation("Database already contains employees. Skipping seed data.");
                    return;
                }

                // Seed sample employees
                var employees = new List<Domain.Entities.Employee>();

                // Sample employee 1
                var emp1 = Domain.Entities.Employee.Create(
                    personalInfo: new Domain.ValueObjects.PersonalInfo("John", "Johnson", new DateTime(1985, 5, 15), 'M', "Robert"),
                    contactInfo: new Domain.ValueObjects.ContactInfo("john.johnson@company.com", "9876543210", "9876543211"),
                    employmentDetails: new Domain.ValueObjects.EmploymentDetails("EMP001", "john.johnson", "John", new DateTime(2015, 1, 10), new DateTime(2015, 1, 10)),
                    gradeInfo: new Domain.ValueObjects.GradeInfo("A-1", "Senior Manager", 1001, "Management", "MGT"),
                    organizationalAssignment: new Domain.ValueObjects.OrganizationalAssignment(1, 100, "HRD", "Human Resources", "Head of HR", "HRD"),
                    salaryInfo: new Domain.ValueObjects.SalaryInfo(150000, "MON")
                );
                
                if (emp1 != null)
                {
                    emp1.OrganizationalAssignment.CurrentLevelId = 5;
                }

                if (emp1 != null)
                {
                    emp1.Status = "ACTIVE";
                    emp1.Salutation = "Mr.";
                    emp1.InclusionStatus = "INCLUDED";
                    employees.Add(emp1);
                }

                // Sample employee 2
                var emp2 = Domain.Entities.Employee.Create(
                    personalInfo: new Domain.ValueObjects.PersonalInfo("Sarah", "Williams", new DateTime(1990, 8, 22), 'F', "Michelle"),
                    contactInfo: new Domain.ValueObjects.ContactInfo("sarah.williams@company.com", "9876543212", "9876543213"),
                    employmentDetails: new Domain.ValueObjects.EmploymentDetails("EMP002", "sarah.williams", "Sarah", new DateTime(2018, 3, 15), new DateTime(2018, 3, 15)),
                    gradeInfo: new Domain.ValueObjects.GradeInfo("A-2", "Manager", 1002, "Management", "MGT"),
                    organizationalAssignment: new Domain.ValueObjects.OrganizationalAssignment(2, 101, "FIN", "Finance", "Finance Manager", "FIN"),
                    salaryInfo: new Domain.ValueObjects.SalaryInfo(120000, "MON")
                );
                
                if (emp2 != null)
                {
                    emp2.OrganizationalAssignment.CurrentLevelId = 4;
                }

                if (emp2 != null)
                {
                    emp2.Status = "ACTIVE";
                    emp2.Salutation = "Ms.";
                    emp2.InclusionStatus = "INCLUDED";
                    employees.Add(emp2);
                }

                // Sample employee 3
                var emp3 = Domain.Entities.Employee.Create(
                    personalInfo: new Domain.ValueObjects.PersonalInfo("Michael", "Brown", new DateTime(1988, 11, 30), 'M', "David"),
                    contactInfo: new Domain.ValueObjects.ContactInfo("michael.brown@company.com", "9876543214", "9876543215"),
                    employmentDetails: new Domain.ValueObjects.EmploymentDetails("EMP003", "michael.brown", "Michael", new DateTime(2019, 6, 1), new DateTime(2019, 6, 1)),
                    gradeInfo: new Domain.ValueObjects.GradeInfo("B-1", "Senior Executive", 1003, "Executive", "EXE"),
                    organizationalAssignment: new Domain.ValueObjects.OrganizationalAssignment(3, 102, "OPS", "Operations", "Operations Lead", "OPS"),
                    salaryInfo: new Domain.ValueObjects.SalaryInfo(95000, "MON")
                );
                
                if (emp3 != null)
                {
                    emp3.OrganizationalAssignment.CurrentLevelId = 3;
                }

                if (emp3 != null)
                {
                    emp3.Status = "ACTIVE";
                    emp3.Salutation = "Mr.";
                    emp3.InclusionStatus = "INCLUDED";
                    employees.Add(emp3);
                }

                // Add employees to context
                foreach (var employee in employees)
                {
                    context.Employees.Add(employee);
                }

                // Save changes
                await context.SaveChangesAsync();
                logger?.LogInformation("Database seeding completed successfully. Added {Count} employees.", employees.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error during database seeding: {Message}", ex.Message);
                // Don't throw on seed errors - log and continue
            }
        }

        /// <summary>
        /// Completely resets the database (for development/testing only)
        /// </summary>
        public static async Task ResetDatabaseAsync(EmployeeServiceDbContext context, ILogger? logger = null)
        {
            try
            {
                logger?.LogWarning("Resetting database - all data will be deleted!");
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                logger?.LogInformation("Database reset completed");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error during database reset: {Message}", ex.Message);
                throw;
            }
        }
    }
}
