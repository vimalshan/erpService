using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfigService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CALENDAR_GSTBUMAP",
                columns: table => new
                {
                    CALENDAR_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CALENDAR_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CALENDAR_R12BU = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CALENDAR_GSTBUMAP", x => x.CALENDAR_ID);
                });

            migrationBuilder.CreateTable(
                name: "CURRENCY_MASTER",
                columns: table => new
                {
                    CURRENCY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CURRENCY_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CURRENCY_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    CURRENCY_SYMBOL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CURRENCY_MASTER", x => x.CURRENCY_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXPCUR_MAST",
                columns: table => new
                {
                    CUR_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CUR_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    CUR_SHTNAME = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CUR_SYMBOL = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPCUR_MAST", x => x.CUR_CODE);
                });

            migrationBuilder.CreateTable(
                name: "EXPENSEGROUP_MAST",
                columns: table => new
                {
                    EXPGROUP_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPGROUP_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPGROUP_TRAVELTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPGROUP_BREAKFLAG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSEGROUP_MAST", x => x.EXPGROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXPENSETYPE_MAST",
                columns: table => new
                {
                    EXPENSE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EXPENSE_NAME = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    EXPENSE_CATID = table.Column<int>(type: "int", nullable: false),
                    EXPENSE_TRAVELTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EXPENSE_SORTNO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSETYPE_MAST", x => x.EXPENSE_ID);
                });

            migrationBuilder.CreateTable(
                name: "GLOBALPAY_PARAMS",
                columns: table => new
                {
                    PARAM_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_CODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_VALUE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLOBALPAY_PARAMS", x => x.PARAM_ID);
                });

            migrationBuilder.CreateTable(
                name: "GRADECAT_EXPRULE",
                columns: table => new
                {
                    EXPRULE_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_GRADECAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_APPLYTOUNIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_UNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_APPLYTOGRADE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_GRADEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_EXPTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_LIMIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_DAYLIMIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_BROKENFLAG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_YPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADECAT_EXPRULE", x => x.EXPRULE_ID);
                });

            migrationBuilder.CreateTable(
                name: "GRADECAT_MODEMAP",
                columns: table => new
                {
                    MODEMAP_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_GRADECAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_APPLYTOUNIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_UNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_APPLYTOGRADE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_GRADEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_MODEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_CLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODEMAP_SPECIALSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADECAT_MODEMAP", x => x.MODEMAP_ID);
                });

            migrationBuilder.CreateTable(
                name: "GRADECAT_STAYRULE",
                columns: table => new
                {
                    STAYRULE_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_GRADECAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_APPLYTOUNIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_UNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_APPLYTOGRADE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_GRADEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_TRAVELTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_CITYCLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_LIMIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_BOOKCHARGES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_NIGHTSTAYVAL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STAYRULE_INCEXP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADECAT_STAYRULE", x => x.STAYRULE_ID);
                });

            migrationBuilder.CreateTable(
                name: "GRADECATEXP_MAP",
                columns: table => new
                {
                    EXPMAP_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPMAP_GRADECAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPMAP_APPLYTOUNIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPMAP_UNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPMAP_APPLYTOGRADE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPMAP_GRADEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPMAP_EXPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADECATEXP_MAP", x => x.EXPMAP_ID);
                });

            migrationBuilder.CreateTable(
                name: "GRADETYPETRAVEL_PARAMS",
                columns: table => new
                {
                    PARAM_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_GRADECAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_APPLYTOUNIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_UNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_ADVANCEELG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_ADVANCELIMIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_ADVANCEDAYS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_ADVANCENOS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_ADVANCEOUT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_TPAPPROVAL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PARAM_SETTIMELIMIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADETYPETRAVEL_PARAMS", x => x.PARAM_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_BUEXCLUDE",
                columns: table => new
                {
                    BU_EXID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BU_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BU_UNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_BUEXCLUDE", x => x.BU_EXID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_BUSCITYSECMAP",
                columns: table => new
                {
                    CITYBUS_MAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYBUS_CITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYBUS_CLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYBUS_BUSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_BUSCITYSECMAP", x => x.CITYBUS_MAPID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_CITYMASTER",
                columns: table => new
                {
                    CITY_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITY_COUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITY_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITY_CODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITY_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_CITYMASTER", x => x.CITY_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_CLASS",
                columns: table => new
                {
                    CLASS_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CLASS_MODEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CLASS_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CLASS_ORDER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_CLASS", x => x.CLASS_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_CONTACT",
                columns: table => new
                {
                    CONTACT_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CONTACT_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_ADMINID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_ADMNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_PHONENOS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_EMAILID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_CONTACT", x => x.CONTACT_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_COUNTRYMASTER",
                columns: table => new
                {
                    COUNTRY_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    COUNTRY_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    COUNTRY_AIRID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    COUNTRY_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    COUNTRY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COUTRY_GHAVAILABLE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    COUNTRY_GHRATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    COUNTRY_NMSGHRATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_COUNTRYMASTER", x => x.COUNTRY_ID);
                });

            migrationBuilder.CreateTable(
                name: "VENDOR_MASTER",
                columns: table => new
                {
                    VENDOR_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ACTIVE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_CODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_CONTACTPERSON = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ADDRESS1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ADDRESS2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ADDRESS3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ADDRESS4 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_PINCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_EMAILID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_CCEMAILID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_SRFTRIGGERID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_MOBILENO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_PHONENOS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_SUBTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_DIRECTMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_USERID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_GSTNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDOR_MASTER", x => x.VENDOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXPENSEGROUP_MAP",
                columns: table => new
                {
                    EXPGROUPMAP_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPGROUPMAP_GROUPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPGROUPMAP_EXPENSEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSEGROUP_MAP", x => x.EXPGROUPMAP_ID);
                    table.ForeignKey(
                        name: "FK_EXPENSEGROUP_MAP_EXPENSEGROUP_MAST_EXPGROUPMAP_GROUPID",
                        column: x => x.EXPGROUPMAP_GROUPID,
                        principalTable: "EXPENSEGROUP_MAST",
                        principalColumn: "EXPGROUP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRADECAT_EXPRULEBRK",
                columns: table => new
                {
                    EXPRULE_BRKID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_FROMHRS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_TOHRS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXPRULE_AMT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADECAT_EXPRULEBRK", x => x.EXPRULE_BRKID);
                    table.ForeignKey(
                        name: "FK_GRADECAT_EXPRULEBRK_GRADECAT_EXPRULE_EXPRULE_ID",
                        column: x => x.EXPRULE_ID,
                        principalTable: "GRADECAT_EXPRULE",
                        principalColumn: "EXPRULE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_CITYMODEMAP",
                columns: table => new
                {
                    CITYMODE_MAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYMODE_CITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYMODE_MODEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYMODE_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYMODE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_CITYMODEMAP", x => x.CITYMODE_MAPID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_CITYMODEMAP_TRAVEL_CITYMASTER_CITYMODE_CITYID",
                        column: x => x.CITYMODE_CITYID,
                        principalTable: "TRAVEL_CITYMASTER",
                        principalColumn: "CITY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_CITYSECMAP",
                columns: table => new
                {
                    CITYSEC_MAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYSEC_CITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYSEC_CLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYSEC_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CITYSEC_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CITYSEC_GRADEFCAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_CITYSECMAP", x => x.CITYSEC_MAPID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_CITYSECMAP_TRAVEL_CITYMASTER_CITYSEC_CITYID",
                        column: x => x.CITYSEC_CITYID,
                        principalTable: "TRAVEL_CITYMASTER",
                        principalColumn: "CITY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_COUNTRYCURMAP",
                columns: table => new
                {
                    CURMAP_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CURMAP_CURRENCYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CURMAP_COUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CURMAP_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CURMAP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_COUNTRYCURMAP", x => x.CURMAP_ID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_COUNTRYCURMAP_TRAVEL_COUNTRYMASTER_CURMAP_COUNTRYID",
                        column: x => x.CURMAP_COUNTRYID,
                        principalTable: "TRAVEL_COUNTRYMASTER",
                        principalColumn: "COUNTRY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_COUNTRYMODEMAP",
                columns: table => new
                {
                    MODE_MAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODE_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODE_COUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODE_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MODE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_COUNTRYMODEMAP", x => x.MODE_MAPID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_COUNTRYMODEMAP_TRAVEL_COUNTRYMASTER_MODE_COUNTRYID",
                        column: x => x.MODE_COUNTRYID,
                        principalTable: "TRAVEL_COUNTRYMASTER",
                        principalColumn: "COUNTRY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_COUNTRYSECMAP",
                columns: table => new
                {
                    SECTOR_MAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SECTOR_COUNTRYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SECTOR_CLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SECTOR_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SECTOR_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_COUNTRYSECMAP", x => x.SECTOR_MAPID);
                    table.ForeignKey(
                        name: "FK_TRAVEL_COUNTRYSECMAP_TRAVEL_COUNTRYMASTER_SECTOR_COUNTRYID",
                        column: x => x.SECTOR_COUNTRYID,
                        principalTable: "TRAVEL_COUNTRYMASTER",
                        principalColumn: "COUNTRY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VENDOR_CHARGES",
                columns: table => new
                {
                    VENDOR_CHARGESID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_RATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_TAXEFFDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VENDOR_TAXCLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VENDOR_ENTBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_ENTON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDOR_CHARGES", x => x.VENDOR_CHARGESID);
                    table.ForeignKey(
                        name: "FK_VENDOR_CHARGES_VENDOR_MASTER_VENDOR_ID",
                        column: x => x.VENDOR_ID,
                        principalTable: "VENDOR_MASTER",
                        principalColumn: "VENDOR_ID");
                });

            migrationBuilder.CreateTable(
                name: "VENDOR_TAXRATE",
                columns: table => new
                {
                    VENDOR_TAXID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VENDOR_TAXNATURE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_TAXRATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_TAXEFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VENDOR_TAXCLSDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VENDOR_ENTBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ENTON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDOR_TAXRATE", x => x.VENDOR_TAXID);
                    table.ForeignKey(
                        name: "FK_VENDOR_TAXRATE_VENDOR_MASTER_VENDOR_ID",
                        column: x => x.VENDOR_ID,
                        principalTable: "VENDOR_MASTER",
                        principalColumn: "VENDOR_ID");
                });

            migrationBuilder.CreateTable(
                name: "VENDOR_UNITMAP",
                columns: table => new
                {
                    VENDOR_UNITMAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_PAYUNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_ORASITEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VENDOR_TERMID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDOR_UNITMAP", x => x.VENDOR_UNITMAPID);
                    table.ForeignKey(
                        name: "FK_VENDOR_UNITMAP_VENDOR_MASTER_VENDOR_ID",
                        column: x => x.VENDOR_ID,
                        principalTable: "VENDOR_MASTER",
                        principalColumn: "VENDOR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSEGROUP_MAP_EXPGROUPMAP_GROUPID",
                table: "EXPENSEGROUP_MAP",
                column: "EXPGROUPMAP_GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_GRADECAT_EXPRULEBRK_EXPRULE_ID",
                table: "GRADECAT_EXPRULEBRK",
                column: "EXPRULE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_CITYMODEMAP_CITYMODE_CITYID",
                table: "TRAVEL_CITYMODEMAP",
                column: "CITYMODE_CITYID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_CITYSECMAP_CITYSEC_CITYID",
                table: "TRAVEL_CITYSECMAP",
                column: "CITYSEC_CITYID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_COUNTRYCURMAP_CURMAP_COUNTRYID",
                table: "TRAVEL_COUNTRYCURMAP",
                column: "CURMAP_COUNTRYID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_COUNTRYMODEMAP_MODE_COUNTRYID",
                table: "TRAVEL_COUNTRYMODEMAP",
                column: "MODE_COUNTRYID");

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_COUNTRYSECMAP_SECTOR_COUNTRYID",
                table: "TRAVEL_COUNTRYSECMAP",
                column: "SECTOR_COUNTRYID");

            migrationBuilder.CreateIndex(
                name: "IX_VENDOR_CHARGES_VENDOR_ID",
                table: "VENDOR_CHARGES",
                column: "VENDOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_VENDOR_TAXRATE_VENDOR_ID",
                table: "VENDOR_TAXRATE",
                column: "VENDOR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_VENDOR_UNITMAP_VENDOR_ID",
                table: "VENDOR_UNITMAP",
                column: "VENDOR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CALENDAR_GSTBUMAP");

            migrationBuilder.DropTable(
                name: "CURRENCY_MASTER");

            migrationBuilder.DropTable(
                name: "EXPCUR_MAST");

            migrationBuilder.DropTable(
                name: "EXPENSEGROUP_MAP");

            migrationBuilder.DropTable(
                name: "EXPENSETYPE_MAST");

            migrationBuilder.DropTable(
                name: "GLOBALPAY_PARAMS");

            migrationBuilder.DropTable(
                name: "GRADECAT_EXPRULEBRK");

            migrationBuilder.DropTable(
                name: "GRADECAT_MODEMAP");

            migrationBuilder.DropTable(
                name: "GRADECAT_STAYRULE");

            migrationBuilder.DropTable(
                name: "GRADECATEXP_MAP");

            migrationBuilder.DropTable(
                name: "GRADETYPETRAVEL_PARAMS");

            migrationBuilder.DropTable(
                name: "TRAVEL_BUEXCLUDE");

            migrationBuilder.DropTable(
                name: "TRAVEL_BUSCITYSECMAP");

            migrationBuilder.DropTable(
                name: "TRAVEL_CITYMODEMAP");

            migrationBuilder.DropTable(
                name: "TRAVEL_CITYSECMAP");

            migrationBuilder.DropTable(
                name: "TRAVEL_CLASS");

            migrationBuilder.DropTable(
                name: "TRAVEL_CONTACT");

            migrationBuilder.DropTable(
                name: "TRAVEL_COUNTRYCURMAP");

            migrationBuilder.DropTable(
                name: "TRAVEL_COUNTRYMODEMAP");

            migrationBuilder.DropTable(
                name: "TRAVEL_COUNTRYSECMAP");

            migrationBuilder.DropTable(
                name: "VENDOR_CHARGES");

            migrationBuilder.DropTable(
                name: "VENDOR_TAXRATE");

            migrationBuilder.DropTable(
                name: "VENDOR_UNITMAP");

            migrationBuilder.DropTable(
                name: "EXPENSEGROUP_MAST");

            migrationBuilder.DropTable(
                name: "GRADECAT_EXPRULE");

            migrationBuilder.DropTable(
                name: "TRAVEL_CITYMASTER");

            migrationBuilder.DropTable(
                name: "TRAVEL_COUNTRYMASTER");

            migrationBuilder.DropTable(
                name: "VENDOR_MASTER");
        }
    }
}
