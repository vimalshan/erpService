using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompetencyService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BAND_CORECOMPETENCY",
                columns: table => new
                {
                    BAND_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    COMPETENCY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAND_CORECOMPETENCY", x => new { x.BAND_ID, x.COMPETENCY_ID });
                });

            migrationBuilder.CreateTable(
                name: "COMPETENCY_RATING_SCALE",
                columns: table => new
                {
                    COMPETENCY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    R1_DESC = table.Column<string>(type: "varchar(250)", nullable: false),
                    R2_DESC = table.Column<string>(type: "varchar(500)", nullable: true),
                    R3_DESC = table.Column<string>(type: "varchar(500)", nullable: false),
                    R4_DESC = table.Column<string>(type: "varchar(500)", nullable: true),
                    R5_DESC = table.Column<string>(type: "varchar(500)", nullable: false),
                    MODIFIED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPETENCY_RATING_SCALE", x => x.COMPETENCY_ID);
                });

            migrationBuilder.CreateTable(
                name: "DD_COMPENDMAST",
                columns: table => new
                {
                    CM_CPD_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CM_CPD_NAM = table.Column<string>(type: "varchar(4000)", nullable: false),
                    CM_EFF_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CM_CLS_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CM_CPD_REM = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CM_JOB_COD = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CM_POS_IND = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CM_NEG_IND = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CM_CPD_SLF = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CM_CPD_TYPE = table.Column<string>(type: "varchar(10)", nullable: true),
                    CM_PARENTID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ModifiedBy = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_COMPENDMAST", x => x.CM_CPD_NUM);
                });

            migrationBuilder.CreateTable(
                name: "DD_COMPETENCY_IND",
                columns: table => new
                {
                    SRL_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    BAND = table.Column<string>(type: "varchar(50)", nullable: true),
                    COMP_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    IND_FLAG = table.Column<string>(type: "char(1)", nullable: true),
                    IND_DEFN = table.Column<string>(type: "varchar(4000)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_VTCCOMPETENCY",
                columns: table => new
                {
                    SRL_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    BAND = table.Column<string>(type: "varchar(50)", nullable: true),
                    COMP_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    COMP_NAM = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EMP_SPECIFIC_COMPETENCY",
                columns: table => new
                {
                    EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    COMPETENCY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    COMPETENCY_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    DD_YEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MODIFIED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_SPECIFIC_COMPETENCY", x => new { x.EMP_SYSID, x.COMPETENCY_ID, x.COMPETENCY_TYPE, x.DD_YEARID });
                });

            migrationBuilder.CreateTable(
                name: "ROLE_SPECIFIC",
                columns: table => new
                {
                    EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    COMPETENCY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EFF_FROM = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    EFF_TO = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    MODIFIED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_SPECIFIC", x => new { x.EMP_SYSID, x.COMPETENCY_ID });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BAND_CORECOMPETENCY");

            migrationBuilder.DropTable(
                name: "COMPETENCY_RATING_SCALE");

            migrationBuilder.DropTable(
                name: "DD_COMPENDMAST");

            migrationBuilder.DropTable(
                name: "DD_COMPETENCY_IND");

            migrationBuilder.DropTable(
                name: "DD_VTCCOMPETENCY");

            migrationBuilder.DropTable(
                name: "EMP_SPECIFIC_COMPETENCY");

            migrationBuilder.DropTable(
                name: "ROLE_SPECIFIC");
        }
    }
}
