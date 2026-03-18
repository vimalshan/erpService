using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using UtilityService.Infrastructure.Data;

#nullable disable

namespace UtilityService.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260315000001_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TOAD_PLAN_SQL",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                USERNAME = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                STATEMENT_ID = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
                TIMESTAMP = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                STATEMENT = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                IS_DELETED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TOAD_PLAN_SQL", x => x.ID);
            });

        migrationBuilder.CreateIndex(
            name: "IX_TOAD_PLAN_SQL_STATEMENT_ID",
            table: "TOAD_PLAN_SQL",
            column: "STATEMENT_ID");

        migrationBuilder.CreateIndex(
            name: "IX_TOAD_PLAN_SQL_USERNAME",
            table: "TOAD_PLAN_SQL",
            column: "USERNAME");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "TOAD_PLAN_SQL");
    }
}
