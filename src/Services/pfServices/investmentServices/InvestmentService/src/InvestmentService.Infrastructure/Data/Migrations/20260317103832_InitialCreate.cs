using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestmentService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CREDITAGENCY_MAST",
                columns: table => new
                {
                    AGENCY_ID = table.Column<int>(type: "int", nullable: false),
                    AGENCY_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CREDITAGENCY_MAST", x => x.AGENCY_ID);
                });

            migrationBuilder.CreateTable(
                name: "CREDITRATING_MAST",
                columns: table => new
                {
                    RATING_ID = table.Column<int>(type: "int", nullable: false),
                    RATING_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CREDITRATING_MAST", x => x.RATING_ID);
                });

            migrationBuilder.CreateTable(
                name: "INV_INTSCHBATCH",
                columns: table => new
                {
                    INV_INTSCHBATHNO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INV_INTSCHINVID = table.Column<long>(type: "bigint", nullable: true),
                    INV_INVSCHYEAR = table.Column<long>(type: "bigint", nullable: true),
                    INV_INVSCHPREVRUNDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_INVSCHLASTRUNDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_INVSCHENTON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_INVSCHENTBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_INTSCHBATCH", x => x.INV_INTSCHBATHNO);
                });

            migrationBuilder.CreateTable(
                name: "INVBROKER_MASTER",
                columns: table => new
                {
                    BROKER_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    BROKER_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BROKER_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVBROKER_MASTER", x => x.BROKER_ID);
                });

            migrationBuilder.CreateTable(
                name: "INVCAT_MAST",
                columns: table => new
                {
                    INVCAT_CODE = table.Column<int>(type: "int", nullable: false),
                    INVCAT_SHTCODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    INVCAT_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    INVCAT_DENOM = table.Column<long>(type: "bigint", nullable: false),
                    INVCAT_GRPID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVCAT_MAST", x => x.INVCAT_CODE);
                });

            migrationBuilder.CreateTable(
                name: "INVSUBCAT_MAST",
                columns: table => new
                {
                    INVSUBCAT_ID = table.Column<int>(type: "int", nullable: false),
                    INVSUBCAT_SHTNAME = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    INVSUBCAT_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    INVSUBCAT_CATID = table.Column<int>(type: "int", nullable: false),
                    INVSUBCAT_INTDEN = table.Column<long>(type: "bigint", nullable: true),
                    INVSUBCAT_SUBCAT = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVSUBCAT_MAST", x => x.INVSUBCAT_ID);
                    table.ForeignKey(
                        name: "FK_INVSUBCAT_MAST_INVCAT_MAST_INVSUBCAT_CATID",
                        column: x => x.INVSUBCAT_CATID,
                        principalTable: "INVCAT_MAST",
                        principalColumn: "INVCAT_CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INV_MAIN",
                columns: table => new
                {
                    INV_NO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INV_GRPID = table.Column<int>(type: "int", nullable: true),
                    INV_CATID = table.Column<int>(type: "int", nullable: true),
                    INV_SUBCATID = table.Column<int>(type: "int", nullable: true),
                    INV_TENURE = table.Column<int>(type: "int", nullable: true),
                    INV_TENUREDAYS = table.Column<int>(type: "int", nullable: true),
                    INV_INTOPTION = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    INV_ORGPURDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_MATDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_CALLPUTOPTION = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    INV_PURDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_CALLPER = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_UNITS = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_PURRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_FACEVALUE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_PREMIUM = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_ISSINTRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_REVINTFROM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_REVINTRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_INTDENOM = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_PURVALUE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_SECMARKET = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    INV_BROKERID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    INV_CUMINTFROM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_CUMINTTO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_CUMINTAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_CUMINTDYS = table.Column<int>(type: "int", nullable: true),
                    INV_CRAGENCY1 = table.Column<int>(type: "int", nullable: true),
                    INV_CRAGENCY2 = table.Column<int>(type: "int", nullable: true),
                    INV_RATING1 = table.Column<int>(type: "int", nullable: true),
                    INV_RATING2 = table.Column<int>(type: "int", nullable: true),
                    INV_CLIENTID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    INV_INTFREQUENCY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    INV_PAYMODE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    INV_INTDATES = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    INV_BANKID = table.Column<int>(type: "int", nullable: true),
                    INV_CHQNUM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    INV_CHQDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_BANKCHARGES = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    INV_CERTNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    INV_ENTEREDBY = table.Column<long>(type: "bigint", nullable: true),
                    INV_ENTEREDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_LASTMODBY = table.Column<long>(type: "bigint", nullable: true),
                    INV_LASTMODON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_LASTSCHDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_YTMRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_NETVAL = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_MAIN", x => x.INV_NO);
                    table.ForeignKey(
                        name: "FK_INV_MAIN_INVBROKER_MASTER_INV_BROKERID",
                        column: x => x.INV_BROKERID,
                        principalTable: "INVBROKER_MASTER",
                        principalColumn: "BROKER_ID");
                    table.ForeignKey(
                        name: "FK_INV_MAIN_INVCAT_MAST_INV_CATID",
                        column: x => x.INV_CATID,
                        principalTable: "INVCAT_MAST",
                        principalColumn: "INVCAT_CODE");
                    table.ForeignKey(
                        name: "FK_INV_MAIN_INVSUBCAT_MAST_INV_SUBCATID",
                        column: x => x.INV_SUBCATID,
                        principalTable: "INVSUBCAT_MAST",
                        principalColumn: "INVSUBCAT_ID");
                });

            migrationBuilder.CreateTable(
                name: "INV_APPRDETAILS",
                columns: table => new
                {
                    INV_APRDETID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    INV_INVID = table.Column<long>(type: "bigint", nullable: false),
                    INV_REFID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    INV_APRLEVEL = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    INV_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    INV_APPROVERSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    INV_APPROVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_APPRDETAILS", x => x.INV_APRDETID);
                    table.ForeignKey(
                        name: "FK_INV_APPRDETAILS_INV_MAIN_INV_INVID",
                        column: x => x.INV_INVID,
                        principalTable: "INV_MAIN",
                        principalColumn: "INV_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INV_BANKDET",
                columns: table => new
                {
                    BNK_TRANID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    BNK_ENTRYTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    BNK_TRANTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    BNK_INVNO = table.Column<long>(type: "bigint", nullable: false),
                    BNK_TRNAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    BNK_ID = table.Column<long>(type: "bigint", nullable: false),
                    BNK_DEMATID = table.Column<long>(type: "bigint", nullable: false),
                    BNK_TRANDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BNK_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_BANKDET", x => x.BNK_TRANID);
                    table.ForeignKey(
                        name: "FK_INV_BANKDET_INV_MAIN_BNK_INVNO",
                        column: x => x.BNK_INVNO,
                        principalTable: "INV_MAIN",
                        principalColumn: "INV_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INV_CALLDET",
                columns: table => new
                {
                    INV_CALLDETID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INV_INVNO = table.Column<long>(type: "bigint", nullable: false),
                    INV_CALLDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INV_CALLAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    INV_CNFSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    INV_INTREVFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    INV_REVINTRATE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    INV_SALEREFID = table.Column<long>(type: "bigint", nullable: true),
                    INV_LASTMODBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    INV_LASTMODON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INV_SLNO = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_CALLDET", x => x.INV_CALLDETID);
                    table.ForeignKey(
                        name: "FK_INV_CALLDET_INV_MAIN_INV_INVNO",
                        column: x => x.INV_INVNO,
                        principalTable: "INV_MAIN",
                        principalColumn: "INV_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INV_SALEDET",
                columns: table => new
                {
                    INV_SALENO = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INV_NO = table.Column<long>(type: "bigint", nullable: false),
                    INV_SALETYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    INV_SALEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INV_INTADJUSTED = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    INV_SALPREMIUM = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    INV_SALVALUE = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    INV_SALTRANID = table.Column<int>(type: "int", nullable: false),
                    INV_SALREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    INV_ENTEREDBY = table.Column<long>(type: "bigint", nullable: false),
                    INV_ENTEREDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INV_LASTMODBY = table.Column<long>(type: "bigint", nullable: false),
                    INV_LASTMODON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_SALEDET", x => x.INV_SALENO);
                    table.ForeignKey(
                        name: "FK_INV_SALEDET_INV_MAIN_INV_NO",
                        column: x => x.INV_NO,
                        principalTable: "INV_MAIN",
                        principalColumn: "INV_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "INV_SCHDET",
                columns: table => new
                {
                    SCH_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCH_INVNO = table.Column<long>(type: "bigint", nullable: false),
                    SCH_SLID = table.Column<long>(type: "bigint", nullable: false),
                    SCH_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SCH_INTFROM = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCH_INTTO = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCH_INTOPTION = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    SCH_DUEAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    SCH_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SCH_RECAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SCH_RECDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCH_RECTRANID = table.Column<long>(type: "bigint", nullable: true),
                    SCH_LOGSYSID = table.Column<long>(type: "bigint", nullable: true),
                    SCH_YEAR = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INV_SCHDET", x => x.SCH_ID);
                    table.ForeignKey(
                        name: "FK_INV_SCHDET_INV_MAIN_SCH_INVNO",
                        column: x => x.SCH_INVNO,
                        principalTable: "INV_MAIN",
                        principalColumn: "INV_NO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INV_APPRDETAILS_INV_INVID",
                table: "INV_APPRDETAILS",
                column: "INV_INVID");

            migrationBuilder.CreateIndex(
                name: "IX_INV_BANKDET_BNK_INVNO",
                table: "INV_BANKDET",
                column: "BNK_INVNO");

            migrationBuilder.CreateIndex(
                name: "IX_INV_CALLDET_INV_INVNO",
                table: "INV_CALLDET",
                column: "INV_INVNO");

            migrationBuilder.CreateIndex(
                name: "IDX_INV_MAIN_STATUS",
                table: "INV_MAIN",
                column: "INV_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_INV_MAIN_INV_BROKERID",
                table: "INV_MAIN",
                column: "INV_BROKERID");

            migrationBuilder.CreateIndex(
                name: "IX_INV_MAIN_INV_CATID",
                table: "INV_MAIN",
                column: "INV_CATID");

            migrationBuilder.CreateIndex(
                name: "IX_INV_MAIN_INV_SUBCATID",
                table: "INV_MAIN",
                column: "INV_SUBCATID");

            migrationBuilder.CreateIndex(
                name: "IDX_INV_SALEDET_INVNO",
                table: "INV_SALEDET",
                column: "INV_NO");

            migrationBuilder.CreateIndex(
                name: "IDX_INV_SCHDET_INVNO",
                table: "INV_SCHDET",
                columns: new[] { "SCH_INVNO", "SCH_DUEDATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_INVCAT_MAST_NAME",
                table: "INVCAT_MAST",
                column: "INVCAT_NAME");

            migrationBuilder.CreateIndex(
                name: "IX_INVSUBCAT_MAST_INVSUBCAT_CATID",
                table: "INVSUBCAT_MAST",
                column: "INVSUBCAT_CATID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CREDITAGENCY_MAST");

            migrationBuilder.DropTable(
                name: "CREDITRATING_MAST");

            migrationBuilder.DropTable(
                name: "INV_APPRDETAILS");

            migrationBuilder.DropTable(
                name: "INV_BANKDET");

            migrationBuilder.DropTable(
                name: "INV_CALLDET");

            migrationBuilder.DropTable(
                name: "INV_INTSCHBATCH");

            migrationBuilder.DropTable(
                name: "INV_SALEDET");

            migrationBuilder.DropTable(
                name: "INV_SCHDET");

            migrationBuilder.DropTable(
                name: "INV_MAIN");

            migrationBuilder.DropTable(
                name: "INVBROKER_MASTER");

            migrationBuilder.DropTable(
                name: "INVSUBCAT_MAST");

            migrationBuilder.DropTable(
                name: "INVCAT_MAST");
        }
    }
}
