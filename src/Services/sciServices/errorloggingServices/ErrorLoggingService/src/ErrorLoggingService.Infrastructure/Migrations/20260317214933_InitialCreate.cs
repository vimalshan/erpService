using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErrorLoggingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ERRSP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ERR_MESS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ERR_SP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ERR_REF = table.Column<int>(type: "int", nullable: true),
                    ERR_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERRSP", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ERRSP");
        }
    }
}
