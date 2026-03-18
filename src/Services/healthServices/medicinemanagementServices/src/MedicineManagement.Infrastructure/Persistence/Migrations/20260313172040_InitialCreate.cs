using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DOCATTEND_MAST",
                columns: table => new
                {
                    DM_SYSID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DM_COD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DM_FLAG = table.Column<string>(type: "CHAR(1)", nullable: true),
                    DM_NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCATTEND_MAST", x => x.DM_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "MED_DRCRFLG",
                columns: table => new
                {
                    MED_FLG = table.Column<string>(type: "CHAR(1)", nullable: true),
                    MED_DRCR = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MEDICINE_ISSUE",
                columns: table => new
                {
                    MD_COM_COD = table.Column<string>(type: "CHAR(3)", nullable: true),
                    MD_TRN_NUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MD_TRN_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MD_ISS_QNT = table.Column<long>(type: "bigint", nullable: true),
                    MD_VIS_NUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MD_MED_COD = table.Column<string>(type: "CHAR(3)", nullable: true),
                    MD_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MD_USR_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MD_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MD_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MD_MOD_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MD_MOD_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MEDICINE_PKG",
                columns: table => new
                {
                    PK_PKG_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    PK_PKG_TYP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PK_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PK_USR_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    PK_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    PK_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PK_MOD_NUM = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    PK_MOD_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICINE_PKG", x => x.PK_PKG_COD);
                });

            migrationBuilder.CreateTable(
                name: "MEDICINE_TYPMAST",
                columns: table => new
                {
                    MT_TYP_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MT_TYP_NAM = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MT_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MT_USR_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MT_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MT_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MT_MOD_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MT_MOD_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICINE_TYPMAST", x => x.MT_TYP_COD);
                });

            migrationBuilder.CreateTable(
                name: "PURCHASE_MAIN",
                columns: table => new
                {
                    MD_COM_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MD_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    MD_VND_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MD_INV_NUM = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MD_INV_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MD_INV_AMT = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    MD_CAN_FLG = table.Column<string>(type: "CHAR(1)", nullable: false),
                    MD_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MD_USR_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: false),
                    MD_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MD_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MD_MOD_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MD_MOD_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PURCHASE_MAIN", x => new { x.MD_COM_COD, x.MD_TRN_NUM });
                });

            migrationBuilder.CreateTable(
                name: "MEDICINE_MAST",
                columns: table => new
                {
                    MM_MED_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MM_MED_NAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MM_MED_TYP = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MM_MED_CAT = table.Column<string>(type: "CHAR(1)", nullable: true),
                    MM_ORD_MIN = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MM_ORD_MAX = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MM_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MM_USR_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MM_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MM_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MM_MOD_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MM_MOD_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICINE_MAST", x => x.MM_MED_COD);
                    table.ForeignKey(
                        name: "FK_MEDICINE_MAST_MEDICINE_TYPMAST_MM_MED_TYP",
                        column: x => x.MM_MED_TYP,
                        principalTable: "MEDICINE_TYPMAST",
                        principalColumn: "MT_TYP_COD",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PURCHASE_SUB",
                columns: table => new
                {
                    MD_COM_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MD_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    MD_SRL_NUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MD_MED_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MD_PKG_TYP = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MD_PKG_QNT = table.Column<long>(type: "bigint", nullable: true),
                    MD_PKG_NOS = table.Column<long>(type: "bigint", nullable: true),
                    MD_TOT_QNT = table.Column<long>(type: "bigint", nullable: true),
                    MD_MFG_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MD_EXP_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MD_LOT_NUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MD_CAN_FLG = table.Column<string>(type: "CHAR(1)", nullable: false),
                    MD_ENT_USR = table.Column<string>(type: "CHAR(25)", nullable: true),
                    MD_USR_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MD_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MD_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MD_MOD_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MD_MOD_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PURCHASE_SUB", x => new { x.MD_COM_COD, x.MD_TRN_NUM, x.MD_SRL_NUM });
                    table.ForeignKey(
                        name: "FK_PURCHASE_SUB_PURCHASE_MAIN_MD_COM_COD_MD_TRN_NUM",
                        columns: x => new { x.MD_COM_COD, x.MD_TRN_NUM },
                        principalTable: "PURCHASE_MAIN",
                        principalColumns: new[] { "MD_COM_COD", "MD_TRN_NUM" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MEDICINE_CREDIT",
                columns: table => new
                {
                    MD_COM_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MD_TRN_COD = table.Column<long>(type: "bigint", nullable: false),
                    MD_MED_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MD_REC_TYP = table.Column<string>(type: "CHAR(1)", nullable: false),
                    MD_MED_QNT = table.Column<long>(type: "bigint", nullable: false),
                    MD_TRN_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MD_LOT_NUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MD_CAN_FLG = table.Column<string>(type: "CHAR(1)", nullable: true),
                    MD_TRN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    MD_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MD_USR_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: false),
                    MD_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MD_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MD_MOD_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                    MD_MOD_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICINE_CREDIT", x => x.MD_COM_COD);
                    table.ForeignKey(
                        name: "FK_MEDICINE_CREDIT_MEDICINE_MAST_MD_MED_COD",
                        column: x => x.MD_MED_COD,
                        principalTable: "MEDICINE_MAST",
                        principalColumn: "MM_MED_COD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_MEDICINE_CREDIT_MD_COM_COD",
                table: "MEDICINE_CREDIT",
                column: "MD_COM_COD");

            migrationBuilder.CreateIndex(
                name: "IDX_MEDICINE_CREDIT_MD_TRN_DAT",
                table: "MEDICINE_CREDIT",
                column: "MD_TRN_DAT");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICINE_CREDIT_MD_MED_COD",
                table: "MEDICINE_CREDIT",
                column: "MD_MED_COD");

            migrationBuilder.CreateIndex(
                name: "IDX_MEDICINE_MAST_MM_MED_COD",
                table: "MEDICINE_MAST",
                column: "MM_MED_COD");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICINE_MAST_MM_MED_TYP",
                table: "MEDICINE_MAST",
                column: "MM_MED_TYP");

            migrationBuilder.CreateIndex(
                name: "IDX_PURCHASE_MAIN_MD_COM_COD",
                table: "PURCHASE_MAIN",
                column: "MD_COM_COD");

            migrationBuilder.CreateIndex(
                name: "IDX_PURCHASE_MAIN_MD_INV_DAT",
                table: "PURCHASE_MAIN",
                column: "MD_INV_DAT");

            migrationBuilder.CreateIndex(
                name: "IDX_PURCHASE_SUB_MD_TRN_NUM",
                table: "PURCHASE_SUB",
                column: "MD_TRN_NUM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DOCATTEND_MAST");

            migrationBuilder.DropTable(
                name: "MED_DRCRFLG");

            migrationBuilder.DropTable(
                name: "MEDICINE_CREDIT");

            migrationBuilder.DropTable(
                name: "MEDICINE_ISSUE");

            migrationBuilder.DropTable(
                name: "MEDICINE_PKG");

            migrationBuilder.DropTable(
                name: "PURCHASE_SUB");

            migrationBuilder.DropTable(
                name: "MEDICINE_MAST");

            migrationBuilder.DropTable(
                name: "PURCHASE_MAIN");

            migrationBuilder.DropTable(
                name: "MEDICINE_TYPMAST");
        }
    }
}
