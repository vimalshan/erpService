using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevelopmentService.Infrastructure.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DD_LETPLAN",
            columns: table => new
            {
                DD_REQNUM          = table.Column<long>(nullable: false),
                DD_SNO             = table.Column<long>(nullable: true),
                DD_USERID          = table.Column<string>(maxLength: 255, nullable: true),
                DD_PINNUM          = table.Column<long>(nullable: true),
                DD_DEVSOURCE       = table.Column<string>(maxLength: 255, nullable: true),
                DD_DEVNEED         = table.Column<string>(maxLength: 255, nullable: true),
                DD_DEVINDICATOR    = table.Column<string>(maxLength: 255, nullable: true),
                DD_DEVMODE         = table.Column<long>(nullable: true),
                DD_RECPROG         = table.Column<string>(maxLength: 255, nullable: true),
                DD_TRAININGPROGRAM = table.Column<string>(maxLength: 255, nullable: true),
                DD_INTERNALTRAINING= table.Column<long>(nullable: true),
                DD_REVDATE         = table.Column<string>(maxLength: 255, nullable: true),
                DD_PRIORITY        = table.Column<long>(nullable: true),
                DD_ENTDATE         = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                DD_APPSTATUS       = table.Column<string>(maxLength: 1, nullable: true),
                DD_BHRSTATUS       = table.Column<string>(maxLength: 1, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_DD_LETPLAN", x => x.DD_REQNUM));

        migrationBuilder.CreateIndex("IDX_DD_LETPLAN_USERID",   "DD_LETPLAN", "DD_USERID");
        migrationBuilder.CreateIndex("IDX_DD_LETPLAN_PRIORITY", "DD_LETPLAN", "DD_PRIORITY");
        migrationBuilder.CreateIndex("IDX_DD_LETPLAN_STATUS",   "DD_LETPLAN", "DD_APPSTATUS");

        migrationBuilder.CreateTable(
            name: "DD_LETPLAN_PROB",
            columns: table => new
            {
                DD_REQNUM          = table.Column<long>(nullable: true),
                DD_SNO             = table.Column<long>(nullable: true),
                DD_USERID          = table.Column<string>(maxLength: 255, nullable: true),
                DD_PINNUM          = table.Column<long>(nullable: true),
                DD_DEVSOURCE       = table.Column<string>(maxLength: 255, nullable: true),
                DD_DEVNEED         = table.Column<string>(maxLength: 255, nullable: true),
                DD_DEVINDICATOR    = table.Column<string>(maxLength: 255, nullable: true),
                DD_DEVMODE         = table.Column<long>(nullable: true),
                DD_RECPROG         = table.Column<string>(maxLength: 255, nullable: true),
                DD_TRAININGPROGRAM = table.Column<string>(maxLength: 255, nullable: true),
                DD_INTERNALTRAINING= table.Column<long>(nullable: true),
                DD_REVDATE         = table.Column<string>(maxLength: 255, nullable: true),
                DD_PRIORITY        = table.Column<long>(nullable: true),
                DD_ENTDATE         = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                DD_APPSTATUS       = table.Column<string>(maxLength: 1, nullable: true),
                DD_BHRSTATUS       = table.Column<string>(maxLength: 1, nullable: true),
                DD_STRDATE         = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                DD_ENDATE          = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
            },
            constraints: table => { });

        migrationBuilder.CreateTable(
            name: "DD_LETBHRPLAN",
            columns: table => new
            {
                DD_REQNUM          = table.Column<long>(nullable: false),
                DD_SNO             = table.Column<long>(nullable: true),
                DD_USERID          = table.Column<string>(maxLength: 255, nullable: true),
                DD_TRAININGPROGRAM = table.Column<string>(maxLength: 255, nullable: true),
                DD_TRAININGCODE    = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                DD_PRIORITY        = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                DD_PINUM           = table.Column<long>(nullable: true),
                DD_FINALACCEPT     = table.Column<string>(maxLength: 255, nullable: true),
                DD_BHRACCEPT       = table.Column<string>(maxLength: 1, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_DD_LETBHRPLAN", x => x.DD_REQNUM));

        migrationBuilder.CreateTable(
            name: "DD_REQNUM_COMPE_IND",
            columns: table => new
            {
                REQNUM  = table.Column<long>(nullable: true),
                COMPNUM = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                INDNUM  = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                FLAG    = table.Column<string>(maxLength: 1, nullable: true),
                PINNUM  = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
            },
            constraints: table => { });

        migrationBuilder.CreateTable(
            name: "DD_COMPETENCY_IND",
            columns: table => new
            {
                SRL_NO   = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                BAND     = table.Column<string>(maxLength: 50, nullable: true),
                COMP_NUM = table.Column<long>(nullable: true),
                IND_FLAG = table.Column<string>(maxLength: 1, nullable: true),
                IND_DEFN = table.Column<string>(maxLength: 4000, nullable: true)
            },
            constraints: table => { });

        migrationBuilder.CreateIndex("IDX_DD_COMPETENCY_COMPNUM", "DD_COMPETENCY_IND", "COMP_NUM");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("DD_LETPLAN");
        migrationBuilder.DropTable("DD_LETPLAN_PROB");
        migrationBuilder.DropTable("DD_LETBHRPLAN");
        migrationBuilder.DropTable("DD_REQNUM_COMPE_IND");
        migrationBuilder.DropTable("DD_COMPETENCY_IND");
    }
}
