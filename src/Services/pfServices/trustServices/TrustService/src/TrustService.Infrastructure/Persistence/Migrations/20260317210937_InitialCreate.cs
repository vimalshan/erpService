using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRUST_MASTER",
                columns: table => new
                {
                    TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    TRUST_SHORT_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    TRUST_TYPE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    TRUST_START_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    TRUST_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    TRUST_ID = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_LINE1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ADDRESS_LINE2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ADDRESS_LINE3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    STATE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PIN_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    COUNTRY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PHONE_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FAX_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TRUST_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A"),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    REGISTRAR_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    REGISTRAR_PHONE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRUST_MASTER", x => x.TRUST_CODE);
                });

            migrationBuilder.CreateTable(
                name: "TRUST_APPROVERS",
                columns: table => new
                {
                    APPROVER_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    APPROVER_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    APPROVER_LEVEL = table.Column<int>(type: "int", nullable: false),
                    APPROVER_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    APPROVER_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRUST_APPROVERS", x => x.APPROVER_ID);
                    table.ForeignKey(
                        name: "FK_TRUST_APPROVERS_TRUST_MASTER_TRUST_CODE",
                        column: x => x.TRUST_CODE,
                        principalTable: "TRUST_MASTER",
                        principalColumn: "TRUST_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRUST_AUDIT_LOG",
                columns: table => new
                {
                    AUDIT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    AUDIT_ACTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AUDIT_TABLE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AUDIT_TIMESTAMP = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    AUDIT_USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    OLD_VALUES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NEW_VALUES = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRUST_AUDIT_LOG", x => x.AUDIT_ID);
                    table.ForeignKey(
                        name: "FK_TRUST_AUDIT_LOG_TRUST_MASTER_TRUST_CODE",
                        column: x => x.TRUST_CODE,
                        principalTable: "TRUST_MASTER",
                        principalColumn: "TRUST_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRUST_CONFIGURATION",
                columns: table => new
                {
                    CONFIG_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CONFIG_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CONFIG_VALUE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CONFIG_CATEGORY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRUST_CONFIGURATION", x => x.CONFIG_ID);
                    table.ForeignKey(
                        name: "FK_TRUST_CONFIGURATION_TRUST_MASTER_TRUST_CODE",
                        column: x => x.TRUST_CODE,
                        principalTable: "TRUST_MASTER",
                        principalColumn: "TRUST_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRUST_FUND_TYPE",
                columns: table => new
                {
                    FUND_TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    FUND_TYPE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    FUND_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    FUND_PREFIX = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    FUND_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRUST_FUND_TYPE", x => new { x.FUND_TRUST_CODE, x.FUND_TYPE });
                    table.ForeignKey(
                        name: "FK_TRUST_FUND_TYPE_TRUST_MASTER_FUND_TRUST_CODE",
                        column: x => x.FUND_TRUST_CODE,
                        principalTable: "TRUST_MASTER",
                        principalColumn: "TRUST_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRUST_ROLE",
                columns: table => new
                {
                    TR_TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    TR_ROLE_ID = table.Column<int>(type: "int", nullable: false),
                    TR_USER_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TR_ROLE_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    TR_USER_NO = table.Column<long>(type: "bigint", nullable: false),
                    TR_EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    TR_CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    TR_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRUST_ROLE", x => new { x.TR_TRUST_CODE, x.TR_ROLE_ID, x.TR_USER_ID });
                    table.ForeignKey(
                        name: "FK_TRUST_ROLE_TRUST_MASTER_TR_TRUST_CODE",
                        column: x => x.TR_TRUST_CODE,
                        principalTable: "TRUST_MASTER",
                        principalColumn: "TRUST_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TRUST_UNITS",
                columns: table => new
                {
                    UNIT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    UNIT_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UNIT_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ADDRESS_LINE1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ADDRESS_LINE2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STATE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UNIT_HEAD_SYSID = table.Column<long>(type: "bigint", nullable: true),
                    EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    UNIT_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRUST_UNITS", x => x.UNIT_ID);
                    table.ForeignKey(
                        name: "FK_TRUST_UNITS_TRUST_MASTER_TRUST_CODE",
                        column: x => x.TRUST_CODE,
                        principalTable: "TRUST_MASTER",
                        principalColumn: "TRUST_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_TRUST_APPROVERS_LEVEL",
                table: "TRUST_APPROVERS",
                columns: new[] { "TRUST_CODE", "APPROVER_LEVEL", "APPROVER_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IX_TRUST_AUDIT_LOG_TRUST_CODE",
                table: "TRUST_AUDIT_LOG",
                column: "TRUST_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_TRUST_CONFIGURATION_TRUST_CODE",
                table: "TRUST_CONFIGURATION",
                column: "TRUST_CODE");

            migrationBuilder.CreateIndex(
                name: "IDX_TRUST_FUND_TYPE_TRUST",
                table: "TRUST_FUND_TYPE",
                columns: new[] { "FUND_TRUST_CODE", "FUND_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IDX_TRUST_MASTER_STATUS",
                table: "TRUST_MASTER",
                column: "TRUST_STATUS");

            migrationBuilder.CreateIndex(
                name: "IDX_TRUST_ROLE_USER",
                table: "TRUST_ROLE",
                columns: new[] { "TR_TRUST_CODE", "TR_USER_ID", "TR_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IDX_TRUST_UNITS_CODE",
                table: "TRUST_UNITS",
                columns: new[] { "TRUST_CODE", "UNIT_CODE", "UNIT_STATUS" });

            migrationBuilder.CreateIndex(
                name: "IX_TRUST_UNITS_UNIT_CODE",
                table: "TRUST_UNITS",
                column: "UNIT_CODE",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRUST_APPROVERS");

            migrationBuilder.DropTable(
                name: "TRUST_AUDIT_LOG");

            migrationBuilder.DropTable(
                name: "TRUST_CONFIGURATION");

            migrationBuilder.DropTable(
                name: "TRUST_FUND_TYPE");

            migrationBuilder.DropTable(
                name: "TRUST_ROLE");

            migrationBuilder.DropTable(
                name: "TRUST_UNITS");

            migrationBuilder.DropTable(
                name: "TRUST_MASTER");
        }
    }
}
