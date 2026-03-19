using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductionManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MAM_PRODUCTION_DET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCTION_NO = table.Column<long>(type: "bigint", nullable: true),
                    PRODUCTION_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PRODUCTION_FG = table.Column<int>(type: "int", nullable: true),
                    PRODUCTION_QTY = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAM_PRODUCTION_DET", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MAM_PRODUCTION_MAP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RM_CODE = table.Column<int>(type: "int", nullable: true),
                    FG_CODE = table.Column<int>(type: "int", nullable: true),
                    SLNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAM_PRODUCTION_MAP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NORMS_MAIN",
                columns: table => new
                {
                    NORM_NO = table.Column<long>(type: "bigint", nullable: false),
                    NORM_EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    NORM_CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NORMS_MAIN", x => x.NORM_NO);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTION_PLANT",
                columns: table => new
                {
                    PRODUCTION_PLANT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COMPANY_UNIT_ID = table.Column<int>(type: "int", nullable: false),
                    PLANT_NAME = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    LOCATION = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_PLANT", x => x.PRODUCTION_PLANT_ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTIONPLAN_ENTRY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORACLE_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MONTH = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PRO_TYPE = table.Column<string>(type: "char(1)", nullable: true),
                    PRO_VALUE = table.Column<int>(type: "int", nullable: true),
                    FACTORY_ID = table.Column<int>(type: "int", nullable: true),
                    ZONE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PRO_YEAR = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTIONPLAN_ENTRY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NORMS_MASTER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NORM_ID = table.Column<long>(type: "bigint", nullable: true),
                    NORM_INPUT_CODE = table.Column<int>(type: "int", nullable: true),
                    NORM_OUTPUT_CODE = table.Column<int>(type: "int", nullable: true),
                    NORM_RATE = table.Column<int>(type: "int", nullable: true),
                    NORM_NO = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NORMS_MASTER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NORMS_MASTER_NORMS_MAIN_NORM_NO",
                        column: x => x.NORM_NO,
                        principalTable: "NORMS_MAIN",
                        principalColumn: "NORM_NO");
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTION_PLAN",
                columns: table => new
                {
                    PRODUCTION_PLANT_ID = table.Column<int>(type: "int", nullable: false),
                    SCI_ITEM_ID = table.Column<int>(type: "int", nullable: false),
                    QTY_PERDAY = table.Column<int>(type: "int", nullable: false),
                    PLAN_START_DATE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PLAN_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTION_PLAN", x => new { x.PRODUCTION_PLANT_ID, x.SCI_ITEM_ID });
                    table.ForeignKey(
                        name: "FK_PRODUCTION_PLAN_PRODUCTION_PLANT_PRODUCTION_PLANT_ID",
                        column: x => x.PRODUCTION_PLANT_ID,
                        principalTable: "PRODUCTION_PLANT",
                        principalColumn: "PRODUCTION_PLANT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCTIONPLANT_PRODUCT_MAP",
                columns: table => new
                {
                    PRODUCTION_PLANT_ID = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: false),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCTIONPLANT_PRODUCT_MAP", x => new { x.PRODUCTION_PLANT_ID, x.PRODUCT_ID });
                    table.ForeignKey(
                        name: "FK_PRODUCTIONPLANT_PRODUCT_MAP_PRODUCTION_PLANT_PRODUCTION_PLANT_ID",
                        column: x => x.PRODUCTION_PLANT_ID,
                        principalTable: "PRODUCTION_PLANT",
                        principalColumn: "PRODUCTION_PLANT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NORMS_MASTER_NORM_NO",
                table: "NORMS_MASTER",
                column: "NORM_NO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MAM_PRODUCTION_DET");

            migrationBuilder.DropTable(
                name: "MAM_PRODUCTION_MAP");

            migrationBuilder.DropTable(
                name: "NORMS_MASTER");

            migrationBuilder.DropTable(
                name: "PRODUCTION_PLAN");

            migrationBuilder.DropTable(
                name: "PRODUCTIONPLAN_ENTRY");

            migrationBuilder.DropTable(
                name: "PRODUCTIONPLANT_PRODUCT_MAP");

            migrationBuilder.DropTable(
                name: "NORMS_MAIN");

            migrationBuilder.DropTable(
                name: "PRODUCTION_PLANT");
        }
    }
}
