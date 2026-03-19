using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeePrideManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOMENT_PRIDE",
                columns: table => new
                {
                    MOMENTPRIDE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MOMENTPRIDE_TITLE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MOMENTPRIDE_BODY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOMENTPRIDE_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MOMENTPRIDE_FOOTER = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MOMENTPRIDE_LOCATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MOMENTPRIDE_IMAGE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MOMENTPRIDE_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MOMENTPRIDE_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOMENT_PRIDE", x => x.MOMENTPRIDE_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOMENT_PRIDE_EMPSYSID",
                table: "MOMENT_PRIDE",
                column: "MOMENTPRIDE_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_MOMENT_PRIDE_MODIFIEDON",
                table: "MOMENT_PRIDE",
                column: "MOMENTPRIDE_MODIFIEDON");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOMENT_PRIDE");
        }
    }
}
