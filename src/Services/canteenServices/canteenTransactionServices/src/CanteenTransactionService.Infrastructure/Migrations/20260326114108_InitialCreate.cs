using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanteenTransactionService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CANTEEDN_DACON",
                columns: table => new
                {
                    CN_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: true),
                    CN_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    CN_EMP_TYP = table.Column<string>(type: "char(1)", nullable: true),
                    CN_SWP_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CN_ITM_COD = table.Column<long>(type: "bigint", nullable: true),
                    CN_ITM_TYP = table.Column<string>(type: "char(1)", nullable: true),
                    CN_EE_CON = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CN_ER_CON = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CN_CAN_NUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CN_ITM_QTY = table.Column<long>(type: "bigint", nullable: true),
                    CN_ENT_USR = table.Column<long>(type: "bigint", nullable: true),
                    CN_ENT_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CN_FLEX1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CN_GRD_CAT = table.Column<string>(type: "char(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEDN_DACON", x => x.CN_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_DAYWISE_AVAILED",
                columns: table => new
                {
                    CN_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    CN_EMP_TYP = table.Column<string>(type: "char(1)", nullable: true),
                    CN_SWP_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CN_ITM_COD = table.Column<long>(type: "bigint", nullable: true),
                    CN_ITM_TYP = table.Column<string>(type: "char(1)", nullable: true),
                    CN_EE_CON = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CN_ER_CON = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CN_CAN_NUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CN_ITM_QTY = table.Column<long>(type: "bigint", nullable: true),
                    CN_ENT_USR = table.Column<long>(type: "bigint", nullable: true),
                    CN_ENT_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CN_FLEX1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CN_GRD_CAT = table.Column<string>(type: "char(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_DAYWISE_AVAILED", x => x.CN_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_MIS_SBT",
                columns: table => new
                {
                    CN_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_EMP_NUM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CN_SWP_TIM = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CN_ITM_COD = table.Column<long>(type: "bigint", nullable: false),
                    CN_ITM_QTN = table.Column<long>(type: "bigint", nullable: false),
                    CN_BAT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CN_BAT_NUM = table.Column<long>(type: "bigint", nullable: false),
                    CN_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CN_CAN_NUM = table.Column<string>(type: "char(1)", nullable: false),
                    CN_GAT_NUM = table.Column<string>(type: "char(3)", nullable: false),
                    CN_UPD_STS = table.Column<string>(type: "char(1)", nullable: false),
                    CN_FLX_FLD1 = table.Column<string>(type: "char(5)", nullable: true),
                    CN_FLX_FLD2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CN_FLX_FLD3 = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CN_FLX_FLD4 = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CN_FLX_FLD5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_MIS_SBT", x => x.CN_SRL_NUM);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CANTEEDN_DACON");

            migrationBuilder.DropTable(
                name: "CANTEEN_DAYWISE_AVAILED");

            migrationBuilder.DropTable(
                name: "CANTEEN_MIS_SBT");
        }
    }
}
