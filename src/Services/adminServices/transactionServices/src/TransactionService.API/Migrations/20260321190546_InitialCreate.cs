using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SP_CATEGORY_DEFAULT",
                columns: table => new
                {
                    CD_STATIONERYID = table.Column<long>(type: "bigint", nullable: false),
                    CD_CATEGORYID = table.Column<long>(type: "bigint", nullable: false),
                    CD_LOCATIONID = table.Column<long>(type: "bigint", nullable: false),
                    CD_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CD_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_CATEGORY_DEFAULT", x => new { x.CD_STATIONERYID, x.CD_CATEGORYID, x.CD_LOCATIONID });
                });

            migrationBuilder.CreateTable(
                name: "SP_DEPT_APPROVER",
                columns: table => new
                {
                    DA_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    DA_DEPT_ID = table.Column<long>(type: "bigint", nullable: false),
                    DA_EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    DA_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DA_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DA_EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DA_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DA_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    DA_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_DEPT_APPROVER", x => new { x.DA_LOCATION_ID, x.DA_DEPT_ID, x.DA_EMP_SYSID });
                });

            migrationBuilder.CreateTable(
                name: "SP_DEPT_BUDGET",
                columns: table => new
                {
                    DB_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    DB_DEPT_ID = table.Column<long>(type: "bigint", nullable: false),
                    DB_FINYEAR_ID = table.Column<long>(type: "bigint", nullable: false),
                    DB_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DB_BUDGETAMOUNT = table.Column<long>(type: "bigint", nullable: false),
                    DB_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    DB_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_DEPT_BUDGET", x => new { x.DB_LOCATION_ID, x.DB_DEPT_ID, x.DB_FINYEAR_ID });
                });

            migrationBuilder.CreateTable(
                name: "SP_LOCATION_ADMIN",
                columns: table => new
                {
                    LA_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    LA_EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    LA_EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LA_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LA_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    LA_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_LOCATION_ADMIN", x => new { x.LA_LOCATION_ID, x.LA_EMP_SYSID });
                });

            migrationBuilder.CreateTable(
                name: "SP_ORDER_MAIN",
                columns: table => new
                {
                    OM_ORDERMAIN_ID = table.Column<long>(type: "bigint", nullable: false),
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
                    RM_REQUESTID = table.Column<long>(type: "bigint", nullable: false),
                    RM_REQUESTEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RM_REQUESTEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RM_LOCATIONID = table.Column<long>(type: "bigint", nullable: true),
                    RM_UNITCODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_REQUEST_MAIN", x => x.RM_REQUESTID);
                });

            migrationBuilder.CreateTable(
                name: "SP_UNIT_APPROVER",
                columns: table => new
                {
                    UA_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    UA_EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    UA_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UA_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UA_EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UA_CLOSURE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UA_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    UA_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_UNIT_APPROVER", x => new { x.UA_LOCATION_ID, x.UA_EMP_SYSID });
                });

            migrationBuilder.CreateTable(
                name: "SP_UNIT_BUDGET",
                columns: table => new
                {
                    UB_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    UB_FINYEAR_ID = table.Column<long>(type: "bigint", nullable: false),
                    UB_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UB_BUDGETAMOUNT = table.Column<long>(type: "bigint", nullable: false),
                    UB_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    UB_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_UNIT_BUDGET", x => new { x.UB_LOCATION_ID, x.UB_FINYEAR_ID });
                });

            migrationBuilder.CreateTable(
                name: "SP_ORDER_SUB",
                columns: table => new
                {
                    OS_ORDERSUB_ID = table.Column<long>(type: "bigint", nullable: false),
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
                    RS_REQUESTSUB_ID = table.Column<long>(type: "bigint", nullable: false),
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
                    RS_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    RS_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RS_APPROVED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "SP_CATEGORY_DEFAULT");

            migrationBuilder.DropTable(
                name: "SP_DEPT_APPROVER");

            migrationBuilder.DropTable(
                name: "SP_DEPT_BUDGET");

            migrationBuilder.DropTable(
                name: "SP_LOCATION_ADMIN");

            migrationBuilder.DropTable(
                name: "SP_ORDER_SUB");

            migrationBuilder.DropTable(
                name: "SP_REQUEST_SUB");

            migrationBuilder.DropTable(
                name: "SP_UNIT_APPROVER");

            migrationBuilder.DropTable(
                name: "SP_UNIT_BUDGET");

            migrationBuilder.DropTable(
                name: "SP_ORDER_MAIN");

            migrationBuilder.DropTable(
                name: "SP_REQUEST_MAIN");
        }
    }
}
