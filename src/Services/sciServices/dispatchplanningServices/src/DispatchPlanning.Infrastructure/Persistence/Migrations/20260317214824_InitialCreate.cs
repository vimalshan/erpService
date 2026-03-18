using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatchPlanning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DISPATCH_PLAN_BREAKUP_ITEM",
                columns: table => new
                {
                    BREAKUP_ITEM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUB_GROUP_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    BREAKUP_ITEM_DESC = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    UNIT_ID = table.Column<int>(type: "int", nullable: false),
                    MAIN_PRODUCT_UNITS_CONFACTOR = table.Column<int>(type: "int", nullable: false),
                    BI_DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CLOSURE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PACKAGE_ID = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPATCH_PLAN_BREAKUP_ITEM", x => x.BREAKUP_ITEM_ID);
                });

            migrationBuilder.CreateTable(
                name: "DISPATCH_PLAN_HEADER",
                columns: table => new
                {
                    DISPATCH_PLAN_HEADER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DISPATCH_PLAN_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DISPATCH_PLAN_MONTH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DISPATCH_PLAN_MPLUS1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DISPATCH_PLAN_MPLUS2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DISPATCH_PLAN_MPLUS3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DISPATCH_PLAN_MPLUS4 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DISPATCH_PLAN_ENTRYDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COMPANY_UNIT_ID = table.Column<int>(type: "int", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPATCH_PLAN_HEADER", x => x.DISPATCH_PLAN_HEADER_ID);
                });

            migrationBuilder.CreateTable(
                name: "DISPATCH_PLAN_MAINGROUP",
                columns: table => new
                {
                    MAIN_GROUP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAIN_GROUP_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GROUP_TYPE = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    PRODUCT_SUMMARY = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    TOTAL_DISPLAY_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MG_DISPLAY_ORDER = table.Column<int>(type: "int", nullable: false),
                    COMPANY_UNIT_ID = table.Column<int>(type: "int", nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPATCH_PLAN_MAINGROUP", x => x.MAIN_GROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DISPATCH_PLAN_SUBGROUP",
                columns: table => new
                {
                    SUB_GROUP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAIN_GROUP_ID = table.Column<int>(type: "int", nullable: false),
                    SUB_GROUP_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: true),
                    SG_DISPLAY_ORDER = table.Column<int>(type: "int", nullable: true),
                    CAPTURE_TOTAL_DIRECTLY = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPATCH_PLAN_SUBGROUP", x => x.SUB_GROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "DISPATCH_PLAN_ITEMWISE",
                columns: table => new
                {
                    DISPATCH_PLAN_HEADER_ID = table.Column<int>(type: "int", nullable: false),
                    BREAKUP_ITEM_ID = table.Column<int>(type: "int", nullable: false),
                    TARGET_WEEK1 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK2 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK3 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK4 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK5 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS1 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS2 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS3 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS4 = table.Column<long>(type: "bigint", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPATCH_PLAN_ITEMWISE", x => new { x.DISPATCH_PLAN_HEADER_ID, x.BREAKUP_ITEM_ID });
                    table.ForeignKey(
                        name: "FK_DISPATCH_PLAN_ITEMWISE_DISPATCH_PLAN_HEADER_DISPATCH_PLAN_HEADER_ID",
                        column: x => x.DISPATCH_PLAN_HEADER_ID,
                        principalTable: "DISPATCH_PLAN_HEADER",
                        principalColumn: "DISPATCH_PLAN_HEADER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DISPATCH_PLAN_SUBGROUPWISE",
                columns: table => new
                {
                    DISPATCH_PLAN_HEADER_ID = table.Column<int>(type: "int", nullable: false),
                    SUB_GROUP_ID = table.Column<int>(type: "int", nullable: false),
                    TARGET_WEEK1 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK2 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK3 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK4 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_WEEK5 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS1 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS2 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS3 = table.Column<long>(type: "bigint", nullable: true),
                    TARGET_MPLUS4 = table.Column<long>(type: "bigint", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPATCH_PLAN_SUBGROUPWISE", x => new { x.DISPATCH_PLAN_HEADER_ID, x.SUB_GROUP_ID });
                    table.ForeignKey(
                        name: "FK_DISPATCH_PLAN_SUBGROUPWISE_DISPATCH_PLAN_HEADER_DISPATCH_PLAN_HEADER_ID",
                        column: x => x.DISPATCH_PLAN_HEADER_ID,
                        principalTable: "DISPATCH_PLAN_HEADER",
                        principalColumn: "DISPATCH_PLAN_HEADER_ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DISPATCH_PLAN_BREAKUP_ITEM");

            migrationBuilder.DropTable(
                name: "DISPATCH_PLAN_ITEMWISE");

            migrationBuilder.DropTable(
                name: "DISPATCH_PLAN_MAINGROUP");

            migrationBuilder.DropTable(
                name: "DISPATCH_PLAN_SUBGROUP");

            migrationBuilder.DropTable(
                name: "DISPATCH_PLAN_SUBGROUPWISE");

            migrationBuilder.DropTable(
                name: "DISPATCH_PLAN_HEADER");
        }
    }
}
