using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskServices.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TASK_MAIL",
                columns: table => new
                {
                    MID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASK_MAIL", x => x.MID);
                });

            migrationBuilder.InsertData(
                table: "TASK_MAIL",
                columns: new[] { "MID", "SYSID" },
                values: new object[,]
                {
                    { 1m, 1001m },
                    { 2m, 1002m },
                    { 3m, 1001m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TASK_MAIL");
        }
    }
}
