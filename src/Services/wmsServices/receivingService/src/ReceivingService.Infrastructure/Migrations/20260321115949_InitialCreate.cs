using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceivingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receiving",
                columns: table => new
                {
                    receiving_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receiving_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    po_id = table.Column<int>(type: "int", nullable: false),
                    warehouse_id = table.Column<int>(type: "int", nullable: false),
                    received_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receiving", x => x.receiving_id);
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
                    quantity_received = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    lot_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    expiry_date = table.Column<DateOnly>(type: "date", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_PO",
                table: "Receiving",
                column: "po_id");

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_ReceivingNumber",
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
                name: "IX_ReceivingLine_Receiving",
                table: "ReceivingLine",
                column: "receiving_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceivingLine");

            migrationBuilder.DropTable(
                name: "Receiving");
        }
    }
}
