using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    BinId = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    QuantityChange = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransaction", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "StockLevel",
                columns: table => new
                {
                    StockLevelId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    BinId = table.Column<int>(type: "int", nullable: false),
                    QuantityOnHand = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
                    QuantityAllocated = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
                    QuantityReserved = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 0m),
                    ReorderLevel = table.Column<int>(type: "int", nullable: true),
                    LastCountDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLevel", x => x.StockLevelId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransaction_Bin",
                table: "InventoryTransaction",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransaction_Date",
                table: "InventoryTransaction",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransaction_Product",
                table: "InventoryTransaction",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransaction_Warehouse",
                table: "InventoryTransaction",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLevel_Bin",
                table: "StockLevel",
                column: "BinId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLevel_Product",
                table: "StockLevel",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLevel_Warehouse",
                table: "StockLevel",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "UQ_StockLevel_Product_Bin",
                table: "StockLevel",
                columns: new[] { "ProductId", "BinId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryTransaction");

            migrationBuilder.DropTable(
                name: "StockLevel");
        }
    }
}
