using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MasterDataService.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HOLDTYPE_MAST",
                columns: table => new
                {
                    HOLD_ID = table.Column<long>(type: "bigint", nullable: false),
                    HOLD_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HOLD_CATEGORY = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOLDTYPE_MAST", x => x.HOLD_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOCATION_SCANPARAMS",
                columns: table => new
                {
                    LOCSCANPARAM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LOC_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCATION_SCANPARAMS", x => x.LOCSCANPARAM_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_MAST",
                columns: table => new
                {
                    LOV_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOV_TYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LOV_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MAST", x => x.LOV_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_TYPEMAST",
                columns: table => new
                {
                    LOV_TYPECODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LOV_TYPENAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TYPEMAST", x => x.LOV_TYPECODE);
                });

            migrationBuilder.CreateTable(
                name: "SCANNER_MASTER",
                columns: table => new
                {
                    DEVICE_ID = table.Column<long>(type: "bigint", nullable: false),
                    DEVICE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DEVICE_LOCID = table.Column<long>(type: "bigint", nullable: false),
                    DEVICE_PATH = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCANNER_MASTER", x => x.DEVICE_ID);
                });

            migrationBuilder.InsertData(
                table: "HOLDTYPE_MAST",
                columns: new[] { "HOLD_ID", "HOLD_CATEGORY", "HOLD_NAME" },
                values: new object[,]
                {
                    { 1L, "Q", "Quality Hold" },
                    { 2L, "S", "Safety Hold" },
                    { 3L, "F", "Financial Hold" }
                });

            migrationBuilder.InsertData(
                table: "LOV_MAST",
                columns: new[] { "LOV_ID", "LOV_NAME", "LOV_TYPE" },
                values: new object[,]
                {
                    { 1L, "Active", "STATUS" },
                    { 2L, "Inactive", "STATUS" },
                    { 3L, "Pending", "STATUS" },
                    { 4L, "General", "CATEGORY" },
                    { 5L, "Special", "CATEGORY" },
                    { 6L, "High", "PRIORITY" },
                    { 7L, "Medium", "PRIORITY" },
                    { 8L, "Low", "PRIORITY" }
                });

            migrationBuilder.InsertData(
                table: "LOV_TYPEMAST",
                columns: new[] { "LOV_TYPECODE", "LOV_TYPENAME" },
                values: new object[,]
                {
                    { "CATEGORY", "Category Codes" },
                    { "PRIORITY", "Priority Levels" },
                    { "STATUS", "Status Codes" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HOLDTYPE_MAST");

            migrationBuilder.DropTable(
                name: "LOCATION_SCANPARAMS");

            migrationBuilder.DropTable(
                name: "LOV_MAST");

            migrationBuilder.DropTable(
                name: "LOV_TYPEMAST");

            migrationBuilder.DropTable(
                name: "SCANNER_MASTER");
        }
    }
}
