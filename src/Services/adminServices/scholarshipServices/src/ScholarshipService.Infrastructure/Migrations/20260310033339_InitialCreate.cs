using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScholarshipService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SCHOLARSHIP_AMOUNT",
                columns: table => new
                {
                    SCH_AMTID = table.Column<long>(type: "bigint", nullable: false),
                    SCH_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    SCH_GRADECAT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SCH_ELGIBLEEXAM = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    SCH_APPLICABLEALLGRADE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCH_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SCH_FROMYEAR = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SCH_CLOSEYEAR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SCH_ELGIBLEAMOUNT = table.Column<long>(type: "bigint", nullable: false),
                    SCH_ELGIBLEYEAR = table.Column<int>(type: "int", nullable: false),
                    SCH_CUTOFFMARKS = table.Column<int>(type: "int", nullable: false),
                    SCH_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SCH_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SCH_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCH_UPDATEDBY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHOLARSHIP_AMOUNT", x => x.SCH_AMTID);
                });

            migrationBuilder.CreateTable(
                name: "SCHOLARSHIP_MAIN",
                columns: table => new
                {
                    SCH_ID = table.Column<int>(type: "int", nullable: false),
                    SCH_EMPSYSID = table.Column<int>(type: "int", nullable: false),
                    SCH_GRADEID = table.Column<int>(type: "int", nullable: false),
                    SCH_DEPENDID = table.Column<int>(type: "int", nullable: false),
                    SCH_CHILDNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SCH_LASTSCHOOL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SCH_LASTYEAROFSCHOOL = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SCH_LASTEXAM = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    SCH_CGPAFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCH_MARKSPER = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    SCH_MARKSGPA = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    SCH_MARKSFILE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SCH_COURSENAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SCH_COURSEJOINYEAR = table.Column<int>(type: "int", nullable: false),
                    SCH_COURSEJOINMONTH = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SCH_COURSEDURATION = table.Column<long>(type: "bigint", nullable: false),
                    SCH_ADMRECPTFILE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SCH_PAYMODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    SCH_CHILDACCNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SCH_CHILLDBANKIFSC = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    SCH_CHILLDBANKMICR = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    SCH_ENTRYSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SCH_SOURCE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCH_DISBAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    SCH_DISBFREQ = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCH_LIVESTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCH_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SCH_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SCH_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCH_UPDATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SCH_APPROVALBY = table.Column<int>(type: "int", nullable: false),
                    SCH_APPROVALON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCH_APPREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SCH_STOPREASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SCH_STOPDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCH_STOPENTEREDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCH_STOPENTEREDBY = table.Column<int>(type: "int", nullable: true),
                    SCH_OFFLINE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCH_OFFLINEYEAR = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHOLARSHIP_MAIN", x => x.SCH_ID);
                });

            migrationBuilder.CreateTable(
                name: "SCHOLARSHIP_DETAIL",
                columns: table => new
                {
                    SCHDET_ID = table.Column<long>(type: "bigint", nullable: false),
                    SCHDET_MAINID = table.Column<int>(type: "int", nullable: false),
                    SCHDET_YEAR = table.Column<int>(type: "int", nullable: false),
                    SCHDET_MARKSFILE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SCHDET_MARKSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCHDET_PAYSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCHDET_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SCHDET_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SCHDET_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCHDET_UPDATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SCHDET_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCHDET_APPROVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SCHDET_PAYAPPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCHDET_PAYAPPROVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SCHDET_PAYDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCHDET_PAYAMOUNT = table.Column<long>(type: "bigint", nullable: true),
                    SCHDET_PAYUPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCHDET_PAYUPDATEDBY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHOLARSHIP_DETAIL", x => x.SCHDET_ID);
                    table.ForeignKey(
                        name: "FK_SCHOLARSHIP_DETAIL_MAIN",
                        column: x => x.SCHDET_MAINID,
                        principalTable: "SCHOLARSHIP_MAIN",
                        principalColumn: "SCH_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_SCHOLARSHIP_AMOUNT_GRADECAT",
                table: "SCHOLARSHIP_AMOUNT",
                columns: new[] { "SCH_GRADECAT", "SCH_ELGIBLEEXAM" });

            migrationBuilder.CreateIndex(
                name: "IX_SCHOLARSHIP_DETAIL_SCHDET_MAINID",
                table: "SCHOLARSHIP_DETAIL",
                column: "SCHDET_MAINID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCHOLARSHIP_AMOUNT");

            migrationBuilder.DropTable(
                name: "SCHOLARSHIP_DETAIL");

            migrationBuilder.DropTable(
                name: "SCHOLARSHIP_MAIN");
        }
    }
}
