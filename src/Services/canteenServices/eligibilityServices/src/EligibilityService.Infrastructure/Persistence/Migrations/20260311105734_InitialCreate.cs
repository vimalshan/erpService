using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EligibilityService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CAN_ELIGIBILITY_MASTER",
                columns: table => new
                {
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_SFT_COD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CN_ITM_COD = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CN_ELG_LMT = table.Column<int>(type: "int", nullable: true),
                    CN_ENT_USR = table.Column<long>(type: "bigint", nullable: true),
                    CN_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CN_TIM_UNT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAN_ELIGIBILITY_MASTER", x => new { x.CN_COM_COD, x.CN_SFT_COD, x.CN_ITM_COD });
                });

            migrationBuilder.CreateTable(
                name: "CAN_ELIGIBILITY_MASTER_HIS",
                columns: table => new
                {
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_SFT_COD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CN_ITM_COD = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CN_ELG_LMT = table.Column<int>(type: "int", nullable: true),
                    CN_MOD_USR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CN_MOD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CAN_SHIFT_MAPPING",
                columns: table => new
                {
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_SFT_COD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CN_SFT_BEF = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CN_SFT_AFT = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAN_SHIFT_MAPPING", x => new { x.CN_COM_COD, x.CN_SFT_COD });
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_DAYWISE_ELIGIBILITY",
                columns: table => new
                {
                    CN_SRL_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    CN_ATT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CN_PRC_NUM = table.Column<long>(type: "bigint", nullable: true),
                    CN_SFT_COD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CN_ITM_COD = table.Column<long>(type: "bigint", nullable: true),
                    CN_SFT_QTY = table.Column<int>(type: "int", nullable: true),
                    CN_SFT_BEF = table.Column<int>(type: "int", nullable: true),
                    CN_SFT_AFT = table.Column<int>(type: "int", nullable: true),
                    CN_ENT_USR = table.Column<long>(type: "bigint", nullable: true),
                    CN_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CN_FLEX1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CN_GRD_TYP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_DAYWISE_ELIGIBILITY", x => x.CN_SRL_NUM);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CANTEEN_DAYWISE_ELIGIBILITY_CN_COM_COD",
                table: "CANTEEN_DAYWISE_ELIGIBILITY",
                column: "CN_COM_COD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CAN_ELIGIBILITY_MASTER");

            migrationBuilder.DropTable(
                name: "CAN_ELIGIBILITY_MASTER_HIS");

            migrationBuilder.DropTable(
                name: "CAN_SHIFT_MAPPING");

            migrationBuilder.DropTable(
                name: "CANTEEN_DAYWISE_ELIGIBILITY");
        }
    }
}
