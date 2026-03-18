using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContributionService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CONTRIBUTION_BREAKUP",
                columns: table => new
                {
                    CONTRIBUTION_BATCH_NO = table.Column<long>(type: "bigint", nullable: false),
                    CONTRIBUTION_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONTRIBUTION_PAYTRANNO = table.Column<long>(type: "bigint", nullable: false),
                    CONTRIBUTION_EDCODE = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    CONTRIBUTION_PAYAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_EEAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_ERAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_COM_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRIBUTION_BREAKUP", x => new { x.CONTRIBUTION_PAYTRANNO, x.CONTRIBUTION_BATCH_NO, x.CONTRIBUTION_ID });
                });

            migrationBuilder.CreateTable(
                name: "CONTRIBUTION_DETAILS",
                columns: table => new
                {
                    CONTRIBUTION_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_BATCH_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_MEMBER_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_UNIT_CODE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    CONTRIBUTION_EMPLOYEE_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_REFERENCE_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CONTRIBUTION_REFERENCE_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTRIBUTION_BASIC_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_FPSBASIC_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_EE_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_ER_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_VE_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_FP_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_LOAN_PRINCIPAL = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_LOAN_INTEREST = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_ENT_BY_USER_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CONTRIBUTION_ENT_EMP_SYS_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CONTRIBUTION_ENT_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CONTRIBUTION_TYPE_CODE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    CONTRIBUTION_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRIBUTION_DETAILS", x => x.CONTRIBUTION_ID);
                });

            migrationBuilder.CreateTable(
                name: "CONTRIBUTION_MAIN",
                columns: table => new
                {
                    CONTRIBUTION_BATCH_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CONTRIBUTION_TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CONTRIBUTION_CATEGORY = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CONTRIBUTION_PAYUNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CONTRIBUTION_PAY_MONTHSTART = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CONTRIBUTION_PAY_MONTHEND = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CONTRIBUTION_STATUS = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    CONTRIBUTION_JVNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CONTRIBUTION_REC_ACTRAN_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CONTRIBUTION_ENT_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    CONTRIBUTION_REFNO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRIBUTION_MAIN", x => x.CONTRIBUTION_BATCH_NO);
                });

            migrationBuilder.CreateTable(
                name: "CONTRIBUTION_PROCESS_LOG",
                columns: table => new
                {
                    LOG_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOG_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LOG_MESSAGE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PROCESS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    USER_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRIBUTION_PROCESS_LOG", x => x.LOG_ID);
                });

            migrationBuilder.CreateTable(
                name: "CONTRIBUTION_TEMP",
                columns: table => new
                {
                    CONTRIBUTION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CONTRIBUTION_BATCH_NO = table.Column<long>(type: "bigint", nullable: false),
                    CONTRIBUTION_MEMBER_NO = table.Column<long>(type: "bigint", nullable: false),
                    CONTRIBUTION_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CONTRIBUTION_EMPLOYEE_NO = table.Column<int>(type: "int", nullable: false),
                    CONTRIBUTION_REFERENCE_NO = table.Column<int>(type: "int", nullable: true),
                    CONTRIBUTION_REFERENCE_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CONTRIBUTION_BASIC_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_FPSBASIC_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_EE_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_ER_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_VE_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_FP_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_LOAN_PRINCIPAL = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_LOAN_INTEREST = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CONTRIBUTION_ENT_BY_USER_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CONTRIBUTION_ENT_EMP_SYS_ID = table.Column<int>(type: "int", nullable: false),
                    CONTRIBUTION_ENT_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CONTRIBUTION_TYPE_CODE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRIBUTION_TEMP", x => x.CONTRIBUTION_ID);
                });

            migrationBuilder.CreateTable(
                name: "SUPERANN_BATCH",
                columns: table => new
                {
                    SN_BATCH_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SN_TRUST_CODE = table.Column<long>(type: "bigint", nullable: true),
                    SN_CATEGORY = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    SN_PAYUNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    SN_PAY_MONTHSTART = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SN_PAY_MONTHEND = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    SN_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    SN_ENT_ON = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SN_CON_AMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SN_PAY_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPERANN_BATCH", x => x.SN_BATCH_NO);
                });

            migrationBuilder.CreateTable(
                name: "SUPERANN_BREAKUP",
                columns: table => new
                {
                    SN_FIN_YER = table.Column<long>(type: "bigint", nullable: true),
                    SN_PIN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    SN_EMP_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SN_FUD_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SN_CON_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    SN_TRS_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SN_EXG_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SN_CON_TYP = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    SN_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    SN_BAT_NO = table.Column<long>(type: "bigint", nullable: true),
                    SN_GRS_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SN_ACT_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SN_PAY_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SUPERANN_CONTRIBUTION",
                columns: table => new
                {
                    SN_SLR_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SN_FIN_YER = table.Column<long>(type: "bigint", nullable: true),
                    SN_PIN_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SN_EMP_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SN_FUD_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SN_CON_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    SN_UNT_NOS = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SN_NAV_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SN_CON_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SN_CON_TYP = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    SN_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPERANN_CONTRIBUTION", x => x.SN_SLR_NUM);
                });

            migrationBuilder.CreateTable(
                name: "SUPERANN_RATE",
                columns: table => new
                {
                    SN_FUD_NUM = table.Column<long>(type: "bigint", nullable: true),
                    SN_MONTH = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    SN_RATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SUPERANN_TRUSTNAME",
                columns: table => new
                {
                    ST_FND_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ST_FND_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPERANN_TRUSTNAME", x => x.ST_FND_NUM);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CONTRIBUTION_BREAKUP");

            migrationBuilder.DropTable(
                name: "CONTRIBUTION_DETAILS");

            migrationBuilder.DropTable(
                name: "CONTRIBUTION_MAIN");

            migrationBuilder.DropTable(
                name: "CONTRIBUTION_PROCESS_LOG");

            migrationBuilder.DropTable(
                name: "CONTRIBUTION_TEMP");

            migrationBuilder.DropTable(
                name: "SUPERANN_BATCH");

            migrationBuilder.DropTable(
                name: "SUPERANN_BREAKUP");

            migrationBuilder.DropTable(
                name: "SUPERANN_CONTRIBUTION");

            migrationBuilder.DropTable(
                name: "SUPERANN_RATE");

            migrationBuilder.DropTable(
                name: "SUPERANN_TRUSTNAME");
        }
    }
}
