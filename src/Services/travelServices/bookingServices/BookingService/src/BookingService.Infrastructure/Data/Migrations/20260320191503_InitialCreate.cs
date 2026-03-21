using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "BOOK_CONFIRMATION",
                schema: "dbo",
                columns: table => new
                {
                    BK_CNF_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BK_CNF_SRL = table.Column<long>(type: "bigint", nullable: false),
                    BK_BOK_NUM = table.Column<long>(type: "bigint", nullable: true),
                    BK_SRL_NUM = table.Column<long>(type: "bigint", nullable: true),
                    BK_REQ_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_FRO_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_TO_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_FRO_CIT = table.Column<long>(type: "bigint", nullable: true),
                    BK_TO_CIT = table.Column<long>(type: "bigint", nullable: true),
                    BK_MOD_COD = table.Column<long>(type: "bigint", nullable: false),
                    BK_FRM_LOC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_TO_LOC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_AIR_LIN = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    BK_TRL_NUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BK_TRL_NAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_ADM_RMK = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BK_TRL_CLS = table.Column<long>(type: "bigint", nullable: true),
                    BK_VND_COD = table.Column<long>(type: "bigint", nullable: true),
                    BK_GHE_COD = table.Column<long>(type: "bigint", nullable: true),
                    BK_ROM_NUM = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BK_PHE_NUM = table.Column<long>(type: "bigint", nullable: true),
                    BK_CPN_COD = table.Column<long>(type: "bigint", nullable: true),
                    BK_CPN_TCK = table.Column<long>(type: "bigint", nullable: true),
                    BK_STS_COD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_NO_PER = table.Column<long>(type: "bigint", nullable: true),
                    BK_DRV_NAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BK_TRL_CST = table.Column<long>(type: "bigint", nullable: true),
                    BK_SLF_CST = table.Column<long>(type: "bigint", nullable: true),
                    BK_SLF_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_TCK_NUM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BK_AGN_COD = table.Column<long>(type: "bigint", nullable: true),
                    BK_TRVL_TYPE = table.Column<long>(type: "bigint", nullable: true),
                    BK_CAB_UNIT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    BK_COST_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BK_CAB_ADD = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BK_TRIP_COD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_CAB_SEGMENT = table.Column<long>(type: "bigint", nullable: true),
                    BK_APP_STS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_ADMN_BOKDAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_REGN_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BK_PRODUCT_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BK_SUBACCOUNT_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOK_CONFIRMATION", x => x.BK_CNF_NUM);
                });

            migrationBuilder.CreateTable(
                name: "BOOK_FORWARD_UNIT",
                schema: "dbo",
                columns: table => new
                {
                    BK_BOK_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    BK_SRL_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    ADM_UNIT = table.Column<long>(type: "bigint", nullable: false),
                    FWD_ADM_UNIT = table.Column<long>(type: "bigint", nullable: false),
                    FWD_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOK_FORWARD_UNIT", x => new { x.BK_BOK_NUM, x.BK_SRL_NUM, x.ADM_UNIT });
                });

            migrationBuilder.CreateTable(
                name: "BOOK_REQUEST",
                schema: "dbo",
                columns: table => new
                {
                    BK_BOK_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    BK_SRL_NUM = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    BK_BOK_TYP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_USR_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BK_USR_NUM = table.Column<long>(type: "bigint", nullable: true),
                    BK_ADM_SLF = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_ADM_UNT = table.Column<int>(type: "int", nullable: true),
                    BK_REQ_TYP = table.Column<long>(type: "bigint", nullable: true),
                    BK_REQ_NUM = table.Column<long>(type: "bigint", nullable: true),
                    BK_MOD_COD = table.Column<long>(type: "bigint", nullable: true),
                    BK_PER_STS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_PER_NAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_FRO_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_FRM_TIM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    BK_RET_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_RET_TIM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    BK_FRO_CIT = table.Column<long>(type: "bigint", nullable: true),
                    BK_TO_CIT = table.Column<long>(type: "bigint", nullable: true),
                    BK_PCK_FLG = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    BK_FRO_LOC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_TO_LOC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_PER_SEX = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_DEP_NOS = table.Column<long>(type: "bigint", nullable: true),
                    BK_ADM_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_BUD_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    BK_CAN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_CAN_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BK_CAN_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BK_APP_STS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_CNF_NUM = table.Column<long>(type: "bigint", nullable: true),
                    BK_APP_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_TRA_CLS = table.Column<long>(type: "bigint", nullable: true),
                    BK_AIR_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    BK_TRVL_TYPE = table.Column<long>(type: "bigint", nullable: true),
                    BK_CAB_TO_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_CAB_TO_UNIT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    BK_CAB_TO_COST = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BK_CAB_TO_ADD = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BK_CAB_TO_TRIP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    BK_CAB_SEGMENT = table.Column<long>(type: "bigint", nullable: true),
                    BK_PRODUCT_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BK_SUBACCOUNT_CODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOK_REQUEST", x => x.BK_BOK_NUM);
                });

            migrationBuilder.CreateTable(
                name: "CABPICK",
                schema: "dbo",
                columns: table => new
                {
                    CITYFROM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CITYTO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PICKFLAG = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "COUPON_MAIN",
                schema: "dbo",
                columns: table => new
                {
                    CPN_CUP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPN_REF_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CPN_REQ_ID = table.Column<long>(type: "bigint", nullable: true),
                    CPN_NOF_TCK = table.Column<long>(type: "bigint", nullable: true),
                    CPN_ARL_NAM = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CPN_CUP_STR = table.Column<long>(type: "bigint", nullable: true),
                    CPN_CUP_END = table.Column<long>(type: "bigint", nullable: true),
                    CPN_VLD_FRM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CPN_VLD_TO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CPN_CUP_CST = table.Column<long>(type: "bigint", nullable: true),
                    CPN_ISE_REK = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CPN_USG_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CPN_USR_ID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CPN_USR_PIN = table.Column<long>(type: "bigint", nullable: true),
                    CPN_ADN_USR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CPN_ADN_UNT = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CPN_ISS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUPON_MAIN", x => x.CPN_CUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "COUPON_REQUEST",
                schema: "dbo",
                columns: table => new
                {
                    CPN_REQ_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPN_REQ_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CPN_REQ_USR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CPN_NOF_CPN = table.Column<long>(type: "bigint", nullable: true),
                    CPN_ARL_NAM = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CPN_REQ_RMK = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CPN_ARG_UNT = table.Column<long>(type: "bigint", nullable: true),
                    CPN_APV_USR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CPN_ACT_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CPN_REQ_STS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CPN_ACT_RMK = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CPN_FLX_FLD1 = table.Column<long>(type: "bigint", nullable: true),
                    CPN_FLX_FLD2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CPN_FLX_FLD3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CPN_FLX_FLD4 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUPON_REQUEST", x => x.CPN_REQ_ID);
                });

            migrationBuilder.CreateTable(
                name: "COUPON_SUB",
                schema: "dbo",
                columns: table => new
                {
                    CPN_CUP_ID = table.Column<long>(type: "bigint", nullable: true),
                    CPN_SRL_NUM = table.Column<long>(type: "bigint", nullable: true),
                    CPN_TCK_NUM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CPN_USG_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ROOMAVAIL_TEMP",
                schema: "dbo",
                columns: table => new
                {
                    BK_GHCODE = table.Column<long>(type: "bigint", nullable: true),
                    BK_ROOMNO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BK_FRODAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BK_TODAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TOTALHR_OCCUPIED = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOOK_CONFIRMATION",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BOOK_FORWARD_UNIT",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BOOK_REQUEST",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CABPICK",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "COUPON_MAIN",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "COUPON_REQUEST",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "COUPON_SUB",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ROOMAVAIL_TEMP",
                schema: "dbo");
        }
    }
}
