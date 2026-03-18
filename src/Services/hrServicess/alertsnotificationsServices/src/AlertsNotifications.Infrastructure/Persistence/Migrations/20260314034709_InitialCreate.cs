using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlertsNotifications.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ALERT_MASTER",
                columns: table => new
                {
                    ALERT_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALERT_APPS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ALERT_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ALERT_TYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ALERT_DESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ALERT_TODESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALERT_CCDESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALERT_GRADECAT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ALERT_UNITSPECIFIC = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERT_MASTER", x => x.ALERT_ID);
                });

            migrationBuilder.CreateTable(
                name: "ALERTGRP_MASTER",
                columns: table => new
                {
                    ALGRP_ID = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                    ALGRP_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ALGRP_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    ALGRP_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALGRP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALGRP_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ALGRP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTGRP_MASTER", x => x.ALGRP_ID);
                });

            migrationBuilder.CreateTable(
                name: "CIRCULAR_TEMPLATE",
                columns: table => new
                {
                    CIRTEMPLATE_ID = table.Column<long>(type: "bigint", nullable: false),
                    CIRTEMPLATE_APPLYTOUNIT = table.Column<long>(type: "bigint", nullable: false),
                    CIRTEMPLATE_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    CIRTEMPLATE_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    CIRTEMPLATE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CIRTEMPLATE_HTML = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CIRTEMPLATE_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CIRTEMPLATE_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CIRTEMPLATE_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CIRCULAR_TEMPLATE", x => x.CIRTEMPLATE_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROBCONFALERT",
                columns: table => new
                {
                    PROBATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    PROBATION_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    PROBATION_GRADE = table.Column<long>(type: "bigint", nullable: false),
                    PROBATION_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SELFAPPRAISAL = table.Column<string>(type: "char(1)", nullable: true),
                    ALERT_SENTON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROBCONFALERT", x => x.PROBATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "CIRCULAR_LIST",
                columns: table => new
                {
                    CIRCULAR_ID = table.Column<long>(type: "bigint", nullable: false),
                    CIRCULAR_NO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CIRCULAR_YEARID = table.Column<int>(type: "int", nullable: false),
                    CIRCULAR_TYPE = table.Column<long>(type: "bigint", nullable: false),
                    CIRCULAR_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    CIRCULAR_BUSPECIFIC = table.Column<int>(type: "int", nullable: false),
                    CIRCULAR_UNITSPECIFIC = table.Column<int>(type: "int", nullable: false),
                    CIRCULAR_HRROLEID = table.Column<int>(type: "int", nullable: true),
                    CIRCULAR_VERSIONNO = table.Column<int>(type: "int", nullable: false),
                    CIRCULAR_TEMPLATEID = table.Column<long>(type: "bigint", nullable: true),
                    CIRCULAR_PDFFILENAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CIRCULAR_RTF = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CIRCULAR_SIGNATORYID = table.Column<long>(type: "bigint", nullable: false),
                    CIRCULAR_SPARSHFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    CIRCULAR_POSTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CIRCULAR_REMOVEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CIRCULAR_DESC = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CIRCULAR_SUBJECT = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CIRCULAR_TOLIST = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CIRCULAR_CCLIST = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CIRCULAR_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CIRCULAR_ATTACHEMPFLAG = table.Column<string>(type: "char(1)", nullable: true),
                    CIRCULAR_APPROVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    CIRCULAR_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CIRCULAR_APPREMARKS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CIRCULAR_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CIRCULAR_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CIRCULAR_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CIRCULAR_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CIRCULAR_LIST", x => x.CIRCULAR_ID);
                    table.ForeignKey(
                        name: "FK_CIRCULAR_LIST_CIRCULAR_TEMPLATE_CIRCULAR_TEMPLATEID",
                        column: x => x.CIRCULAR_TEMPLATEID,
                        principalTable: "CIRCULAR_TEMPLATE",
                        principalColumn: "CIRTEMPLATE_ID");
                });

            migrationBuilder.CreateTable(
                name: "CIRCULAR_SIGNATORY",
                columns: table => new
                {
                    CIRSIGNATORY_ID = table.Column<long>(type: "bigint", nullable: false),
                    CIRSIGNATORY_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    CIRSIGNATORY_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    CIRSIGNATORY_SIGNID = table.Column<long>(type: "bigint", nullable: false),
                    CIRSIGNATORY_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CIRSIGNATORY_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CIRSIGNATORY_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CIRCULAR_SIGNATORY", x => x.CIRSIGNATORY_ID);
                    table.ForeignKey(
                        name: "FK_CIRCULAR_SIGNATORY_CIRCULAR_LIST_CIRSIGNATORY_ID",
                        column: x => x.CIRSIGNATORY_ID,
                        principalTable: "CIRCULAR_LIST",
                        principalColumn: "CIRCULAR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CIRCULAR_LIST_CIRCULAR_TEMPLATEID",
                table: "CIRCULAR_LIST",
                column: "CIRCULAR_TEMPLATEID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ALERT_MASTER");

            migrationBuilder.DropTable(
                name: "ALERTGRP_MASTER");

            migrationBuilder.DropTable(
                name: "CIRCULAR_SIGNATORY");

            migrationBuilder.DropTable(
                name: "PROBCONFALERT");

            migrationBuilder.DropTable(
                name: "CIRCULAR_LIST");

            migrationBuilder.DropTable(
                name: "CIRCULAR_TEMPLATE");
        }
    }
}
