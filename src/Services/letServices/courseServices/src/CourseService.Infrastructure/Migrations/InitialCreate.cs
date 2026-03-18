using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseService.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "COURSE_MAST",
            columns: table => new
            {
                CR_CRS_ID = table.Column<long>(nullable: false),
                CR_CRS_TYP = table.Column<string>(maxLength: 1, nullable: false),
                CR_CRS_DES = table.Column<string>(maxLength: 255, nullable: false),
                CR_EFF_DAT = table.Column<DateTime>(nullable: false),
                CR_CLS_DAT = table.Column<DateTime>(nullable: false),
                CR_OBJ_DES = table.Column<string>(maxLength: 255, nullable: false),
                CR_LOC_COD = table.Column<string>(maxLength: 1, nullable: false),
                CR_ADD_LN1 = table.Column<string>(maxLength: 255, nullable: false),
                CR_ADD_LN2 = table.Column<string>(maxLength: 255, nullable: false),
                CR_ADD_LN3 = table.Column<string>(maxLength: 255, nullable: false),
                CR_PIN_COD = table.Column<long>(nullable: false),
                CR_PHN_NUM = table.Column<string>(maxLength: 255, nullable: false),
                CR_STR_DAT = table.Column<DateTime>(nullable: false),
                CR_END_DAT = table.Column<DateTime>(nullable: false),
                CR_LST_DAT = table.Column<DateTime>(nullable: false),
                CR_NO_DYS = table.Column<long>(nullable: false),
                CR_TRN_TYP = table.Column<string>(maxLength: 1, nullable: false),
                CR_CAN_DAT = table.Column<DateTime>(nullable: true),
                CR_CAN_REM = table.Column<string>(maxLength: 255, nullable: true),
                CR_FIL_NAM = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_NAM1 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_NAM2 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_NAM3 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_DES1 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_DES2 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_DES3 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_CNT1 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_CNT2 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_CNT3 = table.Column<string>(maxLength: 255, nullable: true),
                CR_TRN_COD = table.Column<long>(nullable: true),
                CR_TRN_RAT = table.Column<decimal>(nullable: true),
                CR_CNT_RAT = table.Column<decimal>(nullable: true),
                CR_ADM_RAT = table.Column<decimal>(nullable: true),
                CR_PEN_DAT = table.Column<DateTime>(nullable: true),
                CR_THMB_PIC = table.Column<string>(maxLength: 255, nullable: true),
                CR_CRS_DUR = table.Column<string>(maxLength: 255, nullable: true),
                CR_EVAL_ID = table.Column<long>(nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_MAST", x => x.CR_CRS_ID));

        migrationBuilder.CreateTable(
            name: "COURSE_SCHEDULE",
            columns: table => new
            {
                CS_SCH_SRL = table.Column<long>(nullable: false),
                CS_CRS_ID = table.Column<long>(nullable: false),
                CS_SCH_DAT = table.Column<DateTime>(nullable: false),
                CS_STR_TIM = table.Column<string>(maxLength: 5, nullable: false),
                CS_END_TIM = table.Column<string>(maxLength: 5, nullable: false),
                CS_LOC_NAM = table.Column<string>(maxLength: 65, nullable: false),
                CS_TRN_NAM = table.Column<string>(maxLength: 65, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_COURSE_SCHEDULE", x => x.CS_SCH_SRL);
                table.ForeignKey("FK_COURSE_SCHEDULE_COURSE_MAST", x => x.CS_CRS_ID, "COURSE_MAST", "CR_CRS_ID");
            });

        migrationBuilder.CreateTable(
            name: "COURSE_PARTICIPANT_MGT",
            columns: table => new
            {
                CS_CRS_ID = table.Column<long>(nullable: true),
                CS_NOM_NUM = table.Column<long>(nullable: true),
                CS_USR_COD = table.Column<string>(maxLength: 255, nullable: true),
                CS_CAN_DAT = table.Column<DateTime>(nullable: true),
                CS_CAN_REM = table.Column<string>(maxLength: 255, nullable: true),
                CS_ENR_DAT = table.Column<DateTime>(nullable: true),
                CS_APPR_APPROV = table.Column<string>(maxLength: 1, nullable: true),
                CS_APR_CANCEL = table.Column<string>(maxLength: 1, nullable: true),
                CS_USR_PIN = table.Column<long>(nullable: true),
                CS_APPR_COD = table.Column<string>(maxLength: 255, nullable: true),
                CS_APPR_PIN = table.Column<long>(nullable: true),
                CS_NOM_STS = table.Column<long>(nullable: true),
                CS_REQNUM = table.Column<long>(nullable: true),
                CS_TYPE = table.Column<string>(maxLength: 1, nullable: true),
                CS_CRS_DESC = table.Column<string>(maxLength: 255, nullable: true),
                CS_TRAINING_DATE = table.Column<string>(maxLength: 255, nullable: true),
                CS_STARTDAT = table.Column<DateTime>(nullable: true),
                CS_ENDATE = table.Column<DateTime>(nullable: true),
                CS_ATTEN = table.Column<string>(maxLength: 1, nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "COURSE_BAND",
            columns: table => new
            {
                COURSEBAND_COURSEID = table.Column<long>(nullable: true),
                COURSEBAND_ID = table.Column<long>(nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_BAND", x => x.COURSEBAND_COURSEID));

        migrationBuilder.CreateTable(
            name: "COURSE_COST",
            columns: table => new
            {
                CS_CRS_ID = table.Column<long>(nullable: true),
                CS_CST_COD = table.Column<long>(nullable: true),
                CS_CST_AMT = table.Column<long>(nullable: true),
                CS_CST_TYP = table.Column<string>(maxLength: 1, nullable: true),
                CS_REM_MRK = table.Column<string>(maxLength: 200, nullable: true),
                CS_UNT_COD = table.Column<string>(maxLength: 6, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_COST", x => x.CS_CRS_ID));

        migrationBuilder.CreateTable(
            name: "COURSE_MODEL",
            columns: table => new
            {
                MD_CRS_ID = table.Column<long>(nullable: false),
                MD_SKL_NUM = table.Column<long>(nullable: false),
                MD_LVL_NUM = table.Column<long>(nullable: false),
                MD_SKL_GRP = table.Column<string>(maxLength: 3, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_MODEL", x => new { x.MD_CRS_ID, x.MD_SKL_NUM }));

        // Indexes
        migrationBuilder.CreateIndex("IDX_COURSE_MAST_CRS_TYP", "COURSE_MAST", "CR_CRS_TYP");
        migrationBuilder.CreateIndex("IDX_COURSE_SCHEDULE_CRS_ID", "COURSE_SCHEDULE", "CS_CRS_ID");
        migrationBuilder.CreateIndex("IDX_COURSE_PARTICIPANT_CRS_ID", "COURSE_PARTICIPANT_MGT", "CS_CRS_ID");
        migrationBuilder.CreateIndex("IDX_COURSE_PARTICIPANT_USR_COD", "COURSE_PARTICIPANT_MGT", "CS_USR_COD");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("COURSE_MODEL");
        migrationBuilder.DropTable("COURSE_COST");
        migrationBuilder.DropTable("COURSE_BAND");
        migrationBuilder.DropTable("COURSE_PARTICIPANT_MGT");
        migrationBuilder.DropTable("COURSE_SCHEDULE");
        migrationBuilder.DropTable("COURSE_MAST");
    }
}
