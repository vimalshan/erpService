using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionProcessing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "TRANSACTION_BATCH",
                schema: "dbo",
                columns: table => new
                {
                    BATCH_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BATCH_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BATCH_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BATCH_TOTAL_COUNT = table.Column<int>(type: "int", nullable: true),
                    BATCH_SUCCESS_COUNT = table.Column<int>(type: "int", nullable: true),
                    BATCH_FAILURE_COUNT = table.Column<int>(type: "int", nullable: true),
                    BATCH_TOTAL_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COMPLETED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACTION_BATCH", x => x.BATCH_ID);
                });

            migrationBuilder.CreateTable(
                name: "FINANCIAL_TRANSACTION",
                schema: "dbo",
                columns: table => new
                {
                    TXN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TXN_BATCH_ID = table.Column<long>(type: "bigint", nullable: true),
                    TXN_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TXN_SUB_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TXN_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TXN_CURRENCY_ID = table.Column<long>(type: "bigint", nullable: true),
                    TXN_EXCHANGE_RATE = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    TXN_BASE_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    TXN_REFERENCE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TXN_SOURCE_SERVICE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TXN_SOURCE_ID = table.Column<long>(type: "bigint", nullable: true),
                    TXN_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TXN_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCIAL_TRANSACTION", x => x.TXN_ID);
                    table.ForeignKey(
                        name: "FK_FINANCIAL_TRANSACTION_TRANSACTION_BATCH_TXN_BATCH_ID",
                        column: x => x.TXN_BATCH_ID,
                        principalSchema: "dbo",
                        principalTable: "TRANSACTION_BATCH",
                        principalColumn: "BATCH_ID");
                });

            migrationBuilder.CreateTable(
                name: "DEAL_SETTLEMENT_PROC",
                schema: "dbo",
                columns: table => new
                {
                    SETTLEMENT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TXN_ID = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_ID = table.Column<long>(type: "bigint", nullable: false),
                    SET_ID = table.Column<long>(type: "bigint", nullable: false),
                    SETTLEMENT_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SPOT_RATE = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    EXCHANGE_RATE = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    SETTLEMENT_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    GAIN_LOSS_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    PREMIUM_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    WINDING_FEE = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    NET_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: true),
                    PROCESSING_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_SETTLEMENT_PROC", x => x.SETTLEMENT_ID);
                    table.ForeignKey(
                        name: "FK_DEAL_SETTLEMENT_PROC_FINANCIAL_TRANSACTION_TXN_ID",
                        column: x => x.TXN_ID,
                        principalSchema: "dbo",
                        principalTable: "FINANCIAL_TRANSACTION",
                        principalColumn: "TXN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_DISBURSEMENT_PROC",
                schema: "dbo",
                columns: table => new
                {
                    DISB_PROC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TXN_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_ID = table.Column<long>(type: "bigint", nullable: false),
                    DISB_ID = table.Column<long>(type: "bigint", nullable: false),
                    DISB_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    EXCHANGE_RATE = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    CONVERTED_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: true),
                    PROCESSING_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_DISBURSEMENT_PROC", x => x.DISB_PROC_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_DISBURSEMENT_PROC_FINANCIAL_TRANSACTION_TXN_ID",
                        column: x => x.TXN_ID,
                        principalSchema: "dbo",
                        principalTable: "FINANCIAL_TRANSACTION",
                        principalColumn: "TXN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_REPAYMENT_PROC",
                schema: "dbo",
                columns: table => new
                {
                    REPAY_PROC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TXN_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_ID = table.Column<long>(type: "bigint", nullable: false),
                    REPAY_ID = table.Column<long>(type: "bigint", nullable: false),
                    REPAY_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    EXCHANGE_RATE = table.Column<decimal>(type: "decimal(18,8)", nullable: true),
                    CONVERTED_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    BANK_ACCOUNT_ID = table.Column<long>(type: "bigint", nullable: true),
                    PROCESSING_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_REPAYMENT_PROC", x => x.REPAY_PROC_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_REPAYMENT_PROC_FINANCIAL_TRANSACTION_TXN_ID",
                        column: x => x.TXN_ID,
                        principalSchema: "dbo",
                        principalTable: "FINANCIAL_TRANSACTION",
                        principalColumn: "TXN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRANSACTION_AUDIT",
                schema: "dbo",
                columns: table => new
                {
                    AUDIT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TXN_ID = table.Column<long>(type: "bigint", nullable: false),
                    PREVIOUS_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NEW_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AUDIT_ACTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AUDIT_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AUDIT_BY = table.Column<long>(type: "bigint", nullable: false),
                    AUDIT_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionTxnId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACTION_AUDIT", x => x.AUDIT_ID);
                    table.ForeignKey(
                        name: "FK_TRANSACTION_AUDIT_FINANCIAL_TRANSACTION_TXN_ID",
                        column: x => x.TXN_ID,
                        principalSchema: "dbo",
                        principalTable: "FINANCIAL_TRANSACTION",
                        principalColumn: "TXN_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TRANSACTION_AUDIT_FINANCIAL_TRANSACTION_TransactionTxnId",
                        column: x => x.TransactionTxnId,
                        principalSchema: "dbo",
                        principalTable: "FINANCIAL_TRANSACTION",
                        principalColumn: "TXN_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_SETTLEMENT_PROC_DEAL",
                schema: "dbo",
                table: "DEAL_SETTLEMENT_PROC",
                column: "DEAL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_SETTLEMENT_PROC_TXN_ID",
                schema: "dbo",
                table: "DEAL_SETTLEMENT_PROC",
                column: "TXN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FINANCIAL_TRANSACTION_BATCH",
                schema: "dbo",
                table: "FINANCIAL_TRANSACTION",
                column: "TXN_BATCH_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FINANCIAL_TRANSACTION_DATE",
                schema: "dbo",
                table: "FINANCIAL_TRANSACTION",
                column: "CREATED_ON");

            migrationBuilder.CreateIndex(
                name: "IX_FINANCIAL_TRANSACTION_STATUS",
                schema: "dbo",
                table: "FINANCIAL_TRANSACTION",
                column: "TXN_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_DISBURSEMENT_PROC_LOAN",
                schema: "dbo",
                table: "LOAN_DISBURSEMENT_PROC",
                column: "LOAN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_DISBURSEMENT_PROC_TXN_ID",
                schema: "dbo",
                table: "LOAN_DISBURSEMENT_PROC",
                column: "TXN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_REPAYMENT_PROC_LOAN",
                schema: "dbo",
                table: "LOAN_REPAYMENT_PROC",
                column: "LOAN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_REPAYMENT_PROC_TXN_ID",
                schema: "dbo",
                table: "LOAN_REPAYMENT_PROC",
                column: "TXN_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_AUDIT_TransactionTxnId",
                schema: "dbo",
                table: "TRANSACTION_AUDIT",
                column: "TransactionTxnId");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_AUDIT_TXN",
                schema: "dbo",
                table: "TRANSACTION_AUDIT",
                column: "TXN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_BATCH_STATUS",
                schema: "dbo",
                table: "TRANSACTION_BATCH",
                column: "BATCH_STATUS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEAL_SETTLEMENT_PROC",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LOAN_DISBURSEMENT_PROC",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LOAN_REPAYMENT_PROC",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRANSACTION_AUDIT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FINANCIAL_TRANSACTION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TRANSACTION_BATCH",
                schema: "dbo");
        }
    }
}
