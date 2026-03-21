using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PurchaseOrderService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    po_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    supplier_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_id = table.Column<int>(type: "int", nullable: false),
                    order_date = table.Column<DateTime>(type: "date", nullable: false),
                    expected_date = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.po_id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLine",
                columns: table => new
                {
                    po_line_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    po_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    line_number = table.Column<int>(type: "int", nullable: false),
                    quantity_ordered = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    quantity_received = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLine", x => x.po_line_id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_PurchaseOrder_po_id",
                        column: x => x.po_id,
                        principalTable: "PurchaseOrder",
                        principalColumn: "po_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_po_number",
                table: "PurchaseOrder",
                column: "po_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_supplier_id",
                table: "PurchaseOrder",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_po_id",
                table: "PurchaseOrderLine",
                column: "po_id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_po_id_line_number",
                table: "PurchaseOrderLine",
                columns: new[] { "po_id", "line_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_product_id",
                table: "PurchaseOrderLine",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderLine");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");
        }
    }
}
