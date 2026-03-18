using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SettlementService.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SET_MAIN",
                columns: table => new
                {
                    ST_SET_NUM = table.Column<long>(type: "bigint", nullable: false),
                    ST_TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    ST_MEMBER_NO = table.Column<long>(type: "bigint", nullable: true),
                    ST_SET_TYPE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    ST_SET_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ST_DOL_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ST_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ST_UPDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ST_UPDBY_EMP_SYSID = table.Column<long>(type: "bigint", nullable: true),
                    ST_ACC_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ST_FINYEAR = table.Column<long>(type: "bigint", nullable: true),
                    ST_JV_VOUCHER_TYPE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    ST_JV_NO = table.Column<long>(type: "bigint", nullable: true),
                    ST_SET_INT_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    ST_TAXSTS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ST_TAXRATE = table.Column<long>(type: "bigint", nullable: true),
                    ST_SETTLEMENT_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ST_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "P")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_MAIN", x => x.ST_SET_NUM);
                });

            migrationBuilder.CreateTable(
                name: "SET_APPROVAL",
                columns: table => new
                {
                    APR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SET_NUM = table.Column<long>(type: "bigint", nullable: false),
                    APR_LEVEL = table.Column<int>(type: "int", nullable: false),
                    APR_BY_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    APR_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APR_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    APR_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_APPROVAL", x => x.APR_ID);
                    table.ForeignKey(
                        name: "FK_SET_APPROVAL_SET_MAIN_SET_NUM",
                        column: x => x.SET_NUM,
                        principalTable: "SET_MAIN",
                        principalColumn: "ST_SET_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SET_DEDUCTION",
                columns: table => new
                {
                    SET_DED_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SET_NUM = table.Column<long>(type: "bigint", nullable: false),
                    DED_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DED_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_DEDUCTION", x => x.SET_DED_ID);
                    table.ForeignKey(
                        name: "FK_SET_DEDUCTION_SET_MAIN_SET_NUM",
                        column: x => x.SET_NUM,
                        principalTable: "SET_MAIN",
                        principalColumn: "ST_SET_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SET_PAYMENT",
                columns: table => new
                {
                    PAY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SET_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PAY_MODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PAY_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    PAY_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    PAY_REF_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PAY_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "P")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SET_PAYMENT", x => x.PAY_ID);
                    table.ForeignKey(
                        name: "FK_SET_PAYMENT_SET_MAIN_SET_NUM",
                        column: x => x.SET_NUM,
                        principalTable: "SET_MAIN",
                        principalColumn: "ST_SET_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SET_APPROVAL_SET_NUM",
                table: "SET_APPROVAL",
                column: "SET_NUM");

            migrationBuilder.CreateIndex(
                name: "IX_SET_DEDUCTION_SET_NUM",
                table: "SET_DEDUCTION",
                column: "SET_NUM");

            migrationBuilder.CreateIndex(
                name: "IDX_SET_MAIN_MEMBER",
                table: "SET_MAIN",
                column: "ST_MEMBER_NO");

            migrationBuilder.CreateIndex(
                name: "IDX_SET_MAIN_STATUS",
                table: "SET_MAIN",
                columns: new[] { "ST_STATUS", "ST_SET_DATE" });

            migrationBuilder.CreateIndex(
                name: "IX_SET_PAYMENT_SET_NUM",
                table: "SET_PAYMENT",
                column: "SET_NUM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SET_APPROVAL");

            migrationBuilder.DropTable(
                name: "SET_DEDUCTION");

            migrationBuilder.DropTable(
                name: "SET_PAYMENT");

            migrationBuilder.DropTable(
                name: "SET_MAIN");
        }
    }
}
