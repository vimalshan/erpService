using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActionService.Migrations.Domain
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    action = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    highPriority = table.Column<bool>(type: "bit", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    service = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    site = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    entityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    entityId = table.Column<int>(type: "int", nullable: true),
                    subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    snowLink = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");
        }
    }
}
