using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMP_CALENDAR",
                columns: table => new
                {
                    EMPCAL_ID = table.Column<long>(type: "bigint", nullable: false),
                    EMPCAL_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    EMPCAL_CALENDARID = table.Column<int>(type: "int", nullable: false),
                    EMPCAL_SWIPEID = table.Column<long>(type: "bigint", nullable: true),
                    EMPCAL_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMPCAL_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EMPCAL_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EMPCAL_TRANSFER = table.Column<int>(type: "int", nullable: true),
                    EMPCAL_SETTLEMENTNO = table.Column<long>(type: "bigint", nullable: true),
                    EMPCAL_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EMPCAL_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_CALENDAR", x => x.EMPCAL_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMP_TIMEINFO",
                columns: table => new
                {
                    TIME_INFOID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_EMPATTFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TIME_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TIME_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_TIMEINFO", x => x.TIME_INFOID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_APPROVALMAIL",
                columns: table => new
                {
                    APPMAIL_ID = table.Column<int>(type: "int", nullable: false),
                    APPMAIL_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    APPMAIL_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    APPMAIL_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APPMAIL_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    APPMAIL_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_APPROVALMAIL", x => x.APPMAIL_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_APPROVER",
                columns: table => new
                {
                    APPROVER_ID = table.Column<int>(type: "int", nullable: false),
                    APPROVER_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    APPROVER_LEVEL = table.Column<int>(type: "int", nullable: false),
                    APPROVER_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    APPROVER_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APPROVER_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    APPROVER_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_APPROVER", x => x.APPROVER_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_PATTERN",
                columns: table => new
                {
                    EMPPATTERN_ID = table.Column<long>(type: "bigint", nullable: false),
                    EMPPATTERN_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    EMPPATTERN_MASTID = table.Column<int>(type: "int", nullable: false),
                    EMPPATTERN_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMPPATTERN_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EMPPATTERN_WEEKLYOFFDY = table.Column<int>(type: "int", nullable: false),
                    EMPPATTERN_SUBWEEKLYDY = table.Column<int>(type: "int", nullable: true),
                    EMPPATTERN_SUBFRQ = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    EMPPATTERN_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EMPPATTERN_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_PATTERN", x => x.EMPPATTERN_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_SHIFT",
                columns: table => new
                {
                    EMPSHIFT_ID = table.Column<long>(type: "bigint", nullable: false),
                    EMPSHIFT_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    EMPSHIFT_TIMEUNITID = table.Column<int>(type: "int", nullable: false),
                    EMPSHIFT_CODE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    EMPSHIFT_YEARMONTH = table.Column<int>(type: "int", nullable: false),
                    EMPSHIFT_DAY = table.Column<int>(type: "int", nullable: false),
                    EMPSHIFT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMPSHIFT_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EMPSHIFT_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_SHIFT", x => x.EMPSHIFT_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_SHIFTPATTERN",
                columns: table => new
                {
                    EMPSHIFT_ID = table.Column<long>(type: "bigint", nullable: false),
                    EMPSHIFT_TIMEUNITID = table.Column<long>(type: "bigint", nullable: true),
                    EMPSHIFT_EMPSYSID = table.Column<long>(type: "bigint", nullable: true),
                    EMPSHIFT_YEARMONTH = table.Column<int>(type: "int", nullable: true),
                    EMPSHIFT_ORGPATTERN = table.Column<string>(type: "nvarchar(31)", maxLength: 31, nullable: true),
                    EMPSHIFT_NEWPATTERN = table.Column<string>(type: "nvarchar(31)", maxLength: 31, nullable: true),
                    EMPSHIFT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EMPSHIFT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_SHIFTPATTERN", x => x.EMPSHIFT_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EMP_CALENDAR_EMPSYSID",
                table: "EMP_CALENDAR",
                column: "EMPCAL_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_EMP_TIMEINFO_EMPSYSID",
                table: "EMP_TIMEINFO",
                column: "TIME_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEE_APPROVER_EMPSYSID",
                table: "EMPLOYEE_APPROVER",
                column: "APPROVER_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEE_PATTERN_EMPSYSID",
                table: "EMPLOYEE_PATTERN",
                column: "EMPPATTERN_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_EMPLOYEE_SHIFT_EMPSYSID",
                table: "EMPLOYEE_SHIFT",
                column: "EMPSHIFT_EMPSYSID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMP_CALENDAR");

            migrationBuilder.DropTable(
                name: "EMP_TIMEINFO");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_APPROVALMAIL");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_APPROVER");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_PATTERN");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_SHIFT");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_SHIFTPATTERN");
        }
    }
}
