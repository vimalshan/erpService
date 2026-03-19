using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StrategicStock.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "STRATEGIC_STOCK",
                columns: table => new
                {
                    STRATEGIC_STOCK_ID = table.Column<int>(type: "int", nullable: false),
                    COMPANY_UNIT_ID = table.Column<int>(type: "int", nullable: true),
                    SCI_ITEM_ID = table.Column<int>(type: "int", nullable: false),
                    STRATEGIC_STOCK_TYPE = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    MAX_QTY = table.Column<long>(type: "bigint", nullable: true),
                    EFFECTIVE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CLOSURE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FILLED_QTY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STRATEGIC_STOCK", x => x.STRATEGIC_STOCK_ID);
                });

            migrationBuilder.InsertData(
                table: "STRATEGIC_STOCK",
                columns: new[] { "STRATEGIC_STOCK_ID", "CLOSURE_DATE", "COMPANY_UNIT_ID", "CREATION_DATE", "EFFECTIVE_DATE", "FILLED_QTY", "MAX_QTY", "MODIFIED_DATE", "SCI_ITEM_ID", "SCI_USER_ID_CREATED", "SCI_USER_ID_MODIFIED", "STRATEGIC_STOCK_TYPE" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2026-01-01", 1200L, 5000L, null, 1001, 1, null, "NR" },
                    { 2, null, 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2026-02-01", 800L, 3000L, null, 1002, 1, null, "EM" },
                    { 3, null, 2, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "2026-03-01", 4500L, 10000L, null, 1003, 2, null, "BF" },
                    { 4, "2025-12-31", 2, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "2025-06-15", 2000L, 2000L, "2025-12-31", 1004, 1, 1, "NR" },
                    { 5, null, 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "2026-01-15", 0L, 7500L, null, 1005, 2, null, "EM" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "STRATEGIC_STOCK");
        }
    }
}
