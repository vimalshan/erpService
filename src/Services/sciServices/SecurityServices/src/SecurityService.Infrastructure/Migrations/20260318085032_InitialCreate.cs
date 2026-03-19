using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecurityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCESS_ROLE",
                columns: table => new
                {
                    RA_USR_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    RA_USR_NUM = table.Column<long>(type: "bigint", nullable: true),
                    RA_ROL_COD = table.Column<long>(type: "bigint", nullable: true),
                    RA_UPD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    RA_UPD_NUM = table.Column<long>(type: "bigint", nullable: true),
                    RA_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    RA_STR_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    RA_END_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ACCESS_ROLE_MASTER",
                columns: table => new
                {
                    AR_ROL_COD = table.Column<long>(type: "bigint", nullable: true),
                    AR_ROL_NAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AR_UPD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    AR_UPD_NUM = table.Column<long>(type: "bigint", nullable: true),
                    AR_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ACCESSROLE_MENU",
                columns: table => new
                {
                    ARM_ROL_COD = table.Column<long>(type: "bigint", nullable: true),
                    ARM_MEN_COD = table.Column<long>(type: "bigint", nullable: true),
                    ARM_UPD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ARM_UPD_NUM = table.Column<long>(type: "bigint", nullable: true),
                    ARM_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MENUMASTER",
                columns: table => new
                {
                    MENU_ID = table.Column<long>(type: "bigint", nullable: true),
                    MENU_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    URL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PARENT_MENU_ID = table.Column<long>(type: "bigint", nullable: true),
                    DISPLAYORDER = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ROLE_MAST",
                columns: table => new
                {
                    RL_ROL_COD = table.Column<long>(type: "bigint", nullable: false),
                    RL_ROL_NAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RL_UPD_USR = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RL_UPD_NUM = table.Column<long>(type: "bigint", nullable: true),
                    RL_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_MAST", x => x.RL_ROL_COD);
                });

            migrationBuilder.CreateTable(
                name: "USER_MASTER",
                columns: table => new
                {
                    UM_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    UM_USR_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    UM_USR_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UM_USR_MAI = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UM_USR_PHN = table.Column<long>(type: "bigint", nullable: true),
                    UM_STR_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UM_END_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    UM_USR_TYP = table.Column<string>(type: "char(1)", nullable: true),
                    UM_UPD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    UM_UPD_NUM = table.Column<long>(type: "bigint", nullable: true),
                    UM_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_MASTER", x => x.UM_USR_NUM);
                });

            migrationBuilder.CreateTable(
                name: "USER_MASTER_MAP",
                columns: table => new
                {
                    UM_MAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    UM_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    UM_DEPT_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    UM_STR_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UM_END_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_MASTER_MAP", x => x.UM_MAP_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLE",
                columns: table => new
                {
                    UR_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    UR_ROL_COD = table.Column<long>(type: "bigint", nullable: false),
                    UR_STR_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UR_END_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    UR_UPD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    UR_UPD_NUM = table.Column<long>(type: "bigint", nullable: true),
                    UR_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLE", x => new { x.UR_USR_NUM, x.UR_ROL_COD });
                    table.ForeignKey(
                        name: "FK_USER_ROLE_ROLE_MAST_UR_ROL_COD",
                        column: x => x.UR_ROL_COD,
                        principalTable: "ROLE_MAST",
                        principalColumn: "RL_ROL_COD",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USER_ROLE_USER_MASTER_UR_USR_NUM",
                        column: x => x.UR_USR_NUM,
                        principalTable: "USER_MASTER",
                        principalColumn: "UM_USR_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLE_UR_ROL_COD",
                table: "USER_ROLE",
                column: "UR_ROL_COD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACCESS_ROLE");

            migrationBuilder.DropTable(
                name: "ACCESS_ROLE_MASTER");

            migrationBuilder.DropTable(
                name: "ACCESSROLE_MENU");

            migrationBuilder.DropTable(
                name: "MENUMASTER");

            migrationBuilder.DropTable(
                name: "USER_MASTER_MAP");

            migrationBuilder.DropTable(
                name: "USER_ROLE");

            migrationBuilder.DropTable(
                name: "ROLE_MAST");

            migrationBuilder.DropTable(
                name: "USER_MASTER");
        }
    }
}
