using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CALENDAR_MASTER",
                columns: table => new
                {
                    CALENDAR_ID = table.Column<int>(type: "int", nullable: false),
                    CALENDAR_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CALENDAR_UNITID = table.Column<int>(type: "int", nullable: false),
                    CALENDAR_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CALENDAR_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CALENDAR_STATUS = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CALENDAR_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CALENDAR_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALENDAR_MASTER", x => x.CALENDAR_ID);
                });

            migrationBuilder.CreateTable(
                name: "HOLIDAY_MASTER",
                columns: table => new
                {
                    HOLIDAY_ID = table.Column<int>(type: "int", nullable: false),
                    HOLIDAY_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HOLIDAY_DESCRIPTION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HOLIDAY_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HOLIDAY_UNIT = table.Column<int>(type: "int", nullable: true),
                    HOLIDAY_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    HOLIDAY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HOLIDAY_MASTER", x => x.HOLIDAY_ID);
                });

            migrationBuilder.CreateTable(
                name: "PATTERN_MASTER",
                columns: table => new
                {
                    PATTERN_ID = table.Column<int>(type: "int", nullable: false),
                    PATTERN_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PATTERN_DESCRIPTION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PATTERN_CYCLEID = table.Column<int>(type: "int", nullable: false),
                    PATTERN_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PATTERN_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATTERN_MASTER", x => x.PATTERN_ID);
                });

            migrationBuilder.CreateTable(
                name: "SHIFT_MASTER",
                columns: table => new
                {
                    SHIFT_ID = table.Column<int>(type: "int", nullable: false),
                    SHIFT_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SHIFT_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SHIFT_INTIME = table.Column<TimeOnly>(type: "time", nullable: false),
                    SHIFT_OUTTIME = table.Column<TimeOnly>(type: "time", nullable: false),
                    SHIFT_DURATION = table.Column<decimal>(type: "DECIMAL(5,2)", nullable: false),
                    SHIFT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SHIFT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIFT_MASTER", x => x.SHIFT_ID);
                });

            migrationBuilder.CreateTable(
                name: "CALENDAR_GRACERANGE",
                columns: table => new
                {
                    CALGRACE_ID = table.Column<int>(type: "int", nullable: false),
                    CALGRACE_CALENID = table.Column<int>(type: "int", nullable: false),
                    CALGRACE_GRACEID = table.Column<int>(type: "int", nullable: false),
                    CALGRACE_GRACETIME = table.Column<int>(type: "int", nullable: false),
                    CALGRACE_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CALGRACE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALENDAR_GRACERANGE", x => x.CALGRACE_ID);
                    table.ForeignKey(
                        name: "FK_CALENDAR_GRACERANGE_CALENDAR_MASTER_CALGRACE_CALENID",
                        column: x => x.CALGRACE_CALENID,
                        principalTable: "CALENDAR_MASTER",
                        principalColumn: "CALENDAR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CALENDAR_ROUNDRANGE",
                columns: table => new
                {
                    CALROUND_ID = table.Column<int>(type: "int", nullable: false),
                    CALROUND_CALENID = table.Column<int>(type: "int", nullable: false),
                    CALROUND_ROUNDNO = table.Column<int>(type: "int", nullable: false),
                    CALROUND_ROUNDFROM = table.Column<int>(type: "int", nullable: false),
                    CALROUND_ROUNDTO = table.Column<int>(type: "int", nullable: false),
                    CALROUND_WORKING = table.Column<int>(type: "int", nullable: false),
                    CALROUND_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CALROUND_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALENDAR_ROUNDRANGE", x => x.CALROUND_ID);
                    table.ForeignKey(
                        name: "FK_CALENDAR_ROUNDRANGE_CALENDAR_MASTER_CALROUND_CALENID",
                        column: x => x.CALROUND_CALENID,
                        principalTable: "CALENDAR_MASTER",
                        principalColumn: "CALENDAR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CALENDAR_UNITMAP",
                columns: table => new
                {
                    CALUNIT_ID = table.Column<int>(type: "int", nullable: false),
                    CALUNIT_CALENID = table.Column<int>(type: "int", nullable: false),
                    CALUNIT_UNITID = table.Column<int>(type: "int", nullable: false),
                    CALUNIT_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CALUNIT_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CALUNIT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CALUNIT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALENDAR_UNITMAP", x => x.CALUNIT_ID);
                    table.ForeignKey(
                        name: "FK_CALENDAR_UNITMAP_CALENDAR_MASTER_CALUNIT_CALENID",
                        column: x => x.CALUNIT_CALENID,
                        principalTable: "CALENDAR_MASTER",
                        principalColumn: "CALENDAR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PATTERN_DETAIL",
                columns: table => new
                {
                    PATDET_ID = table.Column<int>(type: "int", nullable: false),
                    PATDET_PATTERNID = table.Column<int>(type: "int", nullable: false),
                    PATDET_DAYNO = table.Column<int>(type: "int", nullable: false),
                    PATDET_SHIFTID = table.Column<int>(type: "int", nullable: false),
                    PATDET_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PATDET_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATTERN_DETAIL", x => x.PATDET_ID);
                    table.ForeignKey(
                        name: "FK_PATTERN_DETAIL_PATTERN_MASTER_PATDET_PATTERNID",
                        column: x => x.PATDET_PATTERNID,
                        principalTable: "PATTERN_MASTER",
                        principalColumn: "PATTERN_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PATTERN_DETAIL_SHIFT_MASTER_PATDET_SHIFTID",
                        column: x => x.PATDET_SHIFTID,
                        principalTable: "SHIFT_MASTER",
                        principalColumn: "SHIFT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SHIFT_EXCEPTION",
                columns: table => new
                {
                    SHIFTEXC_ID = table.Column<int>(type: "int", nullable: false),
                    SHIFTEXC_SHIFTID = table.Column<int>(type: "int", nullable: false),
                    SHIFTEXC_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SHIFTEXC_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SHIFTEXC_NEWSHIFTID = table.Column<int>(type: "int", nullable: false),
                    SHIFTEXC_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SHIFTEXC_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIFT_EXCEPTION", x => x.SHIFTEXC_ID);
                    table.ForeignKey(
                        name: "FK_SHIFT_EXCEPTION_SHIFT_MASTER_SHIFTEXC_NEWSHIFTID",
                        column: x => x.SHIFTEXC_NEWSHIFTID,
                        principalTable: "SHIFT_MASTER",
                        principalColumn: "SHIFT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SHIFT_EXCEPTION_SHIFT_MASTER_SHIFTEXC_SHIFTID",
                        column: x => x.SHIFTEXC_SHIFTID,
                        principalTable: "SHIFT_MASTER",
                        principalColumn: "SHIFT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SHIFT_TIMEMASTER",
                columns: table => new
                {
                    SHIFTTIME_ID = table.Column<int>(type: "int", nullable: false),
                    SHIFTTIME_SHIFTID = table.Column<int>(type: "int", nullable: false),
                    SHIFTTIME_INTIME = table.Column<TimeOnly>(type: "time", nullable: false),
                    SHIFTTIME_OUTTIME = table.Column<TimeOnly>(type: "time", nullable: false),
                    SHIFTTIME_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SHIFTTIME_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SHIFT_TIMEMASTER", x => x.SHIFTTIME_ID);
                    table.ForeignKey(
                        name: "FK_SHIFT_TIMEMASTER_SHIFT_MASTER_SHIFTTIME_SHIFTID",
                        column: x => x.SHIFTTIME_SHIFTID,
                        principalTable: "SHIFT_MASTER",
                        principalColumn: "SHIFT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CALENDAR_GRACERANGE_CALGRACE_CALENID",
                table: "CALENDAR_GRACERANGE",
                column: "CALGRACE_CALENID");

            migrationBuilder.CreateIndex(
                name: "IX_CALENDAR_MASTER_STATUS",
                table: "CALENDAR_MASTER",
                column: "CALENDAR_STATUS");

            migrationBuilder.CreateIndex(
                name: "UQ_CALENDAR_NAME",
                table: "CALENDAR_MASTER",
                column: "CALENDAR_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CALENDAR_ROUNDRANGE_CALROUND_CALENID",
                table: "CALENDAR_ROUNDRANGE",
                column: "CALROUND_CALENID");

            migrationBuilder.CreateIndex(
                name: "IX_CALENDAR_UNITMAP_CALUNIT_CALENID",
                table: "CALENDAR_UNITMAP",
                column: "CALUNIT_CALENID");

            migrationBuilder.CreateIndex(
                name: "IX_HOLIDAY_MASTER_DATE",
                table: "HOLIDAY_MASTER",
                column: "HOLIDAY_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_PATTERN_DETAIL_PATDET_PATTERNID",
                table: "PATTERN_DETAIL",
                column: "PATDET_PATTERNID");

            migrationBuilder.CreateIndex(
                name: "IX_PATTERN_DETAIL_PATDET_SHIFTID",
                table: "PATTERN_DETAIL",
                column: "PATDET_SHIFTID");

            migrationBuilder.CreateIndex(
                name: "IX_PATTERN_MASTER_NAME",
                table: "PATTERN_MASTER",
                column: "PATTERN_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SHIFT_EXCEPTION_SHIFTEXC_NEWSHIFTID",
                table: "SHIFT_EXCEPTION",
                column: "SHIFTEXC_NEWSHIFTID");

            migrationBuilder.CreateIndex(
                name: "IX_SHIFTEXC_SHIFTID",
                table: "SHIFT_EXCEPTION",
                column: "SHIFTEXC_SHIFTID");

            migrationBuilder.CreateIndex(
                name: "IX_SHIFT_MASTER_CODE",
                table: "SHIFT_MASTER",
                column: "SHIFT_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SHIFT_TIMEMASTER_SHIFTTIME_SHIFTID",
                table: "SHIFT_TIMEMASTER",
                column: "SHIFTTIME_SHIFTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CALENDAR_GRACERANGE");

            migrationBuilder.DropTable(
                name: "CALENDAR_ROUNDRANGE");

            migrationBuilder.DropTable(
                name: "CALENDAR_UNITMAP");

            migrationBuilder.DropTable(
                name: "HOLIDAY_MASTER");

            migrationBuilder.DropTable(
                name: "PATTERN_DETAIL");

            migrationBuilder.DropTable(
                name: "SHIFT_EXCEPTION");

            migrationBuilder.DropTable(
                name: "SHIFT_TIMEMASTER");

            migrationBuilder.DropTable(
                name: "CALENDAR_MASTER");

            migrationBuilder.DropTable(
                name: "PATTERN_MASTER");

            migrationBuilder.DropTable(
                name: "SHIFT_MASTER");
        }
    }
}
