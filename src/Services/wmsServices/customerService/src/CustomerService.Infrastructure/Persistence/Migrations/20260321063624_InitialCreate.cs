using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomerService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    company_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    contact_person = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contact_title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    postal_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.customer_id);
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "customer_id", "code", "company_name", "created_date", "is_active", "modified_date", "name", "city", "country", "postal_code", "state", "address", "contact_person", "contact_title", "email", "phone" },
                values: new object[,]
                {
                    { 1, "CUST001", "Acme Corp", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Acme Corporation", "Springfield", "USA", "62701", "IL", "123 Main St", "John Doe", "Sales Manager", "john@acme.com", "+1-555-0101" },
                    { 2, "CUST002", "Globex Inc", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Globex Industries", "Shelbyville", "USA", "62565", "IL", "456 Industrial Blvd", "Jane Smith", "Director", "jane@globex.com", "+1-555-0102" },
                    { 3, "CUST003", "Wayne Corp", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wayne Enterprises", "Gotham", "USA", "07001", "NJ", "1007 Mountain Dr", "Bruce Wayne", "CEO", "bruce@wayne.com", "+1-555-0103" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_code",
                table: "Customer",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
