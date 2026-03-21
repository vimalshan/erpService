using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipmentService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    shipment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipment_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    so_id = table.Column<int>(type: "int", nullable: true),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_id = table.Column<int>(type: "int", nullable: false),
                    shipment_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    service_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    shipped_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    tracking_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    carrier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    total_weight = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    total_volume = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    special_instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.shipment_id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAttempt",
                columns: table => new
                {
                    attempt_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    attempt_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAttempt", x => x.attempt_id);
                    table.ForeignKey(
                        name: "FK_DeliveryAttempt_Shipment_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "Shipment",
                        principalColumn: "shipment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    package_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    package_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    weight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    volume = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    dimensions = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tracking_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    contents_description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.package_id);
                    table.UniqueConstraint("UQ_Package_Shipment", x => new { x.shipment_id, x.package_number });
                    table.ForeignKey(
                        name: "FK_Package_Shipment_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "Shipment",
                        principalColumn: "shipment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentLine",
                columns: table => new
                {
                    shipment_line_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    so_line_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    bin_id = table.Column<int>(type: "int", nullable: false),
                    quantity_shipped = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    lot_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackingHistory",
                columns: table => new
                {
                    tracking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    event_datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingHistory", x => x.tracking_id);
                    table.ForeignKey(
                        name: "FK_TrackingHistory_Shipment_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "Shipment",
                        principalColumn: "shipment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAttempt_shipment_id",
                table: "DeliveryAttempt",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Package_Shipment",
                table: "Package",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_Customer",
                table: "Shipment",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_ShipmentNumber",
                table: "Shipment",
                column: "shipment_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_SO",
                table: "Shipment",
                column: "so_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_TrackingNumber",
                table: "Shipment",
                column: "tracking_number",
                filter: "tracking_number IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_Warehouse",
                table: "Shipment",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_Bin",
                table: "ShipmentLine",
                column: "bin_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_Product",
                table: "ShipmentLine",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_Shipment",
                table: "ShipmentLine",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_SOLine",
                table: "ShipmentLine",
                column: "so_line_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingHistory_Shipment",
                table: "TrackingHistory",
                column: "shipment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAttempt");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "ShipmentLine");

            migrationBuilder.DropTable(
                name: "TrackingHistory");

            migrationBuilder.DropTable(
                name: "Shipment");
        }
    }
}
