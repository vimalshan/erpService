using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthProvider.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // ── Users ────────────────────────────────────────────────────────────
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id               = table.Column<Guid>(nullable: false),
                Username         = table.Column<string>(maxLength: 50, nullable: false),
                Email            = table.Column<string>(maxLength: 320, nullable: false),
                PasswordHash     = table.Column<string>(maxLength: 256, nullable: false),
                FirstName        = table.Column<string>(maxLength: 100, nullable: false),
                LastName         = table.Column<string>(maxLength: 100, nullable: false),
                IsActive         = table.Column<bool>(nullable: false, defaultValue: true),
                IsEmailVerified  = table.Column<bool>(nullable: false, defaultValue: false),
                CreatedAt        = table.Column<DateTime>(nullable: false),
                UpdatedAt        = table.Column<DateTime>(nullable: true),
                LastLoginAt      = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateIndex(name: "IX_Users_Username", table: "Users", column: "Username", unique: true);
        migrationBuilder.CreateIndex(name: "IX_Users_Email",    table: "Users", column: "Email",    unique: true);

        // ── Roles ────────────────────────────────────────────────────────────
        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id          = table.Column<Guid>(nullable: false),
                Name        = table.Column<string>(maxLength: 100, nullable: false),
                Description = table.Column<string>(maxLength: 500, nullable: false, defaultValue: ""),
                IsActive    = table.Column<bool>(nullable: false, defaultValue: true),
                CreatedAt   = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateIndex(name: "IX_Roles_Name", table: "Roles", column: "Name", unique: true);

        // ── Permissions ──────────────────────────────────────────────────────
        migrationBuilder.CreateTable(
            name: "Permissions",
            columns: table => new
            {
                Id        = table.Column<Guid>(nullable: false),
                Name      = table.Column<string>(maxLength: 200, nullable: false),
                Resource  = table.Column<string>(maxLength: 100, nullable: false),
                Action    = table.Column<string>(maxLength: 50, nullable: false),
                CreatedAt = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
            });

        // ── UserRoles ────────────────────────────────────────────────────────
        migrationBuilder.CreateTable(
            name: "UserRoles",
            columns: table => new
            {
                UserId     = table.Column<Guid>(nullable: false),
                RoleId     = table.Column<Guid>(nullable: false),
                AssignedAt = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey("FK_UserRoles_Users", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_UserRoles_Roles", x => x.RoleId, "Roles", "Id", onDelete: ReferentialAction.Cascade);
            });

        // ── RolePermissions ──────────────────────────────────────────────────
        migrationBuilder.CreateTable(
            name: "RolePermissions",
            columns: table => new
            {
                RoleId       = table.Column<Guid>(nullable: false),
                PermissionId = table.Column<Guid>(nullable: false),
                AssignedAt   = table.Column<DateTime>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                table.ForeignKey("FK_RolePermissions_Roles",       x => x.RoleId,       "Roles",       "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_RolePermissions_Permissions", x => x.PermissionId, "Permissions", "Id", onDelete: ReferentialAction.Cascade);
            });

        // ── RefreshTokens ────────────────────────────────────────────────────
        migrationBuilder.CreateTable(
            name: "RefreshTokens",
            columns: table => new
            {
                Id          = table.Column<Guid>(nullable: false),
                UserId      = table.Column<Guid>(nullable: false),
                Token       = table.Column<string>(maxLength: 500, nullable: false),
                ExpiresAt   = table.Column<DateTime>(nullable: false),
                CreatedAt   = table.Column<DateTime>(nullable: false),
                CreatedByIp = table.Column<string>(maxLength: 50, nullable: false, defaultValue: ""),
                RevokedAt   = table.Column<DateTime>(nullable: true),
                RevokedByIp = table.Column<string>(maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                table.ForeignKey("FK_RefreshTokens_Users", x => x.UserId, "Users", "Id", onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(name: "IX_RefreshTokens_Token",  table: "RefreshTokens", column: "Token");
        migrationBuilder.CreateIndex(name: "IX_RefreshTokens_UserId", table: "RefreshTokens", column: "UserId");

        // ── AuditLogs ────────────────────────────────────────────────────────
        migrationBuilder.CreateTable(
            name: "AuditLogs",
            columns: table => new
            {
                Id        = table.Column<Guid>(nullable: false),
                UserId    = table.Column<Guid>(nullable: true),
                Action    = table.Column<string>(maxLength: 100, nullable: false),
                Resource  = table.Column<string>(maxLength: 200, nullable: false),
                Details   = table.Column<string>(maxLength: 2000, nullable: true),
                IpAddress = table.Column<string>(maxLength: 50, nullable: false, defaultValue: ""),
                Timestamp = table.Column<DateTime>(nullable: false),
                IsSuccess = table.Column<bool>(nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuditLogs", x => x.Id);
            });

        migrationBuilder.CreateIndex(name: "IX_AuditLogs_UserId",    table: "AuditLogs", column: "UserId");
        migrationBuilder.CreateIndex(name: "IX_AuditLogs_Timestamp", table: "AuditLogs", column: "Timestamp");

        // ── Seed Reference Data ──────────────────────────────────────────────
        SeedRoles(migrationBuilder);
        SeedPermissions(migrationBuilder);
        SeedRolePermissions(migrationBuilder);
    }

    private static void SeedRoles(MigrationBuilder m)
    {
        m.InsertData("Roles",
            new[] { "Id", "Name", "Description", "IsActive", "CreatedAt" },
            new object[,]
            {
                { new Guid("22222222-0001-0001-0001-000000000001"), "ADMIN",   "Full system administrator", true, DateTime.UtcNow },
                { new Guid("22222222-0002-0001-0001-000000000001"), "USER",    "Standard end user",         true, DateTime.UtcNow },
                { new Guid("22222222-0003-0001-0001-000000000001"), "AUDITOR", "Read-only audit access",    true, DateTime.UtcNow }
            });
    }

    private static void SeedPermissions(MigrationBuilder m)
    {
        m.InsertData("Permissions",
            new[] { "Id", "Name", "Resource", "Action", "CreatedAt" },
            new object[,]
            {
                { new Guid("11111111-0001-0001-0001-000000000001"), "View Users",      "users", "read",   DateTime.UtcNow },
                { new Guid("11111111-0001-0001-0001-000000000002"), "Create Users",    "users", "create", DateTime.UtcNow },
                { new Guid("11111111-0001-0001-0001-000000000003"), "Update Users",    "users", "update", DateTime.UtcNow },
                { new Guid("11111111-0001-0001-0001-000000000004"), "Delete Users",    "users", "delete", DateTime.UtcNow },
                { new Guid("11111111-0002-0001-0001-000000000001"), "View Roles",      "roles", "read",   DateTime.UtcNow },
                { new Guid("11111111-0002-0001-0001-000000000002"), "Manage Roles",    "roles", "manage", DateTime.UtcNow },
                { new Guid("11111111-0003-0001-0001-000000000001"), "View Audit Logs", "audit", "read",   DateTime.UtcNow }
            });
    }

    private static void SeedRolePermissions(MigrationBuilder m)
    {
        m.InsertData("RolePermissions",
            new[] { "RoleId", "PermissionId", "AssignedAt" },
            new object[,]
            {
                // ADMIN – all permissions
                { new Guid("22222222-0001-0001-0001-000000000001"), new Guid("11111111-0001-0001-0001-000000000001"), DateTime.UtcNow },
                { new Guid("22222222-0001-0001-0001-000000000001"), new Guid("11111111-0001-0001-0001-000000000002"), DateTime.UtcNow },
                { new Guid("22222222-0001-0001-0001-000000000001"), new Guid("11111111-0001-0001-0001-000000000003"), DateTime.UtcNow },
                { new Guid("22222222-0001-0001-0001-000000000001"), new Guid("11111111-0001-0001-0001-000000000004"), DateTime.UtcNow },
                { new Guid("22222222-0001-0001-0001-000000000001"), new Guid("11111111-0002-0001-0001-000000000001"), DateTime.UtcNow },
                { new Guid("22222222-0001-0001-0001-000000000001"), new Guid("11111111-0002-0001-0001-000000000002"), DateTime.UtcNow },
                { new Guid("22222222-0001-0001-0001-000000000001"), new Guid("11111111-0003-0001-0001-000000000001"), DateTime.UtcNow },
                // USER – view + update
                { new Guid("22222222-0002-0001-0001-000000000001"), new Guid("11111111-0001-0001-0001-000000000001"), DateTime.UtcNow },
                { new Guid("22222222-0002-0001-0001-000000000001"), new Guid("11111111-0001-0001-0001-000000000003"), DateTime.UtcNow },
                // AUDITOR – view users + audit
                { new Guid("22222222-0003-0001-0001-000000000001"), new Guid("11111111-0001-0001-0001-000000000001"), DateTime.UtcNow },
                { new Guid("22222222-0003-0001-0001-000000000001"), new Guid("11111111-0003-0001-0001-000000000001"), DateTime.UtcNow }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("AuditLogs");
        migrationBuilder.DropTable("RefreshTokens");
        migrationBuilder.DropTable("RolePermissions");
        migrationBuilder.DropTable("UserRoles");
        migrationBuilder.DropTable("Users");
        migrationBuilder.DropTable("Roles");
        migrationBuilder.DropTable("Permissions");
    }
}
