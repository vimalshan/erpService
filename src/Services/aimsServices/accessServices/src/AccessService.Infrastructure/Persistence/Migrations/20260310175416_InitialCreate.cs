using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AIMS_USERMAP",
                columns: table => new
                {
                    USER_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    USER_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USER_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USER_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    USER_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIMS_USERMAP", x => x.USER_EMPSYSID);
                });

            migrationBuilder.CreateTable(
                name: "AIMS_USERMENUMAP",
                columns: table => new
                {
                    USER_ROLEID = table.Column<int>(type: "int", nullable: true),
                    USER_MENUID = table.Column<int>(type: "int", nullable: true),
                    USER_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    USER_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AIMS_USERROLE",
                columns: table => new
                {
                    ROLE_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_EMPSYSID = table.Column<long>(type: "bigint", nullable: true),
                    ROLE_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ROLE_MENUACCESS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ROLE_ORGID = table.Column<int>(type: "int", nullable: true),
                    ROLE_UNITID = table.Column<int>(type: "int", nullable: true),
                    ROLE_CALENDARID = table.Column<long>(type: "bigint", nullable: true),
                    ROLE_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ROLE_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ROLE_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    ROLE_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIMS_USERROLE", x => x.ROLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "MENU_MASTER",
                columns: table => new
                {
                    MENU_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Menu_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MENU_PARENTID = table.Column<int>(type: "int", nullable: true),
                    Menu_PATH = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MENU_CALENDARROLE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MENU_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    MENU_DISPLAYORDER = table.Column<int>(type: "int", nullable: true),
                    MENU_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    MENU_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_MASTER", x => x.MENU_ID);
                });

            migrationBuilder.CreateTable(
                name: "SPARSHMENU_ACCESS",
                columns: table => new
                {
                    ACCESS_ID = table.Column<long>(type: "bigint", nullable: false),
                    ACCESS_UNIT = table.Column<long>(type: "bigint", nullable: false),
                    ACCESS_CALENDAR = table.Column<long>(type: "bigint", nullable: false),
                    ACCESS_GRADECATEGORY = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ACCESS_SPARSHMENUID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPARSHMENU_ACCESS", x => x.ACCESS_ID);
                });

            migrationBuilder.CreateTable(
                name: "SPARSHMENU_MASTER",
                columns: table => new
                {
                    SPARSHMENU_ID = table.Column<long>(type: "bigint", nullable: false),
                    SPARSHMENU_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SPARSHMENU_PAGENAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SPARSHMENU_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SPARSHMENU_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPARSHMENU_MASTER", x => x.SPARSHMENU_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIMS_USERROLE_EMPSYSID",
                table: "AIMS_USERROLE",
                column: "ROLE_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_MASTER_PARENT",
                table: "MENU_MASTER",
                column: "MENU_PARENTID");

            migrationBuilder.CreateIndex(
                name: "IX_SPARSHMENU_ACCESS_UNIT",
                table: "SPARSHMENU_ACCESS",
                column: "ACCESS_UNIT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIMS_USERMAP");

            migrationBuilder.DropTable(
                name: "AIMS_USERMENUMAP");

            migrationBuilder.DropTable(
                name: "AIMS_USERROLE");

            migrationBuilder.DropTable(
                name: "MENU_MASTER");

            migrationBuilder.DropTable(
                name: "SPARSHMENU_ACCESS");

            migrationBuilder.DropTable(
                name: "SPARSHMENU_MASTER");
        }
    }
}
