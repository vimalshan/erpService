using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADVLIC_MASTER",
                columns: table => new
                {
                    ADVLIC_ID = table.Column<long>(type: "bigint", nullable: false),
                    ADVLIC_NO = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ADVLIC_FG = table.Column<int>(type: "int", nullable: true),
                    ADVLIC_EOAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ADVLIC_EXPAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADVLIC_MASTER", x => x.ADVLIC_ID);
                });

            migrationBuilder.CreateTable(
                name: "ITEM_CAPACITY",
                columns: table => new
                {
                    CAPACITY_ID = table.Column<int>(type: "int", nullable: false),
                    CAPACITY_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_CAPACITY", x => x.CAPACITY_ID);
                });

            migrationBuilder.CreateTable(
                name: "ITEM_GRADE",
                columns: table => new
                {
                    ITEM_GRADE_ID = table.Column<int>(type: "int", nullable: false),
                    ITEM_GRADE_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_GRADE", x => x.ITEM_GRADE_ID);
                });

            migrationBuilder.CreateTable(
                name: "ITEM_MAP",
                columns: table => new
                {
                    OSP_ITEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    OSP_UOM_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ITEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    UOM_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    QUANTITY = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ORACLE_CODE = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_MAP", x => new { x.OSP_ITEM_ID, x.OSP_UOM_CODE, x.ITEM_ID, x.UOM_CODE });
                });

            migrationBuilder.CreateTable(
                name: "ITEM_TYPE",
                columns: table => new
                {
                    ITEM_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    ITEM_TYPE_CODE = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_TYPE", x => x.ITEM_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "MATERIAL_TAX_CLASS",
                columns: table => new
                {
                    MATERIAL_TAXCLASS_ID = table.Column<int>(type: "int", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATERIAL_TAX_CLASS", x => x.MATERIAL_TAXCLASS_ID);
                });

            migrationBuilder.CreateTable(
                name: "PACKAGE_TYPE",
                columns: table => new
                {
                    PACKAGE_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    PACKAGE_TYPE_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PACKAGE_TYPE", x => x.PACKAGE_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_MASTER",
                columns: table => new
                {
                    PM_PRO_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PM_PRO_DESC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PM_ORA_DES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PM_UOM_COD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_MASTER", x => x.PM_PRO_COD);
                });

            migrationBuilder.CreateTable(
                name: "PRODUCT_TYPE_MASTER",
                columns: table => new
                {
                    PRODUCT_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    TYPE_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TYPE_DESCRIPTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT_TYPE_MASTER", x => x.PRODUCT_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "UNITS_CLASS",
                columns: table => new
                {
                    UNITS_CLASS_ID = table.Column<int>(type: "int", nullable: false),
                    UNITS_CLASS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNITS_CLASS", x => x.UNITS_CLASS_ID);
                });

            migrationBuilder.CreateTable(
                name: "ADVLIC_ENTITLEMENT",
                columns: table => new
                {
                    ADVLIC_ID = table.Column<long>(type: "bigint", nullable: false),
                    ADVLIC_ENTITLERM = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADVLIC_ENTITLEMENT", x => new { x.ADVLIC_ID, x.ADVLIC_ENTITLERM });
                    table.ForeignKey(
                        name: "FK_ADVLIC_ENTITLEMENT_ADVLIC_MASTER_ADVLIC_ID",
                        column: x => x.ADVLIC_ID,
                        principalTable: "ADVLIC_MASTER",
                        principalColumn: "ADVLIC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRADE_MASTER",
                columns: table => new
                {
                    GM_GRD_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    GM_GRD_DESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GM_PRO_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRADE_MASTER", x => x.GM_GRD_COD);
                    table.ForeignKey(
                        name: "FK_GRADE_MASTER_PRODUCT_MASTER_GM_PRO_COD",
                        column: x => x.GM_PRO_COD,
                        principalTable: "PRODUCT_MASTER",
                        principalColumn: "PM_PRO_COD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UNIT_OF_MEASURE",
                columns: table => new
                {
                    UNIT_ID = table.Column<int>(type: "int", nullable: false),
                    UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    UNIT_OF_MEASURENT = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    UNIT_CLASS_ID = table.Column<int>(type: "int", nullable: false),
                    BASE_UNIT_FLAG = table.Column<string>(type: "char(1)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIT_OF_MEASURE", x => x.UNIT_ID);
                    table.ForeignKey(
                        name: "FK_UNIT_OF_MEASURE_UNITS_CLASS_UNIT_CLASS_ID",
                        column: x => x.UNIT_CLASS_ID,
                        principalTable: "UNITS_CLASS",
                        principalColumn: "UNITS_CLASS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MAIN_PRODUCT_MASTER",
                columns: table => new
                {
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_NAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PRODUCT_DESCRIPTION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UNIT_ID = table.Column<int>(type: "int", nullable: true),
                    PRODUCT_TYPE_ID = table.Column<int>(type: "int", nullable: true),
                    COMPANY_UNIT_ID = table.Column<int>(type: "int", nullable: true),
                    MAM_FLAG = table.Column<string>(type: "char(1)", nullable: true),
                    SCI_USER_ID_CREATED = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAIN_PRODUCT_MASTER", x => x.PRODUCT_ID);
                    table.ForeignKey(
                        name: "FK_MAIN_PRODUCT_MASTER_PRODUCT_TYPE_MASTER_PRODUCT_TYPE_ID",
                        column: x => x.PRODUCT_TYPE_ID,
                        principalTable: "PRODUCT_TYPE_MASTER",
                        principalColumn: "PRODUCT_TYPE_ID");
                    table.ForeignKey(
                        name: "FK_MAIN_PRODUCT_MASTER_UNIT_OF_MEASURE_UNIT_ID",
                        column: x => x.UNIT_ID,
                        principalTable: "UNIT_OF_MEASURE",
                        principalColumn: "UNIT_ID");
                });

            migrationBuilder.CreateTable(
                name: "ITEM_MASTER",
                columns: table => new
                {
                    SCI_ITEM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORACLE_CODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ORACLE_ITEM_ID = table.Column<int>(type: "int", nullable: false),
                    MAIN_PRODUCT_ID = table.Column<int>(type: "int", nullable: true),
                    ITEM_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ORACLE_DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ITEM_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PACKAGE_TYPE_ID = table.Column<int>(type: "int", nullable: true),
                    ITEM_UOM_ID = table.Column<int>(type: "int", nullable: false),
                    MAIN_PRODUCT_UOM_CONFACTOR = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ISBULK_SOURCE = table.Column<string>(type: "varchar(1)", nullable: false),
                    ISBULK_ITEM = table.Column<string>(type: "char(1)", nullable: false),
                    MATERIAL_TAXCLASS = table.Column<int>(type: "int", nullable: true),
                    PRODUCT_CLASS = table.Column<string>(type: "char(2)", nullable: true),
                    EFFECTIVE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CLOSURE_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LEAD_TIME = table.Column<int>(type: "int", nullable: true),
                    ITEM_CAPACITY_ID = table.Column<int>(type: "int", nullable: true),
                    ITEM_USAGE = table.Column<string>(type: "char(2)", nullable: true),
                    MAM_FLAG = table.Column<string>(type: "char(1)", nullable: true),
                    ITEM_ACC_TYPE = table.Column<string>(type: "char(1)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCI_USER_ID_MODIFIED = table.Column<int>(type: "int", nullable: true),
                    MODIFIED_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEM_MASTER", x => x.SCI_ITEM_ID);
                    table.ForeignKey(
                        name: "FK_ITEM_MASTER_ITEM_CAPACITY_ITEM_CAPACITY_ID",
                        column: x => x.ITEM_CAPACITY_ID,
                        principalTable: "ITEM_CAPACITY",
                        principalColumn: "CAPACITY_ID");
                    table.ForeignKey(
                        name: "FK_ITEM_MASTER_MAIN_PRODUCT_MASTER_MAIN_PRODUCT_ID",
                        column: x => x.MAIN_PRODUCT_ID,
                        principalTable: "MAIN_PRODUCT_MASTER",
                        principalColumn: "PRODUCT_ID");
                    table.ForeignKey(
                        name: "FK_ITEM_MASTER_MATERIAL_TAX_CLASS_MATERIAL_TAXCLASS",
                        column: x => x.MATERIAL_TAXCLASS,
                        principalTable: "MATERIAL_TAX_CLASS",
                        principalColumn: "MATERIAL_TAXCLASS_ID");
                    table.ForeignKey(
                        name: "FK_ITEM_MASTER_PACKAGE_TYPE_PACKAGE_TYPE_ID",
                        column: x => x.PACKAGE_TYPE_ID,
                        principalTable: "PACKAGE_TYPE",
                        principalColumn: "PACKAGE_TYPE_ID");
                    table.ForeignKey(
                        name: "FK_ITEM_MASTER_UNIT_OF_MEASURE_ITEM_UOM_ID",
                        column: x => x.ITEM_UOM_ID,
                        principalTable: "UNIT_OF_MEASURE",
                        principalColumn: "UNIT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ITEM_CAPACITY",
                columns: new[] { "CAPACITY_ID", "CAPACITY_NAME", "SCI_USER_ID_CREATED", "CREATION_DATE", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE" },
                values: new object[,]
                {
                    { 1, "SMALL", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null },
                    { 2, "MEDIUM", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null },
                    { 3, "LARGE", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null }
                });

            migrationBuilder.InsertData(
                table: "ITEM_GRADE",
                columns: new[] { "ITEM_GRADE_ID", "SCI_USER_ID_CREATED", "CREATION_DATE", "ITEM_GRADE_NAME", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GRADE-A", null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GRADE-B", null, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "GRADE-C", null, null }
                });

            migrationBuilder.InsertData(
                table: "ITEM_TYPE",
                columns: new[] { "ITEM_TYPE_ID", "SCI_USER_ID_CREATED", "CREATION_DATE", "DESCRIPTION", "ITEM_TYPE_CODE", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Raw Material", "RM", null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finished Goods", "FG", null, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Semi Finished", "SF", null, null },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Spare Parts", "SP", null, null },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Packing Material", "PM", null, null }
                });

            migrationBuilder.InsertData(
                table: "MATERIAL_TAX_CLASS",
                columns: new[] { "MATERIAL_TAXCLASS_ID", "SCI_USER_ID_CREATED", "CREATION_DATE", "DESCRIPTION", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TAXABLE", null, null },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "EXEMPT", null, null },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ZERO-RATED", null, null }
                });

            migrationBuilder.InsertData(
                table: "PACKAGE_TYPE",
                columns: new[] { "PACKAGE_TYPE_ID", "SCI_USER_ID_CREATED", "CREATION_DATE", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE", "PACKAGE_TYPE_NAME" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "BAG" },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "DRUM" },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "CARTON" },
                    { 4, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "CYLINDER" },
                    { 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "PALLET" }
                });

            migrationBuilder.InsertData(
                table: "PRODUCT_TYPE_MASTER",
                columns: new[] { "PRODUCT_TYPE_ID", "SCI_USER_ID_CREATED", "CREATION_DATE", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE", "TYPE_DESCRIPTION", "TYPE_NAME" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Bulk Products", "BULK" },
                    { 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Packed Products", "PACKED" },
                    { 3, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Service Items", "SERVICE" }
                });

            migrationBuilder.InsertData(
                table: "UNITS_CLASS",
                columns: new[] { "UNITS_CLASS_ID", "UNITS_CLASS" },
                values: new object[,]
                {
                    { 1, "WEIGHT" },
                    { 2, "VOLUME" },
                    { 3, "EACH" }
                });

            migrationBuilder.InsertData(
                table: "UNIT_OF_MEASURE",
                columns: new[] { "UNIT_ID", "BASE_UNIT_FLAG", "SCI_USER_ID_CREATED", "CREATION_DATE", "DESCRIPTION", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE", "UNIT_CLASS_ID", "UNIT_CODE", "UNIT_OF_MEASURENT" },
                values: new object[,]
                {
                    { 1, "Y", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 1, "KG", "Kilogram" },
                    { 2, "N", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 1, "MT", "Metric Ton" },
                    { 3, "Y", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 2, "LTR", "Litre" },
                    { 4, "Y", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, 3, "EA", "Each" }
                });

            migrationBuilder.InsertData(
                table: "MAIN_PRODUCT_MASTER",
                columns: new[] { "PRODUCT_ID", "COMPANY_UNIT_ID", "SCI_USER_ID_CREATED", "CREATION_DATE", "MAM_FLAG", "SCI_USER_ID_MODIFIED", "MODIFIED_DATE", "PRODUCT_DESCRIPTION", "PRODUCT_NAME", "PRODUCT_TYPE_ID", "UNIT_ID" },
                values: new object[] { 1, 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Urea Fertilizer", "UREA", 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_GRADE_MASTER_GM_PRO_COD",
                table: "GRADE_MASTER",
                column: "GM_PRO_COD");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_MASTER_ITEM_CAPACITY_ID",
                table: "ITEM_MASTER",
                column: "ITEM_CAPACITY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_MASTER_ITEM_UOM_ID",
                table: "ITEM_MASTER",
                column: "ITEM_UOM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_MASTER_MAIN_PRODUCT_ID",
                table: "ITEM_MASTER",
                column: "MAIN_PRODUCT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_MASTER_MATERIAL_TAXCLASS",
                table: "ITEM_MASTER",
                column: "MATERIAL_TAXCLASS");

            migrationBuilder.CreateIndex(
                name: "IX_ITEM_MASTER_PACKAGE_TYPE_ID",
                table: "ITEM_MASTER",
                column: "PACKAGE_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MAIN_PRODUCT_MASTER_PRODUCT_TYPE_ID",
                table: "MAIN_PRODUCT_MASTER",
                column: "PRODUCT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MAIN_PRODUCT_MASTER_UNIT_ID",
                table: "MAIN_PRODUCT_MASTER",
                column: "UNIT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_UNIT_OF_MEASURE_UNIT_CLASS_ID",
                table: "UNIT_OF_MEASURE",
                column: "UNIT_CLASS_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADVLIC_ENTITLEMENT");

            migrationBuilder.DropTable(
                name: "GRADE_MASTER");

            migrationBuilder.DropTable(
                name: "ITEM_GRADE");

            migrationBuilder.DropTable(
                name: "ITEM_MAP");

            migrationBuilder.DropTable(
                name: "ITEM_MASTER");

            migrationBuilder.DropTable(
                name: "ITEM_TYPE");

            migrationBuilder.DropTable(
                name: "ADVLIC_MASTER");

            migrationBuilder.DropTable(
                name: "PRODUCT_MASTER");

            migrationBuilder.DropTable(
                name: "ITEM_CAPACITY");

            migrationBuilder.DropTable(
                name: "MAIN_PRODUCT_MASTER");

            migrationBuilder.DropTable(
                name: "MATERIAL_TAX_CLASS");

            migrationBuilder.DropTable(
                name: "PACKAGE_TYPE");

            migrationBuilder.DropTable(
                name: "PRODUCT_TYPE_MASTER");

            migrationBuilder.DropTable(
                name: "UNIT_OF_MEASURE");

            migrationBuilder.DropTable(
                name: "UNITS_CLASS");
        }
    }
}
