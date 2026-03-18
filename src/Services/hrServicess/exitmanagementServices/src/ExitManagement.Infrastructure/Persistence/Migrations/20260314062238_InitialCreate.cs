using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExitManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMPLOYEE_EXIT_INT",
                columns: table => new
                {
                    INT_EXITNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    INT_SLNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    INT_QUES_ID = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    INT_FEEDBACK = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    INT_UPDATED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    INT_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_EXIT_INT", x => x.INT_EXITNO);
                });

            migrationBuilder.CreateTable(
                name: "TT_EMPLOYEE_EXITRESPEX",
                columns: table => new
                {
                    TT_ID = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    TT_SYSID = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    TT_CHKMAPID = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    TT_PRI = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TT_SEC = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TT_FHEAD = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TT_EXIT_INTERVIEW",
                columns: table => new
                {
                    QUESTION_ID = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    QUESTION_DESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ORDER_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TT_EXIT_QUESTIONS",
                columns: table => new
                {
                    QUESTION_ID = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    QUESTION_DESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QUESTION_ORDER = table.Column<decimal>(type: "decimal(22,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TTBT_EXIT_TEV",
                columns: table => new
                {
                    EXIT_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EXIT_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EXIT_LET_GIVON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_EXP_RELDT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_RES_TYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    EXIT_RES_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EXIT_REMARKS = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EXIT_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_REL_GIVON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_INTCONDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_INTCONDBY = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EXIT_REVOKE_REASON = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EXIT_REVOKE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_SIGN_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXIT_REVOKE_RESIGNATION = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_PAYROLL_SETTLEMENT = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_STOPSAL_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_NEXTOFFICER = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXIT_SETTYPEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXIT_MAILDIS_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_MAILDEL_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_MAILFWD_SYSID = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EXIT_FORMALITYBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EXIT_FORMALITYON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_USERCONFSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_BYPASSFORMALITY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_APPSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_APPBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EXIT_APPON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_NOTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_MAILSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_CHKSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_LSNO = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EXIT_FSNO = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EXIT_FSBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EXIT_FSON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_NPP = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXIT_MAILTOUSER = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EXIT_CONDUCTDESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EXIT_JVBATCHID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXIT_JVPOSTEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXIT_JVPOSTEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIT_DESGONJOINING = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EXIT_REASONFORLEAVING = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EXIT_UPDATED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXIT_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTBT_EXIT_TEV", x => x.EXIT_NO);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMPLOYEE_EXIT_INT");

            migrationBuilder.DropTable(
                name: "TT_EMPLOYEE_EXITRESPEX");

            migrationBuilder.DropTable(
                name: "TT_EXIT_INTERVIEW");

            migrationBuilder.DropTable(
                name: "TT_EXIT_QUESTIONS");

            migrationBuilder.DropTable(
                name: "TTBT_EXIT_TEV");
        }
    }
}
