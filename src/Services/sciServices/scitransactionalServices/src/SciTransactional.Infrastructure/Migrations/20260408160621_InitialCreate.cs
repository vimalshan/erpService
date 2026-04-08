using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SciTransactional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACTUAL_ORDER_MAP",
                columns: table => new
                {
                    ACTUAL_ORDER_MAP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIED_ORDER_DETAIL_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ACTUAL_LINE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MAPPING_QUANTITY = table.Column<int>(type: "int", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACTUAL_ORDER_MAP", x => x.ACTUAL_ORDER_MAP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ADVLIC_ENTITLEMENT",
                columns: table => new
                {
                    ADVLIC_ID = table.Column<long>(type: "bigint", nullable: false),
                    ADVLIC_ENTITLERM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADVLIC_ENTITLEMENT", x => new { x.ADVLIC_ID, x.ADVLIC_ENTITLERM });
                });

            migrationBuilder.CreateTable(
                name: "ADVLIC_MASTER",
                columns: table => new
                {
                    ADVLIC_ID = table.Column<long>(type: "bigint", nullable: false),
                    ADVLIC_NO = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ADVLIC_FG = table.Column<int>(type: "int", nullable: true),
                    ADVLIC_EOAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ADVLIC_EXPAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADVLIC_MASTER", x => x.ADVLIC_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUTO_MAIL_STATUS",
                columns: table => new
                {
                    AUTO_MAIL_STATUS_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAIL_TYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MAIL_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    MAIL_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    MAIL_REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTO_MAIL_STATUS", x => x.AUTO_MAIL_STATUS_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUTO_MAILID",
                columns: table => new
                {
                    AUTO_MAILID_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    MAILID = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    STARTDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ENDDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    MAIL_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTO_MAILID", x => x.AUTO_MAILID_ID);
                });

            migrationBuilder.CreateTable(
                name: "NORMS_MAIN",
                columns: table => new
                {
                    NORM_NO = table.Column<long>(type: "bigint", nullable: false),
                    NORM_EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    NORM_CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NORMS_MAIN", x => x.NORM_NO);
                });

            migrationBuilder.CreateTable(
                name: "NORMS_MASTER",
                columns: table => new
                {
                    NORM_ID = table.Column<long>(type: "bigint", nullable: false),
                    NORM_INPUT_CODE = table.Column<int>(type: "int", nullable: true),
                    NORM_OUTPUT_CODE = table.Column<int>(type: "int", nullable: true),
                    NORM_RATE = table.Column<int>(type: "int", nullable: true),
                    NORM_NO = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NORMS_MASTER", x => x.NORM_ID);
                });

            migrationBuilder.CreateTable(
                name: "SPARSH_NAVIGATION",
                columns: table => new
                {
                    SN_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SN_USR_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SN_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SN_RAN_NUM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SN_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    SN_SCI_ID = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SN_STS_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPARSH_NAVIGATION", x => x.SN_REQ_NUM);
                });

            migrationBuilder.CreateTable(
                name: "VEHICLE_DIRECT_ENTRY",
                columns: table => new
                {
                    VEHICLE_DIRECT_ENTRY_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DIR_TRK_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    DIR_STG_OLD = table.Column<long>(type: "bigint", nullable: true),
                    DIR_STG_NEW = table.Column<long>(type: "bigint", nullable: true),
                    DIR_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DIR_NO_OF_STG = table.Column<long>(type: "bigint", nullable: true),
                    DIR_ENTERED_BY = table.Column<long>(type: "bigint", nullable: true),
                    DIR_ENTERED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICLE_DIRECT_ENTRY", x => x.VEHICLE_DIRECT_ENTRY_ID);
                });

            migrationBuilder.InsertData(
                table: "ACTUAL_ORDER_MAP",
                columns: new[] { "ACTUAL_ORDER_MAP_ID", "ACTUAL_LINE_ID", "MAPPING_QUANTITY", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE", "TIED_ORDER_DETAIL_ID" },
                values: new object[,]
                {
                    { 1, 5001m, 500, 1, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1001m },
                    { 2, 5002m, 300, 1, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), 1002m },
                    { 3, 5003m, 750, 2, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 1003m }
                });

            migrationBuilder.InsertData(
                table: "ADVLIC_ENTITLEMENT",
                columns: new[] { "ADVLIC_ENTITLERM", "ADVLIC_ID" },
                values: new object[,]
                {
                    { 100, 1L },
                    { 200, 1L },
                    { 100, 2L },
                    { 300, 2L },
                    { 150, 3L }
                });

            migrationBuilder.InsertData(
                table: "ADVLIC_MASTER",
                columns: new[] { "ADVLIC_ID", "ADVLIC_EXPAMT", "ADVLIC_EOAMT", "ADVLIC_FG", "ADVLIC_NO" },
                values: new object[,]
                {
                    { 1L, 250000m, 500000m, 10, "ADVL-2026-001" },
                    { 2L, 750000m, 1000000m, 20, "ADVL-2026-002" },
                    { 3L, null, 300000m, 30, "ADVL-2026-003" }
                });

            migrationBuilder.InsertData(
                table: "AUTO_MAILID",
                columns: new[] { "AUTO_MAILID_ID", "ENDDATE", "ID_TYPE", "MAILID", "MAIL_TYPE", "STARTDATE" },
                values: new object[,]
                {
                    { 1, null, "DISPATCH", "dispatch@sci.com", "TO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, "DISPATCH", "mgr@sci.com", "CC", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "ALERT", "alerts@sci.com", "TO", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "AUTO_MAIL_STATUS",
                columns: new[] { "AUTO_MAIL_STATUS_ID", "MAIL_DATE", "MAIL_REMARKS", "MAIL_STATUS", "MAIL_TYPE" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Sent successfully", "S", "DAILY_REPORT" },
                    { 2, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "S", "WEEKLY_SUMMARY" },
                    { 3, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "SMTP server unreachable", "F", "ALERT_DISPATCH" }
                });

            migrationBuilder.InsertData(
                table: "NORMS_MAIN",
                columns: new[] { "NORM_NO", "NORM_CLS_DATE", "NORM_EFF_DATE" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2L, new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3L, null, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "NORMS_MASTER",
                columns: new[] { "NORM_ID", "NORM_INPUT_CODE", "NORM_NO", "NORM_OUTPUT_CODE", "NORM_RATE" },
                values: new object[,]
                {
                    { 101L, 1001, 1L, 2001, 100 },
                    { 102L, 1002, 1L, 2002, 150 },
                    { 201L, 1003, 2L, 2003, 200 },
                    { 301L, 1004, 3L, 2004, 175 }
                });

            migrationBuilder.InsertData(
                table: "SPARSH_NAVIGATION",
                columns: new[] { "SN_REQ_NUM", "SN_RAN_NUM", "SN_SCI_ID", "SN_STS_FLG", "SN_UPD_DAT", "SN_USR_ID", "SN_USR_NUM" },
                values: new object[,]
                {
                    { 1L, "RND001", "Y", "A", new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "ADMIN", 1L },
                    { 2L, "RND002", "N", "P", new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "USER01", 2L },
                    { 3L, "RND003", "Y", "C", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "ADMIN", 1L }
                });

            migrationBuilder.InsertData(
                table: "VEHICLE_DIRECT_ENTRY",
                columns: new[] { "VEHICLE_DIRECT_ENTRY_ID", "DIR_ENTERED_BY", "DIR_ENTERED_ON", "DIR_STG_NEW", "DIR_NO_OF_STG", "DIR_STG_OLD", "DIR_STATUS", "DIR_TRK_NUM" },
                values: new object[,]
                {
                    { 1, 1L, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3L, 2L, 1L, "A", 1001m },
                    { 2, 1L, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), 5L, 3L, 2L, "A", 1002m },
                    { 3, 2L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), 6L, 5L, 1L, "C", 1003m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACTUAL_ORDER_MAP");

            migrationBuilder.DropTable(
                name: "ADVLIC_ENTITLEMENT");

            migrationBuilder.DropTable(
                name: "ADVLIC_MASTER");

            migrationBuilder.DropTable(
                name: "AUTO_MAIL_STATUS");

            migrationBuilder.DropTable(
                name: "AUTO_MAILID");

            migrationBuilder.DropTable(
                name: "NORMS_MAIN");

            migrationBuilder.DropTable(
                name: "NORMS_MASTER");

            migrationBuilder.DropTable(
                name: "SPARSH_NAVIGATION");

            migrationBuilder.DropTable(
                name: "VEHICLE_DIRECT_ENTRY");
        }
    }
}
