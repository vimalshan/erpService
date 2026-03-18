using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanService.Infrastructure.Persistence.Migrations
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
                    LOAN_NO = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_TRUST_CODE = table.Column<string>(type: "char(3)", nullable: true),
                    LOAN_MEMBER_ID = table.Column<long>(type: "bigint", nullable: true),
                    LOAN_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    LOAN_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    LOAN_TYPE = table.Column<long>(type: "bigint", nullable: true),
                    LOAN_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LOAN_TENURE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LOAN_PRINCIPALOS = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    LOAN_CLSFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    LOAN_UPDBY_EMP_SYSIDC = table.Column<long>(type: "bigint", nullable: true),
                    LOAN_UPDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    LOAN_STATUS = table.Column<string>(type: "char(1)", nullable: false, defaultValue: "A"),
                    LOAN_RATE = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    LOAN_APPROVAL_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    LOAN_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_MAIN", x => x.LOAN_NO);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_DEDUCTION",
                columns: table => new
                {
                    DED_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOAN_NO = table.Column<long>(type: "bigint", nullable: false),
                    CONTRIBUTION_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DED_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DED_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_DEDUCTION", x => x.DED_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_DEDUCTION_LOAN_MAIN_LOAN_NO",
                        column: x => x.LOAN_NO,
                        principalTable: "LOAN_MAIN",
                        principalColumn: "LOAN_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_REPAYMENT",
                columns: table => new
                {
                    REPAY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOAN_NO = table.Column<long>(type: "bigint", nullable: false),
                    REPAY_INSTALLMENT_NO = table.Column<int>(type: "int", nullable: false),
                    REPAY_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    REPAY_DUE_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    REPAY_PAID_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    REPAY_PAID_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    REPAY_STATUS = table.Column<string>(type: "char(1)", nullable: false, defaultValue: "O")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_REPAYMENT", x => x.REPAY_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_REPAYMENT_LOAN_MAIN_LOAN_NO",
                        column: x => x.LOAN_NO,
                        principalTable: "LOAN_MAIN",
                        principalColumn: "LOAN_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_DEDUCTION_LOAN_NO",
                table: "LOAN_DEDUCTION",
                column: "LOAN_NO");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_MAIN_MEMBER",
                table: "LOAN_MAIN",
                column: "LOAN_MEMBER_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_REPAYMENT_LOAN",
                table: "LOAN_REPAYMENT",
                columns: new[] { "LOAN_NO", "REPAY_STATUS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOAN_DEDUCTION");

            migrationBuilder.DropTable(
                name: "LOAN_REPAYMENT");

            migrationBuilder.DropTable(
                name: "LOAN_MAIN");
        }
    }
}
