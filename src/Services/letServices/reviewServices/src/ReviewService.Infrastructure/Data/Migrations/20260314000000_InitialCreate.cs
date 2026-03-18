using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewService.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "REVIEW_MAIN",
            columns: table => new
            {
                REV_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                REV_FED_NUM = table.Column<long>(type: "bigint", nullable: true),
                REV_REM_MRK1 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK2 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK3 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK4 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK5 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK6 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK7 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK8 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK9 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_REM_MRK10 = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_ENT_DATE = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                REV_STATUS = table.Column<string>(type: "CHAR(1)", nullable: true),
                REV_NEXT_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_REVIEW_MAIN", x => x.REV_SRL_NUM));

        migrationBuilder.CreateTable(
            name: "REVIEW_MAST",
            columns: table => new
            {
                RV_TYP_COD = table.Column<string>(type: "CHAR(3)", maxLength: 3, nullable: false),
                RV_TYP_NAM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false),
                RV_GRP_COD = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_REVIEW_MAST", x => x.RV_TYP_COD));

        migrationBuilder.CreateTable(
            name: "REVIEW_SKILL",
            columns: table => new
            {
                SK_REQ_ID = table.Column<long>(type: "bigint", nullable: false),
                SK_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                SK_ACT_NUM = table.Column<long>(type: "bigint", nullable: false),
                SK_REV_NUM = table.Column<long>(type: "bigint", nullable: false),
                SK_SKL_COD = table.Column<long>(type: "bigint", nullable: false),
                SK_LVL_NUM = table.Column<long>(type: "bigint", nullable: false),
                SK_RAT_PER = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                SK_REM_MRK = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_REVIEW_SKILL", x => x.SK_REQ_ID));

        migrationBuilder.CreateTable(
            name: "REVIEW_SUB",
            columns: table => new
            {
                REV_MAIN_SRL = table.Column<long>(type: "bigint", nullable: true),
                REV_REV_NUM = table.Column<long>(type: "bigint", nullable: true),
                REV_NEXT_STATUS = table.Column<string>(type: "CHAR(1)", nullable: true),
                REV_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                REV_BY = table.Column<long>(type: "bigint", nullable: true),
                REV_REM_MRK = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true),
                REV_STATUS = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                REV_PROG_REM = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "COURSE_FEEDMAIN",
            columns: table => new
            {
                FD_CRS_ID = table.Column<long>(type: "bigint", nullable: false),
                FD_USR_ID = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                FD_REV_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                FD_GEN_REM = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                FD_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                FD_MOD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                FD_SRL_NUM = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_FEEDMAIN", x => new { x.FD_USR_ID, x.FD_CRS_ID }));

        migrationBuilder.CreateTable(
            name: "COURSE_FEEDSUB",
            columns: table => new
            {
                FD_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                FD_REQ_SRL = table.Column<long>(type: "bigint", nullable: false),
                FD_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                FD_TYP_COD = table.Column<long>(type: "bigint", nullable: false),
                FD_TYP_NUM = table.Column<long>(type: "bigint", nullable: true),
                FD_TYP_DES = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_FEEDSUB", x => new { x.FD_REQ_NUM, x.FD_REQ_SRL, x.FD_SRL_NUM }));

        migrationBuilder.CreateTable(
            name: "COURSE_FEEDBACKMAIN",
            columns: table => new
            {
                FD_FED_NUM = table.Column<long>(type: "bigint", nullable: true),
                FD_NOM_NUM = table.Column<long>(type: "bigint", nullable: true),
                FD_STS_COD = table.Column<string>(type: "CHAR(1)", nullable: true),
                FD_FED_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                FD_MOD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                FD_FIN_RAT = table.Column<long>(type: "bigint", nullable: true),
                FD_REM_LIN1 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REM_LIN2 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REM_LIN3 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REV_SRL = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                FD_CANCEL_REM = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REQ_NUM = table.Column<long>(type: "bigint", nullable: true),
                FD_REM_LIN9 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REM_LIN4 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REM_LIN5 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REM_LIN6 = table.Column<long>(type: "bigint", nullable: true),
                FD_REM_LIN7 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                FD_REM_LIN8 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "COURSE_FEEDBACKSUB",
            columns: table => new
            {
                FD_FED_NUM = table.Column<long>(type: "bigint", nullable: true),
                FD_FED_TYP = table.Column<long>(type: "bigint", nullable: true),
                FD_RAT_NUM = table.Column<long>(type: "bigint", nullable: true),
                FD_REM_MRK = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "COURSE_REVIEWMAIN",
            columns: table => new
            {
                RV_CRS_ID = table.Column<long>(type: "bigint", nullable: false),
                RV_USR_ID = table.Column<string>(type: "CHAR(1)", nullable: false),
                RV_REV_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                RV_GEN_REM = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                RQ_SUP_USR = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                RV_SRL_NUM = table.Column<string>(type: "CHAR(1)", nullable: false),
                RV_SUP_REM = table.Column<string>(type: "CHAR(1)", nullable: false),
                RV_RAT_PER = table.Column<string>(type: "CHAR(1)", nullable: false),
                RV_FIL_NAM = table.Column<string>(type: "CHAR(1)", nullable: false),
                RV_NXT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                RV_ORG_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_REVIEWMAIN", x => x.RV_CRS_ID));

        migrationBuilder.CreateTable(
            name: "COURSE_REVIEWSUB",
            columns: table => new
            {
                RV_CRS_ID = table.Column<long>(type: "bigint", nullable: false),
                RV_USR_ID = table.Column<string>(type: "CHAR(1)", nullable: false),
                RV_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                RV_TYP_COD = table.Column<string>(type: "CHAR(1)", nullable: false),
                RV_TYP_NUM = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_COURSE_REVIEWSUB", x => new { x.RV_CRS_ID, x.RV_USR_ID }));

        migrationBuilder.CreateTable(
            name: "FEED_MAST",
            columns: table => new
            {
                FD_TYP_COD = table.Column<long>(type: "bigint", nullable: false),
                FD_TYP_NAM = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                FD_NUM_TYP = table.Column<string>(type: "CHAR(1)", nullable: false),
                FD_EVL_COD = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_FEED_MAST", x => x.FD_TYP_COD));

        migrationBuilder.CreateTable(
            name: "FEED_EVALMAST",
            columns: table => new
            {
                FD_EVL_TYP = table.Column<long>(type: "bigint", nullable: true),
                FD_EVL_DES = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                FD_WGT_NUM = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "TRAINER_FEED",
            columns: table => new
            {
                TR_GRP_COD = table.Column<long>(type: "bigint", nullable: false),
                TR_FED_NUM = table.Column<long>(type: "bigint", nullable: false),
                TR_SRL_NUM = table.Column<long>(type: "bigint", nullable: true),
                TR_QTN_GRP = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                TR_GRP_NUM = table.Column<long>(type: "bigint", nullable: true),
                TR_WGT_NUM = table.Column<long>(type: "bigint", nullable: true),
                TR_EFF_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                TR_CLS_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_TRAINER_FEED", x => new { x.TR_GRP_COD, x.TR_FED_NUM }));

        // Indexes
        migrationBuilder.CreateIndex("IDX_REVIEW_FEEDMAIN_CRS_ID", "COURSE_FEEDMAIN", "FD_CRS_ID");
        migrationBuilder.CreateIndex("IDX_REVIEW_REVIEWMAIN_CRS_ID", "COURSE_REVIEWMAIN", "RV_CRS_ID");
        migrationBuilder.CreateIndex("IDX_REVIEW_SKILL_REQ_ID", "REVIEW_SKILL", "SK_REQ_ID");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "REVIEW_SUB");
        migrationBuilder.DropTable(name: "REVIEW_MAIN");
        migrationBuilder.DropTable(name: "REVIEW_MAST");
        migrationBuilder.DropTable(name: "REVIEW_SKILL");
        migrationBuilder.DropTable(name: "COURSE_FEEDSUB");
        migrationBuilder.DropTable(name: "COURSE_FEEDMAIN");
        migrationBuilder.DropTable(name: "COURSE_FEEDBACKSUB");
        migrationBuilder.DropTable(name: "COURSE_FEEDBACKMAIN");
        migrationBuilder.DropTable(name: "COURSE_REVIEWSUB");
        migrationBuilder.DropTable(name: "COURSE_REVIEWMAIN");
        migrationBuilder.DropTable(name: "FEED_MAST");
        migrationBuilder.DropTable(name: "FEED_EVALMAST");
        migrationBuilder.DropTable(name: "TRAINER_FEED");
    }
}
