using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTransaction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHKUP_PFI_HIST",
                columns: table => new
                {
                    CPH_HLTH_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CPH_SYMP_ID = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CPH_EMP_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CPH_YN_FLAG = table.Column<string>(type: "CHAR(1)", nullable: true),
                    CPH_IMM_DAT = table.Column<DateTime>(type: "DATE", nullable: true),
                    CPH_TEST_VAL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHKUP_PFI_HIST", x => new { x.CPH_HLTH_NUM, x.CPH_SYMP_ID });
                });

            migrationBuilder.CreateTable(
                name: "CHKUP_PRE_MAIN",
                columns: table => new
                {
                    CPM_EMP_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CPM_COM_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CPM_HLTH_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CPM_PHYS_HAND = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CPM_PROP_EMP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CPM_IDENT_MARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CPM_FINAL_RMKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CPM_FIT_PH = table.Column<string>(type: "CHAR(3)", nullable: true),
                    CPM_FIT_FINAL = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CPM_CHK_DAT = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHKUP_PRE_MAIN", x => new { x.CPM_EMP_NUM, x.CPM_COM_COD });
                });

            migrationBuilder.CreateTable(
                name: "HEALTH_DYN_DET",
                columns: table => new
                {
                    CDD_HLTH_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CDD_CHKUP_COD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CDD_COM_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CDD_CTRLSRC_ID = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CDD_DYN_VAL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CDD_EMP_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    CDD_SYS_DAT = table.Column<DateTime>(type: "DATE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEALTH_DYN_DET", x => new { x.CDD_HLTH_NUM, x.CDD_CHKUP_COD, x.CDD_COM_COD, x.CDD_CTRLSRC_ID });
                });

            migrationBuilder.CreateTable(
                name: "HLTH_CHKUP_CARD",
                columns: table => new
                {
                    HCC_HLTH_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    HCC_EMP_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    HCC_EMP_DATE = table.Column<DateTime>(type: "DATE", nullable: true),
                    HCC_COM_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    HCC_PER_DET = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HCC_COMPL_DET = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HCC_ADV_RMK1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HCC_ADV_RMK2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HCC_DOC_DATE1 = table.Column<DateTime>(type: "DATE", nullable: true),
                    HCC_DOC_DATE2 = table.Column<DateTime>(type: "DATE", nullable: true),
                    HCC_ADV_FOLLOW1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HCC_ADV_FOLLOW2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HLTH_CHKUP_CARD", x => x.HCC_HLTH_NUM);
                });

            migrationBuilder.CreateTable(
                name: "HLTH_CHKCARD_SUB",
                columns: table => new
                {
                    HCS_HLTH_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    HCS_SYMP_ID = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false),
                    HCS_FLAG_YN = table.Column<string>(type: "CHAR(1)", nullable: true),
                    HCS_SYMP_VAL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HCS_EMP_NUM = table.Column<decimal>(type: "NUMERIC(10,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HLTH_CHKCARD_SUB", x => new { x.HCS_HLTH_NUM, x.HCS_SYMP_ID });
                    table.ForeignKey(
                        name: "FK_HLTH_CHKCARD_SUB_HLTH_CHKUP_CARD_HCS_HLTH_NUM",
                        column: x => x.HCS_HLTH_NUM,
                        principalTable: "HLTH_CHKUP_CARD",
                        principalColumn: "HCC_HLTH_NUM",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHKUP_PFI_HIST");

            migrationBuilder.DropTable(
                name: "CHKUP_PRE_MAIN");

            migrationBuilder.DropTable(
                name: "HEALTH_DYN_DET");

            migrationBuilder.DropTable(
                name: "HLTH_CHKCARD_SUB");

            migrationBuilder.DropTable(
                name: "HLTH_CHKUP_CARD");
        }
    }
}
