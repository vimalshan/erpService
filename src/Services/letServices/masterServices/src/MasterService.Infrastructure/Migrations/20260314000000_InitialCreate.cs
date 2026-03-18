using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BENEFIT_MAST",
                columns: table => new
                {
                    BE_BEN_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    BE_BEN_DES = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_BENEFIT_MAST", x => x.BE_BEN_COD));

            migrationBuilder.CreateTable(
                name: "CAT_MAST",
                columns: table => new
                {
                    CT_CAT_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CT_CAT_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    CT_SRL_NUM = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_CAT_MAST", x => x.CT_CAT_COD));

            migrationBuilder.CreateTable(
                name: "COMP_FINYEAR",
                columns: table => new
                {
                    AC_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AC_STR_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    AC_END_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    AC_CLS_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMP_FINYEAR", x => x.AC_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "COST_MAST",
                columns: table => new
                {
                    CS_CST_COD = table.Column<long>(type: "bigint", nullable: false),
                    CS_CST_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_COST_MAST", x => x.CS_CST_COD));

            migrationBuilder.CreateTable(
                name: "FUNCTION_GROUP",
                columns: table => new
                {
                    GR_GRP_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    GR_GRP_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    GR_SRL_NUM = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_FUNCTION_GROUP", x => x.GR_GRP_COD));

            migrationBuilder.CreateTable(
                name: "FUNCTION_MAST",
                columns: table => new
                {
                    FN_FNC_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    FN_FNC_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    FN_GRP_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    FN_UNT_COD = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    FN_SRL_NUM = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_FUNCTION_MAST", x => x.FN_FNC_COD));

            migrationBuilder.CreateTable(
                name: "GOAL_MAST",
                columns: table => new
                {
                    GL_GOL_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    GL_GOL_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_GOAL_MAST", x => x.GL_GOL_COD));

            migrationBuilder.CreateTable(
                name: "JOB_MAST",
                columns: table => new
                {
                    JB_JOB_COD = table.Column<long>(type: "bigint", nullable: false),
                    JB_JOB_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    JB_CAT_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    JB_SRL_NUM = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_JOB_MAST", x => x.JB_JOB_COD));

            migrationBuilder.CreateTable(
                name: "MODE_MAST",
                columns: table => new
                {
                    MD_MOD_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    MD_MOD_DES = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_MODE_MAST", x => x.MD_MOD_COD));

            migrationBuilder.CreateTable(
                name: "SKILL_GROUP",
                columns: table => new
                {
                    SK_GRP_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    SK_GRP_NAM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_SKILL_GROUP", x => x.SK_GRP_COD));

            migrationBuilder.CreateTable(
                name: "SKILL_MAST",
                columns: table => new
                {
                    SK_SKL_COD = table.Column<long>(type: "bigint", nullable: false),
                    SK_SKL_NAM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SK_SKL_TYP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SK_WGT_NUM = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SK_SKL_REM = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    SK_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SK_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_SKILL_MAST", x => x.SK_SKL_COD));

            migrationBuilder.CreateTable(
                name: "SOURCE_MAST",
                columns: table => new
                {
                    SR_SRC_COD = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false),
                    SR_SRC_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_SOURCE_MAST", x => x.SR_SRC_COD));

            migrationBuilder.CreateTable(
                name: "TRAIN_GROUP",
                columns: table => new
                {
                    TR_GRP_COD = table.Column<long>(type: "bigint", nullable: false),
                    TR_GRP_NAM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_TRAIN_GROUP", x => x.TR_GRP_COD));

            migrationBuilder.CreateTable(
                name: "TRAIN_MAST",
                columns: table => new
                {
                    TR_TRN_COD = table.Column<long>(type: "bigint", nullable: false),
                    TR_TRN_NAM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TR_TRN_ADD1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_TRN_ADD2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_TRN_ADD3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_TRN_ADD4 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_CNT_NAM1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_CNT_NAM2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_REM_MRK = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_PHN_NUM1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_PHN_NUM2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_FAX_NUM1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_FAX_NUM2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_EML_ADD1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_EML_ADD2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_GRP_COD = table.Column<long>(type: "bigint", nullable: true),
                    TR_VND_RAT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    TR_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TR_CAN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TR_CAN_REM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_BRC_FIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_VND_EXP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table => table.PrimaryKey("PK_TRAIN_MAST", x => x.TR_TRN_COD));

            // Create indexes matching the original schema
            migrationBuilder.CreateIndex("IDX_FUNCTION_MAST_GRP_COD", "FUNCTION_MAST", "FN_GRP_COD");
            migrationBuilder.CreateIndex("IDX_JOB_MAST_CAT_COD", "JOB_MAST", "JB_CAT_COD");
            migrationBuilder.CreateIndex("IDX_COMP_FINYEAR_STR_DAT", "COMP_FINYEAR", "AC_STR_DAT");
            migrationBuilder.CreateIndex("IDX_SKILL_MAST_SKL_TYP", "SKILL_MAST", "SK_SKL_TYP");
            migrationBuilder.CreateIndex("IDX_TRAIN_MAST_GRP_COD", "TRAIN_MAST", "TR_GRP_COD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BENEFIT_MAST");
            migrationBuilder.DropTable(name: "CAT_MAST");
            migrationBuilder.DropTable(name: "COMP_FINYEAR");
            migrationBuilder.DropTable(name: "COST_MAST");
            migrationBuilder.DropTable(name: "FUNCTION_GROUP");
            migrationBuilder.DropTable(name: "FUNCTION_MAST");
            migrationBuilder.DropTable(name: "GOAL_MAST");
            migrationBuilder.DropTable(name: "JOB_MAST");
            migrationBuilder.DropTable(name: "MODE_MAST");
            migrationBuilder.DropTable(name: "SKILL_GROUP");
            migrationBuilder.DropTable(name: "SKILL_MAST");
            migrationBuilder.DropTable(name: "SOURCE_MAST");
            migrationBuilder.DropTable(name: "TRAIN_GROUP");
            migrationBuilder.DropTable(name: "TRAIN_MAST");
        }
    }
}
