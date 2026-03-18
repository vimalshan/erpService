using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccountingService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACC_DET",
                columns: table => new
                {
                    AC_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    AC_TRUST_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    AC_TRAN_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    AC_TRAN_NO = table.Column<long>(type: "bigint", nullable: false),
                    AC_DOC_NO = table.Column<long>(type: "bigint", nullable: false),
                    AC_FIN_YER = table.Column<long>(type: "bigint", nullable: false),
                    AC_DOC_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    AC_MAIN_CODE = table.Column<string>(type: "CHAR(6)", nullable: false),
                    AC_SUB_CODE = table.Column<string>(type: "CHAR(6)", nullable: false),
                    AC_DC_TYPE = table.Column<string>(type: "CHAR(1)", nullable: false),
                    AC_TRAN_AMT = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: false),
                    AC_REF_TRANCODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    AC_REF_TRANNO = table.Column<long>(type: "bigint", nullable: false),
                    AC_REMARKS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACC_DET", x => x.AC_SYS_ID);
                });

            migrationBuilder.CreateTable(
                name: "ACC_LOOKUP",
                columns: table => new
                {
                    CON_TYP = table.Column<string>(type: "CHAR(1)", nullable: false),
                    ED_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    ACC_COD = table.Column<long>(type: "bigint", nullable: true),
                    TRN_TYP = table.Column<string>(type: "CHAR(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACC_LOOKUP", x => x.CON_TYP);
                });

            migrationBuilder.CreateTable(
                name: "MAINACCOUNT_MASTER",
                columns: table => new
                {
                    MAIN_ACCOUNT_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MAIN_ACCOUNT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MAIN_ACCOUNT_SHRT_NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MAIN_CLOSURE_FLAG = table.Column<string>(type: "CHAR(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAINACCOUNT_MASTER", x => x.MAIN_ACCOUNT_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PF_SUB_ACCOUNT",
                columns: table => new
                {
                    SUB_ACC_COD = table.Column<long>(type: "bigint", nullable: false),
                    SUB_ACC_NAM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PF_SUB_ACCOUNT", x => x.SUB_ACC_COD);
                });

            migrationBuilder.CreateTable(
                name: "TRAN_DET",
                columns: table => new
                {
                    TD_TRUST_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    TRANSACTION_ID = table.Column<int>(type: "int", nullable: false),
                    TD_TRANSACTION_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    TD_TRANSACTION_TYPE = table.Column<string>(type: "CHAR(1)", nullable: true),
                    TD_TRANSACTION_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    TD_AMOUNT = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: false),
                    TD_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TD_MEMBER_NO = table.Column<int>(type: "int", nullable: true),
                    TD_REFERENCE_TYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TD_CONTRIBUTION_REFERENCE_NO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TD_TYPE_CODE = table.Column<string>(type: "CHAR(1)", nullable: false),
                    TD_LAST_MODIFIED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    TD_LAST_MODIFIED_EMP_SYSID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TD_FINYEAR = table.Column<long>(type: "bigint", nullable: false),
                    TD_JV_VOUCHER_TYPE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    TD_JV_NO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TD_CANCEL_STATUS = table.Column<long>(type: "bigint", nullable: true),
                    TD_CANCEL_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    TD_TRN_SUB_TYPE = table.Column<string>(type: "CHAR(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAN_DET", x => new { x.TD_TRUST_CODE, x.TRANSACTION_ID });
                });

            migrationBuilder.CreateTable(
                name: "TRANSACTION_MASTER",
                columns: table => new
                {
                    TRANSACTION_TRUST_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    TRANSACTION_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    TRANSACTION_NAME = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TRANSACTION_TYPE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    TRANSACTION_VALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACTION_MASTER", x => new { x.TRANSACTION_TRUST_CODE, x.TRANSACTION_CODE });
                });

            migrationBuilder.CreateTable(
                name: "GL_POSTING",
                columns: table => new
                {
                    POSTING_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACCOUNT_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    POSTING_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    DEBIT_AMOUNT = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: false, defaultValue: 0m),
                    CREDIT_AMOUNT = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: false, defaultValue: 0m),
                    REFERENCE_ID = table.Column<long>(type: "bigint", nullable: false),
                    POSTING_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GL_POSTING", x => x.POSTING_ID);
                    table.ForeignKey(
                        name: "FK_GL_POSTING_ACCOUNT",
                        column: x => x.ACCOUNT_CODE,
                        principalTable: "MAINACCOUNT_MASTER",
                        principalColumn: "MAIN_ACCOUNT_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MAINACCOUNT_MASTER",
                columns: new[] { "MAIN_ACCOUNT_CODE", "MAIN_ACCOUNT_NAME", "MAIN_ACCOUNT_SHRT_NAME", "MAIN_CLOSURE_FLAG" },
                values: new object[,]
                {
                    { "100000", "Cash and Cash Equivalents", "Cash", "N" },
                    { "110000", "Bank Accounts", "Bank", "N" },
                    { "200000", "Member Contributions Payable", "Contributions", "N" },
                    { "210000", "Employer Contributions", "Emp Contrib", "N" },
                    { "300000", "Investment Portfolio", "Investments", "N" },
                    { "310000", "Investment Income", "Inv Income", "N" },
                    { "400000", "Operating Expenses", "Expenses", "N" },
                    { "410000", "Administrative Expenses", "Admin Exp", "N" },
                    { "500000", "Member Benefits Payable", "Benefits", "N" },
                    { "510000", "Withdrawal Benefits", "Withdrawals", "N" }
                });

            migrationBuilder.InsertData(
                table: "PF_SUB_ACCOUNT",
                columns: new[] { "SUB_ACC_COD", "SUB_ACC_NAM" },
                values: new object[,]
                {
                    { 1001L, "Employee Contribution" },
                    { 1002L, "Employer Contribution" },
                    { 1003L, "Voluntary Contribution" },
                    { 2001L, "Normal Withdrawal" },
                    { 2002L, "Death Benefit" },
                    { 2003L, "Disability Benefit" },
                    { 3001L, "Fixed Deposit Return" },
                    { 3002L, "Equity Dividend" }
                });

            migrationBuilder.InsertData(
                table: "TRANSACTION_MASTER",
                columns: new[] { "TRANSACTION_CODE", "TRANSACTION_TRUST_CODE", "TRANSACTION_NAME", "TRANSACTION_TYPE", "TRANSACTION_VALUE" },
                values: new object[,]
                {
                    { "CON", "PF1", "Contributions", "C  ", "Member Contribution" },
                    { "DIV", "PF1", "Dividend", "D  ", "Investment Dividend" },
                    { "JV1", "PF1", "Journal Voucher", "J  ", "Journal Entry" },
                    { "TRF", "PF1", "Transfer In/Out", "T  ", "Fund Transfer" },
                    { "WIT", "PF1", "Withdrawal", "W  ", "Member Withdrawal" },
                    { "CON", "PF2", "Contributions", "C  ", "Member Contribution" },
                    { "WIT", "PF2", "Withdrawal", "W  ", "Member Withdrawal" }
                });

            migrationBuilder.CreateIndex(
                name: "IDX_ACC_DET_TRUST_DATE",
                table: "ACC_DET",
                columns: new[] { "AC_TRUST_CODE", "AC_DOC_DAT" });

            migrationBuilder.CreateIndex(
                name: "IDX_GL_POSTING_ACCOUNT",
                table: "GL_POSTING",
                columns: new[] { "ACCOUNT_CODE", "POSTING_DATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_TRAN_DET_TYPE",
                table: "TRAN_DET",
                columns: new[] { "TD_TRANSACTION_TYPE", "TD_TRANSACTION_DATE" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACC_DET");

            migrationBuilder.DropTable(
                name: "ACC_LOOKUP");

            migrationBuilder.DropTable(
                name: "GL_POSTING");

            migrationBuilder.DropTable(
                name: "PF_SUB_ACCOUNT");

            migrationBuilder.DropTable(
                name: "TRAN_DET");

            migrationBuilder.DropTable(
                name: "TRANSACTION_MASTER");

            migrationBuilder.DropTable(
                name: "MAINACCOUNT_MASTER");
        }
    }
}
