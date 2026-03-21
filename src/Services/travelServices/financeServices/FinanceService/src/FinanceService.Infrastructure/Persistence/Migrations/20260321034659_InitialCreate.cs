using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AP_INVOICES_INTERFACE",
                columns: table => new
                {
                    INVOICE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INVOICE_NUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INVOICE_TYPE_LOOKUP_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    INVOICE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_ID = table.Column<long>(type: "bigint", nullable: true),
                    VENDOR_SITE_ID = table.Column<long>(type: "bigint", nullable: true),
                    INVOICE_AMOUNT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    INVOICE_CURRENCY_CODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EXCHANGE_RATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EXCHANGE_RATE_TYPE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TERMS_ID = table.Column<long>(type: "bigint", nullable: true),
                    PAYMENT_METHOD_LOOKUP_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                    LAST_UPDATE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    CREATION_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    ORG_ID = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    AGENCY_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AP_INVOICES_INTERFACE", x => x.INVOICE_ID);
                });

            migrationBuilder.CreateTable(
                name: "AP_TERMS_TL",
                columns: table => new
                {
                    TERM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ENABLED_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DUE_CUTOFF_DAY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AP_TERMS_TL", x => x.TERM_ID);
                });

            migrationBuilder.CreateTable(
                name: "JVPOSTDET",
                columns: table => new
                {
                    JVINTCODE = table.Column<long>(type: "bigint", nullable: false),
                    JVDOCNUM = table.Column<int>(type: "int", nullable: false),
                    JV_COM_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    JV_GRD_TYP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    JV_ST_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JV_ED_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JV_COMMENT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JV_STATUS = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JV_PAY_NUM = table.Column<long>(type: "bigint", nullable: true),
                    JV_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JVPOSTDET", x => x.JVINTCODE);
                });

            migrationBuilder.CreateTable(
                name: "PAY_JV",
                columns: table => new
                {
                    AC_ENT_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    AC_FIN_YER = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AC_DOC_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AC_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AC_PAY_NUM = table.Column<long>(type: "bigint", nullable: true),
                    AC_PAY_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AC_ACC_COD = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: true),
                    AC_TRN_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    AC_NAR_TON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AC_PST_TYP = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    AC_ENT_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AC_CAN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AC_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAY_JV", x => new { x.AC_SRL_NUM, x.AC_FIN_YER, x.AC_DOC_NUM, x.AC_ENT_COD });
                });

            migrationBuilder.CreateTable(
                name: "PAY_OTHDET",
                columns: table => new
                {
                    PY_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    PY_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PY_PAY_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_VND_COD = table.Column<long>(type: "bigint", nullable: true),
                    PY_TRN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PY_PAY_MOD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    PY_PAY_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    PY_CHQ_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PY_CHQ_NUM = table.Column<long>(type: "bigint", nullable: true),
                    PY_PAY_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PY_REM_MRK = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PY_STS_COD = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAY_OTHDET", x => x.PY_COM_COD);
                });

            migrationBuilder.CreateTable(
                name: "PAYMENT_DETAILS",
                columns: table => new
                {
                    SNO = table.Column<long>(type: "bigint", nullable: true),
                    BOOK_NO = table.Column<long>(type: "bigint", nullable: true),
                    VENDOR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TS_TKT_CST = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TS_TKT_ADJ = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TS_BASE_STAX = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TS_APPROVE_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TS_STATUS = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    TM_INV_NUM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    TM_INV_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TM_INV_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TM_TOTAPPRAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TM_TOTAL = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TM_JVNO = table.Column<long>(type: "bigint", nullable: true),
                    TM_PAYMENTTERMS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SERVICETAX = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_ACCOUNT",
                columns: table => new
                {
                    AC_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AC_UNT_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    AC_USR_COD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AC_USR_NUM = table.Column<long>(type: "bigint", nullable: true),
                    AC_DC_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    AC_TRN_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    AC_ACC_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    AC_REM_MRK = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AC_ACC_TYP = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    AC_JV_STS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_ACCOUNT", x => x.AC_TRN_NUM);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_BATCH_MAIN",
                columns: table => new
                {
                    TM_UNT_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    TM_BAT_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    TM_BAT_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TM_INV_NUM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TM_INV_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TM_BAT_STS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    TM_ADM_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TM_FIN_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TM_AGN_COD = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    TM_TOTAPPRAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TM_TOTAL = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TM_JVNO = table.Column<long>(type: "bigint", nullable: true),
                    TM_CGSTAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TM_SGSTAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TM_IGSTAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_BATCH_MAIN", x => new { x.TM_UNT_COD, x.TM_BAT_NUM });
                });

            migrationBuilder.CreateTable(
                name: "AP_INVOICE_LINES_INTERFACE",
                columns: table => new
                {
                    INVOICE_ID = table.Column<long>(type: "bigint", nullable: false),
                    LINE_NUMBER = table.Column<long>(type: "bigint", nullable: false),
                    INVOICE_LINE_ID = table.Column<long>(type: "bigint", nullable: true),
                    LINE_TYPE_LOOKUP_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ACCOUNTING_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    LAST_UPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ORG_ID = table.Column<long>(type: "bigint", nullable: true),
                    ACCOUNT_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PROJECT_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SGSTAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    CGSTAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    IGSTAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AP_INVOICE_LINES_INTERFACE", x => new { x.INVOICE_ID, x.LINE_NUMBER });
                    table.ForeignKey(
                        name: "FK_AP_INVOICE_LINES_INTERFACE_AP_INVOICES_INTERFACE_INVOICE_ID",
                        column: x => x.INVOICE_ID,
                        principalTable: "AP_INVOICES_INTERFACE",
                        principalColumn: "INVOICE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_BATCH_SUB",
                columns: table => new
                {
                    TS_UNT_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    TS_BAT_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    TS_SRL_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    TS_BOK_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    TS_TKT_CST = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TS_TKT_ADJ = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TS_APPROVE_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TS_REASON = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TS_STATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TS_CGSTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TS_SGSTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TS_IGSTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_BATCH_SUB", x => new { x.TS_UNT_COD, x.TS_BAT_NUM, x.TS_SRL_NUM });
                    table.ForeignKey(
                        name: "FK_TRAVEL_BATCH_SUB_TRAVEL_BATCH_MAIN_TS_UNT_COD_TS_BAT_NUM",
                        columns: x => new { x.TS_UNT_COD, x.TS_BAT_NUM },
                        principalTable: "TRAVEL_BATCH_MAIN",
                        principalColumns: new[] { "TM_UNT_COD", "TM_BAT_NUM" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AP_INVOICE_LINES_INTERFACE");

            migrationBuilder.DropTable(
                name: "AP_TERMS_TL");

            migrationBuilder.DropTable(
                name: "JVPOSTDET");

            migrationBuilder.DropTable(
                name: "PAY_JV");

            migrationBuilder.DropTable(
                name: "PAY_OTHDET");

            migrationBuilder.DropTable(
                name: "PAYMENT_DETAILS");

            migrationBuilder.DropTable(
                name: "TRAVEL_ACCOUNT");

            migrationBuilder.DropTable(
                name: "TRAVEL_BATCH_SUB");

            migrationBuilder.DropTable(
                name: "AP_INVOICES_INTERFACE");

            migrationBuilder.DropTable(
                name: "TRAVEL_BATCH_MAIN");
        }
    }
}
