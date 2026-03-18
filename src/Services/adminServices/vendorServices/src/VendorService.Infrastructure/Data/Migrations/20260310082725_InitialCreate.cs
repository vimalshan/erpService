using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TDS_VENDORS",
                columns: table => new
                {
                    VENDOR_ID = table.Column<long>(type: "bigint", nullable: true),
                    VENDOR_NAME = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                    EMAIL_ADDRESS = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    PAN_NO = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TDSFILE_DETAILS",
                columns: table => new
                {
                    FILE_ID = table.Column<long>(type: "bigint", nullable: false),
                    FILE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PAN_NO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    EMAIL_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    FILE_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDSFILE_DETAILS", x => x.FILE_ID);
                });

            migrationBuilder.CreateTable(
                name: "VENDOR_MASTER",
                columns: table => new
                {
                    VM_ID = table.Column<long>(type: "bigint", nullable: false),
                    VM_CATID = table.Column<long>(type: "bigint", nullable: false),
                    VM_LOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    VM_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VM_EMAIL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VM_ADDRESS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VM_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    VM_UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    VM_LIVESTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDOR_MASTER", x => x.VM_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_VENDOR_MASTER_LOCID",
                table: "VENDOR_MASTER",
                column: "VM_LOC_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_VENDOR_MASTER_STATUS",
                table: "VENDOR_MASTER",
                column: "VM_LIVESTATUS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TDS_VENDORS");

            migrationBuilder.DropTable(
                name: "TDSFILE_DETAILS");

            migrationBuilder.DropTable(
                name: "VENDOR_MASTER");
        }
    }
}
