using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationStructureService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DEPARTMENT_MASTER",
                columns: table => new
                {
                    DEPARTMENT_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DEPARTMENT_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DEPARTMENT_LIVFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    DEPARTMENT_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    DEPARTMENT_UPDATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEPARTMENT_MASTER", x => x.DEPARTMENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "DIVISION_MASTER",
                columns: table => new
                {
                    DIVISION_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DIVISION_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DIVISION_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DIVISION_LIVEFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    DIVISION_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    DIVISION_UPDATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIVISION_MASTER", x => x.DIVISION_ID);
                });

            migrationBuilder.CreateTable(
                name: "GRADE_MASTER",
                columns: table => new
                {
                    GRADE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    GRADE_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    GRADE_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GRADE_DESIGNATION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GRADE_CATEGORYCODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    GRADE_LIVFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    GRADE_MAN_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    GRADE_PRIORITY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GRADE_SUBCAT = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    GRADE_DEFRATING = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    GRADE_PROMSCORE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GRADE_LEVELNOS = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GRADE_CADREID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADE_MASTER", x => x.GRADE_ID);
                });

            migrationBuilder.CreateTable(
                name: "HRROLE_MASTER",
                columns: table => new
                {
                    HRROLE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    HRROLE_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    HRROLE_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRROLE_MASTER", x => x.HRROLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "LEVEL_MASTER",
                columns: table => new
                {
                    LEVEL_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LEVEL_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LEVEL_DESIGNATION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LEVEL_GRADEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LEVEL_LIVEFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    LEVEL_PRIORITY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LEVEL_LASTUPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LEVEL_LASTUPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEVEL_MASTER", x => x.LEVEL_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOCATION_MASTER",
                columns: table => new
                {
                    LOCATION_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOCATION_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LOCATION_REGION_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOCATION_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    LOCATION_UPDATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCATION_MASTER", x => x.LOCATION_CODE);
                });

            migrationBuilder.CreateTable(
                name: "LOV_MASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOV_TYPE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    LOV_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LOV_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LOV_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MASTER", x => x.LOV_ID);
                });

            migrationBuilder.CreateTable(
                name: "POSITION_MASTER",
                columns: table => new
                {
                    POSITION_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    POS_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    POS_GRADE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    POSITION_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    POSITION_DESIGNATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    POS_EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    POS_CLOSED_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    REFERENCE_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    DELETED_FLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    POSITION_JD_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ENTERED_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ENTERED_PIN_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CTC = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROCESS_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    REASON_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    REPLACE_POSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    POS_MODIFIED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    POS_MODIFIED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    POS_UNIT_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    POS_REFNO = table.Column<long>(type: "bigint", nullable: true),
                    POS_EVEGRADE_ID = table.Column<long>(type: "bigint", nullable: false),
                    POSITION_EVEDESIGNATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POSITION_MASTER", x => x.POSITION_ID);
                });

            migrationBuilder.CreateTable(
                name: "REGION_MASTER",
                columns: table => new
                {
                    REGION_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    REGION_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    REGION_COUNTRY_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    REGION_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    REGION_UPDATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REGION_MASTER", x => x.REGION_CODE);
                });

            migrationBuilder.CreateTable(
                name: "SITE_MASTER",
                columns: table => new
                {
                    SITE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SITE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_SHORT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_ADDRESS_LINE_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_ADDRESS_LINE_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_ADDRESS_LINE_3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_ADDRESS_LINE_4 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_ADDRESS_PIN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SITE_CITY_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SITE_CATEGORY_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SITE_PHONE_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_PHONE_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_FAXNO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_LANDMARK_1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_LANDMARK_2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_IMAGEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_VISITORPOLICYPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_NEARESTAIRPORT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_DISTANCEAIRPORT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_NEARESTRAIL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_DISTANCERAIL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_LOCATION_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SITE_ATTACHEDEMPLOYEE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    SITE_CONTACT_NAME1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_CONTACT_PHONE1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_CONTACT_NAME2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_CONTACT_PHONE2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SITE_LIVFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    SITE_TRAVELLOCID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SITE_MASTER", x => x.SITE_ID);
                });

            migrationBuilder.CreateTable(
                name: "UNIT_MASTER",
                columns: table => new
                {
                    UNIT_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    UNIT_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UNIT_SHTNAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    UNIT_BUSINESSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    UNIT_BUSINESS_CODE = table.Column<string>(type: "nchar(9)", fixedLength: true, maxLength: 9, nullable: false),
                    UNIT_LIVFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    UNIT_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    UNIT_UPDATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    UNIT_PAYFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    UNIT_PAYLIVEFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    UNIT_ORGID = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                    UNIT_RPTFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    UNIT_REGLANGFLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    UNIT_REGLANGCODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    UNIT_PFFLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIT_MASTER", x => x.UNIT_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEPARTMENT_MASTER");

            migrationBuilder.DropTable(
                name: "DIVISION_MASTER");

            migrationBuilder.DropTable(
                name: "GRADE_MASTER");

            migrationBuilder.DropTable(
                name: "HRROLE_MASTER");

            migrationBuilder.DropTable(
                name: "LEVEL_MASTER");

            migrationBuilder.DropTable(
                name: "LOCATION_MASTER");

            migrationBuilder.DropTable(
                name: "LOV_MASTER");

            migrationBuilder.DropTable(
                name: "POSITION_MASTER");

            migrationBuilder.DropTable(
                name: "REGION_MASTER");

            migrationBuilder.DropTable(
                name: "SITE_MASTER");

            migrationBuilder.DropTable(
                name: "UNIT_MASTER");
        }
    }
}
