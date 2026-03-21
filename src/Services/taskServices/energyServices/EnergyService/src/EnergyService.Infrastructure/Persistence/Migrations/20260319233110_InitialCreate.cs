using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EC_PROCESS",
                columns: table => new
                {
                    EC_PROCESS_ID = table.Column<int>(type: "int", nullable: false),
                    EC_PROCESS_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    EC_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    EC_CLOSE_FLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    LAST_MODIFIED_BY = table.Column<int>(type: "int", nullable: false),
                    LAST_MODIFIED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EC_PROCESS", x => x.EC_PROCESS_ID);
                });

            migrationBuilder.CreateTable(
                name: "EC_PROCESS_ACCESS",
                columns: table => new
                {
                    PA_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PA_PROCESS_ID = table.Column<int>(type: "int", nullable: false),
                    PA_EMP_SYSID = table.Column<int>(type: "int", nullable: false),
                    PA_START_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    PA_CLOSE_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PA_LAST_MODIFIEDBY = table.Column<int>(type: "int", nullable: false),
                    PA_LAST_MODIFIEDON = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EC_PROCESS_ACCESS", x => x.PA_ID);
                    table.ForeignKey(
                        name: "FK_EC_PROCESS_ACCESS_EC_PROCESS_PA_PROCESS_ID",
                        column: x => x.PA_PROCESS_ID,
                        principalTable: "EC_PROCESS",
                        principalColumn: "EC_PROCESS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EC_PROCESS_MAILID",
                columns: table => new
                {
                    PM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PM_PROCESS_ID = table.Column<int>(type: "int", nullable: false),
                    PM_MAIL_ID = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    PM_DELIVERY_TYPE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    PM_START_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    PM_CLOSE_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PM_LAST_MODIFIEDBY = table.Column<int>(type: "int", nullable: false),
                    PM_LAST_MODIFIEDON = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EC_PROCESS_MAILID", x => x.PM_ID);
                    table.ForeignKey(
                        name: "FK_EC_PROCESS_MAILID_EC_PROCESS_PM_PROCESS_ID",
                        column: x => x.PM_PROCESS_ID,
                        principalTable: "EC_PROCESS",
                        principalColumn: "EC_PROCESS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EC_READING",
                columns: table => new
                {
                    EB_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EB_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    EB_PROCESS_ID = table.Column<int>(type: "int", nullable: false),
                    EB_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    EB_TARGET = table.Column<long>(type: "bigint", nullable: true),
                    EB_READING = table.Column<long>(type: "bigint", nullable: true),
                    EB_RESET_READING = table.Column<long>(type: "bigint", nullable: true),
                    EB_ACTUAL_USAGE = table.Column<long>(type: "bigint", nullable: true),
                    EB_TODATE = table.Column<long>(type: "bigint", nullable: true),
                    EB_REMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LAST_MODIFIED_BY = table.Column<int>(type: "int", nullable: false),
                    LAST_MODIFIED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EC_READING", x => x.EB_ID);
                    table.ForeignKey(
                        name: "FK_EC_READING_EC_PROCESS_EB_PROCESS_ID",
                        column: x => x.EB_PROCESS_ID,
                        principalTable: "EC_PROCESS",
                        principalColumn: "EC_PROCESS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EC_PROCESS_ACCESS_PA_PROCESS_ID",
                table: "EC_PROCESS_ACCESS",
                column: "PA_PROCESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_EC_PROCESS_MAILID_PM_PROCESS_ID",
                table: "EC_PROCESS_MAILID",
                column: "PM_PROCESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_EC_READING_EB_PROCESS_ID",
                table: "EC_READING",
                column: "EB_PROCESS_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EC_PROCESS_ACCESS");

            migrationBuilder.DropTable(
                name: "EC_PROCESS_MAILID");

            migrationBuilder.DropTable(
                name: "EC_READING");

            migrationBuilder.DropTable(
                name: "EC_PROCESS");
        }
    }
}
