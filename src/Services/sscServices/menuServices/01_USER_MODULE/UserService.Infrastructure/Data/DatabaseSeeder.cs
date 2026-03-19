using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data;

/// <summary>
/// Seeds initial data into the database if not already present.
/// Passwords are BCrypt-hashed at seeding time — never stored in plaintext.
///
/// Default credentials:
///   admin@company.com      / Admin@123456
///   john.doe@company.com   / User@123456
///   jane.smith@company.com / User@123456
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(UserServiceDbContext db, ILogger logger)
    {
        if (await db.Users.AnyAsync())
        {
            logger.LogInformation("Database already seeded — skipping.");
            return;
        }

        logger.LogInformation("Seeding database with default users...");

        var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin@123456", workFactor: 11);
        var userHash  = BCrypt.Net.BCrypt.HashPassword("User@123456",  workFactor: 11);

        var admin = User.Create("Admin User",  adminHash, "admin@company.com",      enteredBy: 1);
        var john  = User.Create("John Doe",    userHash,  "john.doe@company.com",   enteredBy: 1);
        var jane  = User.Create("Jane Smith",  userHash,  "jane.smith@company.com", enteredBy: 1);

        db.Users.AddRange(admin, john, jane);
        await db.SaveChangesAsync();

        var now = DateTime.UtcNow;

        db.UserRoleMappings.AddRange(
            new UserRoleMapping { UserId = admin.Id, RoleId = 7, IsDefault = true, CreatedDate = now, CreatedBy = 1 },
            new UserRoleMapping { UserId = john.Id,  RoleId = 1, IsDefault = true, CreatedDate = now, CreatedBy = 1 },
            new UserRoleMapping { UserId = jane.Id,  RoleId = 1, IsDefault = true, CreatedDate = now, CreatedBy = 1 });

        db.UserOrganizationMappings.AddRange(
            new UserOrganizationMapping { UserId = admin.Id, BusinessUnitId = "ORG001", CreatedDate = now, CreatedBy = 1 },
            new UserOrganizationMapping { UserId = john.Id,  BusinessUnitId = "ORG002", CreatedDate = now, CreatedBy = 1 },
            new UserOrganizationMapping { UserId = jane.Id,  BusinessUnitId = "ORG001", CreatedDate = now, CreatedBy = 1 });

        db.UserLocationMappings.AddRange(
            new UserLocationMapping { UserId = admin.Id, LocationId = 1, CreatedDate = now, CreatedBy = 1 },
            new UserLocationMapping { UserId = john.Id,  LocationId = 2, CreatedDate = now, CreatedBy = 1 },
            new UserLocationMapping { UserId = jane.Id,  LocationId = 1, CreatedDate = now, CreatedBy = 1 });

        await db.SaveChangesAsync();

        logger.LogInformation("Seeding complete — 3 users created with roles, orgs and locations.");
    }
}
