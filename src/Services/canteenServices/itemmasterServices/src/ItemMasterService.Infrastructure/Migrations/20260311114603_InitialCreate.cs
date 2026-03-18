using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemMasterService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CANTEEN_ITEM_MASTER",
                columns: table => new
                {
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_ITM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_ITM_DES = table.Column<string>(type: "CHAR(50)", maxLength: 50, nullable: true),
                    CN_ITM_TYP = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: true),
                    CN_ITM_REF = table.Column<string>(type: "CHAR(10)", maxLength: 10, nullable: true),
                    CN_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    CN_ENT_USR = table.Column<string>(type: "CHAR(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_ITEM_MASTER", x => new { x.CN_COM_COD, x.CN_ITM_COD });
                });

            migrationBuilder.CreateTable(
                name: "CANTEENGRADE_ITEM_PRICE",
                columns: table => new
                {
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CN_ITM_COD = table.Column<long>(type: "bigint", nullable: true),
                    CN_EMP_CON = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: true),
                    CN_EPR_CON = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: true),
                    CN_EFF_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    CN_CLS_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    CN_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    CN_ENT_USR = table.Column<string>(type: "CHAR(50)", maxLength: 50, nullable: false),
                    CN_GRD_TYP = table.Column<string>(type: "CHAR(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEENGRADE_ITEM_PRICE", x => x.CN_COM_COD);
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_ITEM_PRICE_MASTER",
                columns: table => new
                {
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_ITM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_EFF_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    CN_EMP_CON = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: true),
                    CN_EPR_CON = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: true),
                    CN_CLS_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    CN_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    CN_ENT_USR = table.Column<string>(type: "CHAR(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_ITEM_PRICE_MASTER", x => new { x.CN_COM_COD, x.CN_ITM_COD, x.CN_EFF_DAT });
                    table.ForeignKey(
                        name: "FK_CANTEEN_ITEM_PRICE_MASTER_CANTEEN_ITEM_MASTER_CN_COM_COD_CN_ITM_COD",
                        columns: x => new { x.CN_COM_COD, x.CN_ITM_COD },
                        principalTable: "CANTEEN_ITEM_MASTER",
                        principalColumns: new[] { "CN_COM_COD", "CN_ITM_COD" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CANTEEN_ITEM_PRICE_MASTER");

            migrationBuilder.DropTable(
                name: "CANTEENGRADE_ITEM_PRICE");

            migrationBuilder.DropTable(
                name: "CANTEEN_ITEM_MASTER");
        }
    }
}
