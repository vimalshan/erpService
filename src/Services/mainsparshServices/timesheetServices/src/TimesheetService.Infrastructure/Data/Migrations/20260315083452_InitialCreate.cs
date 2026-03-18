using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimesheetService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TSE_TIMESHEET",
                columns: table => new
                {
                    TIMESHEET_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    TIMESHEET_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    WORK_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    START_TIME = table.Column<TimeOnly>(type: "time", nullable: true),
                    END_TIME = table.Column<TimeOnly>(type: "time", nullable: true),
                    TOTAL_HOURS = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PROJECT_ID = table.Column<long>(type: "bigint", nullable: true),
                    TASK_ID = table.Column<long>(type: "bigint", nullable: true),
                    WORK_DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RECORDED_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TIMESHEET_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "DRAFT"),
                    APPROVAL_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    APPROVED_BY = table.Column<long>(type: "bigint", nullable: true),
                    APPROVED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    REJECTION_REASON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSE_TIMESHEET", x => x.TIMESHEET_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TSE_TIMESHEET_APPROVAL",
                table: "TSE_TIMESHEET",
                column: "APPROVAL_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_TSE_TIMESHEET_EMP_SYSID",
                table: "TSE_TIMESHEET",
                column: "EMP_SYSID");

            migrationBuilder.CreateIndex(
                name: "IX_TSE_TIMESHEET_PROJECT",
                table: "TSE_TIMESHEET",
                column: "PROJECT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TSE_TIMESHEET_RECORDED_DATE",
                table: "TSE_TIMESHEET",
                column: "RECORDED_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_TSE_TIMESHEET_STATUS",
                table: "TSE_TIMESHEET",
                column: "TIMESHEET_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_TSE_TIMESHEET_TASK",
                table: "TSE_TIMESHEET",
                column: "TASK_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TSE_TIMESHEET_WORK_DATE",
                table: "TSE_TIMESHEET",
                column: "WORK_DATE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TSE_TIMESHEET");
        }
    }
}
