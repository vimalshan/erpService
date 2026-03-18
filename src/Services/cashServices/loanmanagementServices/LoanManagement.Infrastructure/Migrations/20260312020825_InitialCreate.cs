using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOAN_MAIN",
                columns: table => new
                {
                    LOAN_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOAN_KEY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    LOAN_ORGID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOAN_ORGCURR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LOAN_CURR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LOAN_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOAN_TYPEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOAN_BANKID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOAN_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOAN_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOAN_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LOAN_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOAN_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOAN_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_MAIN", x => x.LOAN_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_DISBSCH",
                columns: table => new
                {
                    DISB_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DISB_LOANID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DISB_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DISB_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DISB_EXCRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DISB_EXCAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DISB_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    DISB_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_DISBSCH", x => x.DISB_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_DISBSCH_MAIN",
                        column: x => x.DISB_LOANID,
                        principalTable: "LOAN_MAIN",
                        principalColumn: "LOAN_ID");
                });

            migrationBuilder.CreateTable(
                name: "LOAN_INTEREST",
                columns: table => new
                {
                    INT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INT_LOANID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    INT_RATETYPE = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    INT_PER = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INT_FLOATTYPEID = table.Column<long>(type: "bigint", nullable: true),
                    INT_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INT_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_INTEREST", x => x.INT_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_INTEREST_MAIN",
                        column: x => x.INT_LOANID,
                        principalTable: "LOAN_MAIN",
                        principalColumn: "LOAN_ID");
                });

            migrationBuilder.CreateTable(
                name: "LOAN_REPAYSCH",
                columns: table => new
                {
                    REPAY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REPAY_LOANID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    REPAY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REPAY_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    REPAY_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    REPAY_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REPAY_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_REPAYSCH", x => x.REPAY_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_REPAYSCH_MAIN",
                        column: x => x.REPAY_LOANID,
                        principalTable: "LOAN_MAIN",
                        principalColumn: "LOAN_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_DISBSCH_DATE",
                table: "LOAN_DISBSCH",
                column: "DISB_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_DISBSCH_LOANID",
                table: "LOAN_DISBSCH",
                column: "DISB_LOANID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_INTEREST_LOANID",
                table: "LOAN_INTEREST",
                column: "INT_LOANID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_MAIN_DATE",
                table: "LOAN_MAIN",
                column: "LOAN_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_MAIN_ORGID",
                table: "LOAN_MAIN",
                column: "LOAN_ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_REPAYSCH_DATE",
                table: "LOAN_REPAYSCH",
                column: "REPAY_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_REPAYSCH_LOANID",
                table: "LOAN_REPAYSCH",
                column: "REPAY_LOANID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOAN_DISBSCH");

            migrationBuilder.DropTable(
                name: "LOAN_INTEREST");

            migrationBuilder.DropTable(
                name: "LOAN_REPAYSCH");

            migrationBuilder.DropTable(
                name: "LOAN_MAIN");
        }
    }
}
