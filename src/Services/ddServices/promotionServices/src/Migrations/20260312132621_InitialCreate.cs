using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromotionService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DD_APPRAISALAMOUNT",
                columns: table => new
                {
                    DD_SRL_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DD_BND_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_BND_APR = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DD_BND_AMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_BND_MAX = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_BND_MIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_BND_EFF = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_BND_END = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_BND_PER = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_MIN_CTC = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_MIN_PER = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_GRADECODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DD_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_APPRAISALAMOUNT", x => x.DD_SRL_NO);
                });

            migrationBuilder.CreateTable(
                name: "DD_CTGPROMOTION",
                columns: table => new
                {
                    DD_REQ_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DD_APPRSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_QTNNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APPTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DD_ANS1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_ANS2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_LEVELID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_NEWGRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_LASTUPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_LASTUPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_PROMO_REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_CTGPROMOTION", x => x.DD_REQ_NUM);
                });

            migrationBuilder.CreateTable(
                name: "DD_GRADE_INCTYPE",
                columns: table => new
                {
                    DD_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_FORMCAT = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DD_YEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_INCTYPE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DD_GRADECODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DD_PROBRATING = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DD_VPPER = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_HPPER = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_HORIZONTAL",
                columns: table => new
                {
                    PROMOTION_TRANID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROMOTION_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_SCORE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_GRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_CURLEVELID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_NEWLEVELID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_EFFFROM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PROMOTION_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PROMOTION_POSITIONID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_OLDPOSNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROMOTION_OLDPOSDESG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROMOTION_NEWPOSNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROMOTION_NEWPOSDESG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROMOTION_POSUPDATEBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROMOTION_POSUPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PROMOTION_CONFIRM_HRMS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_HORIZONTAL", x => x.PROMOTION_TRANID);
                });

            migrationBuilder.CreateTable(
                name: "DD_HORIZONTAL_POSITION",
                columns: table => new
                {
                    PROMOTION_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROMOTION_DDYEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROMOTION_POSITIONID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROMOTION_OLDPOSNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PROMOTION_OLDPOSDESG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PROMOTION_NEWPOSNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PROMOTION_NEWPOSDESG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PROMOTION_POSUPDATEBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROMOTION_POSUPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROMOTION_CONFIRM_HRMS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_HORIZONTAL_POSITION", x => new { x.PROMOTION_EMPSYSID, x.PROMOTION_DDYEARID, x.PROMOTION_POSITIONID });
                });

            migrationBuilder.CreateTable(
                name: "DD_INCDIRECT",
                columns: table => new
                {
                    DDINC_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DDINC_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DDINC_YEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DDINC_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DDINC_SALTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DDINC_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DDINC_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DDINC_RATAMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DDINC_PROMAMNT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DDINC_PER = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_INCDIRECT", x => x.DDINC_ID);
                });

            migrationBuilder.CreateTable(
                name: "DD_PERFORMANCERATING",
                columns: table => new
                {
                    PER_REQNUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PER_RATING = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PER_PIN_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PER_COMMENTS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PER_RATING1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PER_USERID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PER_SRLNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PER_MEAN_RATING = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PER_MEAN_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PER_ACH_RATING = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PER_RESULT_AVG = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PER_APPROACH_AVG = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_PROMOTIONLETTER",
                columns: table => new
                {
                    DD_PIN_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_CRT_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_SIG_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_SIG_DSG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_APR_BUS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_APR_PR1 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR2 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR3 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR4 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR5 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR6 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_PRN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_APR_SIN = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_APR_DSG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_APR_BND = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_APR_INC = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_PAY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_FLX = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_PROMOTIONPERIOD",
                columns: table => new
                {
                    DD_PRM_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_PRD_DSC = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_RATING",
                columns: table => new
                {
                    DD_RAT_FROM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_RAT_TO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_RAT_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_RAT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DD_RAT_FIN = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DD_RAT_PRO = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DD_RAT_REQ = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_RAT_CHR = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DD_BND_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_BAS_AMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_CTC_AMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_PRM_FLG = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_SPL_SKL = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_PRM_BND = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    NEW_PROMO_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CASH_LEVEL = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CASH_AMOUNT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CASH_REASON = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CSH_OUTCOME = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    DD_BLT_PER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_BLT_COMP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_CLT_PER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_CLT_COMP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_RAT_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    NEW_CASH_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DD_POSITIONID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_PRMHORLEVELID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_PAYROLL = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DD_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_REQNUM_COMPE_INDPROM",
                columns: table => new
                {
                    REQNUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    COMPNUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    INDNUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PINNUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_SUBLEVEL_INC",
                columns: table => new
                {
                    SLINC_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SLINC_YEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SLINC_ENDDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SLINC_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SLINC_LEVELID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SLINC_RATING = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SLINC_RATEAMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SLINC_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SLINC_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SLINC_MINAMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SLINC_MAXAMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_VTCCORRECTION",
                columns: table => new
                {
                    VTC_RATEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    VTC_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    VTC_FINYEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    VTC_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VTC_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    VTC_OLDRATING = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VTC_NEWRATING = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VTC_OLDCASH = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    VTC_NEWCASH = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    VTC_OLDPROMO = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VTC_NEWPROMO = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VTC_OLDRATIONAL = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VTC_NEWRATIONAL = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VTC_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VTC_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    VTC_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VTC_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    VTC_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VTC_APPROVEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    VTC_APPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_VTCCORRECTION", x => x.VTC_RATEID);
                });

            migrationBuilder.CreateTable(
                name: "DD_VTCDETERREM",
                columns: table => new
                {
                    DE_BND_NAM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DE_VAL_NAM = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DD_VAL_DSC = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_FIN_YEAR = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_VTCINCLIST",
                columns: table => new
                {
                    VTC_DDYEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    VTC_REQ_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    VTC_DDTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VTC_SALTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VTC_REQ_USERID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    VTC_REQ_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    VTC_REQ_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UnitId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BusinessId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GradeId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GradeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LevelId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GroupDoj = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentCtc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RatingReviewDD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatingBlt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatingClt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BandId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RatingBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncrementAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PreHorizontalPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RatingPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewHorizontalPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HorizontalPromotionEligible = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HorizontalPromotionBlt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerticalPromotionBlt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromotionBand = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromotionIncrementAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RatifyFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatifyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PinNo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Business = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisedCtc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentIncrease = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OldPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OldRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceMonths = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MyPromotion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProbationFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastVerticalPromoDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IncrementType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SameAppraisalReview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoReviewDD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoLevelType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoLevelId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromoGradeId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromoLevelTypeBlt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoLevelIdBlt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromoSubLevelIdBlt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DDPayroll = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogEmployeeSystemId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LogUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogRunOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NewGrade2017 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewGradeId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncrementPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConfirmPayFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmPayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VerticalPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HorizontalPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrgId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BusId = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    RatingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeSystemId = table.Column<long>(type: "bigint", nullable: false),
                    DDYear = table.Column<int>(type: "int", nullable: false),
                    AppraisalScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompetencyScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GoalCompletionScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RatingGrade = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RatingCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.RatingId);
                });

            migrationBuilder.CreateTable(
                name: "VTCAssessments",
                columns: table => new
                {
                    VTCAssessmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeSystemId = table.Column<long>(type: "bigint", nullable: false),
                    DDYear = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssessedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VTCAssessments", x => x.VTCAssessmentId);
                });

            migrationBuilder.CreateTable(
                name: "IncrementRequests",
                columns: table => new
                {
                    IncrementId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeSystemId = table.Column<long>(type: "bigint", nullable: false),
                    IncrementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentBaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProposedBaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncrementAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncrementPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncrementReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveFromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBySystemId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncrementRequests", x => x.IncrementId);
                    table.ForeignKey(
                        name: "FK_IncrementRequests_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "RatingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionRecommendations",
                columns: table => new
                {
                    PromotionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeSystemId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentDesignation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProposedDesignation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProposedGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromotionEffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProposedSalaryIncrease = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PromotionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBySystemId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionRecommendations", x => x.PromotionId);
                    table.ForeignKey(
                        name: "FK_PromotionRecommendations_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "RatingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncrementRequests_EmployeeSystemId",
                table: "IncrementRequests",
                column: "EmployeeSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_IncrementRequests_RatingId",
                table: "IncrementRequests",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_IncrementRequests_Status",
                table: "IncrementRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionRecommendations_EmployeeSystemId",
                table: "PromotionRecommendations",
                column: "EmployeeSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionRecommendations_RatingId",
                table: "PromotionRecommendations",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionRecommendations_Status",
                table: "PromotionRecommendations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_EmployeeSystemId_DDYear",
                table: "Ratings",
                columns: new[] { "EmployeeSystemId", "DDYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RatingGrade",
                table: "Ratings",
                column: "RatingGrade");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_Status",
                table: "Ratings",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DD_APPRAISALAMOUNT");

            migrationBuilder.DropTable(
                name: "DD_CTGPROMOTION");

            migrationBuilder.DropTable(
                name: "DD_GRADE_INCTYPE");

            migrationBuilder.DropTable(
                name: "DD_HORIZONTAL");

            migrationBuilder.DropTable(
                name: "DD_HORIZONTAL_POSITION");

            migrationBuilder.DropTable(
                name: "DD_INCDIRECT");

            migrationBuilder.DropTable(
                name: "DD_PERFORMANCERATING");

            migrationBuilder.DropTable(
                name: "DD_PROMOTIONLETTER");

            migrationBuilder.DropTable(
                name: "DD_PROMOTIONPERIOD");

            migrationBuilder.DropTable(
                name: "DD_RATING");

            migrationBuilder.DropTable(
                name: "DD_REQNUM_COMPE_INDPROM");

            migrationBuilder.DropTable(
                name: "DD_SUBLEVEL_INC");

            migrationBuilder.DropTable(
                name: "DD_VTCCORRECTION");

            migrationBuilder.DropTable(
                name: "DD_VTCDETERREM");

            migrationBuilder.DropTable(
                name: "DD_VTCINCLIST");

            migrationBuilder.DropTable(
                name: "IncrementRequests");

            migrationBuilder.DropTable(
                name: "PromotionRecommendations");

            migrationBuilder.DropTable(
                name: "VTCAssessments");

            migrationBuilder.DropTable(
                name: "Ratings");
        }
    }
}
