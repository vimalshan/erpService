using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusServices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BUS_MASTER",
                columns: table => new
                {
                    BUS_ID = table.Column<int>(type: "int", nullable: false),
                    BUS_REGNUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BUS_DESCRIPTION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BUS_CAPACITY = table.Column<int>(type: "int", nullable: false),
                    BUS_CAPACITY_RESERVED = table.Column<int>(type: "int", nullable: true),
                    BUS_OPERATINGFROM = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    BUS_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    BUS_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUS_MASTER", x => x.BUS_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_BUS",
                columns: table => new
                {
                    EMPBUS_ID = table.Column<long>(type: "bigint", nullable: false),
                    EMPBUS_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    EMPBUS_BUSID = table.Column<int>(type: "int", nullable: false),
                    EMPBUS_ROUTEID = table.Column<int>(type: "int", nullable: false),
                    EMPBUS_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    EMPBUS_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    EMPBUS_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EMPBUS_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_BUS", x => x.EMPBUS_ID);
                });

            migrationBuilder.CreateTable(
                name: "BUS_ARRIVALDET",
                columns: table => new
                {
                    ARRIVAL_ID = table.Column<long>(type: "bigint", nullable: false),
                    ARRIVAL_BUS_ID = table.Column<int>(type: "int", nullable: false),
                    ARRIVAL_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ARRIVAL_TIME = table.Column<TimeOnly>(type: "time", nullable: false),
                    ARRIVAL_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ARRIVAL_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ARRIVAL_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ARRIVAL_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUS_ARRIVALDET", x => x.ARRIVAL_ID);
                    table.ForeignKey(
                        name: "FK_BUS_ARRIVALDET_BUS_MASTER_ARRIVAL_BUS_ID",
                        column: x => x.ARRIVAL_BUS_ID,
                        principalTable: "BUS_MASTER",
                        principalColumn: "BUS_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BUSDEDUCTION_RATEMAST",
                columns: table => new
                {
                    DEDUCT_ID = table.Column<int>(type: "int", nullable: false),
                    DEDUCT_BUSID = table.Column<int>(type: "int", nullable: false),
                    DEDUCT_AMOUNT = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DEDUCT_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    DEDUCT_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    DEDUCT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    DEDUCT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUSDEDUCTION_RATEMAST", x => x.DEDUCT_ID);
                    table.ForeignKey(
                        name: "FK_BUSDEDUCTION_RATEMAST_BUS_MASTER_DEDUCT_BUSID",
                        column: x => x.DEDUCT_BUSID,
                        principalTable: "BUS_MASTER",
                        principalColumn: "BUS_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BUSROUTE_MASTER",
                columns: table => new
                {
                    ROUTE_ID = table.Column<int>(type: "int", nullable: false),
                    ROUTE_BUS_ID = table.Column<int>(type: "int", nullable: false),
                    ROUTE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ROUTE_DESCRIPTION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ROUTE_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ROUTE_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ROUTE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUSROUTE_MASTER", x => x.ROUTE_ID);
                    table.ForeignKey(
                        name: "FK_BUSROUTE_MASTER_BUS_MASTER_ROUTE_BUS_ID",
                        column: x => x.ROUTE_BUS_ID,
                        principalTable: "BUS_MASTER",
                        principalColumn: "BUS_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BUS_ARRIVALDET_ARRIVAL_BUS_ID",
                table: "BUS_ARRIVALDET",
                column: "ARRIVAL_BUS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BUS_MASTER_BUS_REGNUM",
                table: "BUS_MASTER",
                column: "BUS_REGNUM",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BUSDEDUCTION_RATEMAST_DEDUCT_BUSID",
                table: "BUSDEDUCTION_RATEMAST",
                column: "DEDUCT_BUSID");

            migrationBuilder.CreateIndex(
                name: "IX_BUSROUTE_MASTER_ROUTE_BUS_ID",
                table: "BUSROUTE_MASTER",
                column: "ROUTE_BUS_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BUS_ARRIVALDET");

            migrationBuilder.DropTable(
                name: "BUSDEDUCTION_RATEMAST");

            migrationBuilder.DropTable(
                name: "BUSROUTE_MASTER");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_BUS");

            migrationBuilder.DropTable(
                name: "BUS_MASTER");
        }
    }
}
