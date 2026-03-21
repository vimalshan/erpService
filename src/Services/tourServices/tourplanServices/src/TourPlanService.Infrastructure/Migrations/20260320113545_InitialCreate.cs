using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourPlanService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TOURPLAN_MAIN",
                columns: table => new
                {
                    TP_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_STARTDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TP_ENDDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TP_PURPOSE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_STATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_CATEGORY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_BOOKINC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_CREATEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TP_APPROVEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_APPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TP_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TP_FROMCITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_FROMCITYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_TOCITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_TOCITYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_SUPREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_CONTACTNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_GRADETYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_HOMECOUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_TRAVELSECTORID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_COSTEFFECTIVE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_COSTJUSTIFY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_CLAIMTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_SPECIALREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_APPREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_APPLEVEL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_BALPAYAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_CEOEMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_DAEFFDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TP_DACLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TP_DAVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_DATOOLTIP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_EXPSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_EXPAPPROVEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_EXPAPPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TP_RECOMMENDERSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_PAYUNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_DADAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_DARATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_EXPPAYMODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_EXPJVID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_EXPSUBMITEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TP_EXPSUBMITEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_ESTIMATECONVRATE1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_ESTIMATECONVRATE2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_ACTREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_ESTIMATECONVRATE3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_CLOSURESTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_MAIN", x => x.TP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_ADVANCE",
                columns: table => new
                {
                    ADV_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_AMOUNT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_JVID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_APPSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_APPBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADV_APPON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ADV_CURRENCY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_RATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_TOTAL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ADV_APPREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ADV_FINREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ADV_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ADV_PAYMODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_ADVANCE", x => x.ADV_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_ADVANCE_TOURPLAN_MAIN_ADV_TPID",
                        column: x => x.ADV_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_AGENDA",
                columns: table => new
                {
                    AGENDA_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AGENDA_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AGENDA_CITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AGENDA_MEET = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AGENDA_OUTCOME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AGENDA_TYPE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_AGENDA", x => x.AGENDA_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_AGENDA_TOURPLAN_MAIN_AGENDA_TPID",
                        column: x => x.AGENDA_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_COSTCENTRE",
                columns: table => new
                {
                    TPCOST_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_BUCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_CCCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_SUBACCCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_PRODUCTCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_LOCSEGMENT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_ALLLPER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPCOST_DEFAULT = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_COSTCENTRE", x => x.TPCOST_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_COSTCENTRE_TOURPLAN_MAIN_TPCOST_TPID",
                        column: x => x.TPCOST_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_DABREAK",
                columns: table => new
                {
                    TPDA_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPDA_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPDA_COUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPDA_CURRENCY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPDA_DAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPDA_RATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPDA_GHDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TPDA_GHRATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_DABREAK", x => x.TPDA_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_DABREAK_TOURPLAN_MAIN_TPDA_TPID",
                        column: x => x.TPDA_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_EXPENSE",
                columns: table => new
                {
                    TPEXP_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXP_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXP_EXPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXP_CUR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXP_EXPAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXP_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_EXPENSE", x => x.TPEXP_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_EXPENSE_TOURPLAN_MAIN_TPEXP_TPID",
                        column: x => x.TPEXP_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_FOREXMAIN",
                columns: table => new
                {
                    FORREQ_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_PASSNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_PASSNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_PASSLOCATION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_PASSEXPDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FORREQ_DESTINATION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_STATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FORREQ_RECEIVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FORREQ_REFNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_TAX1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_TAX2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_TAX3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_TAX4 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_TAX5 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_VENDORID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_CURRENCY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_TOTVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_RECBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_ADLREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_ADVREFNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_NETPAY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_CURDENOADJ = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_ENCASHCERTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FORREQ_BASAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_CGSTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_SGSTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_IGSTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_CGSTCHARGES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_SGSTCHARGES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_IGSTCHARGES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_FOREXMAIN", x => x.FORREQ_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_FOREXMAIN_TOURPLAN_MAIN_FORREQ_TPID",
                        column: x => x.FORREQ_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_INTSCH",
                columns: table => new
                {
                    INTSCH_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_FROMDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INTSCH_FROMTIME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_FROMCITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_FROMCITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_FROMCOUNTRY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_TODATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INTSCH_TOTIME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_TOCITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_TOCITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_TOCOUNTRY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    INTSCH_APPROXCOST = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_INTSCH", x => x.INTSCH_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_INTSCH_TOURPLAN_MAIN_INTSCH_TPID",
                        column: x => x.INTSCH_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_LEAVE",
                columns: table => new
                {
                    LEAVE_TPLEAVEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LEAVE_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LEAVE_FROMDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LEAVE_TODATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LEAVE_FROMSESSION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LEAVE_TOSESSION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LEAVE_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LEAVE_DAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LEAVE_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LEAVE_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_LEAVE", x => x.LEAVE_TPLEAVEID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_LEAVE_TOURPLAN_MAIN_LEAVE_TPID",
                        column: x => x.LEAVE_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_NMSSCH",
                columns: table => new
                {
                    NMSSCH_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_CITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_CITYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_FROMDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NMSSCH_FROMTIME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_TODATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NMSSCH_TOTIME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_NODAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_MODEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_CLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_PURPOSE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NMSSCH_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_NMSSCH", x => x.NMSSCH_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_NMSSCH_TOURPLAN_MAIN_NMSSCH_TPID",
                        column: x => x.NMSSCH_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_SLFEXP",
                columns: table => new
                {
                    EXP_TKTID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_EXPCAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_TRAVELMODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_FROMDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EXP_FROMCITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_FROMCITYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_TODATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EXP_TOCITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_TOCITYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_NOOFDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_ENTITLEVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_VALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_SERTAXVAL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_ADLVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_TRAVELCLASS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_APPROVEDAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXP_FINREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EXP_EXPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_SLFEXP", x => x.EXP_TKTID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_SLFEXP_TOURPLAN_MAIN_EXP_TPID",
                        column: x => x.EXP_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_DOMDABREAK",
                columns: table => new
                {
                    DOMDA_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DOMDA_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_FROMDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOMDA_TODATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOMDA_DADAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_DAEFFDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOMDA_DACLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOMDA_DAACTUALDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_DARATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_LEAVEDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_FOODEXPDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_OWNSTAYTDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_FINALDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DOMDA_FINALVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_DOMDABREAK", x => x.DOMDA_ID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_DOMDABREAK_TOURPLAN_MAIN_DOMDA_TPID",
                        column: x => x.DOMDA_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID");
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_EXPENSEINTMAIN",
                columns: table => new
                {
                    TPEXPMAIN_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_CLAIMTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TPEXPMAIN_LOCCUR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TPEXPMAIN_SETDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TPEXPMAIN_APPSETDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TPEXPMAIN_INTCUR1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_INTCUR2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_INTCNV1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_INTCNV2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_INTVAL1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_INTVAL2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPMAIN_BALAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_EXPENSEINTMAIN", x => x.TPEXPMAIN_ID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_EXPENSEINTMAIN_TOURPLAN_MAIN_TPEXPMAIN_TPID",
                        column: x => x.TPEXPMAIN_TPID,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_FOREXCHQDET",
                columns: table => new
                {
                    FOREX_CHQDETID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_REQID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_CHQNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_CHQDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FOREX_BANKNAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_FOREXCHQDET", x => x.FOREX_CHQDETID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_FOREXCHQDET_TOURPLAN_FOREXMAIN_FOREX_REQID",
                        column: x => x.FOREX_REQID,
                        principalTable: "TOURPLAN_FOREXMAIN",
                        principalColumn: "FORREQ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_FOREXDET",
                columns: table => new
                {
                    FOREX_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_REQID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_SRCVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_CURRENCY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_VALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FOREX_EXGRATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FOREX_EXGVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FOREX_PAYMODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FOREX_REQCURVAL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FOREX_REQCURRECD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_FOREXDET", x => x.FOREX_ID);
                    table.ForeignKey(
                        name: "FK_TOURPLAN_FOREXDET_TOURPLAN_FOREXMAIN_FOREX_REQID",
                        column: x => x.FOREX_REQID,
                        principalTable: "TOURPLAN_FOREXMAIN",
                        principalColumn: "FORREQ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_EXPENSEINTDET",
                columns: table => new
                {
                    TPEXPDET_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPDET_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPDET_GROUPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPDET_CURRENCY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPDET_VALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPDET_ACTVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPDET_APPAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPDET_EXPFLAG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_EXPENSEINTDET", x => x.TPEXPDET_ID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_EXPENSEINTDET_TRAVEL_EXPENSEINTMAIN_TPEXPDET_TPID",
                        column: x => x.TPEXPDET_TPID,
                        principalTable: "TRAVEL_EXPENSEINTMAIN",
                        principalColumn: "TPEXPMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_EXPENSEINTBRK",
                columns: table => new
                {
                    TPEXPBRK_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPBRK_DETID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPBRK_EXPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPBRK_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TPEXPBRK_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPBRK_AMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPBRK_ACTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPBRK_APPAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TPEXPBRK_PAYMODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_EXPENSEINTBRK", x => x.TPEXPBRK_ID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_EXPENSEINTBRK_TRAVEL_EXPENSEINTDET_TPEXPBRK_DETID",
                        column: x => x.TPEXPBRK_DETID,
                        principalTable: "TRAVEL_EXPENSEINTDET",
                        principalColumn: "TPEXPDET_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_ADVANCE_ADV_TPID",
                table: "TOURPLAN_ADVANCE",
                column: "ADV_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_AGENDA_AGENDA_TPID",
                table: "TOURPLAN_AGENDA",
                column: "AGENDA_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_COSTCENTRE_TPCOST_TPID",
                table: "TOURPLAN_COSTCENTRE",
                column: "TPCOST_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_DABREAK_TPDA_TPID",
                table: "TOURPLAN_DABREAK",
                column: "TPDA_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_EXPENSE_TPEXP_TPID",
                table: "TOURPLAN_EXPENSE",
                column: "TPEXP_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_FOREXCHQDET_FOREX_REQID",
                table: "TOURPLAN_FOREXCHQDET",
                column: "FOREX_REQID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_FOREXDET_FOREX_REQID",
                table: "TOURPLAN_FOREXDET",
                column: "FOREX_REQID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_FOREXMAIN_FORREQ_TPID",
                table: "TOURPLAN_FOREXMAIN",
                column: "FORREQ_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_INTSCH_INTSCH_TPID",
                table: "TOURPLAN_INTSCH",
                column: "INTSCH_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_LEAVE_LEAVE_TPID",
                table: "TOURPLAN_LEAVE",
                column: "LEAVE_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_NMSSCH_NMSSCH_TPID",
                table: "TOURPLAN_NMSSCH",
                column: "NMSSCH_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_SLFEXP_EXP_TPID",
                table: "TOURPLAN_SLFEXP",
                column: "EXP_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_DOMDABREAK_DOMDA_TPID",
                table: "TRAVEL_DOMDABREAK",
                column: "DOMDA_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_EXPENSEINTBRK_TPEXPBRK_DETID",
                table: "TRAVEL_EXPENSEINTBRK",
                column: "TPEXPBRK_DETID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_EXPENSEINTDET_TPEXPDET_TPID",
                table: "TRAVEL_EXPENSEINTDET",
                column: "TPEXPDET_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_EXPENSEINTMAIN_TPEXPMAIN_TPID",
                table: "TRAVEL_EXPENSEINTMAIN",
                column: "TPEXPMAIN_TPID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TOURPLAN_ADVANCE");

            migrationBuilder.DropTable(
                name: "TOURPLAN_AGENDA");

            migrationBuilder.DropTable(
                name: "TOURPLAN_COSTCENTRE");

            migrationBuilder.DropTable(
                name: "TOURPLAN_DABREAK");

            migrationBuilder.DropTable(
                name: "TOURPLAN_EXPENSE");

            migrationBuilder.DropTable(
                name: "TOURPLAN_FOREXCHQDET");

            migrationBuilder.DropTable(
                name: "TOURPLAN_FOREXDET");

            migrationBuilder.DropTable(
                name: "TOURPLAN_INTSCH");

            migrationBuilder.DropTable(
                name: "TOURPLAN_LEAVE");

            migrationBuilder.DropTable(
                name: "TOURPLAN_NMSSCH");

            migrationBuilder.DropTable(
                name: "TOURPLAN_SLFEXP");

            migrationBuilder.DropTable(
                name: "TRAVEL_DOMDABREAK");

            migrationBuilder.DropTable(
                name: "TRAVEL_EXPENSEINTBRK");

            migrationBuilder.DropTable(
                name: "TOURPLAN_FOREXMAIN");

            migrationBuilder.DropTable(
                name: "TRAVEL_EXPENSEINTDET");

            migrationBuilder.DropTable(
                name: "TRAVEL_EXPENSEINTMAIN");

            migrationBuilder.DropTable(
                name: "TOURPLAN_MAIN");
        }
    }
}
