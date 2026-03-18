using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EximManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EXIM_DATA_EXPORT",
                columns: table => new
                {
                    DATA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EXIM_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HSCODE = table.Column<long>(type: "bigint", nullable: true),
                    PRODDESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PORTDEST = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    COUNTRYDEST = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PORTORIGIN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    STDQTY = table.Column<long>(type: "bigint", nullable: true),
                    STDUNIT = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    STDUNITRATE = table.Column<decimal>(type: "decimal(38,6)", nullable: true),
                    UnitRateDol = table.Column<long>(type: "bigint", nullable: true),
                    FOBINR = table.Column<long>(type: "bigint", nullable: true),
                    FOBDOL = table.Column<long>(type: "bigint", nullable: true),
                    MODESHIP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMONTH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FILE_ID = table.Column<long>(type: "bigint", nullable: true),
                    EXP_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ExpAdd1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpAdd2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IMP_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImpAdd1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpAdd2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IMP_COUNTRY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Qty = table.Column<long>(type: "bigint", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitRateInr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitRateFc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValueFc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IEC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SB_NO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    INV_NO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrawBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentQue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HS2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HS4 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InvSlNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChallanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HS_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ChaPanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INV_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_DATA_EXPORT", x => x.DATA_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXIM_DATA_IMPORT",
                columns: table => new
                {
                    DATA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EXIM_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HSCODE = table.Column<long>(type: "bigint", nullable: true),
                    PRODDESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PORTDEST = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    COUNTRYORG = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    STDQTY = table.Column<decimal>(type: "decimal(38,6)", nullable: true),
                    STDUNIT = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    STDUNITRATE = table.Column<decimal>(type: "decimal(38,6)", nullable: true),
                    UNITRATEDOL = table.Column<decimal>(type: "decimal(38,6)", nullable: true),
                    FOBINR = table.Column<decimal>(type: "decimal(38,6)", nullable: true),
                    FOBDOL = table.Column<decimal>(type: "decimal(38,6)", nullable: true),
                    ApplicableDutyInr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MODESHIP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMONTH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FILE_ID = table.Column<long>(type: "bigint", nullable: true),
                    IMP_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImpAdd1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpAdd2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpPinCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpContactPer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EXP_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ExpAdd1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QTY = table.Column<decimal>(type: "decimal(38,6)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitRateInr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPriceFc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualDutyInr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvadInr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvadUsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PortOrg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChaPanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IEC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BE_NO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InvNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HS2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HS4 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HS_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InvValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INV_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PossibleDuplicate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_DATA_IMPORT", x => x.DATA_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXIM_DATAFILE",
                columns: table => new
                {
                    FILE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FILE_TYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FILE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ORIGINALCOUNT = table.Column<long>(type: "bigint", nullable: true),
                    FINALCOUNT = table.Column<long>(type: "bigint", nullable: true),
                    FILE_UPLOADEDBY = table.Column<long>(type: "bigint", nullable: true),
                    FILE_UPLOADEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FILE_SOURCE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DEL_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DELETED_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DELETED_BY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DATATYPE_CODE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DATATYPE_MONTH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DATA_XML = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_DATAFILE", x => x.FILE_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXIM_PRODUCT",
                columns: table => new
                {
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PRODUCT_ORACLE_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    LAST_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STATUS = table.Column<string>(type: "char(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_PRODUCT", x => x.PRODUCT_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXIM_PRODUCT_SEARCH",
                columns: table => new
                {
                    SEARCH_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SEARCH_ITC_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SEARCH_TEXT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NOTIN_TEXT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    LAST_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_PRODUCT_SEARCH", x => x.SEARCH_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXIM_PRODUCTGROUP",
                columns: table => new
                {
                    GROUP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GROUP_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    LAST_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    STATUS = table.Column<string>(type: "char(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_PRODUCTGROUP", x => x.GROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXIM_PRODUCTGROUP_MAP",
                columns: table => new
                {
                    MAP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GROUP_ID = table.Column<long>(type: "bigint", nullable: false),
                    PRODUCT_ID = table.Column<long>(type: "bigint", nullable: false),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    LAST_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_PRODUCTGROUP_MAP", x => x.MAP_ID);
                });

            migrationBuilder.CreateTable(
                name: "EXIM_USERMASTER",
                columns: table => new
                {
                    EXIM_USERID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EXIM_EMPSYSID = table.Column<long>(type: "bigint", nullable: true),
                    EXIM_SPARSHID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EXIM_USER_EFFECTIVEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EXIM_USER_CLOSUREDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXIM_USER_ENTEREDBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXIM_USERMASTER", x => x.EXIM_USERID);
                });

            migrationBuilder.InsertData(
                table: "EXIM_PRODUCT",
                columns: new[] { "PRODUCT_ID", "LAST_UPDATED_BY", "LAST_UPDATED_ON", "PRODUCT_NAME", "PRODUCT_ORACLE_CODE", "STATUS" },
                values: new object[,]
                {
                    { 1001L, 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cotton Yarn", "CY001", "A" },
                    { 1002L, 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Polyester Fabric", "PF001", "A" },
                    { 1003L, 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Denim Cloth", "DC001", "A" },
                    { 1004L, 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Silk Threads", "ST001", "A" },
                    { 1005L, 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Woollen Yarn", "WY001", "A" }
                });

            migrationBuilder.InsertData(
                table: "EXIM_PRODUCTGROUP",
                columns: new[] { "GROUP_ID", "GROUP_NAME", "LAST_UPDATED_BY", "LAST_UPDATED_ON", "STATUS" },
                values: new object[,]
                {
                    { 101L, "Textile Yarns", 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A" },
                    { 102L, "Woven Fabrics", 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A" },
                    { 103L, "Denim Products", 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A" },
                    { 104L, "Silk Products", 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A" },
                    { 105L, "Woollen Products", 1L, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EXIM_DATA_EXPORT");

            migrationBuilder.DropTable(
                name: "EXIM_DATA_IMPORT");

            migrationBuilder.DropTable(
                name: "EXIM_DATAFILE");

            migrationBuilder.DropTable(
                name: "EXIM_PRODUCT");

            migrationBuilder.DropTable(
                name: "EXIM_PRODUCT_SEARCH");

            migrationBuilder.DropTable(
                name: "EXIM_PRODUCTGROUP");

            migrationBuilder.DropTable(
                name: "EXIM_PRODUCTGROUP_MAP");

            migrationBuilder.DropTable(
                name: "EXIM_USERMASTER");
        }
    }
}
