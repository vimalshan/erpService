using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemberService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MEMBER_AUDIT_LOG",
                columns: table => new
                {
                    AUDIT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MEMBER_NO = table.Column<long>(type: "bigint", nullable: false),
                    AUDIT_ACTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AUDIT_TIMESTAMP = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    AUDIT_USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    AUDIT_OLD_VALUES = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    AUDIT_NEW_VALUES = table.Column<string>(type: "VARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEMBER_AUDIT_LOG", x => x.AUDIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "MEMBER_MASTER",
                columns: table => new
                {
                    MEMBER_NO = table.Column<long>(type: "bigint", nullable: false),
                    MEMBER_TRUST_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MEMBER_FPSTRUST_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MEMBER_OPF_NO = table.Column<int>(type: "int", nullable: false),
                    MEMBER_PENSION_NO = table.Column<int>(type: "int", nullable: false),
                    MEMBER_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    MEMBER_FATHERNAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    MEMBER_DOB = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MEMBER_ENR_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MEMBER_DOJ = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MEMBER_EMPLOYEE_TYPE = table.Column<string>(type: "CHAR(2)", nullable: false),
                    MEMBER_UNIT_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    MEMBER_EMP_NUM = table.Column<long>(type: "bigint", nullable: false),
                    MEMBER_EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    MEMBER_ENROLL_USER_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MEMBER_ENROLL_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    MEMBER_ENROLL_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    MEMBER_CLOSURE_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MEMBER_LEAVE_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    MEMBER_LEAVE_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MEMBER_STATUS = table.Column<string>(type: "CHAR(1)", nullable: false),
                    MEMBER_UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    MEMBER_UPDATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEMBER_MASTER", x => x.MEMBER_NO);
                });

            migrationBuilder.CreateTable(
                name: "NOMINEE_GAURDIAN",
                columns: table => new
                {
                    GN_TRUST_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    GN_NOMINEE_MEMBER_NO = table.Column<long>(type: "bigint", nullable: false),
                    GN_NOMINEE_SERIAL_NO = table.Column<long>(type: "bigint", nullable: false),
                    GAURDIAN_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    GN_ADDRESS_LINE1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GN_ADDRESS_LINE2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GN_ADDRESS_LINE3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GN_ADDRESS_LINE4 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GN_PHONE_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GN_EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GN_EFF_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    GN_CLS_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    GAURDIAN_RELATIONSHIP = table.Column<string>(type: "CHAR(3)", nullable: false),
                    GN_STATUS = table.Column<string>(type: "CHAR(1)", nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOMINEE_GAURDIAN", x => new { x.GN_TRUST_CODE, x.GN_NOMINEE_MEMBER_NO, x.GN_NOMINEE_SERIAL_NO });
                });

            migrationBuilder.CreateTable(
                name: "MEMBER_CONTACT",
                columns: table => new
                {
                    CONTACT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MEMBER_NO = table.Column<long>(type: "bigint", nullable: false),
                    CONTACT_TYPE = table.Column<string>(type: "CHAR(1)", nullable: false),
                    ADDRESS_LINE_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ADDRESS_LINE_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ADDRESS_LINE_3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STATE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PIN_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    COUNTRY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PHONE_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EFF_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    CLS_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEMBER_CONTACT", x => x.CONTACT_ID);
                    table.ForeignKey(
                        name: "FK_MEMBER_CONTACT_MEMBER_MASTER_MEMBER_NO",
                        column: x => x.MEMBER_NO,
                        principalTable: "MEMBER_MASTER",
                        principalColumn: "MEMBER_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MEMBER_NOMINEE",
                columns: table => new
                {
                    NOMINEE_MEMBER_NO = table.Column<long>(type: "BIGINT", nullable: false),
                    NOMINEE_SERIAL_NO = table.Column<int>(type: "int", nullable: false),
                    NOMINEE_FUND_TYPE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    NOMINEE_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    NOMINEE_RELATIONSHIP_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    NOMINEE_PERCENTAGE = table.Column<long>(type: "bigint", nullable: false),
                    NOMINEE_DOB = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    NOMINEE_ADDRESS_LINE_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NOMINEE_ADDRESS_LINE_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NOMINEE_ADDRESS_LINE_3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NOMINEE_PHONE_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NOMINEE_EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NOMINEE_EFF_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    NOMINEE_CLS_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    NOMINEE_MINOR_FLAG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NOMINEE_TRUST_CODE = table.Column<string>(type: "CHAR(3)", nullable: false),
                    NOMINEE_STATUS = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEMBER_NOMINEE", x => new { x.NOMINEE_MEMBER_NO, x.NOMINEE_SERIAL_NO, x.NOMINEE_FUND_TYPE });
                    table.ForeignKey(
                        name: "FK_MEMBER_NOMINEE_MEMBER_MASTER_NOMINEE_MEMBER_NO",
                        column: x => x.NOMINEE_MEMBER_NO,
                        principalTable: "MEMBER_MASTER",
                        principalColumn: "MEMBER_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MEMBER_PAYROLL",
                columns: table => new
                {
                    PAYROLL_MEMBER_NO = table.Column<long>(type: "bigint", nullable: false),
                    PAYROLL_UNT_COD = table.Column<string>(type: "CHAR(3)", nullable: false),
                    PAYROLL_EMP_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PAYROLL_EFF_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    PAYROLL_CLS_DATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    PAYROLL_STATUS = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEMBER_PAYROLL", x => new { x.PAYROLL_MEMBER_NO, x.PAYROLL_UNT_COD });
                    table.ForeignKey(
                        name: "FK_MEMBER_PAYROLL_MEMBER_MASTER_PAYROLL_MEMBER_NO",
                        column: x => x.PAYROLL_MEMBER_NO,
                        principalTable: "MEMBER_MASTER",
                        principalColumn: "MEMBER_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_MEMBER_AUDIT_LOG_MEMBER",
                table: "MEMBER_AUDIT_LOG",
                columns: new[] { "MEMBER_NO", "AUDIT_TIMESTAMP" });

            migrationBuilder.CreateIndex(
                name: "IDX_MEMBER_CONTACT_MEMBER",
                table: "MEMBER_CONTACT",
                columns: new[] { "MEMBER_NO", "CONTACT_TYPE" });

            migrationBuilder.CreateIndex(
                name: "IDX_MEMBER_MASTER_EMP_SYSID",
                table: "MEMBER_MASTER",
                column: "MEMBER_EMP_SYSID");

            migrationBuilder.CreateIndex(
                name: "IDX_MEMBER_MASTER_TRUST_STATUS",
                table: "MEMBER_MASTER",
                columns: new[] { "MEMBER_TRUST_CODE", "MEMBER_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IDX_MEMBER_NOMINEE_MEMBER",
                table: "MEMBER_NOMINEE",
                columns: new[] { "NOMINEE_MEMBER_NO", "NOMINEE_EFF_DATE", "NOMINEE_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IDX_MEMBER_PAYROLL_STATUS",
                table: "MEMBER_PAYROLL",
                columns: new[] { "PAYROLL_MEMBER_NO", "PAYROLL_STATUS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MEMBER_AUDIT_LOG");

            migrationBuilder.DropTable(
                name: "MEMBER_CONTACT");

            migrationBuilder.DropTable(
                name: "MEMBER_NOMINEE");

            migrationBuilder.DropTable(
                name: "MEMBER_PAYROLL");

            migrationBuilder.DropTable(
                name: "NOMINEE_GAURDIAN");

            migrationBuilder.DropTable(
                name: "MEMBER_MASTER");
        }
    }
}
