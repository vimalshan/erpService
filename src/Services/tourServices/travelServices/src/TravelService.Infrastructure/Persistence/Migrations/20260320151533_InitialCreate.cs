using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    RequestedOn = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    FORREQ_TOTVALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_RECBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FORREQ_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_ADLREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FORREQ_ADVREFNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_FOREXMAIN", x => x.FORREQ_ID);
                });

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
                    TP_FROMCOUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_FROMCOUNTRYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_TOCITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_TOCITYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_TOCOUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_TOCOUNTRYNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_SUPREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TP_CONTACTNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_GRADETYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_PAYUNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_CLAIMTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_APPREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_EXPSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TP_CLOSURESTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    TP_ACTREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOURPLAN_MAIN", x => x.TP_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_APPRDETAILS",
                columns: table => new
                {
                    TRAVEL_APRDETID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRAVEL_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRAVEL_SOURCE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRAVEL_SOURCEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRAVEL_APPROVEDSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRAVEL_APPROVERSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRAVEL_APPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TRAVEL_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TRAVEL_APPROVERTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_APPRDETAILS", x => x.TRAVEL_APRDETID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_BATCHMAIN",
                columns: table => new
                {
                    BATCH_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_ADMINID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_PAYUNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_BATCHDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_INVNUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_INVDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCH_INVAMOUNT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_STATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_ADMREMARK = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_FINREMARK = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_VENDORID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_APPAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_BILAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_SERTAX = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_CESTAX = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_ADLTAX = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_TOTPAY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_JVID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_TERM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_BILLDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCH_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_CREATEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCH_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_APPROVEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_APPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCH_FINAPPROVEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_FINAPPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCH_CABTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_DOCREFNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCH_SOURCEUID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_BATCHMAIN", x => x.BATCH_ID);
                });

            migrationBuilder.CreateTable(
                name: "ForexChequeDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ForexRequestId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForexChequeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForexChequeDetails_TOURPLAN_FOREXMAIN_ForexRequestId",
                        column: x => x.ForexRequestId,
                        principalTable: "TOURPLAN_FOREXMAIN",
                        principalColumn: "FORREQ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForexDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ForexRequestId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    SourceCurrencyValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForexValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExchangeValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayMode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForexDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForexDetails_TOURPLAN_FOREXMAIN_ForexRequestId",
                        column: x => x.ForexRequestId,
                        principalTable: "TOURPLAN_FOREXMAIN",
                        principalColumn: "FORREQ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TOURPLAN_ADVANCE",
                columns: table => new
                {
                    ADV_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_AMOUNT = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ADV_JVID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_APPSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_APPBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADV_APPON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ADV_CURRENCY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADV_RATE = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ADV_TOTAL = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
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
                name: "TourPlanAgendas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartyToMeet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesiredOutcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgendaDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanAgendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanAgendas_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPlanCostCentres",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    BusinessUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubAccountCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationSegment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllocationPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanCostCentres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanCostCentres_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPlanDaBreaks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Days = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GuestHouseDays = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GuestHouseRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanDaBreaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanDaBreaks_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPlanExpenses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ExpenseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpenseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanExpenses_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPlanIntSchedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromCityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproximateCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanIntSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanIntSchedules_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPlanLeaves",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromSession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToSession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaveType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaveDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaveId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanLeaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanLeaves_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPlanNmsSchedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    CityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TravelModeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelClassId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanNmsSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanNmsSchedules_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TourPlanSelfExpenses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TourPlanId = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ExpenseCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromCityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromCityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToCityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EntitlementValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceTaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdditionalCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TravelClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinanceRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPlanSelfExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourPlanSelfExpenses_TOURPLAN_MAIN_TourPlanId",
                        column: x => x.TourPlanId,
                        principalTable: "TOURPLAN_MAIN",
                        principalColumn: "TP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_BATCHSUB",
                columns: table => new
                {
                    BATCHSUB_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_BATCHID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_BOOKCNFID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_BOOKNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_BASAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_ADJAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_TOTAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_APPAMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_SERTAX = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_CESTAX = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_ADLTAX = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_TOTPAY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_REFDET = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_VENREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_CREDITTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BATCHSUB_ADMREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_TKTREFERENCE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_FORREQID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_INVNUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BATCHSUB_INVDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCHSUB_VENDORID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_BATCHSUB", x => x.BATCHSUB_ID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_BATCHSUB_TRAVEL_BATCHMAIN_BATCHSUB_BATCHID",
                        column: x => x.BATCHSUB_BATCHID,
                        principalTable: "TRAVEL_BATCHMAIN",
                        principalColumn: "BATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForexChequeDetails_ForexRequestId",
                table: "ForexChequeDetails",
                column: "ForexRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ForexDetails_ForexRequestId",
                table: "ForexDetails",
                column: "ForexRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TOURPLAN_ADVANCE_ADV_TPID",
                table: "TOURPLAN_ADVANCE",
                column: "ADV_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanAgendas_TourPlanId",
                table: "TourPlanAgendas",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanCostCentres_TourPlanId",
                table: "TourPlanCostCentres",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanDaBreaks_TourPlanId",
                table: "TourPlanDaBreaks",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanExpenses_TourPlanId",
                table: "TourPlanExpenses",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanIntSchedules_TourPlanId",
                table: "TourPlanIntSchedules",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanLeaves_TourPlanId",
                table: "TourPlanLeaves",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanNmsSchedules_TourPlanId",
                table: "TourPlanNmsSchedules",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TourPlanSelfExpenses_TourPlanId",
                table: "TourPlanSelfExpenses",
                column: "TourPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_BATCHSUB_BATCHSUB_BATCHID",
                table: "TRAVEL_BATCHSUB",
                column: "BATCHSUB_BATCHID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForexChequeDetails");

            migrationBuilder.DropTable(
                name: "ForexDetails");

            migrationBuilder.DropTable(
                name: "TOURPLAN_ADVANCE");

            migrationBuilder.DropTable(
                name: "TourPlanAgendas");

            migrationBuilder.DropTable(
                name: "TourPlanCostCentres");

            migrationBuilder.DropTable(
                name: "TourPlanDaBreaks");

            migrationBuilder.DropTable(
                name: "TourPlanExpenses");

            migrationBuilder.DropTable(
                name: "TourPlanIntSchedules");

            migrationBuilder.DropTable(
                name: "TourPlanLeaves");

            migrationBuilder.DropTable(
                name: "TourPlanNmsSchedules");

            migrationBuilder.DropTable(
                name: "TourPlanSelfExpenses");

            migrationBuilder.DropTable(
                name: "TRAVEL_APPRDETAILS");

            migrationBuilder.DropTable(
                name: "TRAVEL_BATCHSUB");

            migrationBuilder.DropTable(
                name: "TOURPLAN_FOREXMAIN");

            migrationBuilder.DropTable(
                name: "TOURPLAN_MAIN");

            migrationBuilder.DropTable(
                name: "TRAVEL_BATCHMAIN");
        }
    }
}
