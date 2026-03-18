using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppraisalService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DD_APPRAISALBAND",
                columns: table => new
                {
                    DD_BND_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DD_BND_DSC = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    DD_BND_DSG = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    DD_SIG_NAM = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    DD_SIG_DSG = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    DD_BND_COD = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    DD_FORMFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    DD_GRADEID = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalBand", x => x.DD_BND_ID);
                });

            migrationBuilder.CreateTable(
                name: "DD_APPRAISALMAIN",
                columns: table => new
                {
                    AP_REQ_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AP_USR_COD = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    AP_USR_NUM = table.Column<long>(type: "bigint", nullable: true),
                    AP_PIN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    AP_ENT_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AP_GRADEID = table.Column<long>(type: "bigint", nullable: true),
                    AP_UNITID = table.Column<long>(type: "bigint", nullable: true),
                    AP_YEARID = table.Column<long>(type: "bigint", nullable: true),
                    AP_CAN_REM = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AP_ST_FIN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AP_ED_FIN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AP_FIN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AP_DD_TYPE = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    AP_CAN_APPRID = table.Column<long>(type: "bigint", nullable: true),
                    AP_CANCELDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AP_SUBORDINATE = table.Column<string>(type: "char(1)", nullable: true),
                    DD_USR_SLT = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true),
                    DD_USR_FNM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                    DD_USR_MNM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                    DD_USR_LNM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                    DD_USR_DSG = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    DD_CEO_NAM = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    DD_CEO_DSG = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    DD_VTC_RAT = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true),
                    DD_PRM_BND = table.Column<long>(type: "bigint", nullable: true),
                    DD_EMP_TYP = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    DD_PAYROLL = table.Column<string>(type: "char(1)", nullable: true),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AP_STS_COD = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalMain", x => x.AP_REQ_NUM);
                });

            migrationBuilder.CreateTable(
                name: "DD_APPRAISEEGOAL_CUR",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AG_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AG_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AG_PIN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    AG_USR_ID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    AG_PER_DES = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AG_UNT_FRM = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    AG_UNT_TO = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    AG_WEIGHTAGE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AG_APP_RMK = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AG_CAN_RMK = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AG_FIN_STR = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AG_FIN_END = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AG_CATEGORY = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    AG_UOM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                    AG_APS_STS = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true),
                    AG_ACH = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AG_DIFF = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AG_MOD_SRLNO = table.Column<long>(type: "bigint", nullable: true),
                    AG_EXPCOD = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    AG_GOL_FLG = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    AG_ACCOUNTABILITYID = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeGoal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DD_APPRAISERASSESS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AP_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AP_CPD_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AP_ASS_SRL = table.Column<long>(type: "bigint", nullable: false),
                    AP_ASS_RAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AP_COMP_RATING = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AP_REM_MRK = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    AP_SLF_DEV = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AP_JOB_DEV = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AP_TRG_DEV = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AP_USR_COD = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    AP_REF_SRLNO = table.Column<long>(type: "bigint", nullable: true),
                    AP_PIN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    AP_CAN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AP_CAN_REM = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                    AP_ROLE = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    AppraisalMainRequestNumber = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetencyAssessment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetencyAssessment_AppraisalMain",
                        column: x => x.AppraisalMainRequestNumber,
                        principalTable: "DD_APPRAISALMAIN",
                        principalColumn: "AP_REQ_NUM");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetencyAssessment_AppraisalMainRequestNumber",
                table: "DD_APPRAISERASSESS",
                column: "AppraisalMainRequestNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DD_APPRAISERASSESS");

            migrationBuilder.DropTable(
                name: "DD_APPRAISALBAND");

            migrationBuilder.DropTable(
                name: "DD_APPRAISEEGOAL_CUR");

            migrationBuilder.DropTable(
                name: "DD_APPRAISALMAIN");
        }
    }
}
