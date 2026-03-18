using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalVisit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VISIT_MAIN",
                columns: table => new
                {
                    VM_COM_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VM_VIS_NUM = table.Column<long>(type: "bigint", nullable: false),
                    VM_USR_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    VM_PIN_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    VM_WRK_NAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VM_CONTRCT_ID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VM_CONTRCT_NAM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VM_VIS_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    VM_OTH_HOSP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VM_VIS_SHIFT = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    VM_VIS_TYP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    VM_ATT_COD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VM_DOC_COD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VM_PAT_DIA = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VM_TRT_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VM_TST_ADV = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VM_DOC_REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VM_DIA_CAT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    VM_DIA_SUBCAT = table.Column<long>(type: "bigint", nullable: true),
                    VM_MED_GIV = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    VM_NXT_REV = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    VM_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    VM_ENT_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    VM_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    DV_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    VM_MOD_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    VM_MOD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    VM_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISIT_MAIN", x => new { x.VM_COM_COD, x.VM_VIS_NUM });
                });

            migrationBuilder.CreateTable(
                name: "VISIT_SUB",
                columns: table => new
                {
                    VS_COM_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VS_VIS_NUM = table.Column<long>(type: "bigint", nullable: false),
                    VS_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    VS_TST_TYP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VS_TST_VAL = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISIT_SUB", x => new { x.VS_COM_COD, x.VS_VIS_NUM, x.VS_SRL_NUM });
                    table.ForeignKey(
                        name: "FK_VISIT_SUB_VISIT_MAIN_VS_COM_COD_VS_VIS_NUM",
                        columns: x => new { x.VS_COM_COD, x.VS_VIS_NUM },
                        principalTable: "VISIT_MAIN",
                        principalColumns: new[] { "VM_COM_COD", "VM_VIS_NUM" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_VISIT_MAIN_VM_COM_COD",
                table: "VISIT_MAIN",
                column: "VM_COM_COD");

            migrationBuilder.CreateIndex(
                name: "IDX_VISIT_MAIN_VM_VIS_DAT",
                table: "VISIT_MAIN",
                column: "VM_VIS_DAT");

            migrationBuilder.CreateIndex(
                name: "IDX_VISIT_SUB_VS_VIS_NUM",
                table: "VISIT_SUB",
                column: "VS_VIS_NUM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VISIT_SUB");

            migrationBuilder.DropTable(
                name: "VISIT_MAIN");
        }
    }
}
