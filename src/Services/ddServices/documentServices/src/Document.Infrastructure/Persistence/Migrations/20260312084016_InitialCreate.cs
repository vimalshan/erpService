using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Document.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DD_ANNEXURE1",
                columns: table => new
                {
                    DD_CRT_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_USR_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_USR_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_AN1_PR1 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_AN2_PR2 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_AN3_PR3 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_AN4_PR4 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_SIG_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_SIG_DSG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_USR_RNM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_USR_UNT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_PRN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_APR_LMP = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_BAS = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_FLX = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_ANNEXURE2",
                columns: table => new
                {
                    DD_CRT_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_USR_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_USR_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_BAS_OLD = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_BAS_NEW = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_FLX_PAY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_SIG_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_SIG_DSG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_PRN_DAT = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DD_BND_NAM = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_APPRAISALLETTER",
                columns: table => new
                {
                    DD_SRL_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DD_APR_BND = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_TYP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DD_APR_FRM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_APR_END = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_APR_PR1 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR2 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR3 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR4 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR5 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_BAS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_PRN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_APPRAISALLETTER", x => x.DD_SRL_NO);
                });

            migrationBuilder.CreateTable(
                name: "DD_APPRAISALLETTER_NEW",
                columns: table => new
                {
                    DD_SRL_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DD_APR_BND = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_TYP = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    DD_APR_FRM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_APR_END = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_APR_PR1 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR2 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR3 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR4 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR5 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_APR_PR6 = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DD_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_BAS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_PRN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_LET_TYP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_APPRAISALLETTER_NEW", x => x.DD_SRL_NO);
                });

            migrationBuilder.CreateTable(
                name: "DD_GENERATELETTER",
                columns: table => new
                {
                    DD_CRT_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_USR_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_USR_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_SIG_NAM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_SIG_DSG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_USR_RNM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_USR_UNT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_PRN_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_APR_LMP = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_BAS = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_FLX = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DD_LETTERTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DD_FINALRATING = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DD_APR_INC = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_PRM_LEVEL = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_APR_DSG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_APR_BND = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_SIG_NAM2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DD_SIG_DSG2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_INC_TEMPID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_RAT_TEMPID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DD_SIGNATORY",
                columns: table => new
                {
                    DD_SIG_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DD_SIG_NAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DD_SIG_DSG = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DD_LIVE_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DD_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_SIG_IMG = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DD_DIGITALSIGN_PFXFILENAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DD_DIGITALSIGN_PFXPASSWORD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_SIG_IMGALT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_SIGNATORY", x => x.DD_SIG_NUM);
                });

            migrationBuilder.CreateTable(
                name: "DDLETTER_LOGHISTORY",
                columns: table => new
                {
                    DDLETTER_LOGSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DDLETTER_IPADDRESS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DDLETTER_OPENEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DDLETTER_FINYEARID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DDLETTER_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DDLETTER_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DD_ANNEXURE1");

            migrationBuilder.DropTable(
                name: "DD_ANNEXURE2");

            migrationBuilder.DropTable(
                name: "DD_APPRAISALLETTER");

            migrationBuilder.DropTable(
                name: "DD_APPRAISALLETTER_NEW");

            migrationBuilder.DropTable(
                name: "DD_GENERATELETTER");

            migrationBuilder.DropTable(
                name: "DD_SIGNATORY");

            migrationBuilder.DropTable(
                name: "DDLETTER_LOGHISTORY");
        }
    }
}
