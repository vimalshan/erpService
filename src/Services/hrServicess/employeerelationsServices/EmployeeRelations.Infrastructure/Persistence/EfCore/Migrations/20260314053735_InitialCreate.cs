using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeRelations.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DISCIPLINARY_MAIN",
                columns: table => new
                {
                    DISCIPLINE_MAINID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISCIPLINE_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISCIPLINE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DISCIPLINE_DETAILS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DISCIPLINE_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISCIPLINE_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DISCIPLINE_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DISCIPLINE_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISCIPLINARY_MAIN", x => x.DISCIPLINE_MAINID);
                });

            migrationBuilder.CreateTable(
                name: "EWS_MAIN",
                columns: table => new
                {
                    EWS_ID = table.Column<long>(type: "bigint", nullable: false),
                    EWS_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    EWS_PERIODNO = table.Column<int>(type: "int", nullable: false),
                    EWS_HRENTRYBY = table.Column<long>(type: "bigint", nullable: true),
                    EWS_HRENTRYDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EWS_HRFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    EWS_EES = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_PULSE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_DD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_IJP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_COMP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_LEAVE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_FINAL = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_HRREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EWS_CHRFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_CHRREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EWS_APRFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_APRREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EWS_REOPEN = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EWS_REOPENBY = table.Column<long>(type: "bigint", nullable: true),
                    EWS_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EWS_CTC = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    EWS_APRSCORE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EWS_MAIN", x => x.EWS_ID);
                });

            migrationBuilder.CreateTable(
                name: "SURVEY_MASTER",
                columns: table => new
                {
                    SURVEY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SURVEY_IMAGE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SURVEY_STARTDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SURVEY_ENDDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SURVEY_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SURVEY_AUTOLOCK = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SURVEY_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SURVEY_TEMPLATEID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SURVEY_MASTER", x => x.SURVEY_ID);
                });

            migrationBuilder.CreateTable(
                name: "DISCIPLINARY_ACTION",
                columns: table => new
                {
                    DISACTION_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISACTION_MAINID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISACTION_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISACTION_TYPEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISACTION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DISACTION_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DISACTION_DOC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DISACTION_ENTRYSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DISACTION_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISACTION_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DISACTION_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DISACTION_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DISACTION_APPROVEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DISACTION_APPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DISACTION_RETURNREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISCIPLINARY_ACTION", x => x.DISACTION_ID);
                    table.ForeignKey(
                        name: "FK_DISCIPLINARY_ACTION_DISCIPLINARY_MAIN_DISACTION_MAINID",
                        column: x => x.DISACTION_MAINID,
                        principalTable: "DISCIPLINARY_MAIN",
                        principalColumn: "DISCIPLINE_MAINID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DISCIPLINARY_EMP",
                columns: table => new
                {
                    DISEMP_MAINID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISEMP_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DISEMP_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DISEMP_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISCIPLINARY_EMP", x => new { x.DISEMP_MAINID, x.DISEMP_EMPSYSID });
                    table.ForeignKey(
                        name: "FK_DISCIPLINARY_EMP_DISCIPLINARY_MAIN_DISEMP_MAINID",
                        column: x => x.DISEMP_MAINID,
                        principalTable: "DISCIPLINARY_MAIN",
                        principalColumn: "DISCIPLINE_MAINID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EWS_APPINPUTS",
                columns: table => new
                {
                    APP_INPUTID = table.Column<long>(type: "bigint", nullable: false),
                    APP_EWSID = table.Column<long>(type: "bigint", nullable: false),
                    APP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    APP_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APP_ENTEREDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    APP_ENGLEVEL = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    APP_LEAVEFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    APP_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    APRR_REOPEN = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EWS_APPINPUTS", x => x.APP_INPUTID);
                    table.ForeignKey(
                        name: "FK_EWS_APPINPUTS_EWS_MAIN_APP_EWSID",
                        column: x => x.APP_EWSID,
                        principalTable: "EWS_MAIN",
                        principalColumn: "EWS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SURVEY_QUESTIONS",
                columns: table => new
                {
                    SURVEY_QUESTID = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_QUESTNAME = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SURVEY_QUESTTYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SURVEY_MAXOPTLIMIT = table.Column<int>(type: "int", nullable: true),
                    SURVEY_SECTIONID = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_MANDATORY = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SURVEY_SORT = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_MINOPTLIMIT = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SURVEY_QUESTIONS", x => x.SURVEY_QUESTID);
                    table.ForeignKey(
                        name: "FK_SURVEY_QUESTIONS_SURVEY_MASTER_SURVEY_ID",
                        column: x => x.SURVEY_ID,
                        principalTable: "SURVEY_MASTER",
                        principalColumn: "SURVEY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SURVEY_RESPONSEMAIN",
                columns: table => new
                {
                    RESPONSE_ID = table.Column<long>(type: "bigint", nullable: false),
                    RESPONSE_SURVEYID = table.Column<long>(type: "bigint", nullable: false),
                    RESPONSE_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    RESPONSE_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RESPONSE_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RESPONSE_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    RESPONSE_SKIP = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SURVEY_RESPONSEMAIN", x => x.RESPONSE_ID);
                    table.ForeignKey(
                        name: "FK_SURVEY_RESPONSEMAIN_SURVEY_MASTER_RESPONSE_SURVEYID",
                        column: x => x.RESPONSE_SURVEYID,
                        principalTable: "SURVEY_MASTER",
                        principalColumn: "SURVEY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SURVEY_OPTIONS",
                columns: table => new
                {
                    SURVEY_OPTIONID = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_QUESTIONID = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SURVEY_OPTIONS", x => x.SURVEY_OPTIONID);
                    table.ForeignKey(
                        name: "FK_SURVEY_OPTIONS_SURVEY_QUESTIONS_SURVEY_QUESTIONID",
                        column: x => x.SURVEY_QUESTIONID,
                        principalTable: "SURVEY_QUESTIONS",
                        principalColumn: "SURVEY_QUESTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SURVEY_RESPONSEDET",
                columns: table => new
                {
                    RESPONSE_QUESTIONID = table.Column<long>(type: "bigint", nullable: false),
                    RESPONSE_ID = table.Column<long>(type: "bigint", nullable: false),
                    RESPONSE_OPTION = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RESPONSE_TEXT = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SURVEY_RESPONSEDET", x => new { x.RESPONSE_ID, x.RESPONSE_QUESTIONID });
                    table.ForeignKey(
                        name: "FK_SURVEY_RESPONSEDET_SURVEY_RESPONSEMAIN_RESPONSE_ID",
                        column: x => x.RESPONSE_ID,
                        principalTable: "SURVEY_RESPONSEMAIN",
                        principalColumn: "RESPONSE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DISCIPLINARY_ACTION_DISACTION_MAINID",
                table: "DISCIPLINARY_ACTION",
                column: "DISACTION_MAINID");

            migrationBuilder.CreateIndex(
                name: "IX_EWS_APPINPUTS_APP_EWSID",
                table: "EWS_APPINPUTS",
                column: "APP_EWSID");

            migrationBuilder.CreateIndex(
                name: "IX_SURVEY_OPTIONS_SURVEY_QUESTIONID",
                table: "SURVEY_OPTIONS",
                column: "SURVEY_QUESTIONID");

            migrationBuilder.CreateIndex(
                name: "IX_SURVEY_QUESTIONS_SURVEY_ID",
                table: "SURVEY_QUESTIONS",
                column: "SURVEY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SURVEY_RESPONSEMAIN_RESPONSE_SURVEYID",
                table: "SURVEY_RESPONSEMAIN",
                column: "RESPONSE_SURVEYID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DISCIPLINARY_ACTION");

            migrationBuilder.DropTable(
                name: "DISCIPLINARY_EMP");

            migrationBuilder.DropTable(
                name: "EWS_APPINPUTS");

            migrationBuilder.DropTable(
                name: "SURVEY_OPTIONS");

            migrationBuilder.DropTable(
                name: "SURVEY_RESPONSEDET");

            migrationBuilder.DropTable(
                name: "DISCIPLINARY_MAIN");

            migrationBuilder.DropTable(
                name: "EWS_MAIN");

            migrationBuilder.DropTable(
                name: "SURVEY_QUESTIONS");

            migrationBuilder.DropTable(
                name: "SURVEY_RESPONSEMAIN");

            migrationBuilder.DropTable(
                name: "SURVEY_MASTER");
        }
    }
}
