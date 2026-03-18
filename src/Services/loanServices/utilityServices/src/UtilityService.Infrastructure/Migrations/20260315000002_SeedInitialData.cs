using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using UtilityService.Infrastructure.Data;

#nullable disable

namespace UtilityService.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260315000002_SeedInitialData")]
public partial class SeedInitialData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            INSERT INTO TOAD_PLAN_SQL (USERNAME, STATEMENT_ID, [TIMESTAMP], STATEMENT, IS_DELETED, CREATED_AT)
            VALUES
                ('SYSTEM', 'SEED-001', GETUTCDATE(), 'SELECT * FROM INFORMATION_SCHEMA.TABLES', 0, GETUTCDATE()),
                ('SYSTEM', 'SEED-002', GETUTCDATE(), 'SELECT COUNT(*) FROM TOAD_PLAN_SQL', 0, GETUTCDATE())
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DELETE FROM TOAD_PLAN_SQL WHERE STATEMENT_ID IN ('SEED-001', 'SEED-002')
            """);
    }
}
