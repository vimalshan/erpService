using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamServices.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TEAM_MASTER",
                columns: table => new
                {
                    TEAM_ID = table.Column<long>(type: "bigint", nullable: false),
                    TEAM_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TEAM_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TEAM_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEAM_MASTER", x => x.TEAM_ID);
                });

            migrationBuilder.CreateTable(
                name: "TEAM_EMPMAP",
                columns: table => new
                {
                    TEAMEMP_ID = table.Column<long>(type: "bigint", nullable: false),
                    TEAMEMP_TEAMID = table.Column<long>(type: "bigint", nullable: false),
                    TEAMEMP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    TEAMEMP_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TEAMEMP_CLOSEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    TEAMEMP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TEAMEMP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEAM_EMPMAP", x => x.TEAMEMP_ID);
                    table.ForeignKey(
                        name: "FK_TEAM_EMPMAP_TEAM_MASTER_TEAMEMP_TEAMID",
                        column: x => x.TEAMEMP_TEAMID,
                        principalTable: "TEAM_MASTER",
                        principalColumn: "TEAM_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TEAM_UNITMAP",
                columns: table => new
                {
                    TEAM_MAPID = table.Column<long>(type: "bigint", nullable: false),
                    TEAM_ID = table.Column<long>(type: "bigint", nullable: false),
                    TEAM_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    TEAM_GRADECATEGORY = table.Column<string>(type: "char(1)", nullable: false),
                    TEAM_CADREID = table.Column<long>(type: "bigint", nullable: true),
                    TEAM_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TEAM_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEAM_UNITMAP", x => x.TEAM_MAPID);
                    table.ForeignKey(
                        name: "FK_TEAM_UNITMAP_TEAM_MASTER_TEAM_ID",
                        column: x => x.TEAM_ID,
                        principalTable: "TEAM_MASTER",
                        principalColumn: "TEAM_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TEAM_EMPMAP_TEAMEMP_TEAMID",
                table: "TEAM_EMPMAP",
                column: "TEAMEMP_TEAMID");

            migrationBuilder.CreateIndex(
                name: "IX_TEAM_UNITMAP_TEAM_ID",
                table: "TEAM_UNITMAP",
                column: "TEAM_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TEAM_EMPMAP");

            migrationBuilder.DropTable(
                name: "TEAM_UNITMAP");

            migrationBuilder.DropTable(
                name: "TEAM_MASTER");
        }
    }
}
