using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileAppManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOB_APPDEVICE_DETAILS",
                columns: table => new
                {
                    MD_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MD_DEVICEID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MD_ACTIVE = table.Column<string>(type: "char(1)", nullable: false),
                    MD_DEVICETYPE = table.Column<string>(type: "char(1)", nullable: true),
                    MD_IMEINO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MD_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MD_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MD_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOB_APPDEVICE_DETAILS", x => new { x.MD_EMPSYSID, x.MD_DEVICEID });
                });

            migrationBuilder.CreateTable(
                name: "MOB_LOGINDET",
                columns: table => new
                {
                    LD_LOGINID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LD_USERSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LD_DEVICEID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LD_LOGON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LD_GUID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LD_IMEINO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LD_DEVICETYPE = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOB_LOGINDET", x => x.LD_LOGINID);
                });

            migrationBuilder.CreateTable(
                name: "MOBAPP_REGISTER",
                columns: table => new
                {
                    REGISTER_ID = table.Column<long>(type: "bigint", nullable: false),
                    REGISTER_EMPSYSID = table.Column<long>(type: "bigint", nullable: true),
                    REGISTER_USERID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REGISTER_USERSYSID = table.Column<long>(type: "bigint", nullable: true),
                    REGISTER_USERTYPE = table.Column<string>(type: "char(1)", nullable: true),
                    REGISTER_PINNO = table.Column<long>(type: "bigint", nullable: true),
                    REGISTER_PINGENERATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    REGISTER_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    REGISTER_STATUS = table.Column<string>(type: "char(1)", nullable: true),
                    REGISTER_MOBILENO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REGISTER_IMEINO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REGISTER_GUID = table.Column<string>(type: "char(1)", nullable: true),
                    REGISTER_DEVICEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REGISTER_DTYPE = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOBAPP_REGISTER", x => x.REGISTER_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOB_APPDEVICE_ACTIVE",
                table: "MOB_APPDEVICE_DETAILS",
                column: "MD_ACTIVE");

            migrationBuilder.CreateIndex(
                name: "IX_MOB_APPDEVICE_DEVICE",
                table: "MOB_APPDEVICE_DETAILS",
                column: "MD_DEVICEID");

            migrationBuilder.CreateIndex(
                name: "IX_MOB_LOGIN_DEVICE",
                table: "MOB_LOGINDET",
                column: "LD_DEVICEID");

            migrationBuilder.CreateIndex(
                name: "IX_MOB_LOGIN_LOGON",
                table: "MOB_LOGINDET",
                column: "LD_LOGON");

            migrationBuilder.CreateIndex(
                name: "IX_MOB_LOGIN_USERID",
                table: "MOB_LOGINDET",
                column: "LD_USERSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_MOBAPP_REG_STATUS",
                table: "MOBAPP_REGISTER",
                column: "REGISTER_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_MOBAPP_REG_USERID",
                table: "MOBAPP_REGISTER",
                column: "REGISTER_USERID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOB_APPDEVICE_DETAILS");

            migrationBuilder.DropTable(
                name: "MOB_LOGINDET");

            migrationBuilder.DropTable(
                name: "MOBAPP_REGISTER");
        }
    }
}
