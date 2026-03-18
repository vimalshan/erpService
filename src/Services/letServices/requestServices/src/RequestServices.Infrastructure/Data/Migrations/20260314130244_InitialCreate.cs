using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequestServices.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "REQUEST_ACTION",
                columns: table => new
                {
                    RQ_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_REQ_ID = table.Column<long>(type: "bigint", nullable: true),
                    RQ_ACT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    RQ_KEY_EXP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RQ_USG_EXP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RQ_TIM_EXP = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    RQ_SUP_EXP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RQ_CAN_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    RQ_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    RQ_ENT_USR = table.Column<string>(type: "char(1)", nullable: true),
                    RQ_REV_USR = table.Column<string>(type: "char(1)", nullable: true),
                    RQ_REV_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RQ_REV_NOS = table.Column<long>(type: "bigint", nullable: true),
                    RQ_CRS_ID = table.Column<long>(type: "bigint", nullable: true),
                    RQ_ACT_FLG = table.Column<string>(type: "char(1)", nullable: true),
                    RQ_CAN_REM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_ACTION", x => x.RQ_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "REQUEST_APP",
                columns: table => new
                {
                    RQ_REQ_ID = table.Column<long>(type: "bigint", nullable: false),
                    RQ_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_APP_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RQ_APP_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_APP_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RQ_APP_USR = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_APP", x => new { x.RQ_REQ_ID, x.RQ_SRL_NUM });
                });

            migrationBuilder.CreateTable(
                name: "REQUEST_MAIN",
                columns: table => new
                {
                    RQ_REQ_ID = table.Column<long>(type: "bigint", nullable: false),
                    RQ_EMP_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    RQ_REQ_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RQ_SUP_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_MAIN", x => x.RQ_REQ_ID);
                });

            migrationBuilder.CreateTable(
                name: "REQUEST_NEW",
                columns: table => new
                {
                    RQ_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_REQ_ID = table.Column<long>(type: "bigint", nullable: false),
                    RQ_SKL_NAM = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RQ_LVL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_FNC_DES = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RQ_CAT_COD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RQ_SKL_TYP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RQ_STS_COD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RQ_CRS_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_NEW", x => x.RQ_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "REQUEST_SUB",
                columns: table => new
                {
                    RQ_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_REQ_ID = table.Column<long>(type: "bigint", nullable: false),
                    RQ_REQ_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RQ_MOD_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RQ_MOD_USR = table.Column<string>(type: "char(1)", nullable: false),
                    RQ_REQ_SRC = table.Column<string>(type: "char(1)", nullable: false),
                    RQ_MOD_TRN = table.Column<string>(type: "char(1)", nullable: false),
                    RQ_GOL_DES = table.Column<string>(type: "char(1)", nullable: false),
                    RQ_STS_COD = table.Column<string>(type: "char(1)", nullable: false),
                    RQ_TRN_NED = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_CAN_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    RQ_CAN_REM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RQ_MEN_USR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_MEN_REM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_CRS_ID = table.Column<long>(type: "bigint", nullable: false),
                    RQ_APP_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_REV_DYS = table.Column<long>(type: "bigint", nullable: false),
                    RQ_REV_USR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_REV_MOD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_STR_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RQ_END_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    RQ_REF_REQ = table.Column<long>(type: "bigint", nullable: false),
                    RQ_REF_SRL = table.Column<long>(type: "bigint", nullable: false),
                    RQ_SUP_USR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_ENT_USR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_ENT_MOD = table.Column<string>(type: "char(1)", nullable: false),
                    RQ_APP_TIM = table.Column<long>(type: "bigint", nullable: false),
                    RQ_BUS_BEN = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_EXP_CNP = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_CRS_DES = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RQ_CRS_AVL = table.Column<string>(type: "char(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REQUEST_SUB", x => x.RQ_SRL_NUM);
                    table.ForeignKey(
                        name: "FK_REQUEST_SUB_REQUEST_MAIN_RQ_REQ_ID",
                        column: x => x.RQ_REQ_ID,
                        principalTable: "REQUEST_MAIN",
                        principalColumn: "RQ_REQ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_REQUEST_APP_REQ_ID",
                table: "REQUEST_APP",
                column: "RQ_REQ_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_REQUEST_MAIN_EMP_USR",
                table: "REQUEST_MAIN",
                column: "RQ_EMP_USR");

            migrationBuilder.CreateIndex(
                name: "IDX_REQUEST_MAIN_SUP_USR",
                table: "REQUEST_MAIN",
                column: "RQ_SUP_USR");

            migrationBuilder.CreateIndex(
                name: "IDX_REQUEST_SUB_REQ_ID",
                table: "REQUEST_SUB",
                column: "RQ_REQ_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "REQUEST_ACTION");

            migrationBuilder.DropTable(
                name: "REQUEST_APP");

            migrationBuilder.DropTable(
                name: "REQUEST_NEW");

            migrationBuilder.DropTable(
                name: "REQUEST_SUB");

            migrationBuilder.DropTable(
                name: "REQUEST_MAIN");
        }
    }
}
