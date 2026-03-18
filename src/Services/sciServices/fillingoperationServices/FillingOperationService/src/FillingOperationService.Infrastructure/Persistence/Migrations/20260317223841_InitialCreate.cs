using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FillingOperationService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FILLING_CAPACITY",
                columns: table => new
                {
                    FILLING_POINT_GROUP_ID = table.Column<int>(type: "int", nullable: false),
                    MAIN_PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    PACKAGE_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    ITEM_CAPACITY_ID = table.Column<int>(type: "int", nullable: false),
                    CAPACITY_PER_SHIFT = table.Column<int>(type: "int", nullable: false),
                    USAGE_PRIORITY = table.Column<int>(type: "int", nullable: false),
                    SCI_USERID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USERID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILLING_CAPACITY", x => new { x.FILLING_POINT_GROUP_ID, x.MAIN_PRODUCT_ID, x.PACKAGE_TYPE_ID });
                });

            migrationBuilder.CreateTable(
                name: "FILLING_LINE_PRODUCT_MAP",
                columns: table => new
                {
                    FILLING_LINE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAIN_PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILLING_LINE_PRODUCT_MAP", x => x.FILLING_LINE_ID);
                });

            migrationBuilder.CreateTable(
                name: "FILLING_PLANT",
                columns: table => new
                {
                    FILLING_PLANT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COMPANY_UNIT_ID = table.Column<int>(type: "int", nullable: false),
                    FILLING_PLANT_NAME = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LOCATION = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILLING_PLANT", x => x.FILLING_PLANT_ID);
                });

            migrationBuilder.CreateTable(
                name: "FL_SWITCHOVER_TIME",
                columns: table => new
                {
                    FILLING_LINE_ID = table.Column<int>(type: "int", nullable: false),
                    FROM_MAIN_PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    TO_MAIN_PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    TIME_IN_HOURS = table.Column<int>(type: "int", nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FL_SWITCHOVER_TIME", x => new { x.FILLING_LINE_ID, x.FROM_MAIN_PRODUCT_ID, x.TO_MAIN_PRODUCT_ID });
                });

            migrationBuilder.CreateTable(
                name: "FL_WORKING_SHIFT",
                columns: table => new
                {
                    FL_WORKING_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    FILLINGLINE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SHIFT_CODE = table.Column<string>(type: "char(1)", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CLOSE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FPG_DOWNTIME",
                columns: table => new
                {
                    FPG_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FILLING_POINT_GROUP_ID = table.Column<int>(type: "int", nullable: true),
                    START_DATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    END_DATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NO_OF_FILLING_POINTS = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DOWNTIME_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FPG_DOWNTIME", x => x.FPG_ID);
                });

            migrationBuilder.CreateTable(
                name: "PLAN_DEVIATION",
                columns: table => new
                {
                    REASON_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLAN_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FILLING_LINE_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLAN_DEVIATION", x => x.REASON_ID);
                });

            migrationBuilder.CreateTable(
                name: "FILLING_LINE",
                columns: table => new
                {
                    FILLING_LINE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FILLING_PLANT_ID = table.Column<int>(type: "int", nullable: false),
                    FILLING_LINE_NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NO_OF_FILLING_POINTS = table.Column<int>(type: "int", nullable: false),
                    PACKAGE_TYPE_ID = table.Column<int>(type: "int", nullable: true),
                    ISCLOSED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILLING_LINE", x => x.FILLING_LINE_ID);
                    table.ForeignKey(
                        name: "FK_FILLING_LINE_FILLING_PLANT_FILLING_PLANT_ID",
                        column: x => x.FILLING_PLANT_ID,
                        principalTable: "FILLING_PLANT",
                        principalColumn: "FILLING_PLANT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FILLING_POINT_GROUP",
                columns: table => new
                {
                    FILLING_POINT_GROUP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FILLING_POINT_GROUP_NAM = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FILLING_LINE_ID = table.Column<int>(type: "int", nullable: false),
                    NO_OF_FILLING_POINT = table.Column<int>(type: "int", nullable: false),
                    EXCLUSIVE_USE = table.Column<int>(type: "int", nullable: true),
                    ISCLOSED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILLING_POINT_GROUP", x => x.FILLING_POINT_GROUP_ID);
                    table.ForeignKey(
                        name: "FK_FILLING_POINT_GROUP_FILLING_LINE_FILLING_LINE_ID",
                        column: x => x.FILLING_LINE_ID,
                        principalTable: "FILLING_LINE",
                        principalColumn: "FILLING_LINE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FILLING_LINE_FILLING_PLANT_ID",
                table: "FILLING_LINE",
                column: "FILLING_PLANT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FILLING_POINT_GROUP_FILLING_LINE_ID",
                table: "FILLING_POINT_GROUP",
                column: "FILLING_LINE_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FILLING_CAPACITY");

            migrationBuilder.DropTable(
                name: "FILLING_LINE_PRODUCT_MAP");

            migrationBuilder.DropTable(
                name: "FILLING_POINT_GROUP");

            migrationBuilder.DropTable(
                name: "FL_SWITCHOVER_TIME");

            migrationBuilder.DropTable(
                name: "FL_WORKING_SHIFT");

            migrationBuilder.DropTable(
                name: "FPG_DOWNTIME");

            migrationBuilder.DropTable(
                name: "PLAN_DEVIATION");

            migrationBuilder.DropTable(
                name: "FILLING_LINE");

            migrationBuilder.DropTable(
                name: "FILLING_PLANT");
        }
    }
}
