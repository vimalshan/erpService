using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stationery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SP_DEPT_BUDGET",
                columns: table => new
                {
                    DB_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    DB_DEPT_ID = table.Column<long>(type: "bigint", nullable: false),
                    DB_FINYEAR_ID = table.Column<long>(type: "bigint", nullable: false),
                    DB_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    DB_BUDGETAMOUNT = table.Column<long>(type: "bigint", nullable: false),
                    DB_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    DB_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_DEPT_BUDGET", x => new { x.DB_LOCATION_ID, x.DB_DEPT_ID, x.DB_FINYEAR_ID });
                });

            migrationBuilder.CreateTable(
                name: "SP_ORDER_MAIN",
                columns: table => new
                {
                    OM_ORDERMAIN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OM_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    OM_VENDORID = table.Column<long>(type: "bigint", nullable: false),
                    OM_DELIVERYDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OM_ORDEREDDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OM_ORDEREDBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_ORDER_MAIN", x => x.OM_ORDERMAIN_ID);
                });

            migrationBuilder.CreateTable(
                name: "SP_REQUEST_MAIN",
                columns: table => new
                {
                    RM_REQUESTID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RM_REQUESTEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RM_REQUESTEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RM_LOCATIONID = table.Column<long>(type: "bigint", nullable: true),
                    RM_UNITCODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_REQUEST_MAIN", x => x.RM_REQUESTID);
                });

            migrationBuilder.CreateTable(
                name: "SP_UNIT_BUDGET",
                columns: table => new
                {
                    UB_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    UB_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    UB_FINYEAR_ID = table.Column<long>(type: "bigint", nullable: false),
                    UB_BUDGETAMOUNT = table.Column<long>(type: "bigint", nullable: false),
                    UB_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    UB_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_UNIT_BUDGET", x => new { x.UB_LOCATION_ID, x.UB_UNIT_CODE, x.UB_FINYEAR_ID });
                });

            migrationBuilder.CreateTable(
                name: "STATIONARY_MASTER",
                columns: table => new
                {
                    SM_STATIONARYID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SM_CATID = table.Column<long>(type: "bigint", nullable: false),
                    SM_LOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    SM_DESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SM_UOMID = table.Column<long>(type: "bigint", nullable: false),
                    SM_MAKE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SM_PRICE_PERUNIT = table.Column<long>(type: "bigint", nullable: true),
                    SM_REORDER_LEVEL = table.Column<long>(type: "bigint", nullable: true),
                    SM_VMID = table.Column<long>(type: "bigint", nullable: false),
                    SM_CLOSED = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    SM_OPENINGSTOCK = table.Column<long>(type: "bigint", nullable: false),
                    SM_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    SM_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATIONARY_MASTER", x => x.SM_STATIONARYID);
                });

            migrationBuilder.CreateTable(
                name: "SP_ORDER_SUB",
                columns: table => new
                {
                    OS_ORDERSUB_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OS_ORDERMAIN_ID = table.Column<long>(type: "bigint", nullable: false),
                    OS_REQUESTSUB_ID = table.Column<long>(type: "bigint", nullable: false),
                    OS_ORDERED_QTY = table.Column<long>(type: "bigint", nullable: false),
                    OS_RECEIVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OS_RECEIVED_BY = table.Column<long>(type: "bigint", nullable: false),
                    OS_ORDERPRICE = table.Column<long>(type: "bigint", nullable: false),
                    OS_ACTUALPRICE = table.Column<long>(type: "bigint", nullable: false),
                    OS_RECEIVEDDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OS_DELIVERYDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OS_RECEIPTENTRYBY = table.Column<long>(type: "bigint", nullable: true),
                    OS_RECEIPTENTRYON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_ORDER_SUB", x => x.OS_ORDERSUB_ID);
                    table.ForeignKey(
                        name: "FK_SP_ORDER_SUB_SP_ORDER_MAIN_OS_ORDERMAIN_ID",
                        column: x => x.OS_ORDERMAIN_ID,
                        principalTable: "SP_ORDER_MAIN",
                        principalColumn: "OM_ORDERMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SP_REQUEST_SUB",
                columns: table => new
                {
                    RS_REQUESTSUB_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RS_REQUESTID = table.Column<long>(type: "bigint", nullable: false),
                    RS_STATIONARYID = table.Column<long>(type: "bigint", nullable: false),
                    RS_DEPTID = table.Column<long>(type: "bigint", nullable: false),
                    RS_EXPECTED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RS_USER_SYSID = table.Column<long>(type: "bigint", nullable: true),
                    RS_REQUESTEDQTY = table.Column<long>(type: "bigint", nullable: false),
                    RS_INDENTEDQTY = table.Column<long>(type: "bigint", nullable: true),
                    RS_APPROVEDQTY = table.Column<long>(type: "bigint", nullable: true),
                    RS_APPROVER_SYSID = table.Column<long>(type: "bigint", nullable: true),
                    RS_APPROVER_RAMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RS_RECEIVED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RS_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    RS_APPROVED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RS_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    RS_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_REQUEST_SUB", x => x.RS_REQUESTSUB_ID);
                    table.ForeignKey(
                        name: "FK_SP_REQUEST_SUB_SP_REQUEST_MAIN_RS_REQUESTID",
                        column: x => x.RS_REQUESTID,
                        principalTable: "SP_REQUEST_MAIN",
                        principalColumn: "RM_REQUESTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SP_ORDER_SUB_OS_ORDERMAIN_ID",
                table: "SP_ORDER_SUB",
                column: "OS_ORDERMAIN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SP_REQUEST_SUB_RS_REQUESTID",
                table: "SP_REQUEST_SUB",
                column: "RS_REQUESTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SP_DEPT_BUDGET");

            migrationBuilder.DropTable(
                name: "SP_ORDER_SUB");

            migrationBuilder.DropTable(
                name: "SP_REQUEST_SUB");

            migrationBuilder.DropTable(
                name: "SP_UNIT_BUDGET");

            migrationBuilder.DropTable(
                name: "STATIONARY_MASTER");

            migrationBuilder.DropTable(
                name: "SP_ORDER_MAIN");

            migrationBuilder.DropTable(
                name: "SP_REQUEST_MAIN");
        }
    }
}
