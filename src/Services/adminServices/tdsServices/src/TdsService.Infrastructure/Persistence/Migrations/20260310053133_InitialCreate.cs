using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TdsService.Infrastructure.Persistence.Migrations
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
                    VENDOR_ID = table.Column<long>(type: "bigint", nullable: false),
                    VENDOR_NAME = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: false),
                    EMAIL_ADDRESS = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    PAN_NO = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDS_VENDORS", x => x.VENDOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "TDSFILE_DETAILS",
                columns: table => new
                {
                    FILE_ID = table.Column<long>(type: "bigint", nullable: false),
                    FILE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PAN_NO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    EMAIL_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    FILE_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    BLOB_URI = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CREATED_AT = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UPDATED_AT = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TDSFILE_DETAILS", x => x.FILE_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_TDS_VENDORS_PANNO",
                table: "TDS_VENDORS",
                column: "PAN_NO");

            migrationBuilder.CreateIndex(
                name: "IDX_TDSFILE_PANNO",
                table: "TDSFILE_DETAILS",
                column: "PAN_NO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TDS_VENDORS");

            migrationBuilder.DropTable(
                name: "TDSFILE_DETAILS");
        }
    }
}
