using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationSetup.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DEAL_ORGPARAMS",
                columns: table => new
                {
                    ORG_PARAMID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORG_PARAMTYPE = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    ORG_PARAMVALUE = table.Column<long>(type: "bigint", nullable: false),
                    ORG_ID = table.Column<long>(type: "bigint", nullable: false),
                    ORG_MODIFIEDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ORG_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_ORGPARAMS", x => x.ORG_PARAMID);
                });

            migrationBuilder.CreateTable(
                name: "DEAL_PPLIMIT",
                columns: table => new
                {
                    PP_LIMITID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PP_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    PP_TRANTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PP_BASCURR = table.Column<long>(type: "bigint", nullable: false),
                    PP_LIMITAMT = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: true),
                    PP_FINYEAR = table.Column<int>(type: "int", nullable: false),
                    PP_LIMITACT = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: true),
                    PP_CERTIFICATEUPLOAD = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PP_MODIFIEDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PP_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_PPLIMIT", x => x.PP_LIMITID);
                });

            migrationBuilder.CreateTable(
                name: "DEAL_ROLE",
                columns: table => new
                {
                    ROLE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ROLE_LEVEL = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_MODIFIEDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ROLE_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_ROLE", x => x.ROLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "DEAL_USERMAP",
                columns: table => new
                {
                    ROLE_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_ID = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_BUSINESS = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_USERMAP", x => x.ROLE_MAPID);
                    table.ForeignKey(
                        name: "FK_DEAL_USERMAP_ROLE",
                        column: x => x.ROLE_ID,
                        principalTable: "DEAL_ROLE",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_ORGPARAMS_ORGID",
                table: "DEAL_ORGPARAMS",
                column: "ORG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_ORGPARAMS_PARAMTYPE",
                table: "DEAL_ORGPARAMS",
                column: "ORG_PARAMTYPE");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_PPLIMIT_FINYEAR",
                table: "DEAL_PPLIMIT",
                column: "PP_FINYEAR");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_PPLIMIT_ORGID",
                table: "DEAL_PPLIMIT",
                column: "PP_ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_ROLE_NAME",
                table: "DEAL_ROLE",
                column: "ROLE_NAME");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_USERMAP_EMPID",
                table: "DEAL_USERMAP",
                column: "ROLE_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_USERMAP_ORGID",
                table: "DEAL_USERMAP",
                column: "ROLE_ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_USERMAP_ROLE_ID",
                table: "DEAL_USERMAP",
                column: "ROLE_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEAL_ORGPARAMS");

            migrationBuilder.DropTable(
                name: "DEAL_PPLIMIT");

            migrationBuilder.DropTable(
                name: "DEAL_USERMAP");

            migrationBuilder.DropTable(
                name: "DEAL_ROLE");
        }
    }
}
