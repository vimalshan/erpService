using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceService.Infrastructure.Persistence.Migrations
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
                    BATCH_ID = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_MONTHFROM = table.Column<int>(type: "int", nullable: false),
                    BATCH_MONTHTO = table.Column<int>(type: "int", nullable: false),
                    BATCH_YEARFROM = table.Column<int>(type: "int", nullable: false),
                    BATCH_YEAREND = table.Column<int>(type: "int", nullable: false),
                    BATCH_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BATCH_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_BATCH", x => x.BATCH_ID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_GRACEADJUST",
                columns: table => new
                {
                    GRACE_ID = table.Column<long>(type: "bigint", nullable: false),
                    GRACE_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    GRACE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GRACE_MINUTES = table.Column<int>(type: "int", nullable: false),
                    GRACE_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GRACE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_GRACEADJUST", x => x.GRACE_ID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_LEAVEADJUST",
                columns: table => new
                {
                    LEAVEADJ_ID = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEADJ_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEADJ_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LEAVEADJ_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LEAVEADJ_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEADJ_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_LEAVEADJUST", x => x.LEAVEADJ_ID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_NIGHT",
                columns: table => new
                {
                    NIGHT_ID = table.Column<long>(type: "bigint", nullable: false),
                    NIGHT_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    NIGHT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NIGHT_NIGHTTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    NIGHT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    NIGHT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_NIGHT", x => x.NIGHT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_OVERTIME",
                columns: table => new
                {
                    OT_ID = table.Column<long>(type: "bigint", nullable: false),
                    OT_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    OT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OT_HOURS = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OT_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OT_APPROVED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    OT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_OVERTIME", x => x.OT_ID);
                });

            migrationBuilder.CreateTable(
                name: "SWIPE_RAWPUNCH",
                columns: table => new
                {
                    SWIPE_ID = table.Column<long>(type: "bigint", nullable: false),
                    SWIPE_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    SWIPE_PUNCHTIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SWIPE_GATENO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SWIPE_PUNCHSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SWIPE_PULLSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SWIPE_VERIFIED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SWIPE_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SWIPE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SWIPE_RAWPUNCH", x => x.SWIPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "SWIPE_RAWPUNCH_LOG",
                columns: table => new
                {
                    SWIPE_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    SWIPE_PUNCHTIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SWIPE_GATENO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SWIPE_PUNCHSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SWIPE_PULLSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    LOG_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOG_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SWIPE_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_LOPMAIN",
                columns: table => new
                {
                    LOP_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    LOP_BATCHID = table.Column<long>(type: "bigint", nullable: false),
                    LOP_DAYS = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LOP_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_LOPMAIN", x => x.LOP_ID);
                    table.ForeignKey(
                        name: "FK_LOP_BATCHID",
                        column: x => x.LOP_BATCHID,
                        principalTable: "ATTENDANCE_BATCH",
                        principalColumn: "BATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ATTENDANCE_SUMMARY",
                columns: table => new
                {
                    SUMMARY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUMMARY_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    SUMMARY_BATCHID = table.Column<long>(type: "bigint", nullable: false),
                    SUMMARY_ATTTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SUMMARY_DAYS = table.Column<int>(type: "int", nullable: false),
                    SUMMARY_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SUMMARY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTENDANCE_SUMMARY", x => x.SUMMARY_ID);
                    table.ForeignKey(
                        name: "FK_SUMMARY_BATCHID",
                        column: x => x.SUMMARY_BATCHID,
                        principalTable: "ATTENDANCE_BATCH",
                        principalColumn: "BATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_BATCH_STATUS",
                table: "ATTENDANCE_BATCH",
                column: "BATCH_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_LOPMAIN_LOP_BATCHID",
                table: "ATTENDANCE_LOPMAIN",
                column: "LOP_BATCHID");

            migrationBuilder.CreateIndex(
                name: "IX_LOP_EMPSYSID",
                table: "ATTENDANCE_LOPMAIN",
                column: "LOP_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_OT_EMPSYSID",
                table: "ATTENDANCE_OVERTIME",
                column: "OT_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_ATTENDANCE_SUMMARY_SUMMARY_BATCHID",
                table: "ATTENDANCE_SUMMARY",
                column: "SUMMARY_BATCHID");

            migrationBuilder.CreateIndex(
                name: "IX_SWIPE_EMPSYSID",
                table: "SWIPE_RAWPUNCH",
                column: "SWIPE_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_SWIPE_PUNCHTIME",
                table: "SWIPE_RAWPUNCH",
                column: "SWIPE_PUNCHTIME");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ATTENDANCE_GRACEADJUST");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_LEAVEADJUST");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_LOPMAIN");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_NIGHT");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_OVERTIME");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_SUMMARY");

            migrationBuilder.DropTable(
                name: "SWIPE_RAWPUNCH");

            migrationBuilder.DropTable(
                name: "SWIPE_RAWPUNCH_LOG");

            migrationBuilder.DropTable(
                name: "ATTENDANCE_BATCH");
        }
    }
}
