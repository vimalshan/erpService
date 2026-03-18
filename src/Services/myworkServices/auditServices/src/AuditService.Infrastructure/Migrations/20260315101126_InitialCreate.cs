using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUDIT_GOODPRACTICE",
                columns: table => new
                {
                    PRACTICE_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_TITLE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PRACTICE_DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRACTICE_BENEFITS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PRACTICE_REMARKS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRACTICE_PROCESS = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_UNIT = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PRACTICE_ATTACHMENT1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PRACTICE_ATTACHMENT2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_GOODPRACTICE", x => x.PRACTICE_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_MASTER",
                columns: table => new
                {
                    AUDIT_ID = table.Column<long>(type: "bigint", nullable: false),
                    AUDIT_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AUDIT_UNIT = table.Column<long>(type: "bigint", nullable: false),
                    AUDIT_FROM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AUDIT_TO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AUDIT_DEFLOCATION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AUDIT_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    AUDIT_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AUDIT_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AUDIT_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUDIT_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AUDIT_PLANYEAR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUDIT_FILE1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AUDIT_FILE2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AUDIT_FILE3 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AUDIT_PLANFROM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AUDIT_PLANTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AUDIT_COMPLETED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    AUDIT_FIRMNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AUDIT_FIELDFROM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AUDIT_FIELDTO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AUDIT_CORDID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUDIT_PROCESS = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_MASTER", x => x.AUDIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_OBSERVATIONAPP",
                columns: table => new
                {
                    APP_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    APP_OBVID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    APP_ESCSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    APP_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APP_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APP_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    APP_OBVSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    APP_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APP_REVDUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_OBSERVATIONAPP", x => x.APP_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_PROCESS_MASTER",
                columns: table => new
                {
                    AUDITPROCESS_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AUDITPROCESS_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AUDITPROCESS_CREATEDBY = table.Column<long>(type: "bigint", nullable: true),
                    AUDIT_PROCESS_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_PROCESS_MASTER", x => x.AUDITPROCESS_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_USERACCESS",
                columns: table => new
                {
                    AUC_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AUC_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AUC_BUSINESSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUC_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUC_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUC_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AUC_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUC_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_USERACCESS", x => x.AUC_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_USERMASTER",
                columns: table => new
                {
                    AUM_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AUM_LIVESTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    AUM_LASTMODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    AUM_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AUM_MAILSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    AUM_USERTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    AUM_HRMSOPTED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_USERMASTER", x => x.AUM_EMPSYSID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_YEARMASTER",
                columns: table => new
                {
                    AYM_YEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AYM_FROM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AYM_TO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AYM_LASTMODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AYM_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_YEARMASTER", x => x.AYM_YEARID);
                });

            migrationBuilder.CreateTable(
                name: "IA_HTML_EMAIL",
                columns: table => new
                {
                    OBV_ID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MFROM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MTO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MBCC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SUBJECT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MESSAGE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MSERVER = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MPORT = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ONDATE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "IAESCALATION_MAILS",
                columns: table => new
                {
                    MAIL_ID = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    MAIL_OBSERVATIONAUDITID = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    MAIL_AUDITEESYSID = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    MAIL_ESCALATOSYSID = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    MAIL_SUBJECT = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MAIL_CONTENT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MAIL_TO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MAIL_CC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MAIL_SENTBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MAIL_SENTON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IAESCALATION_MAILS", x => x.MAIL_ID);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_GOODPRACTICERATING",
                columns: table => new
                {
                    PRACTICE_RATINGID = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_RATINGBY = table.Column<long>(type: "bigint", nullable: false),
                    PRACTICE_RATING = table.Column<int>(type: "int", nullable: false),
                    PRACTICE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_GOODPRACTICERATING", x => x.PRACTICE_RATINGID);
                    table.ForeignKey(
                        name: "FK_AUDIT_GOODPRACTICERATING_AUDIT_GOODPRACTICE_PRACTICE_ID",
                        column: x => x.PRACTICE_ID,
                        principalTable: "AUDIT_GOODPRACTICE",
                        principalColumn: "PRACTICE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AUDIT_OBSERVATION",
                columns: table => new
                {
                    OBV_ID = table.Column<long>(type: "bigint", nullable: false),
                    OBV_AUDITID = table.Column<long>(type: "bigint", nullable: false),
                    OBV_TITLE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OBV_DESCRIPTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    OBV_RISK = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OBV_AUDITEE = table.Column<long>(type: "bigint", nullable: false),
                    OBV_ESC1 = table.Column<long>(type: "bigint", nullable: false),
                    OBV_ESC2 = table.Column<long>(type: "bigint", nullable: false),
                    OBV_MANCOMMENTS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    OBV_IMPLICATION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OBV_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OBV_ORGDUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OBV_ORGREV1DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OBV_ORGREV2DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OBV_DELAY1REMARKS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OBV_DELAY2REMARKS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OBV_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    OBV_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OBV_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    OBV_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OBV_COMPLETEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OBV_LOCATION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OBV_AUDITORNAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OBV_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OBV_APPSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    OBV_ENTRYSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    OBV_REPEATFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    OBV_DUPFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    OBV_PROCESS = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_OBSERVATION", x => x.OBV_ID);
                    table.ForeignKey(
                        name: "FK_AUDIT_OBSERVATION_AUDIT_MASTER_OBV_AUDITID",
                        column: x => x.OBV_AUDITID,
                        principalTable: "AUDIT_MASTER",
                        principalColumn: "AUDIT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AUDIT_GOODPRACTICERATING_PRACTICE_ID",
                table: "AUDIT_GOODPRACTICERATING",
                column: "PRACTICE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_AUDIT_OBSERVATION_OBV_AUDITID",
                table: "AUDIT_OBSERVATION",
                column: "OBV_AUDITID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_GOODPRACTICERATING");

            migrationBuilder.DropTable(
                name: "AUDIT_OBSERVATION");

            migrationBuilder.DropTable(
                name: "AUDIT_OBSERVATIONAPP");

            migrationBuilder.DropTable(
                name: "AUDIT_PROCESS_MASTER");

            migrationBuilder.DropTable(
                name: "AUDIT_USERACCESS");

            migrationBuilder.DropTable(
                name: "AUDIT_USERMASTER");

            migrationBuilder.DropTable(
                name: "AUDIT_YEARMASTER");

            migrationBuilder.DropTable(
                name: "IA_HTML_EMAIL");

            migrationBuilder.DropTable(
                name: "IAESCALATION_MAILS");

            migrationBuilder.DropTable(
                name: "AUDIT_GOODPRACTICE");

            migrationBuilder.DropTable(
                name: "AUDIT_MASTER");
        }
    }
}
