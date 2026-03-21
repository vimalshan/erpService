using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterDataService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AREA_MASTER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AREA_ID = table.Column<int>(type: "int", nullable: false),
                    AREA_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AREA_MASTER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "COUPON_TEMP",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CPN_ID = table.Column<long>(type: "bigint", nullable: false),
                    AIR_LIN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TOT_CPN = table.Column<long>(type: "bigint", nullable: false),
                    USD_CPN = table.Column<long>(type: "bigint", nullable: false),
                    BAL_CPN = table.Column<long>(type: "bigint", nullable: false),
                    VLS_TIL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUPON_TEMP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GL_CODE_COMBINATIONS_KFV",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROW_ID = table.Column<long>(type: "bigint", nullable: false),
                    CODE_COMBINATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    CHART_OF_ACCOUNTS_ID = table.Column<long>(type: "bigint", nullable: false),
                    CONCATENATED_SEGMENTS = table.Column<string>(type: "nvarchar(207)", maxLength: 207, nullable: false),
                    GL_ACCOUNT_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ENABLED_FLAG = table.Column<bool>(type: "bit", nullable: false),
                    SUMMARY_FLAG = table.Column<bool>(type: "bit", nullable: false),
                    CONTEXT = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LAST_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    LAST_UPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GL_CODE_COMBINATIONS_KFV", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GUEST_ROOM_AVAIL_TEMP",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GHS_FLR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    GHS_ROM_NUM = table.Column<long>(type: "bigint", nullable: false),
                    GHS_ROM_STS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    GHS_FLR_VAL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUEST_ROOM_AVAIL_TEMP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROUTE_MASTER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROUTE_ID = table.Column<int>(type: "int", nullable: false),
                    ROUTE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROUTE_MASTER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TAX_SLABS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TAX_TYPE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TAX_EFFDAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TAX_CLSDAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TAX_RATE = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    VENDORCODE = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAX_SLABS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_GUESTHOUSE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AD_ADM_COD = table.Column<long>(type: "bigint", nullable: false),
                    AD_ADM_NAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AD_ADM_TYP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    AD_ADM_AMOUNT = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_GUESTHOUSE", x => x.Id);
                    table.UniqueConstraint("AK_TRAVEL_GUESTHOUSE_AD_ADM_COD", x => x.AD_ADM_COD);
                });

            migrationBuilder.CreateTable(
                name: "GH_ROOMS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GH_GHS_COD = table.Column<long>(type: "bigint", nullable: false),
                    GH_ROM_SRL = table.Column<long>(type: "bigint", nullable: false),
                    GH_NOF_PER = table.Column<long>(type: "bigint", nullable: false),
                    GH_ROM_NUM = table.Column<long>(type: "bigint", nullable: false),
                    GH_GHS_FLR = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GH_ROOMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GH_ROOMS_TRAVEL_GUESTHOUSE_GH_GHS_COD",
                        column: x => x.GH_GHS_COD,
                        principalTable: "TRAVEL_GUESTHOUSE",
                        principalColumn: "AD_ADM_COD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AREA_MASTER_AREA_ID",
                table: "AREA_MASTER",
                column: "AREA_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GH_ROOMS_GH_GHS_COD",
                table: "GH_ROOMS",
                column: "GH_GHS_COD");

            migrationBuilder.CreateIndex(
                name: "IX_ROUTE_MASTER_ROUTE_ID",
                table: "ROUTE_MASTER",
                column: "ROUTE_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_GUESTHOUSE_AD_ADM_COD",
                table: "TRAVEL_GUESTHOUSE",
                column: "AD_ADM_COD",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AREA_MASTER");

            migrationBuilder.DropTable(
                name: "COUPON_TEMP");

            migrationBuilder.DropTable(
                name: "GH_ROOMS");

            migrationBuilder.DropTable(
                name: "GL_CODE_COMBINATIONS_KFV");

            migrationBuilder.DropTable(
                name: "GUEST_ROOM_AVAIL_TEMP");

            migrationBuilder.DropTable(
                name: "ROUTE_MASTER");

            migrationBuilder.DropTable(
                name: "TAX_SLABS");

            migrationBuilder.DropTable(
                name: "TRAVEL_GUESTHOUSE");
        }
    }
}
