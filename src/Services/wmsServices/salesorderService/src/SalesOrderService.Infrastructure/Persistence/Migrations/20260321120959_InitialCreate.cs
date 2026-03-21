using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesOrderService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesOrder",
                columns: table => new
                {
                    so_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    so_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_id = table.Column<int>(type: "int", nullable: false),
                    order_date = table.Column<DateOnly>(type: "date", nullable: false),
                    requested_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrder", x => x.so_id);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderLine",
                columns: table => new
                {
                    so_line_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    so_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    line_number = table.Column<int>(type: "int", nullable: false),
                    quantity_ordered = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    quantity_shipped = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
                    unit_price = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderLine", x => x.so_line_id);
                    table.ForeignKey(
                        name: "FK_SalesOrderLine_SalesOrder_so_id",
                        column: x => x.so_id,
                        principalTable: "SalesOrder",
                        principalColumn: "so_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_Customer",
                table: "SalesOrder",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_SONumber",
                table: "SalesOrder",
                column: "so_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLine_Product",
                table: "SalesOrderLine",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLine_SO",
                table: "SalesOrderLine",
                column: "so_id");

            migrationBuilder.CreateIndex(
                name: "UQ_SOLine_SO_Line",
                table: "SalesOrderLine",
                columns: new[] { "so_id", "line_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesOrderLine");

            migrationBuilder.DropTable(
                name: "SalesOrder");
        }
    }
}
