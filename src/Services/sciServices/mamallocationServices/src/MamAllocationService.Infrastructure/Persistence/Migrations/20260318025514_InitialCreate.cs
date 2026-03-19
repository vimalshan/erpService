using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MamAllocationService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MAM_ALLOCATION_DET",
                columns: table => new
                {
                    ALL_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALL_RM = table.Column<int>(type: "int", nullable: false),
                    ALL_ENTOPEN = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_FORMIVDDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_FORMIVIDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_FORMIVIDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_FORMIVDDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_FORMIVIDFWO = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_FORMIVDDFWO = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_CLOSEDDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_CLOSEIDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_CLOSEIDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_CLOSEDDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_ENTDEBIT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_PRODENTDEBIT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_DISPENTCREDIT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_NETENT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_ADDDDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_ADDIDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_ADDIDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_ADDDDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_PROD = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_CONS = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_RG1DDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_RG1DDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_CLOSERG1DDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_CLOSERG1DDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_SALEFORMIVIDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_SALEFORMIVDDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_SALERG1DDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_SALE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_ADDRGDDF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_ADDRGDDP = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAM_ALLOCATION_DET", x => new { x.ALL_DATE, x.ALL_RM });
                });

            migrationBuilder.CreateTable(
                name: "MAM_ALLOCATION_PRODDET",
                columns: table => new
                {
                    ALL_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    ALL_SRL = table.Column<long>(type: "bigint", nullable: true),
                    ALL_FG = table.Column<int>(type: "int", nullable: true),
                    DDF_QTY = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DDP_QTY = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    PRD_QTY = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ALL_RM = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MAM_ALLOCATIONFG",
                columns: table => new
                {
                    ALL_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    FG_CODE = table.Column<long>(type: "bigint", nullable: true),
                    DOM_DISPATCH = table.Column<int>(type: "int", nullable: true),
                    EXP_DISPATCH = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DUTY_FREE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DUTY_PAID = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MAM_ARRIVAL_DET",
                columns: table => new
                {
                    ARRIVAL_NO = table.Column<long>(type: "bigint", nullable: true),
                    ARRIVAL_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    ARRIVAL_QTY = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ARRIVAL_ITEM = table.Column<int>(type: "int", nullable: true),
                    ARRIVAL_RECEIPTNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MAM_CONSUMPTION_DET",
                columns: table => new
                {
                    CONSUMPTION_NO = table.Column<long>(type: "bigint", nullable: true),
                    CONSUMPTION_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CONSUMPTION_RM = table.Column<int>(type: "int", nullable: true),
                    CONSUMPTION_QTY = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MAM_DISPATCH_DET",
                columns: table => new
                {
                    DISPATCH_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DISPATCH_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    DISPATCH_FG = table.Column<int>(type: "int", nullable: true),
                    DISPATCH_QTY = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DISPATCH_TYPE = table.Column<string>(type: "char(1)", nullable: true),
                    DISPATCH_AREDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    DISPATCH_INVOICENO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DISPATCH_ADVNO = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MAM_FG_ALLOCATION",
                columns: table => new
                {
                    SNO = table.Column<long>(type: "bigint", nullable: true),
                    FG_CODE = table.Column<long>(type: "bigint", nullable: true),
                    FLAG = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MAM_PRODUCT_ALLOCATION",
                columns: table => new
                {
                    SNO = table.Column<long>(type: "bigint", nullable: true),
                    RM_CODE = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MAM_ALLOCATION_DET");

            migrationBuilder.DropTable(
                name: "MAM_ALLOCATION_PRODDET");

            migrationBuilder.DropTable(
                name: "MAM_ALLOCATIONFG");

            migrationBuilder.DropTable(
                name: "MAM_ARRIVAL_DET");

            migrationBuilder.DropTable(
                name: "MAM_CONSUMPTION_DET");

            migrationBuilder.DropTable(
                name: "MAM_DISPATCH_DET");

            migrationBuilder.DropTable(
                name: "MAM_FG_ALLOCATION");

            migrationBuilder.DropTable(
                name: "MAM_PRODUCT_ALLOCATION");
        }
    }
}
