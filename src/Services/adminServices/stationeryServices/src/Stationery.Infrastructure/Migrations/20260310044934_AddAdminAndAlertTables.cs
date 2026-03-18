using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stationery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAndAlertTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SP_DEPT_APPROVER",
                columns: table => new
                {
                    DA_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    DA_DEPT_ID = table.Column<long>(type: "bigint", nullable: false),
                    DA_EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    DA_TYPE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    DA_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    DA_EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DA_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DA_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    DA_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_DEPT_APPROVER", x => new { x.DA_LOCATION_ID, x.DA_DEPT_ID, x.DA_EMP_SYSID, x.DA_TYPE });
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
                name: "SP_UNIT_APPROVER",
                columns: table => new
                {
                    UA_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    UA_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    UA_EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    UA_TYPE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    UA_EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UA_CLOSURE_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UA_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    UA_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SP_UNIT_APPROVER", x => new { x.UA_LOCATION_ID, x.UA_UNIT_CODE, x.UA_EMP_SYSID, x.UA_TYPE });
                });

            migrationBuilder.CreateTable(
                name: "STATIONERY_REORDER_ALERT",
                columns: table => new
                {
                    AlertID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationaryID = table.Column<long>(type: "bigint", nullable: false),
                    AlertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentStock = table.Column<long>(type: "bigint", nullable: false),
                    ReorderLevel = table.Column<long>(type: "bigint", nullable: false),
                    Resolved = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATIONERY_REORDER_ALERT", x => x.AlertID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SP_DEPT_APPROVER");

            migrationBuilder.DropTable(
                name: "SP_LOCATION_ADMIN");

            migrationBuilder.DropTable(
                name: "SP_UNIT_APPROVER");

            migrationBuilder.DropTable(
                name: "STATIONERY_REORDER_ALERT");
        }
    }
}
