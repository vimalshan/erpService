using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APPLICATION_HISTORY",
                columns: table => new
                {
                    APP_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    APP_SL = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    APP_UNIT = table.Column<string>(type: "CHAR(3)", nullable: true),
                    APP_VACANCYID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    APP_STATUS = table.Column<string>(type: "CHAR(2)", nullable: false),
                    APP_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    APP_UPDATEDBY = table.Column<decimal>(type: "DECIMAL(22,0)", nullable: true),
                    APP_UPDATEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPLICATION_HISTORY", x => x.APP_ID);
                });

            migrationBuilder.CreateTable(
                name: "VACANCY_MAIN",
                columns: table => new
                {
                    VACANCY_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    VACANCY_UNIT = table.Column<string>(type: "CHAR(3)", nullable: false),
                    VACANCY_GRADE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    VACANCY_POSITIONID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    VACANCY_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    VACANCY_REPORTING = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    VACANCY_LOCATION = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    VACANCY_PROCESS = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    VACANCY_AGE = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    VACANCY_EXPERIENCE = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    VACANCY_QUALIFICATION = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    VACANCY_NARRATION1 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    VACANCY_NARRATION2 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    VACANCY_NARRATION3 = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    VACANCY_NARRATION4 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VACANCY_ATTACHMENT = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    VACANCY_LASTDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_ADINTRAFLAG = table.Column<string>(type: "CHAR(1)", nullable: false),
                    VACANCY_ADINTRAFRODATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_ADINTRATODATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_ADINTERFLAG = table.Column<string>(type: "CHAR(1)", nullable: false),
                    VACANCY_ADINTERFRODATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_ADINTERTODATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_POSTBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    VACANCY_POSTDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_MODBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    VACANCY_MODDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_LIVESTATUS = table.Column<string>(type: "CHAR(1)", nullable: false),
                    VACANCY_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    VACANCY_INTREFRFLAG = table.Column<string>(type: "CHAR(1)", nullable: false),
                    VACANCY_INTREFMAILID = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    VACANCY_UNITID = table.Column<decimal>(type: "DECIMAL(22,0)", nullable: false),
                    VACANCY_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    VACANCY_GRADELIST = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    VACANCY_GRADETYPE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    VACANCY_NOS = table.Column<decimal>(type: "DECIMAL(22,0)", nullable: true),
                    VACANCY_CTCFROM = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    VACANCY_CTCTO = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    VACANCY_DESIGNATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VACANCY_DOWNLOADFORM = table.Column<string>(type: "CHAR(1)", nullable: false),
                    VACANCY_APPLICATIONFORM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VACANCY_UPLOADRESUME = table.Column<string>(type: "CHAR(1)", nullable: false),
                    VACANCY_INTREFCLSDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    VACANCY_DISABILITYFLAG = table.Column<string>(type: "CHAR(1)", nullable: false),
                    VACANCY_DISABILITYLIMIT = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VACANCY_MAIN", x => x.VACANCY_ID);
                });

            migrationBuilder.CreateTable(
                name: "WEBPROSPECT_MAST",
                columns: table => new
                {
                    WEBUSER_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    WEBUSER_PWD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    WEBUSER_FRS_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    WEBUSER_MID_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    WEBUSER_LST_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    WEBUSER_EMAILID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WEBUSER_STATUS = table.Column<string>(type: "CHAR(1)", nullable: false),
                    WEBUSER_DATEOFBIRTH = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    WEBUSER_CREATEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    WEBUSER_TYPE = table.Column<string>(type: "CHAR(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WEBPROSPECT_MAST", x => x.WEBUSER_ID);
                });

            migrationBuilder.CreateTable(
                name: "APPLICATION_QUALIFICATION",
                columns: table => new
                {
                    APP_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    APP_QUAL_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    APP_QUAL_CODE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    APP_QUAL_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    APP_QUAL_YEARFRO = table.Column<string>(type: "CHAR(7)", nullable: true),
                    APP_QUAL_YEARTO = table.Column<string>(type: "CHAR(7)", nullable: true),
                    APP_QUAL_INST_CODE = table.Column<string>(type: "CHAR(3)", nullable: true),
                    APP_QUAL_INST_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    APP_QUAL_EDU_TYPE = table.Column<string>(type: "CHAR(1)", nullable: true),
                    APP_QUAL_SPE_CODE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    APP_QUAL_SPE_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    APP_QUAL_PERCENTAGE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    APP_QUAL_DEGREE_CODE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    APP_QUAL_DEGREE_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    APP_QUAL_INST_OTHERS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPLICATION_QUALIFICATION", x => new { x.APP_ID, x.APP_QUAL_ID });
                    table.ForeignKey(
                        name: "FK_APPLICATION_QUALIFICATION_APPLICATION_HISTORY_APP_ID",
                        column: x => x.APP_ID,
                        principalTable: "APPLICATION_HISTORY",
                        principalColumn: "APP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "APPLICATION_TRAINING",
                columns: table => new
                {
                    APP_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    APP_TRAINING_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    APP_TRAINING_TITLE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    APP_TRAINING_DURATION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    APP_TRAINING_INSTITUTE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    APP_TRAINING_LOCATION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPLICATION_TRAINING", x => new { x.APP_ID, x.APP_TRAINING_ID });
                    table.ForeignKey(
                        name: "FK_APPLICATION_TRAINING_APPLICATION_HISTORY_APP_ID",
                        column: x => x.APP_ID,
                        principalTable: "APPLICATION_HISTORY",
                        principalColumn: "APP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROSPECT_ADDRESS",
                columns: table => new
                {
                    ADDRESS_EMP_SYSID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    ADDRESS_FLAG = table.Column<string>(type: "CHAR(1)", nullable: false),
                    ADDRESS_1 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_2 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_3 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_4 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_CITY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    ADDRESS_PINCODE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    ADDRESS_UPDATED_BY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    ADDRESS_UPDATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    ADDRESS_MOBNO = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_LANDLINE = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROSPECT_ADDRESS", x => new { x.ADDRESS_EMP_SYSID, x.ADDRESS_FLAG });
                    table.ForeignKey(
                        name: "FK_PROSPECT_ADDRESS_WEBPROSPECT_MAST_ADDRESS_EMP_SYSID",
                        column: x => x.ADDRESS_EMP_SYSID,
                        principalTable: "WEBPROSPECT_MAST",
                        principalColumn: "WEBUSER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROSPECT_QUALIFICATION",
                columns: table => new
                {
                    QUAL_EMP_SYSID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    QUAL_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    QUAL_CODE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    QUAL_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_YEARFRO = table.Column<string>(type: "CHAR(6)", nullable: true),
                    QUAL_YEARTO = table.Column<string>(type: "CHAR(6)", nullable: true),
                    QUAL_INST_CODE = table.Column<decimal>(type: "DECIMAL(22,0)", nullable: true),
                    QUAL_INST_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_EDU_TYPE = table.Column<string>(type: "CHAR(1)", nullable: true),
                    QUAL_SPE_CODE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    QUAL_SPE_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_PERCENTAGE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QUAL_DEGREE_CODE = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    QUAL_DEGREE_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_UPDATEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    QUAL_UPDATEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROSPECT_QUALIFICATION", x => new { x.QUAL_EMP_SYSID, x.QUAL_ID });
                    table.ForeignKey(
                        name: "FK_PROSPECT_QUALIFICATION_WEBPROSPECT_MAST_QUAL_EMP_SYSID",
                        column: x => x.QUAL_EMP_SYSID,
                        principalTable: "WEBPROSPECT_MAST",
                        principalColumn: "WEBUSER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROSPECT_REFERENCE",
                columns: table => new
                {
                    REF_EMP_SYS_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    REF_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    REF_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    REF_DESGN = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    REF_ADDRESS1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    REF_ADDRESS2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    REF_PHONE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    REF_EMAIL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROSPECT_REFERENCE", x => new { x.REF_EMP_SYS_ID, x.REF_ID });
                    table.ForeignKey(
                        name: "FK_PROSPECT_REFERENCE_WEBPROSPECT_MAST_REF_EMP_SYS_ID",
                        column: x => x.REF_EMP_SYS_ID,
                        principalTable: "WEBPROSPECT_MAST",
                        principalColumn: "WEBUSER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROSPECT_TRAINING",
                columns: table => new
                {
                    TRAINING_EMP_SYSID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    TRAINING_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    TRAINING_TITLE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TRAINING_DURATION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TRAINING_INSTITUTE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TRAINING_LOCATION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROSPECT_TRAINING", x => new { x.TRAINING_EMP_SYSID, x.TRAINING_ID });
                    table.ForeignKey(
                        name: "FK_PROSPECT_TRAINING_WEBPROSPECT_MAST_TRAINING_EMP_SYSID",
                        column: x => x.TRAINING_EMP_SYSID,
                        principalTable: "WEBPROSPECT_MAST",
                        principalColumn: "WEBUSER_ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APPLICATION_QUALIFICATION");

            migrationBuilder.DropTable(
                name: "APPLICATION_TRAINING");

            migrationBuilder.DropTable(
                name: "PROSPECT_ADDRESS");

            migrationBuilder.DropTable(
                name: "PROSPECT_QUALIFICATION");

            migrationBuilder.DropTable(
                name: "PROSPECT_REFERENCE");

            migrationBuilder.DropTable(
                name: "PROSPECT_TRAINING");

            migrationBuilder.DropTable(
                name: "VACANCY_MAIN");

            migrationBuilder.DropTable(
                name: "APPLICATION_HISTORY");

            migrationBuilder.DropTable(
                name: "WEBPROSPECT_MAST");
        }
    }
}
