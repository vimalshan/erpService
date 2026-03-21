using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnitService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UM_ACCESS_MASTER",
                columns: table => new
                {
                    UA_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UA_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UA_EMP_SYSID = table.Column<int>(type: "int", nullable: false),
                    UA_ACCESS_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UA_START_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UA_CLOSE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UA_LAST_MODIFIEDBY = table.Column<int>(type: "int", nullable: false),
                    UA_LAST_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UA_MODULE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UM_ACCESS_MASTER", x => x.UA_ID);
                });

            migrationBuilder.CreateTable(
                name: "UM_BUDGET_MASTER",
                columns: table => new
                {
                    BM_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    BM_EQUIPMENT_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    BM_START_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BM_CLOSE_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    BM_LAST_MODIFIEDBY = table.Column<int>(type: "int", nullable: true),
                    BM_LAST_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UM_BUDGET_MASTER", x => x.BM_UNIT_CODE);
                });

            migrationBuilder.CreateTable(
                name: "UM_CATEGORY_MASTER",
                columns: table => new
                {
                    UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CATEGORY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CATEGORY_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    LAST_MODIFIEDBY = table.Column<int>(type: "int", nullable: true),
                    LAST_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UM_CATEGORY_MASTER", x => x.UNIT_CODE);
                });

            migrationBuilder.CreateTable(
                name: "UM_EQUIPMENT_MASTER",
                columns: table => new
                {
                    EM_EQUIPMENT_ID = table.Column<int>(type: "int", nullable: false),
                    EM_EQUIPMENT_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    EM_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EM_CATEGORY = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    EM_START_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    EM_CLOSE_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    EM_LAST_MODIFIEDBY = table.Column<int>(type: "int", nullable: false),
                    EM_LAST_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UM_EQUIPMENT_MASTER", x => x.EM_EQUIPMENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "UM_MAILID_MASTER",
                columns: table => new
                {
                    MM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MM_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MM_MAIL_ID = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    MM_DELIVERY_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    MM_START_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MM_CLOSE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MM_LAST_MODIFIEDBY = table.Column<int>(type: "int", nullable: false),
                    MM_LAST_MODIFIEDON = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MM_MODULE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UM_MAILID_MASTER", x => x.MM_ID);
                });

            migrationBuilder.CreateTable(
                name: "UM_STATUS_CONFIRM",
                columns: table => new
                {
                    STATUS_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    STATUS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    STATUS_CONFIRM_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    STATUS_CONFIRM_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UM_EQUIP_STATUS",
                columns: table => new
                {
                    ES_ID = table.Column<int>(type: "int", nullable: false),
                    ES_EQUIPMENT_ID = table.Column<int>(type: "int", nullable: false),
                    ES_STATUS_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    ES_STATUS_ID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ES_START_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ES_CLOSE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ES_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ES_HOURS = table.Column<long>(type: "bigint", nullable: true),
                    ES_FILEPATH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ES_CREATED_BY = table.Column<int>(type: "int", nullable: true),
                    ES_CREATED_ON = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ES_LAST_MODIFIED_BY = table.Column<int>(type: "int", nullable: true),
                    ES_LAST_MODIFIED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UM_EQUIP_STATUS", x => x.ES_ID);
                    table.ForeignKey(
                        name: "FK_UM_EQUIP_STATUS_UM_EQUIPMENT_MASTER_ES_EQUIPMENT_ID",
                        column: x => x.ES_EQUIPMENT_ID,
                        principalTable: "UM_EQUIPMENT_MASTER",
                        principalColumn: "EM_EQUIPMENT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UM_EQUIP_STATUS_ES_EQUIPMENT_ID",
                table: "UM_EQUIP_STATUS",
                column: "ES_EQUIPMENT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UM_ACCESS_MASTER");

            migrationBuilder.DropTable(
                name: "UM_BUDGET_MASTER");

            migrationBuilder.DropTable(
                name: "UM_CATEGORY_MASTER");

            migrationBuilder.DropTable(
                name: "UM_EQUIP_STATUS");

            migrationBuilder.DropTable(
                name: "UM_MAILID_MASTER");

            migrationBuilder.DropTable(
                name: "UM_STATUS_CONFIRM");

            migrationBuilder.DropTable(
                name: "UM_EQUIPMENT_MASTER");
        }
    }
}
