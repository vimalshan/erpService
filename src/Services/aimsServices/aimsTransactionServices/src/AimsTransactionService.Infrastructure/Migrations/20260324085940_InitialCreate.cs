using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AimsTransactionService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ATTENDANCE_BATCH",
                columns: table => new
                {
                    ATB_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    ATB_MONTHSTART = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ATB_MONTHEND = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ATB_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ATB_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ATB_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_BATCH", x => x.ATB_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_LOPDET",
                columns: table => new
                {
                    ALD_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    ALD_LOPMAINID = table.Column<long>(type: "bigint", nullable: false),
                    ALD_LOPDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALD_LOPHOURS = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ALD_LOPREASON = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_LOPDET", x => x.ALD_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_LOPMAIN",
                columns: table => new
                {
                    ALM_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    ALM_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    ALM_MONTHSTART = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALM_MONTHEND = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALM_CALENDARDAYS = table.Column<int>(type: "int", nullable: false),
                    ALM_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ALM_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_LOPMAIN", x => x.ALM_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_OVERTIME",
                columns: table => new
                {
                    ATO_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    ATO_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    ATO_OTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ATO_OTHOURS = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ATO_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ATO_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ATO_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_OVERTIME", x => x.ATO_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_SUMMARY",
                columns: table => new
                {
                    ATS_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    ATS_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    ATS_MONTHSTART = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ATS_MONTHEND = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ATS_WORKINGDAYS = table.Column<int>(type: "int", nullable: false),
                    ATS_PRESENTDAYS = table.Column<int>(type: "int", nullable: false),
                    ATS_ABSENTDAYS = table.Column<int>(type: "int", nullable: false),
                    ATS_OTHOURS = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    ATS_LOPDAYS = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ATS_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ATS_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_SUMMARY", x => x.ATS_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "COMPOFF_ADJUST",
                columns: table => new
                {
                    COA_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    COA_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    COA_HOURSREQUESTED = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    COA_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    COA_REQUESTEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    COA_REQUESTEDBY = table.Column<long>(type: "bigint", nullable: false),
                    COA_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    COA_APPROVEDBY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPOFF_ADJUST", x => x.COA_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "LEAVE_CREDIT",
                columns: table => new
                {
                    LVC_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    LVC_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    LVC_LEAVEID = table.Column<int>(type: "int", nullable: false),
                    LVC_CREDITDAYS = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LVC_CREDITDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LVC_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LVC_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LVC_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_CREDIT", x => x.LVC_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "LEAVE_DETAILS",
                columns: table => new
                {
                    LVD_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    LVD_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    LVD_LEAVEID = table.Column<long>(type: "bigint", nullable: false),
                    LVD_FROMDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LVD_TODATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LVD_LEAVEDAYS = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LVD_REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LVD_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LVD_APPLIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LVD_APPLIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LVD_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    LVD_APPROVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    LVD_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_DETAILS", x => x.LVD_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "LEAVE_DETAILSAPR",
                columns: table => new
                {
                    LDA_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    LDA_LVDSYSID = table.Column<long>(type: "bigint", nullable: false),
                    LDA_APPROVEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LDA_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_DETAILSAPR", x => x.LDA_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "SWIPE_RAWPUNCH",
                columns: table => new
                {
                    SRP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    SRP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    SRP_GATENO = table.Column<int>(type: "int", nullable: false),
                    SRP_INOUTSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SRP_MACHINENO = table.Column<int>(type: "int", nullable: true),
                    SRP_REFERENCENO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SRP_PUNCHTIME = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SRP_PULLSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SRP_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SRP_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SWIPE_RAWPUNCH", x => x.SRP_SYSID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_BATCH_STATUS",
                table: "ATTENDANCE_BATCH",
                column: "ATB_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_LOPDET_LOPMAINID",
                table: "ATTENDANCE_LOPDET",
                column: "ALD_LOPMAINID");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_LOPMAIN_EMPSYSID",
                table: "ATTENDANCE_LOPMAIN",
                column: "ALM_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_OVERTIME_EMPSYSID",
                table: "ATTENDANCE_OVERTIME",
                column: "ATO_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_SUMMARY_EMPSYSID",
                table: "ATTENDANCE_SUMMARY",
                column: "ATS_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_COMPOFF_ADJUST_EMPSYSID",
                table: "COMPOFF_ADJUST",
                column: "COA_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_COMPOFF_ADJUST_STATUS",
                table: "COMPOFF_ADJUST",
                column: "COA_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_LEAVE_CREDIT_EMP_LEAVE",
                table: "LEAVE_CREDIT",
                columns: new[] { "LVC_EMPSYSID", "LVC_LEAVEID" });

            migrationBuilder.CreateIndex(
                name: "IX_LEAVE_DETAILS_EMPSYSID",
                table: "LEAVE_DETAILS",
                column: "LVD_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_LEAVE_DETAILS_STATUS",
                table: "LEAVE_DETAILS",
                column: "LVD_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_LEAVE_DETAILSAPR_LVDSYSID",
                table: "LEAVE_DETAILSAPR",
                column: "LDA_LVDSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_SWIPE_RAWPUNCH_EMPSYSID",
                table: "SWIPE_RAWPUNCH",
                column: "SRP_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_SWIPE_RAWPUNCH_PUNCHTIME",
                table: "SWIPE_RAWPUNCH",
                column: "SRP_PUNCHTIME");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ATTENDANCE_BATCH");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_LOPDET");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_LOPMAIN");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_OVERTIME");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_SUMMARY");

            migrationBuilder.DropTable(
                name: "COMPOFF_ADJUST");

            migrationBuilder.DropTable(
                name: "LEAVE_CREDIT");

            migrationBuilder.DropTable(
                name: "LEAVE_DETAILS");

            migrationBuilder.DropTable(
                name: "LEAVE_DETAILSAPR");

            migrationBuilder.DropTable(
                name: "SWIPE_RAWPUNCH");
        }
    }
}
