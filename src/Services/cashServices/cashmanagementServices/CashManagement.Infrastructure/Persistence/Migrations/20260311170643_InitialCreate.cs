using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BANK_ACCOUNT",
                columns: table => new
                {
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: false),
                    BANK_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BANK_ACCOUNT_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BANK_BRANCH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BANK_ACCOUNT_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BANK_ACCOUNT_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANK_ACCOUNT", x => x.BANK_ACCOUNT_ID);
                });

            migrationBuilder.CreateTable(
                name: "BANK_RECONCILIATION",
                columns: table => new
                {
                    RECON_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: false),
                    BANK_STATEMENT_BALANCE = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    LEDGER_BALANCE = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    UNCLEARED_CHEQUES = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DIFFERENCE_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    RECONCILIATION_STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RECONCILIATION_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANK_RECONCILIATION", x => x.RECON_ID);
                });

            migrationBuilder.CreateTable(
                name: "CASH_UNIT",
                columns: table => new
                {
                    CASH_UNIT_ID = table.Column<long>(type: "bigint", nullable: false),
                    CASH_UNIT_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CASH_UNIT_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CASH_UNIT_LOCATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CASH_UNIT_INCHARGE = table.Column<long>(type: "bigint", nullable: true),
                    CASH_UNIT_OPENINGBAL = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CASH_UNIT_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CASH_UNIT", x => x.CASH_UNIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "CHEQUE_REGISTER_AUDIT",
                columns: table => new
                {
                    AUDIT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CHEQUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: false),
                    CHEQUE_NUMBER = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PREVIOUS_STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NEW_STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AUDIT_ACTION = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AUDIT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHEQUE_REGISTER_AUDIT", x => x.AUDIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "BANK_TRANSACTION",
                columns: table => new
                {
                    BANK_TXN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: false),
                    BANK_TXN_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    BANK_TXN_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    BANK_TXN_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BANK_TXN_REFERENCE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BANK_TXN_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BANK_TXN_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANK_TRANSACTION", x => x.BANK_TXN_ID);
                    table.ForeignKey(
                        name: "FK_BANK_TRANSACTION_BANK_ACCOUNT_BANK_ACCOUNT_ID",
                        column: x => x.BANK_ACCOUNT_ID,
                        principalTable: "BANK_ACCOUNT",
                        principalColumn: "BANK_ACCOUNT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHEQUE_REGISTER",
                columns: table => new
                {
                    CHEQUE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: false),
                    CHEQUE_NUMBER = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PAYEE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CHEQUE_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CHEQUE_ISSUE_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    CHEQUE_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    CHEQUE_REFERENCE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CHEQUE_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CHEQUE_BOUNCE_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHEQUE_REGISTER", x => x.CHEQUE_ID);
                    table.ForeignKey(
                        name: "FK_CHEQUE_REGISTER_BANK_ACCOUNT_BANK_ACCOUNT_ID",
                        column: x => x.BANK_ACCOUNT_ID,
                        principalTable: "BANK_ACCOUNT",
                        principalColumn: "BANK_ACCOUNT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CASH_TRANSACTION",
                columns: table => new
                {
                    CASH_TXN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CASH_UNIT_ID = table.Column<long>(type: "bigint", nullable: false),
                    CASH_TXN_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    CASH_TXN_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CASH_TXN_SOURCE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CASH_TXN_PAYEE_ID = table.Column<long>(type: "bigint", nullable: true),
                    CASH_TXN_REF_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CASH_TXN_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CASH_TXN_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CASH_TXN_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    AUTHORIZED_BY = table.Column<long>(type: "bigint", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CASH_TRANSACTION", x => x.CASH_TXN_ID);
                    table.ForeignKey(
                        name: "FK_CASH_TRANSACTION_CASH_UNIT_CASH_UNIT_ID",
                        column: x => x.CASH_UNIT_ID,
                        principalTable: "CASH_UNIT",
                        principalColumn: "CASH_UNIT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BANK_RECONCILIATION_ACCOUNT",
                table: "BANK_RECONCILIATION",
                column: "BANK_ACCOUNT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BANK_TRANSACTION_ACCOUNT_DATE",
                table: "BANK_TRANSACTION",
                columns: new[] { "BANK_ACCOUNT_ID", "BANK_TXN_DATE" });

            migrationBuilder.CreateIndex(
                name: "IX_BANK_TRANSACTION_TYPE",
                table: "BANK_TRANSACTION",
                column: "BANK_TXN_TYPE");

            migrationBuilder.CreateIndex(
                name: "IX_CASH_TRANSACTION_TYPE",
                table: "CASH_TRANSACTION",
                column: "CASH_TXN_TYPE");

            migrationBuilder.CreateIndex(
                name: "IX_CASH_TRANSACTION_UNIT_DATE",
                table: "CASH_TRANSACTION",
                columns: new[] { "CASH_UNIT_ID", "CASH_TXN_DATE" });

            migrationBuilder.CreateIndex(
                name: "IX_CHEQUE_REGISTER_ACCOUNT",
                table: "CHEQUE_REGISTER",
                column: "BANK_ACCOUNT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CHEQUE_REGISTER_STATUS",
                table: "CHEQUE_REGISTER",
                column: "CHEQUE_STATUS");

            migrationBuilder.CreateIndex(
                name: "UQ_CHEQUE_REGISTER",
                table: "CHEQUE_REGISTER",
                columns: new[] { "BANK_ACCOUNT_ID", "CHEQUE_NUMBER" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BANK_RECONCILIATION");

            migrationBuilder.DropTable(
                name: "BANK_TRANSACTION");

            migrationBuilder.DropTable(
                name: "CASH_TRANSACTION");

            migrationBuilder.DropTable(
                name: "CHEQUE_REGISTER");

            migrationBuilder.DropTable(
                name: "CHEQUE_REGISTER_AUDIT");

            migrationBuilder.DropTable(
                name: "CASH_UNIT");

            migrationBuilder.DropTable(
                name: "BANK_ACCOUNT");
        }
    }
}
