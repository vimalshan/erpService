using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompensationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAA_BUDGET",
                columns: table => new
                {
                    BUDGET_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BUSINESS_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YEAR_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BUDGET_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UPDATED_BY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAA_BUDGET", x => x.BUDGET_ID);
                });

            migrationBuilder.CreateTable(
                name: "SAA_BUDGETLOG",
                columns: table => new
                {
                    LOGID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BUDGETID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BUDGET_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UPDATED_BY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MOD_BY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MOD_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAA_BUDGETLOG", x => x.LOGID);
                });

            migrationBuilder.CreateTable(
                name: "SAA_LEVEL",
                columns: table => new
                {
                    LEVEL_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LEVEL_DESC = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LEVEL_AMOUNT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LEVEL_REASON = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LEVEL_MIN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LEVEL_MAX = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LEVEL_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LEVEL_CLOSEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LEVEL_UPDATEDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LEVEL_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAA_LEVEL", x => x.LEVEL_ID);
                });

            migrationBuilder.CreateTable(
                name: "SAA_PERIOD",
                columns: table => new
                {
                    PERIOD_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YEAR_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QUARTER_NO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Status_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PERIOD_OPENDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PERIOD_CLOSEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CIRCULAR_GENON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CIRCULAR_GENBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    REMINDER_LETON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FORM_OPENDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APRAISER_LASTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REVIEWER_LASTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BHR_LASTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UHR_LASTDATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAA_PERIOD", x => x.PERIOD_ID);
                });

            migrationBuilder.CreateTable(
                name: "SAA_RECOMMEND",
                columns: table => new
                {
                    RECOMMEND_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YEAR_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PERIOD_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EMP_SYSID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LEVEL_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CTC_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MAXIMUM_CAP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ELIGIBILITY_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RECOMMEND_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    INITIATIVE_TAKEN = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    RESULTS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ADD_REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    Status_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REJECTION_BY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    REJECTION_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REJECTION_REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RECOMMEND_BY = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    RECOMMEND_SUBMITBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RECOMMEND_SUBMITON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REVIEWER_SUBMITBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    REVIEWER_SUBMITON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BHR_SUBMITBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BHR_SUBMITON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CHR_SUBMITBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CHR_SUBMITON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UHR_SUBMITBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UHR_SUBMITON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FINAL_LEVEL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FINAL_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    INITIATIVE_LETTER = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RESULTS_LETTER = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAA_RECOMMEND", x => x.RECOMMEND_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAA_BUDGET");

            migrationBuilder.DropTable(
                name: "SAA_BUDGETLOG");

            migrationBuilder.DropTable(
                name: "SAA_LEVEL");

            migrationBuilder.DropTable(
                name: "SAA_PERIOD");

            migrationBuilder.DropTable(
                name: "SAA_RECOMMEND");
        }
    }
}
