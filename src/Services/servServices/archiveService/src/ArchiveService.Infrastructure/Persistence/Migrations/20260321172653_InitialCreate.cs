using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchiveService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OLD_SERVICE_ORDER_HDR",
                columns: table => new
                {
                    SERNO_DELL = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    BRANCH = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SAP_LOGIN = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    POSTING_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SAP_ID = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    SLA = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PRODUCT_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SERVICE_TAG = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    RELATED_CASE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LOB = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CALL_STATUS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CURRENT_RC = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ENGINEER_ID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ENGINEER_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ENGMOB_NO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ORG_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CUSTOMER_NAME = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CONTACT_NO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ALT_CNTNO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ADDRESS = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DISPATCH_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CUSTETA_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PARTETA_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TECH_SUPNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PRB_DESC = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LONG_DESC = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    REASON_CODE = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ACTIVITY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ONSITE_DT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CMPLTD_DT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FLAG = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ENTERED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ENTERED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CHANGED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CHANGED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OLD_SERVICE_ORDER_HDR", x => x.SERNO_DELL);
                });

            migrationBuilder.CreateTable(
                name: "TOOL_KIT_DUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KIT_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    APP_PASSWORD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    INST_PASSWORD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IMEI_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ENGINEER_ID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FLAG = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ENTERED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ENTERED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CHANGED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CHANGED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOOL_KIT_DUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SERVICE_ORDER_DET_DUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SERNO_DELL = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PART_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QUANTITY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    UNIQUE_ID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PART_STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ENTERED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ENTERED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CHANGED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CHANGED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SERVICE_ORDER_DET_DUP", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SERVICE_ORDER_DET_DUP_OLD_SERVICE_ORDER_HDR_SERNO_DELL",
                        column: x => x.SERNO_DELL,
                        principalTable: "OLD_SERVICE_ORDER_HDR",
                        principalColumn: "SERNO_DELL");
                });

            migrationBuilder.CreateTable(
                name: "TOOLKIT_TRANSACTION_DUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TOOLKIT_ID = table.Column<long>(type: "bigint", nullable: true),
                    TOOLKIT_NAME_ID = table.Column<int>(type: "int", nullable: true),
                    ENGINEER_ID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ISSUER_ID = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    REMARKS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ADDITIONAL_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ENTERED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ENTERED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CHANGED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CHANGED_BY = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOOLKIT_TRANSACTION_DUP", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TOOLKIT_TRANSACTION_DUP_TOOL_KIT_DUP_TOOLKIT_ID",
                        column: x => x.TOOLKIT_ID,
                        principalTable: "TOOL_KIT_DUP",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_ORDER_HDR",
                table: "OLD_SERVICE_ORDER_HDR",
                column: "SAP_ID",
                unique: true,
                filter: "[SAP_ID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SERVICE_ORDER_DET_DUP_SERNO_DELL",
                table: "SERVICE_ORDER_DET_DUP",
                column: "SERNO_DELL");

            migrationBuilder.CreateIndex(
                name: "IX_TOOLKIT_TRANSACTION_DUP_TOOLKIT_ID",
                table: "TOOLKIT_TRANSACTION_DUP",
                column: "TOOLKIT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SERVICE_ORDER_DET_DUP");

            migrationBuilder.DropTable(
                name: "TOOLKIT_TRANSACTION_DUP");

            migrationBuilder.DropTable(
                name: "OLD_SERVICE_ORDER_HDR");

            migrationBuilder.DropTable(
                name: "TOOL_KIT_DUP");
        }
    }
}
