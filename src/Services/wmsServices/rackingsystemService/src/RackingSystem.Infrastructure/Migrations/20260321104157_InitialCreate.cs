using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RackingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rack",
                columns: table => new
                {
                    rack_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    zone_id = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    rack_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    max_load_weight = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rack", x => x.rack_id);
                });

            migrationBuilder.CreateTable(
                name: "Shelf",
                columns: table => new
                {
                    shelf_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rack_id = table.Column<int>(type: "int", nullable: false),
                    shelf_level = table.Column<int>(type: "int", nullable: false),
                    shelf_position = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    capacity_qty = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    capacity_weight = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelf", x => x.shelf_id);
                    table.ForeignKey(
                        name: "FK_Shelf_Rack_rack_id",
                        column: x => x.rack_id,
                        principalTable: "Rack",
                        principalColumn: "rack_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bin",
                columns: table => new
                {
                    bin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    zone_id = table.Column<int>(type: "int", nullable: false),
                    shelf_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    bin_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    capacity_qty = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    capacity_weight = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    capacity_volume = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "AVAILABLE"),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bin", x => x.bin_id);
                    table.ForeignKey(
                        name: "FK_Bin_Shelf_shelf_id",
                        column: x => x.shelf_id,
                        principalTable: "Shelf",
                        principalColumn: "shelf_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bin_Barcode",
                table: "Bin",
                column: "barcode");

            migrationBuilder.CreateIndex(
                name: "IX_Bin_Code",
                table: "Bin",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_Bin_Shelf",
                table: "Bin",
                column: "shelf_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bin_Zone",
                table: "Bin",
                column: "zone_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Bin_Zone_Code",
                table: "Bin",
                columns: new[] { "zone_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rack_Zone",
                table: "Rack",
                column: "zone_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Rack_Zone_Code",
                table: "Rack",
                columns: new[] { "zone_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shelf_Rack",
                table: "Shelf",
                column: "rack_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Shelf_Rack_Level_Position",
                table: "Shelf",
                columns: new[] { "rack_id", "shelf_level", "shelf_position" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bin");

            migrationBuilder.DropTable(
                name: "Shelf");

            migrationBuilder.DropTable(
                name: "Rack");
        }
    }
}
