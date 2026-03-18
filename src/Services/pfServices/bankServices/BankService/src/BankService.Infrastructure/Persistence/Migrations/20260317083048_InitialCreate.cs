using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankService.Infrastructure.Persistence.Migrations
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
                    ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACCOUNT_NUMBER = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ACCOUNT_TITLE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BANK_CODE = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    ACCOUNT_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ACCOUNT_BALANCE = table.Column<decimal>(type: "decimal(19,0)", nullable: false, defaultValue: 0m),
                    ACCOUNT_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A"),
                    OPENING_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CLOSING_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANK_ACCOUNT", x => x.ACCOUNT_ID);
                });

            migrationBuilder.CreateTable(
                name: "BANK_MASTER",
                columns: table => new
                {
                    BANK_TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    BANK_CODE = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    BANK_NAME = table.Column<string>(type: "nchar(65)", fixedLength: true, maxLength: 65, nullable: false),
                    MICR_CODE = table.Column<string>(type: "nchar(9)", fixedLength: true, maxLength: 9, nullable: false),
                    BRANCH_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    BRANCH_ADDRESS_LINE_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BRANCH_ADDRESS_LINE_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BRANCH_ADDRESS_LINE_3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BRANCH_ADDRESS_LINE_4 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BRANCH_PHONE_NO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BRANCH_FAX_NO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BRANCH_EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BRANCH_CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    BRANCH_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BANK_MASTER", x => new { x.BANK_TRUST_CODE, x.BANK_CODE });
                });

            migrationBuilder.CreateTable(
                name: "CHEQUE_DET",
                columns: table => new
                {
                    CHEQUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    CHEQUE_ACTRAN_NO = table.Column<long>(type: "bigint", nullable: true),
                    CHEQUE_BRANCH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CHEQUE_NO = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    CHEQUE_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    CHEQUE_BANK = table.Column<long>(type: "bigint", nullable: true),
                    CHEQUE_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CHEQUE_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    CHEQUE_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "I"),
                    CHEQUE_PAYEE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CHEQUE_CLEARED_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHEQUE_DET", x => x.CHEQUE_ID);
                });

            migrationBuilder.CreateTable(
                name: "CHEQUE_REGISTER",
                columns: table => new
                {
                    REGISTER_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CHEQUE_NO_FROM = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CHEQUE_NO_TO = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    CHEQUE_BOOK_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ISSUED_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    REGISTER_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHEQUE_REGISTER", x => x.REGISTER_ID);
                    table.ForeignKey(
                        name: "FK_CHEQUE_REGISTER_ACCOUNT",
                        column: x => x.ACCOUNT_ID,
                        principalTable: "BANK_ACCOUNT",
                        principalColumn: "ACCOUNT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PAYMENT_RECONCILIATION",
                columns: table => new
                {
                    RECON_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CHEQUE_ID = table.Column<long>(type: "bigint", nullable: false),
                    RECON_REFERENCE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RECON_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    RECON_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    RECON_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "O")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAYMENT_RECONCILIATION", x => x.RECON_ID);
                    table.ForeignKey(
                        name: "FK_PAYMENT_RECON_CHEQUE",
                        column: x => x.CHEQUE_ID,
                        principalTable: "CHEQUE_DET",
                        principalColumn: "CHEQUE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_BANK_ACCOUNT_TRUST",
                table: "BANK_ACCOUNT",
                columns: new[] { "TRUST_CODE", "ACCOUNT_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IX_BANK_ACCOUNT_ACCOUNT_NUMBER",
                table: "BANK_ACCOUNT",
                column: "ACCOUNT_NUMBER",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_BANK_MASTER_TRUST",
                table: "BANK_MASTER",
                columns: new[] { "BANK_TRUST_CODE", "BRANCH_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IDX_CHEQUE_DET_STATUS",
                table: "CHEQUE_DET",
                columns: new[] { "CHEQUE_STATUS", "CHEQUE_DATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_CHEQUE_REGISTER_ACCOUNT",
                table: "CHEQUE_REGISTER",
                columns: new[] { "ACCOUNT_ID", "REGISTER_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IX_PAYMENT_RECONCILIATION_CHEQUE_ID",
                table: "PAYMENT_RECONCILIATION",
                column: "CHEQUE_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BANK_MASTER");

            migrationBuilder.DropTable(
                name: "CHEQUE_REGISTER");

            migrationBuilder.DropTable(
                name: "PAYMENT_RECONCILIATION");

            migrationBuilder.DropTable(
                name: "BANK_ACCOUNT");

            migrationBuilder.DropTable(
                name: "CHEQUE_DET");
        }
    }
}
