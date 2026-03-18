using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeductionService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADHOC_PAY_DED",
                columns: table => new
                {
                    PY_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    PY_CAN_UNT = table.Column<long>(type: "bigint", nullable: true),
                    PY_SRL_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_BAT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_TRN_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PY_ED_COD = table.Column<string>(type: "CHAR(6)", nullable: true),
                    PY_REF_NUM = table.Column<double>(type: "float", nullable: true),
                    PY_PAY_AMT = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    PY_OPP_AMT = table.Column<long>(type: "bigint", nullable: true),
                    PY_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PY_ENT_USR = table.Column<long>(type: "bigint", nullable: true),
                    PY_CAN_FLG = table.Column<string>(type: "CHAR(1)", nullable: true),
                    PY_ATT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_COM_COD = table.Column<string>(type: "CHAR(3)", nullable: true),
                    PY_EMP_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_UPD_FLG = table.Column<string>(type: "CHAR(1)", nullable: true),
                    PY_SEQ_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_GRD_TYP = table.Column<string>(type: "CHAR(3)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ADHOC_PAY_DED_HIS",
                columns: table => new
                {
                    PY_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    PY_CAN_UNT = table.Column<long>(type: "bigint", nullable: false),
                    PY_SRL_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_BAT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_TRN_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PY_ED_COD = table.Column<string>(type: "CHAR(6)", nullable: true),
                    PY_REF_NUM = table.Column<double>(type: "float", nullable: true),
                    PY_PAY_AMT = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    PY_OPP_AMT = table.Column<long>(type: "bigint", nullable: true),
                    PY_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PY_ENT_USR = table.Column<long>(type: "bigint", nullable: true),
                    PY_CAN_FLG = table.Column<string>(type: "CHAR(1)", nullable: true),
                    PY_ATT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_COM_COD = table.Column<string>(type: "CHAR(3)", nullable: true),
                    PY_EMP_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_UPD_FLG = table.Column<string>(type: "CHAR(1)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DEDUCTION_ACCESS",
                columns: table => new
                {
                    DE_UNT_ACC = table.Column<long>(type: "bigint", nullable: true),
                    DE_COM_COD = table.Column<long>(type: "bigint", nullable: true),
                    DE_DED_TYP = table.Column<string>(type: "CHAR(3)", nullable: true),
                    DE_SYS_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    DE_ENT_USR = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    DE_ENT_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    DE_CLS_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADHOC_PAY_DED");

            migrationBuilder.DropTable(
                name: "ADHOC_PAY_DED_HIS");

            migrationBuilder.DropTable(
                name: "DEDUCTION_ACCESS");
        }
    }
}
