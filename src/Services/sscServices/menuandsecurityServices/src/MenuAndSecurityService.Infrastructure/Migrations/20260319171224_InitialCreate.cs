using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuAndSecurityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MENU_MASTER",
                columns: table => new
                {
                    MENU_ID = table.Column<long>(type: "bigint", nullable: false),
                    MENU_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MENU_PAGENAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MENU_PARENTID = table.Column<long>(type: "bigint", nullable: true),
                    MENU_DISPLAYORDER = table.Column<int>(type: "int", nullable: false),
                    MENU_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MENU_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_MASTER", x => x.MENU_ID);
                    table.ForeignKey(
                        name: "FK_MENU_MASTER_MENU_MASTER_MENU_PARENTID",
                        column: x => x.MENU_PARENTID,
                        principalTable: "MENU_MASTER",
                        principalColumn: "MENU_ID");
                });

            migrationBuilder.CreateTable(
                name: "ROLE_MENUACCESS",
                columns: table => new
                {
                    MENU_ACCESSID = table.Column<long>(type: "bigint", nullable: false),
                    MENU_ID = table.Column<long>(type: "bigint", nullable: false),
                    MENU_ROLEID = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    ROLE_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_MENUACCESS", x => x.MENU_ACCESSID);
                    table.ForeignKey(
                        name: "FK_ROLE_MENUACCESS_MENU_MASTER_MENU_ID",
                        column: x => x.MENU_ID,
                        principalTable: "MENU_MASTER",
                        principalColumn: "MENU_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MENU_MASTER_MENU_PARENTID",
                table: "MENU_MASTER",
                column: "MENU_PARENTID");

            migrationBuilder.CreateIndex(
                name: "IX_ROLE_MENUACCESS_MENU_ID",
                table: "ROLE_MENUACCESS",
                column: "MENU_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROLE_MENUACCESS");

            migrationBuilder.DropTable(
                name: "MENU_MASTER");
        }
    }
}
