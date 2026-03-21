using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ADMIN_FINUSERMAP",
                columns: table => new
                {
                    FINANCE_MAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FINANCE_PAYUNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FINANCE_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FINANCE_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FINANCE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN_FINUSERMAP", x => x.FINANCE_MAPID);
                });

            migrationBuilder.CreateTable(
                name: "ADMIN_MASTER",
                columns: table => new
                {
                    ADMIN_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_PIC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_UNITID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_UNITHEADSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_LOCSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN_MASTER", x => x.ADMIN_ID);
                });

            migrationBuilder.CreateTable(
                name: "ADMIN_ACCESSRIGHTS",
                columns: table => new
                {
                    ADMIN_RIGHTSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_LOCATIONID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_RIGHTSFOR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_RIGHTSTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_USERID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_ALERTID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_CONTACTNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_CONTACTDES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_ENTON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ADMIN_ENTBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN_ACCESSRIGHTS", x => x.ADMIN_RIGHTSID);
                    table.ForeignKey(
                        name: "FK_ADMIN_ACCESSRIGHTS_ADMIN_MASTER_ADMIN_LOCATIONID",
                        column: x => x.ADMIN_LOCATIONID,
                        principalTable: "ADMIN_MASTER",
                        principalColumn: "ADMIN_ID");
                });

            migrationBuilder.CreateTable(
                name: "ADMIN_USERMAP",
                columns: table => new
                {
                    ADMIN_MAPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_BOOKTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_MODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN_USERMAP", x => x.ADMIN_MAPID);
                    table.ForeignKey(
                        name: "FK_ADMIN_USERMAP_ADMIN_MASTER_ADMIN_ID",
                        column: x => x.ADMIN_ID,
                        principalTable: "ADMIN_MASTER",
                        principalColumn: "ADMIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ADMIN_ACCESSRIGHTSLOG",
                columns: table => new
                {
                    ADMIN_LOGID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_RIGHTSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ADMIN_LOCATIONID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_RIGHTSFOR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_RIGHTSTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_USERID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_ALERTID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_CONTACTNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_CONTACTDES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ADMIN_ENTON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    ADMIN_ENTBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADMIN_ACCESSRIGHTSLOG", x => new { x.ADMIN_RIGHTSID, x.ADMIN_LOGID });
                    table.ForeignKey(
                        name: "FK_ADMIN_ACCESSRIGHTSLOG_ADMIN_ACCESSRIGHTS_ADMIN_RIGHTSID",
                        column: x => x.ADMIN_RIGHTSID,
                        principalTable: "ADMIN_ACCESSRIGHTS",
                        principalColumn: "ADMIN_RIGHTSID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADMIN_ACCESSRIGHTS_ADMIN_LOCATIONID",
                table: "ADMIN_ACCESSRIGHTS",
                column: "ADMIN_LOCATIONID");

            migrationBuilder.CreateIndex(
                name: "IX_ADMIN_USERMAP_ADMIN_ID",
                table: "ADMIN_USERMAP",
                column: "ADMIN_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ADMIN_ACCESSRIGHTSLOG");

            migrationBuilder.DropTable(
                name: "ADMIN_FINUSERMAP");

            migrationBuilder.DropTable(
                name: "ADMIN_USERMAP");

            migrationBuilder.DropTable(
                name: "ADMIN_ACCESSRIGHTS");

            migrationBuilder.DropTable(
                name: "ADMIN_MASTER");
        }
    }
}
