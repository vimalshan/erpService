using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMTransactional.Infrastructure.Migrations
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
                    order_date = table.Column<DateTime>(type: "date", nullable: false),
                    expected_date = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.po_id);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrder",
                columns: table => new
                {
                    so_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    so_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    order_date = table.Column<DateTime>(type: "date", nullable: false),
                    requested_date = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrder", x => x.so_id);
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
                    quantity_received = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
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

            migrationBuilder.CreateTable(
                name: "Receiving",
                columns: table => new
                {
                    receiving_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receiving_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    po_id = table.Column<int>(type: "int", nullable: false),
                    received_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receiving", x => x.receiving_id);
                    table.ForeignKey(
                        name: "FK_Receiving_PurchaseOrder_po_id",
                        column: x => x.po_id,
                        principalTable: "PurchaseOrder",
                        principalColumn: "po_id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    shipment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipment_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    so_id = table.Column<int>(type: "int", nullable: false),
                    shipped_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    tracking_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    carrier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.shipment_id);
                    table.ForeignKey(
                        name: "FK_Shipment_SalesOrder_so_id",
                        column: x => x.so_id,
                        principalTable: "SalesOrder",
                        principalColumn: "so_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingLine",
                columns: table => new
                {
                    receiving_line_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receiving_id = table.Column<int>(type: "int", nullable: false),
                    po_line_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    bin_id = table.Column<int>(type: "int", nullable: false),
                    quantity_received = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    lot_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingLine", x => x.receiving_line_id);
                    table.ForeignKey(
                        name: "FK_ReceivingLine_Receiving_receiving_id",
                        column: x => x.receiving_id,
                        principalTable: "Receiving",
                        principalColumn: "receiving_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentLine",
                columns: table => new
                {
                    shipment_line_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    so_line_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    bin_id = table.Column<int>(type: "int", nullable: false),
                    quantity_shipped = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    lot_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentLine", x => x.shipment_line_id);
                    table.ForeignKey(
                        name: "FK_ShipmentLine_Shipment_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "Shipment",
                        principalColumn: "shipment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_po_number",
                table: "PurchaseOrder",
                column: "po_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_Supplier",
                table: "PurchaseOrder",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_Product",
                table: "PurchaseOrderLine",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "UQ_POLine_PO_Line",
                table: "PurchaseOrderLine",
                columns: new[] { "po_id", "line_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_PO",
                table: "Receiving",
                column: "po_id");

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_receiving_number",
                table: "Receiving",
                column: "receiving_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_Bin",
                table: "ReceivingLine",
                column: "bin_id");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_POLine",
                table: "ReceivingLine",
                column: "po_line_id");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_Product",
                table: "ReceivingLine",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_receiving_id",
                table: "ReceivingLine",
                column: "receiving_id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_Customer",
                table: "SalesOrder",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_so_number",
                table: "SalesOrder",
                column: "so_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLine_Product",
                table: "SalesOrderLine",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "UQ_SOLine_SO_Line",
                table: "SalesOrderLine",
                columns: new[] { "so_id", "line_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_shipment_number",
                table: "Shipment",
                column: "shipment_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_SO",
                table: "Shipment",
                column: "so_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_Bin",
                table: "ShipmentLine",
                column: "bin_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_Product",
                table: "ShipmentLine",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_shipment_id",
                table: "ShipmentLine",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_SOLine",
                table: "ShipmentLine",
                column: "so_line_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderLine");

            migrationBuilder.DropTable(
                name: "ReceivingLine");

            migrationBuilder.DropTable(
                name: "SalesOrderLine");

            migrationBuilder.DropTable(
                name: "ShipmentLine");

            migrationBuilder.DropTable(
                name: "Receiving");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");

            migrationBuilder.DropTable(
                name: "SalesOrder");
        }
    }
}
