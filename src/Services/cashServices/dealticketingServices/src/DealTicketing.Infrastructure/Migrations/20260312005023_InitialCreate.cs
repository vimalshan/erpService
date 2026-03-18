using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealTicketing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DEAL_BANKMASTER",
                columns: table => new
                {
                    BANK_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BANK_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BANK_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BANK_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BANK_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    BANK_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_BANKMASTER", x => x.BANK_ID);
                });

            migrationBuilder.CreateTable(
                name: "DEAL_CATEGORYMASTER",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CATEGORY_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CATEGORY_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    CATEGORY_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CATEGORY_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_CATEGORYMASTER", x => x.CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "DEAL_LOVMASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOV_TYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LOV_NAME = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_LOVMASTER", x => x.LOV_ID);
                });

            migrationBuilder.CreateTable(
                name: "DEALTICKET_BATCH",
                columns: table => new
                {
                    DEAL_BATCHID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEAL_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DEAL_DERTYPE = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_SCREENSHOT = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DEAL_BOOKEDBY = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_BANKTRADER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DEAL_BANKID = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_OPTIONTYPE = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_BUSINESSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DEAL_REJSTATUS = table.Column<string>(type: "char(1)", nullable: true),
                    DEAL_REJREASON = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DEAL_ERRREMARKS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DEAL_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DEAL_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DEAL_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEALTICKET_BATCH", x => x.DEAL_BATCHID);
                    table.ForeignKey(
                        name: "FK_DEALTICKET_BATCH_BANKMASTER",
                        column: x => x.DEAL_BANKID,
                        principalTable: "DEAL_BANKMASTER",
                        principalColumn: "BANK_ID");
                });

            migrationBuilder.CreateTable(
                name: "DEALTICKET_DET",
                columns: table => new
                {
                    DEAL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEAL_NO = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_VERSIONID = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_BATCHID = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_TRANTYPE = table.Column<string>(type: "char(1)", nullable: true),
                    DEAL_POSITION = table.Column<string>(type: "char(2)", nullable: true),
                    DEAL_ENTRYDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DEAL_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_BANKID = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_CURRENCY1 = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_CURRENCY2 = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_SPOTRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_FORPOINTS = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_BANKMARGIN = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_BOOKRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_MATDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DEAL_DEALTYPE = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_BUSINESS = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_CATEGORY = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_STRIKEPRICE = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_PPLMITOUT = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_APPSTATUS = table.Column<string>(type: "char(1)", nullable: true),
                    DEAL_APPREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DEAL_ERRREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DEAL_CORRECTNESS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DEAL_SIGNED = table.Column<string>(type: "char(1)", nullable: true),
                    DEAL_APPBUSINESS = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_DEALCONFNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DEAL_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DEAL_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DEAL_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DEAL_IRLOAN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DEAL_IRTYPE = table.Column<string>(type: "char(3)", nullable: true),
                    DEAL_STARTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DEAL_NOTPRINCIPAL = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_IRSTYPE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DEAL_TOPAY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DEAL_TOREC = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DEAL_RATESCREENSHOT = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DEAL_RATEPER = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_LOANAMT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DEAL_LOANCURRENCY = table.Column<long>(type: "bigint", nullable: true),
                    DEAL_SETAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_CANAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_ROLLAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_SETSTATUS = table.Column<string>(type: "char(1)", nullable: true),
                    DEAL_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DEAL_NETBASISPOINT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    DEAL_ROLLOVERDEALNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DEAL_BOOKINGCHARGES = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DEAL_SENTOBANK = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEALTICKET_DET", x => x.DEAL_ID);
                    table.ForeignKey(
                        name: "FK_DEALTICKET_DET_BANKMASTER",
                        column: x => x.DEAL_BANKID,
                        principalTable: "DEAL_BANKMASTER",
                        principalColumn: "BANK_ID");
                    table.ForeignKey(
                        name: "FK_DEALTICKET_DET_BATCH",
                        column: x => x.DEAL_BATCHID,
                        principalTable: "DEALTICKET_BATCH",
                        principalColumn: "DEAL_BATCHID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DEALTICKET_LOANSCH",
                columns: table => new
                {
                    DEAL_SCHID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEAL_ID = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_SCHDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DEAL_SCHAMT = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEALTICKET_LOANSCH", x => x.DEAL_SCHID);
                    table.ForeignKey(
                        name: "FK_DEALTICKET_LOANSCH_DET",
                        column: x => x.DEAL_ID,
                        principalTable: "DEALTICKET_DET",
                        principalColumn: "DEAL_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DEALTICKET_SET",
                columns: table => new
                {
                    SET_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SET_DEALID = table.Column<long>(type: "bigint", nullable: false),
                    SET_SPOTRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SET_MONEYTYPE = table.Column<string>(type: "char(3)", nullable: true),
                    SET_EXCTYPE = table.Column<string>(type: "char(1)", nullable: true),
                    SET_GAINLOSSAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    SET_TYPE = table.Column<string>(type: "char(3)", nullable: true),
                    SET_CANDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SET_PREMIUMRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_PREMIUMAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_IRDAYS = table.Column<long>(type: "bigint", nullable: true),
                    SET_IRSTARTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SET_IRAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_WINDFEE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_WINDRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_CREDITDEBIT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    SET_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SET_EXCHANGERATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_ACTGAINLOSSAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SET_DCDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SET_DCAMNT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    SET_BANKNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SET_BANKACNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEALTICKET_SET", x => x.SET_ID);
                    table.ForeignKey(
                        name: "FK_DEALTICKET_SET_DET",
                        column: x => x.SET_DEALID,
                        principalTable: "DEALTICKET_DET",
                        principalColumn: "DEAL_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DEATICKET_ATTACHMENT",
                columns: table => new
                {
                    DEAL_ATTACHMENTID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEAL_BATCHID = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_ID = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_ATTACHMENTTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DEAL_ATTACHMENTFILE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEATICKET_ATTACHMENT", x => x.DEAL_ATTACHMENTID);
                    table.ForeignKey(
                        name: "FK_DEATICKET_ATTACHMENT_BATCH",
                        column: x => x.DEAL_BATCHID,
                        principalTable: "DEALTICKET_BATCH",
                        principalColumn: "DEAL_BATCHID");
                    table.ForeignKey(
                        name: "FK_DEATICKET_ATTACHMENT_DET",
                        column: x => x.DEAL_ID,
                        principalTable: "DEALTICKET_DET",
                        principalColumn: "DEAL_ID");
                });

            migrationBuilder.CreateTable(
                name: "DEATICKETSET_ATTACHMENT",
                columns: table => new
                {
                    DEAL_ATTACHMENTID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEAL_SETID = table.Column<long>(type: "bigint", nullable: false),
                    DEAL_ATTACHMENTTYPE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DEAL_ATTACHMENTFILE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEATICKETSET_ATTACHMENT", x => x.DEAL_ATTACHMENTID);
                    table.ForeignKey(
                        name: "FK_DEATICKETSET_ATTACHMENT_SET",
                        column: x => x.DEAL_SETID,
                        principalTable: "DEALTICKET_SET",
                        principalColumn: "SET_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEALTICKET_BATCH_BANKID",
                table: "DEALTICKET_BATCH",
                column: "DEAL_BANKID");

            migrationBuilder.CreateIndex(
                name: "IX_DEALTICKET_BATCH_DATE",
                table: "DEALTICKET_BATCH",
                column: "DEAL_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_DEALTICKET_DET_BATCHID",
                table: "DEALTICKET_DET",
                column: "DEAL_BATCHID");

            migrationBuilder.CreateIndex(
                name: "IX_DEALTICKET_DET_DEAL_BANKID",
                table: "DEALTICKET_DET",
                column: "DEAL_BANKID");

            migrationBuilder.CreateIndex(
                name: "IX_DEALTICKET_DET_DEALID",
                table: "DEALTICKET_DET",
                column: "DEAL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DEALTICKET_LOANSCH_DEAL_ID",
                table: "DEALTICKET_LOANSCH",
                column: "DEAL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DEALTICKET_SET_DEALID",
                table: "DEALTICKET_SET",
                column: "SET_DEALID");

            migrationBuilder.CreateIndex(
                name: "IX_DEATICKET_ATTACHMENT_DEAL_BATCHID",
                table: "DEATICKET_ATTACHMENT",
                column: "DEAL_BATCHID");

            migrationBuilder.CreateIndex(
                name: "IX_DEATICKET_ATTACHMENT_DEALID",
                table: "DEATICKET_ATTACHMENT",
                column: "DEAL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DEATICKETSET_ATTACHMENT_DEAL_SETID",
                table: "DEATICKETSET_ATTACHMENT",
                column: "DEAL_SETID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEAL_CATEGORYMASTER");

            migrationBuilder.DropTable(
                name: "DEAL_LOVMASTER");

            migrationBuilder.DropTable(
                name: "DEALTICKET_LOANSCH");

            migrationBuilder.DropTable(
                name: "DEATICKET_ATTACHMENT");

            migrationBuilder.DropTable(
                name: "DEATICKETSET_ATTACHMENT");

            migrationBuilder.DropTable(
                name: "DEALTICKET_SET");

            migrationBuilder.DropTable(
                name: "DEALTICKET_DET");

            migrationBuilder.DropTable(
                name: "DEALTICKET_BATCH");

            migrationBuilder.DropTable(
                name: "DEAL_BANKMASTER");
        }
    }
}
