using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompensationBenefits.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BASIC_SLABINC",
                columns: table => new
                {
                    SLAB_INCID = table.Column<long>(type: "bigint", nullable: false),
                    SLAB_GRADEID = table.Column<long>(type: "bigint", nullable: false),
                    SLAB_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    SLAB_INCSTRTDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SLAB_INCCLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SLAB_INCMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SLAB_INCMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BASIC_SLABINC", x => x.SLAB_INCID);
                });

            migrationBuilder.CreateTable(
                name: "COMP_PARAMS",
                columns: table => new
                {
                    CP_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CP_COUNTRYCODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CP_EDGROUP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CP_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CP_EDID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CP_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CP_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMP_PARAMS", x => x.CP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DILIGENCE_RATEMAST",
                columns: table => new
                {
                    DILIGENCE_ID = table.Column<long>(type: "bigint", nullable: false),
                    DILIGENCE_PAYUNITID = table.Column<long>(type: "bigint", nullable: false),
                    DILIGENCE_GRADECATEGORY = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DILIGENCE_EDID = table.Column<long>(type: "bigint", nullable: false),
                    DILIGENCE_YEARID = table.Column<int>(type: "int", nullable: false),
                    DILIGENCE_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DILIGENCE_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DILIGENCE_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DILIGENCE_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    DILIGENCE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DILIGENCE_BENLOGID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DILIGENCE_RATEMAST", x => x.DILIGENCE_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMP_RETIRALS_EMPSPECIFIC",
                columns: table => new
                {
                    EMPRET_ID = table.Column<long>(type: "bigint", nullable: false),
                    EMPRET_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    EMPRET_PAYTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EMPRET_EDID = table.Column<long>(type: "bigint", nullable: false),
                    EMPRET_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMPRET_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EMPRET_PERCENTAGE = table.Column<long>(type: "bigint", nullable: false),
                    EMPRET_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EMPRET_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMPRET_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    EMPRET_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_RETIRALS_EMPSPECIFIC", x => x.EMPRET_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMP_RETIRALSDET",
                columns: table => new
                {
                    ERDET_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ERDET_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ERDET_PFCLSDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ERDET_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ERDET_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ERDET_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_RETIRALSDET", x => x.ERDET_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_CTCREMARKS",
                columns: table => new
                {
                    CTCREM_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTCREM_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTCREM_LINE1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CTCREM_LINE2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CTCREM_LINE3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CTCREM_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CTCREM_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_CTCREMARKS", x => x.CTCREM_EMP_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "MEDICLAIM_EXCEPTION",
                columns: table => new
                {
                    MEDICLAIM_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MEDICLAIM_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MEDICLAIM_MASTER",
                columns: table => new
                {
                    MEDICLAIM_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MEDICLAIM_REFNAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MEDICLAIM_PROVIDERID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MEDICLAIM_TPPID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MEDICLAIM_STARTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MEDICLAIM_CLOSEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MEDICLAIM_MAXENTRYDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MEDICLAIM_INSREFNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MEDICLAIM_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MEDICLAIM_PAIDBY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MEICLAIM_SERVICETAXPER = table.Column<long>(type: "bigint", nullable: true),
                    MEDICLAIM_COMPPAYLIMIT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MEDICLAIM_LOADINGPER = table.Column<long>(type: "bigint", nullable: true),
                    MEDICLAIM_NONCLAIMPER = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICLAIM_MASTER", x => x.MEDICLAIM_ID);
                });

            migrationBuilder.CreateTable(
                name: "MEDICLAIM_PREMPERCENTAGE",
                columns: table => new
                {
                    MED_PPID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MED_RELATIONSHIPID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MED_PERCENTAGE = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICLAIM_PREMPERCENTAGE", x => x.MED_PPID);
                });

            migrationBuilder.CreateTable(
                name: "MOBILE_ADDLIMIT",
                columns: table => new
                {
                    ADD_ID = table.Column<long>(type: "bigint", nullable: false),
                    ADD_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    ADD_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ADD_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ADD_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ADD_AMT = table.Column<long>(type: "bigint", nullable: false),
                    ADD_CALENDARID = table.Column<long>(type: "bigint", nullable: false),
                    ADD_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ADD_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ADD_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ADD_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOBILE_ADDLIMIT", x => x.ADD_ID);
                });

            migrationBuilder.CreateTable(
                name: "MOBILE_CONNECTION",
                columns: table => new
                {
                    CONN_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONN_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    CONN_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CONN_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CONN_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CONN_PHONENO = table.Column<long>(type: "bigint", nullable: false),
                    CONN_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CONN_OPENREQUESTNO = table.Column<long>(type: "bigint", nullable: true),
                    CONN_CLOSEREQUESTNO = table.Column<long>(type: "bigint", nullable: true),
                    CONN_CALENDARID = table.Column<long>(type: "bigint", nullable: false),
                    CONN_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CONN_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CONN_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    CONN_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOBILE_CONNECTION", x => x.CONN_ID);
                });

            migrationBuilder.CreateTable(
                name: "MOBILE_LIMITMAST",
                columns: table => new
                {
                    LIMIT_ID = table.Column<long>(type: "bigint", nullable: false),
                    LIMIT_ORG = table.Column<long>(type: "bigint", nullable: false),
                    LIMIT_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    LIMIT_GRADECATID = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LIMIT_GRADEID = table.Column<long>(type: "bigint", nullable: false),
                    LIMIT_ELGAMT = table.Column<long>(type: "bigint", nullable: false),
                    LIMIT_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LIMIT_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LIMIT_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LIMIT_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LIMIT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    LIMIT_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOBILE_LIMITMAST", x => x.LIMIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "PMS_CASHPAY",
                columns: table => new
                {
                    CASHPAY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CASHPAY_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CASHPAY_GRADECAT = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CASHPAY_PAYTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CASHPAY_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CASHPAY_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CASHPAY_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CASHPAY_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMS_CASHPAY", x => x.CASHPAY_ID);
                });

            migrationBuilder.CreateTable(
                name: "RETRIALS_RANGEMAST",
                columns: table => new
                {
                    RRMAST_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RRMAST_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RRMAST_FROMYEAR = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RRMAST_TOYEAR = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RRMAST_PERCENTAGE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RRMAST_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RRMAST_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RETRIALS_RANGEMAST", x => x.RRMAST_ID);
                });

            migrationBuilder.CreateTable(
                name: "SALARY_MAIN",
                columns: table => new
                {
                    SALARY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALARY_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SALARY_CTC = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALARY_STRUCTUREID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALARY_FOOTERID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALARY_COPYEMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SALARY_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALARY_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SALARY_CANCELLEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SALARY_CANCELLEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALARY_MAIN", x => x.SALARY_ID);
                });

            migrationBuilder.CreateTable(
                name: "SALSTRUCTURE_MAIN",
                columns: table => new
                {
                    STRUCTURE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    STRUCTURE_GRADECATEGORY = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    STRUCTURE_APPLYTOALL = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    STRUCTURE_CTCMIN = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_CTCMAX = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_FOOTERID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STRUCTURE_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STRUCTURE_LASTMODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTURE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STRUCTURET_APPLYTOALLUNIT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    STRUCTURE_OFFERFOOTERID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALSTRUCTURE_MAIN", x => x.STRUCTURE_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEVCTC",
                columns: table => new
                {
                    CTC_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CTC_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CTC_ED_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC_ED_FREQ = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CTC_ED_AMTPA = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC_TRANNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC_SOURCE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CTC_STRUCTUREID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CTC_FORMULA = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CTC_LOGNO = table.Column<decimal>(type: "decimal(22,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MEDICLAIM_DET",
                columns: table => new
                {
                    MED_NOMINATIONRUNID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MED_NOMINATIONID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MED_RELATIONSHIP = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MED_NOMINEENAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MED_NOMINEEDOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MED_NOMINEEAGE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MED_NOMINEEGENDER = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    MED_PREMIUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    MED_TAXSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    MED_NETPREMIUM = table.Column<long>(type: "bigint", nullable: true),
                    MED_PREMIUMSERVICETAX = table.Column<long>(type: "bigint", nullable: true),
                    MED_GROSSPREMIUM = table.Column<long>(type: "bigint", nullable: true),
                    MediclaimMasterMediclaimId = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICLAIM_DET", x => x.MED_NOMINATIONRUNID);
                    table.ForeignKey(
                        name: "FK_MEDICLAIM_DET_MEDICLAIM_MASTER_MediclaimMasterMediclaimId",
                        column: x => x.MediclaimMasterMediclaimId,
                        principalTable: "MEDICLAIM_MASTER",
                        principalColumn: "MEDICLAIM_ID");
                });

            migrationBuilder.CreateTable(
                name: "MEDICLAIM_YEARLYPREM",
                columns: table => new
                {
                    MEDYP_YEARLYPREMID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MEDYP_MEDICLAIMID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MEDYP_SUMASSURED = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MEDYP_PREMIUMAMNT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MEDYP_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MEDYP_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MEDYP_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICLAIM_YEARLYPREM", x => x.MEDYP_YEARLYPREMID);
                    table.ForeignKey(
                        name: "FK_MEDICLAIM_YEARLYPREM_MEDICLAIM_MASTER_MEDYP_MEDICLAIMID",
                        column: x => x.MEDYP_MEDICLAIMID,
                        principalTable: "MEDICLAIM_MASTER",
                        principalColumn: "MEDICLAIM_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PMS_CASHPAYDET",
                columns: table => new
                {
                    CASHPAY_DETID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CASHPAY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CASHPAY_PER = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CASHPAY_PAYDATE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMS_CASHPAYDET", x => x.CASHPAY_DETID);
                    table.ForeignKey(
                        name: "FK_PMS_CASHPAYDET_PMS_CASHPAY_CASHPAY_ID",
                        column: x => x.CASHPAY_ID,
                        principalTable: "PMS_CASHPAY",
                        principalColumn: "CASHPAY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALARY_DET",
                columns: table => new
                {
                    SALDET_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALDET_SALARYID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALDET_SRL = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALDET_ANNGROUP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SALDET_EDID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALDET_CATEGORY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SALDET_EDNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SALDET_EDAMT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SALDET_FREQUENCY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SALDET_SUPERCHAR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SALDET_SUPERDESC = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SALDET_YEARTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SALDET_GLOBALUNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SALDET_FORMULA = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SALDET_SHOWMONTHLY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SALDET_ANNEXONLY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALARY_DET", x => x.SALDET_ID);
                    table.ForeignKey(
                        name: "FK_SALARY_DET_SALARY_MAIN_SALDET_SALARYID",
                        column: x => x.SALDET_SALARYID,
                        principalTable: "SALARY_MAIN",
                        principalColumn: "SALARY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALSTRUCTURE_DET",
                columns: table => new
                {
                    STRUCTDET_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_STRUCTUREID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_EDID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_AMTTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    STRUCTDET_CALTYPE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_CATEGORY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    STRUCTDET_FREQUENCY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    STRUCTDET_EDAMT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_MINVALUE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_MAXVALUE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    STRUCTDET_GLOBALUNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    STRUCTDET_SUPERCHAR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    STRUCTDET_SUPERDESC = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    STRUCTDET_MODIFY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    STRUCTDET_FORMULA = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    STRUCTDET_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STRUCTDET_LASTMODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STRUCTDET_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STRUCTURE_SHOWMONTHLY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    STRUCTURE_ANNEXONLY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALSTRUCTURE_DET", x => x.STRUCTDET_ID);
                    table.ForeignKey(
                        name: "FK_SALSTRUCTURE_DET_SALSTRUCTURE_MAIN_STRUCTDET_STRUCTUREID",
                        column: x => x.STRUCTDET_STRUCTUREID,
                        principalTable: "SALSTRUCTURE_MAIN",
                        principalColumn: "STRUCTURE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MEDICLAIM_DET_MediclaimMasterMediclaimId",
                table: "MEDICLAIM_DET",
                column: "MediclaimMasterMediclaimId");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICLAIM_YEARLYPREM_MEDYP_MEDICLAIMID",
                table: "MEDICLAIM_YEARLYPREM",
                column: "MEDYP_MEDICLAIMID");

            migrationBuilder.CreateIndex(
                name: "IX_PMS_CASHPAYDET_CASHPAY_ID",
                table: "PMS_CASHPAYDET",
                column: "CASHPAY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SALARY_DET_SALDET_SALARYID",
                table: "SALARY_DET",
                column: "SALDET_SALARYID");

            migrationBuilder.CreateIndex(
                name: "IX_SALSTRUCTURE_DET_STRUCTDET_STRUCTUREID",
                table: "SALSTRUCTURE_DET",
                column: "STRUCTDET_STRUCTUREID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BASIC_SLABINC");

            migrationBuilder.DropTable(
                name: "COMP_PARAMS");

            migrationBuilder.DropTable(
                name: "DILIGENCE_RATEMAST");

            migrationBuilder.DropTable(
                name: "EMP_RETIRALS_EMPSPECIFIC");

            migrationBuilder.DropTable(
                name: "EMP_RETIRALSDET");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_CTCREMARKS");

            migrationBuilder.DropTable(
                name: "MEDICLAIM_DET");

            migrationBuilder.DropTable(
                name: "MEDICLAIM_EXCEPTION");

            migrationBuilder.DropTable(
                name: "MEDICLAIM_PREMPERCENTAGE");

            migrationBuilder.DropTable(
                name: "MEDICLAIM_YEARLYPREM");

            migrationBuilder.DropTable(
                name: "MOBILE_ADDLIMIT");

            migrationBuilder.DropTable(
                name: "MOBILE_CONNECTION");

            migrationBuilder.DropTable(
                name: "MOBILE_LIMITMAST");

            migrationBuilder.DropTable(
                name: "PMS_CASHPAYDET");

            migrationBuilder.DropTable(
                name: "RETRIALS_RANGEMAST");

            migrationBuilder.DropTable(
                name: "SALARY_DET");

            migrationBuilder.DropTable(
                name: "SALSTRUCTURE_DET");

            migrationBuilder.DropTable(
                name: "TEVCTC");

            migrationBuilder.DropTable(
                name: "MEDICLAIM_MASTER");

            migrationBuilder.DropTable(
                name: "PMS_CASHPAY");

            migrationBuilder.DropTable(
                name: "SALARY_MAIN");

            migrationBuilder.DropTable(
                name: "SALSTRUCTURE_MAIN");
        }
    }
}

