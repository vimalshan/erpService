using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PurchaseSalesService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOG_PURCHASE_DETAILS",
                columns: table => new
                {
                    PD_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PD_TRC_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PD_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PD_PUR_COD = table.Column<long>(type: "bigint", nullable: false),
                    PD_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    PD_ORA_MRC = table.Column<long>(type: "bigint", nullable: true),
                    PD_SUP_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PD_TON_NUM_LD = table.Column<long>(type: "bigint", nullable: true),
                    PD_TON_NUM_UD = table.Column<long>(type: "bigint", nullable: true),
                    PD_USR_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PD_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PD_UPD_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PD_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PD_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PD_MOD_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PD_MOD_DAT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LOG_SALE_MAIN",
                columns: table => new
                {
                    SL_SER_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_TRC_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_PUR_COD = table.Column<long>(type: "bigint", nullable: false),
                    SL_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    SL_ISO_NUM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SL_ISO_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SL_PRO_DES = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SL_USR_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SL_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_UPD_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SL_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SL_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SL_MOD_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_MOD_DAT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LOG_SALE_SUB",
                columns: table => new
                {
                    SS_REF_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SS_SER_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SS_PRO_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SS_PRO_QTN = table.Column<decimal>(type: "decimal(38,6)", precision: 38, scale: 6, nullable: true),
                    SS_PRO_GRD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SS_USR_COM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SS_CHB_INV = table.Column<long>(type: "bigint", nullable: true),
                    SS_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SS_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SS_MOD_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SS_MOD_DAT = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PURCHASE_DETAILS",
                columns: table => new
                {
                    PD_SRL_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PD_TRC_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PD_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PD_PUR_COD = table.Column<long>(type: "bigint", nullable: false),
                    PD_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    PD_ORA_MRC = table.Column<long>(type: "bigint", nullable: true),
                    PD_SUP_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PD_TON_NUM_LD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PD_TON_NUM_UD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PD_USR_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PD_USR_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PD_UPD_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PD_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PURCHASE_DETAILS", x => x.PD_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "SALE_MAIN",
                columns: table => new
                {
                    SL_SER_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SL_TRC_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_PUR_COD = table.Column<long>(type: "bigint", nullable: false),
                    SL_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    SL_ISO_NUM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SL_ISO_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SL_PRO_DES = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SL_USR_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SL_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SL_UPD_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SL_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SL_VEH_CUS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALE_MAIN", x => x.SL_SER_NUM);
                });

            migrationBuilder.CreateTable(
                name: "SALE_SUB",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SS_REF_NUM = table.Column<long>(type: "bigint", nullable: true),
                    SS_SER_NUM = table.Column<long>(type: "bigint", nullable: true),
                    SS_PRO_COD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SS_PRO_QTN = table.Column<decimal>(type: "decimal(38,6)", precision: 38, scale: 6, nullable: true),
                    SS_PRO_GRD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SS_USR_COM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SS_CHB_INV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SS_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALE_SUB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALE_SUB_SALE_MAIN_SS_SER_NUM",
                        column: x => x.SS_SER_NUM,
                        principalTable: "SALE_MAIN",
                        principalColumn: "SL_SER_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SALE_SUB_SS_SER_NUM",
                table: "SALE_SUB",
                column: "SS_SER_NUM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOG_PURCHASE_DETAILS");

            migrationBuilder.DropTable(
                name: "LOG_SALE_MAIN");

            migrationBuilder.DropTable(
                name: "LOG_SALE_SUB");

            migrationBuilder.DropTable(
                name: "PURCHASE_DETAILS");

            migrationBuilder.DropTable(
                name: "SALE_SUB");

            migrationBuilder.DropTable(
                name: "SALE_MAIN");
        }
    }
}
