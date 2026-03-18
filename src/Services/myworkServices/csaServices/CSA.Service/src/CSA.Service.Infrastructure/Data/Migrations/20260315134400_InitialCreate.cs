using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSA.Service.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CSA_MAIN_UPLOAD",
                columns: table => new
                {
                    CONTROL_TITLE = table.Column<string>(type: "varchar(200)", nullable: false),
                    CONTROL_DESCRIPTION = table.Column<string>(type: "varchar(2000)", nullable: true),
                    CONTROL_TYPE = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_METHOD = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_RISK = table.Column<string>(type: "varchar(2000)", nullable: true),
                    CONTROL_PRIORITY = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_PROCESS = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_SUBPROCESS = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_PERIODICITY = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_EVIDENCEFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_EVIDENCE = table.Column<string>(type: "varchar(200)", nullable: true),
                    CONTROL_APPROVERFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    SESSIONID = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CSA_MAIN_UPLOADERR",
                columns: table => new
                {
                    CONTROL_TITLE = table.Column<string>(type: "varchar(200)", nullable: false),
                    CONTROL_DESCRIPTION = table.Column<string>(type: "varchar(2000)", nullable: true),
                    CONTROL_TYPE = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_METHOD = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_RISK = table.Column<string>(type: "varchar(2000)", nullable: true),
                    CONTROL_PRIORITY = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_PROCESS = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_SUBPROCESS = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_PERIODICITY = table.Column<string>(type: "char(30)", nullable: true),
                    CONTROL_EVIDENCEFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_EVIDENCE = table.Column<string>(type: "varchar(200)", nullable: true),
                    CONTROL_APPROVERFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    ERRMSG = table.Column<string>(type: "varchar(200)", nullable: true),
                    SESSIONID = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CSA_PROCESSMAST",
                columns: table => new
                {
                    PROCESS_ID = table.Column<long>(type: "bigint", nullable: false),
                    PROCESS_NAME = table.Column<string>(type: "varchar(2000)", nullable: false),
                    PROCESS_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    PROCESS_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROCESS_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    PROCESS_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_PROCESSMAST", x => x.PROCESS_ID);
                });

            migrationBuilder.CreateTable(
                name: "CSA_RCSURVEYMAIN",
                columns: table => new
                {
                    SURVEY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SURVEY_TITLE = table.Column<string>(type: "varchar(1000)", nullable: false),
                    SURVEY_DUEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SURVEY_CLOSEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SURVEY_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SURVEY_ENDDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SURVEY_ALERT1 = table.Column<long>(type: "bigint", nullable: true),
                    SURVEY_ALERT2 = table.Column<long>(type: "bigint", nullable: true),
                    SURVEY_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SURVEY_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURVEY_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SURVEY_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_RCSURVEYMAIN", x => x.SURVEY_ID);
                });

            migrationBuilder.CreateTable(
                name: "CSA_UNITMASTER",
                columns: table => new
                {
                    UNIT_ID = table.Column<long>(type: "bigint", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "varchar(200)", nullable: false),
                    UNIT_SHTNAME = table.Column<string>(type: "varchar(200)", nullable: false),
                    UNIT_CODE = table.Column<string>(type: "char(3)", nullable: false),
                    UNIT_BUSINESSID = table.Column<long>(type: "bigint", nullable: false),
                    UNIT_LIVFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    UNIT_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    UNIT_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    UNIT_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    UNIT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    UNIT_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_UNITMASTER", x => x.UNIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "CSA_USERS",
                columns: table => new
                {
                    USER_EMPNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    USER_PINNUM = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    USER_NAME = table.Column<string>(type: "varchar(65)", nullable: true),
                    USER_SYSID = table.Column<long>(type: "bigint", nullable: true),
                    USER_EMAIL = table.Column<string>(type: "varchar(65)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CSADATA",
                columns: table => new
                {
                    TITLE = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    CONTROL_METHOD = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CONTROL_TYPE = table.Column<string>(type: "varchar(4000)", nullable: true),
                    PRIORITY = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CONTROL_DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    RISK = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    APPROVAL_REQUIRED = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CONTROLRECORD_REQUIRED = table.Column<string>(type: "varchar(4000)", nullable: true),
                    FREQUENCYOFCONTROL = table.Column<string>(type: "varchar(4000)", nullable: true),
                    PERIODICITY = table.Column<string>(type: "varchar(4000)", nullable: true),
                    PROCESS = table.Column<string>(type: "varchar(4000)", nullable: true),
                    SUB_PROCESS = table.Column<string>(type: "varchar(4000)", nullable: true),
                    CREATED = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MODIFIED = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "varchar(4000)", nullable: true),
                    ID = table.Column<long>(type: "bigint", nullable: true),
                    ITEM_TYPE = table.Column<string>(type: "varchar(4000)", nullable: true),
                    PATH = table.Column<string>(type: "varchar(4000)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CSA_SUBPROCESSMAST",
                columns: table => new
                {
                    SUBPROCESS_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUBPROCESS_PROCESSID = table.Column<long>(type: "bigint", nullable: false),
                    SUBPROCESS_NAME = table.Column<string>(type: "varchar(2000)", nullable: false),
                    SUBPROCESS_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SUBPROCESS_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SUBPROCESS_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SUBPROCESS_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_SUBPROCESSMAST", x => x.SUBPROCESS_ID);
                    table.ForeignKey(
                        name: "FK_CSA_SUBPROCESSMAST_CSA_PROCESSMAST_SUBPROCESS_PROCESSID",
                        column: x => x.SUBPROCESS_PROCESSID,
                        principalTable: "CSA_PROCESSMAST",
                        principalColumn: "PROCESS_ID");
                });

            migrationBuilder.CreateTable(
                name: "CSA_MAIN",
                columns: table => new
                {
                    CONTROL_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONTROL_TITLE = table.Column<string>(type: "varchar(200)", nullable: false),
                    CONTROL_DESCRIPTION = table.Column<string>(type: "varchar(2000)", nullable: true),
                    CONTROL_TYPE = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_METHOD = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_RISK = table.Column<string>(type: "varchar(2000)", nullable: true),
                    CONTROL_PRIORITY = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_PROCESS = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_SUBPROCESS = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_PERIODICITY = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_EVIDENCEFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_APPROVERFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    CONTROL_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CONTROL_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_MAIN", x => x.CONTROL_ID);
                    table.ForeignKey(
                        name: "FK_CSA_MAIN_CSA_PROCESSMAST_CONTROL_PROCESS",
                        column: x => x.CONTROL_PROCESS,
                        principalTable: "CSA_PROCESSMAST",
                        principalColumn: "PROCESS_ID");
                    table.ForeignKey(
                        name: "FK_CSA_MAIN_CSA_SUBPROCESSMAST_CONTROL_SUBPROCESS",
                        column: x => x.CONTROL_SUBPROCESS,
                        principalTable: "CSA_SUBPROCESSMAST",
                        principalColumn: "SUBPROCESS_ID");
                });

            migrationBuilder.CreateTable(
                name: "CSA_EVIDENCE",
                columns: table => new
                {
                    CONTROLEV_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONTROLEV_CONTROLID = table.Column<long>(type: "bigint", nullable: false),
                    CONTROLEV_NAME = table.Column<string>(type: "varchar(2000)", nullable: true),
                    CONTROLEV_TEMPNAME = table.Column<string>(type: "varchar(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_EVIDENCE", x => x.CONTROLEV_ID);
                    table.ForeignKey(
                        name: "FK_CSA_EVIDENCE_CSA_MAIN_CONTROLEV_CONTROLID",
                        column: x => x.CONTROLEV_CONTROLID,
                        principalTable: "CSA_MAIN",
                        principalColumn: "CONTROL_ID");
                });

            migrationBuilder.CreateTable(
                name: "CSA_RCSURVEYQUESTION",
                columns: table => new
                {
                    SURQ_ID = table.Column<long>(type: "bigint", nullable: false),
                    SURQ_SURVEYID = table.Column<long>(type: "bigint", nullable: false),
                    SURQ_CONTROLID = table.Column<long>(type: "bigint", nullable: false),
                    SURQ_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    SURQ_OWNERID = table.Column<long>(type: "bigint", nullable: false),
                    SURQ_APPROVERID = table.Column<long>(type: "bigint", nullable: false),
                    SURQ_ORGDUEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SURQ_DUEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SURQ_CANCELDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURQ_ASSFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    SURQ_APPFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    SURQ_REMFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    SURQ_REMDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURQ_ASSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURQ_APPDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURQ_DELAYDAYS = table.Column<long>(type: "bigint", nullable: true),
                    SURQ_REMNOS = table.Column<long>(type: "bigint", nullable: true),
                    SURQ_UNITNAME = table.Column<string>(type: "varchar(50)", nullable: true),
                    SURQ_ENTFLG = table.Column<string>(type: "char(1)", nullable: true),
                    SURQ_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SURQ_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURQ_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SURQ_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_RCSURVEYQUESTION", x => x.SURQ_ID);
                    table.ForeignKey(
                        name: "FK_CSA_RCSURVEYQUESTION_CSA_MAIN_SURQ_CONTROLID",
                        column: x => x.SURQ_CONTROLID,
                        principalTable: "CSA_MAIN",
                        principalColumn: "CONTROL_ID");
                    table.ForeignKey(
                        name: "FK_CSA_RCSURVEYQUESTION_CSA_RCSURVEYMAIN_SURQ_SURVEYID",
                        column: x => x.SURQ_SURVEYID,
                        principalTable: "CSA_RCSURVEYMAIN",
                        principalColumn: "SURVEY_ID");
                    table.ForeignKey(
                        name: "FK_CSA_RCSURVEYQUESTION_CSA_UNITMASTER_SURQ_UNITID",
                        column: x => x.SURQ_UNITID,
                        principalTable: "CSA_UNITMASTER",
                        principalColumn: "UNIT_ID");
                });

            migrationBuilder.CreateTable(
                name: "CSA_RCUNITMAPDET",
                columns: table => new
                {
                    RCMAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    RCMAP_CONTROLID = table.Column<long>(type: "bigint", nullable: false),
                    RCMAP_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    RCMAP_OWNERID = table.Column<long>(type: "bigint", nullable: false),
                    RCMAP_APPROVERID = table.Column<long>(type: "bigint", nullable: false),
                    RCMAP_REPMANAGER = table.Column<string>(type: "char(1)", nullable: false),
                    RCMAP_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RCMAP_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    RCMAP_DUEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RCMAP_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    RCMAP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    RCMAP_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    RCMAP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_RCUNITMAPDET", x => x.RCMAP_ID);
                    table.ForeignKey(
                        name: "FK_CSA_RCUNITMAPDET_CSA_MAIN_RCMAP_CONTROLID",
                        column: x => x.RCMAP_CONTROLID,
                        principalTable: "CSA_MAIN",
                        principalColumn: "CONTROL_ID");
                    table.ForeignKey(
                        name: "FK_CSA_RCUNITMAPDET_CSA_UNITMASTER_RCMAP_UNITID",
                        column: x => x.RCMAP_UNITID,
                        principalTable: "CSA_UNITMASTER",
                        principalColumn: "UNIT_ID");
                });

            migrationBuilder.CreateTable(
                name: "CSA_RCSURVEYFEED",
                columns: table => new
                {
                    SURQFEED_ID = table.Column<long>(type: "bigint", nullable: false),
                    SURQFEED_SURQID = table.Column<long>(type: "bigint", nullable: false),
                    SURQFEED_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    SURQFEED_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    SURQFEED_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    SURQFEED_REMFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    SURQFEED_REMDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURQFEED_REMARKS = table.Column<string>(type: "varchar(2000)", nullable: true),
                    SURQFEED_ENTEREDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SURQFEED_EVIDENCEFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    SURQFEED_APPFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    SURQFEED_APPREMARKS = table.Column<string>(type: "varchar(2000)", nullable: false),
                    SURQFEED_APPDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SURQFEED_APPBY = table.Column<long>(type: "bigint", nullable: true),
                    SURQFEED_ENTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_RCSURVEYFEED", x => x.SURQFEED_ID);
                    table.ForeignKey(
                        name: "FK_CSA_RCSURVEYFEED_CSA_RCSURVEYQUESTION_SURQFEED_SURQID",
                        column: x => x.SURQFEED_SURQID,
                        principalTable: "CSA_RCSURVEYQUESTION",
                        principalColumn: "SURQ_ID");
                });

            migrationBuilder.CreateTable(
                name: "CSA_RCSURVEYATTACHMENT",
                columns: table => new
                {
                    SURQATT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SURQATT_FEEDID = table.Column<long>(type: "bigint", nullable: false),
                    SURQATT_CONTROLEVID = table.Column<long>(type: "bigint", nullable: false),
                    SURQATT_ATTACHMENT = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSA_RCSURVEYATTACHMENT", x => x.SURQATT_ID);
                    table.ForeignKey(
                        name: "FK_CSA_RCSURVEYATTACHMENT_CSA_EVIDENCE_SURQATT_CONTROLEVID",
                        column: x => x.SURQATT_CONTROLEVID,
                        principalTable: "CSA_EVIDENCE",
                        principalColumn: "CONTROLEV_ID");
                    table.ForeignKey(
                        name: "FK_CSA_RCSURVEYATTACHMENT_CSA_RCSURVEYFEED_SURQATT_FEEDID",
                        column: x => x.SURQATT_FEEDID,
                        principalTable: "CSA_RCSURVEYFEED",
                        principalColumn: "SURQFEED_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_EVIDENCE_CONTROLEV_CONTROLID",
                table: "CSA_EVIDENCE",
                column: "CONTROLEV_CONTROLID");

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_MAIN_CONTROL_TITLE",
                table: "CSA_MAIN",
                column: "CONTROL_TITLE");

            migrationBuilder.CreateIndex(
                name: "IX_CSA_MAIN_CONTROL_PROCESS",
                table: "CSA_MAIN",
                column: "CONTROL_PROCESS");

            migrationBuilder.CreateIndex(
                name: "IX_CSA_MAIN_CONTROL_SUBPROCESS",
                table: "CSA_MAIN",
                column: "CONTROL_SUBPROCESS");

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_RCSURVEYATTACHMENT_SURQATT_CONTROLEVID",
                table: "CSA_RCSURVEYATTACHMENT",
                column: "SURQATT_CONTROLEVID");

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_RCSURVEYATTACHMENT_SURQATT_FEEDID",
                table: "CSA_RCSURVEYATTACHMENT",
                column: "SURQATT_FEEDID");

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_RCSURVEYFEED_SURQFEED_SURQID",
                table: "CSA_RCSURVEYFEED",
                column: "SURQFEED_SURQID");

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_RCSURVEYQUESTION_SURQ_SURVEYID",
                table: "CSA_RCSURVEYQUESTION",
                column: "SURQ_SURVEYID");

            migrationBuilder.CreateIndex(
                name: "IX_CSA_RCSURVEYQUESTION_SURQ_CONTROLID",
                table: "CSA_RCSURVEYQUESTION",
                column: "SURQ_CONTROLID");

            migrationBuilder.CreateIndex(
                name: "IX_CSA_RCSURVEYQUESTION_SURQ_UNITID",
                table: "CSA_RCSURVEYQUESTION",
                column: "SURQ_UNITID");

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_RCUNITMAPDET_RCMAP_CONTROLID",
                table: "CSA_RCUNITMAPDET",
                column: "RCMAP_CONTROLID");

            migrationBuilder.CreateIndex(
                name: "IX_CSA_RCUNITMAPDET_RCMAP_UNITID",
                table: "CSA_RCUNITMAPDET",
                column: "RCMAP_UNITID");

            migrationBuilder.CreateIndex(
                name: "IDX_CSA_SUBPROCESSMAST_SUBPROCESS_PROCESSID",
                table: "CSA_SUBPROCESSMAST",
                column: "SUBPROCESS_PROCESSID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CSA_MAIN_UPLOAD");

            migrationBuilder.DropTable(
                name: "CSA_MAIN_UPLOADERR");

            migrationBuilder.DropTable(
                name: "CSA_RCSURVEYATTACHMENT");

            migrationBuilder.DropTable(
                name: "CSA_RCUNITMAPDET");

            migrationBuilder.DropTable(
                name: "CSA_USERS");

            migrationBuilder.DropTable(
                name: "CSADATA");

            migrationBuilder.DropTable(
                name: "CSA_EVIDENCE");

            migrationBuilder.DropTable(
                name: "CSA_RCSURVEYFEED");

            migrationBuilder.DropTable(
                name: "CSA_RCSURVEYQUESTION");

            migrationBuilder.DropTable(
                name: "CSA_MAIN");

            migrationBuilder.DropTable(
                name: "CSA_RCSURVEYMAIN");

            migrationBuilder.DropTable(
                name: "CSA_UNITMASTER");

            migrationBuilder.DropTable(
                name: "CSA_SUBPROCESSMAST");

            migrationBuilder.DropTable(
                name: "CSA_PROCESSMAST");
        }
    }
}
