using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOOKCONF_CAB",
                columns: table => new
                {
                    CNFCAB_CONFID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNFCAB_BOOKID = table.Column<long>(type: "bigint", nullable: false),
                    CNFCAB_ID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKCONF_CAB", x => x.CNFCAB_CONFID);
                });

            migrationBuilder.CreateTable(
                name: "BOOKCONF_STAY",
                columns: table => new
                {
                    CNFSTY_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CNFSTY_BOOKID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CNFSTY_CHECKINDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    CNFSTY_CHECKOUTDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    CNFSTY_GHSITEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CNFSTY_CNFID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKCONF_STAY", x => x.CNFSTY_ID);
                });

            migrationBuilder.CreateTable(
                name: "BOOKCONF_TICKET",
                columns: table => new
                {
                    CNFTKT_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CNFTKT_BOOKID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CNFTKT_TICKETID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CNFTKT_ENTDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    CNFTKT_DEPDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    CNFTKT_COST = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CNFTKT_CNFID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKCONF_TICKET", x => x.CNFTKT_ID);
                });

            migrationBuilder.CreateTable(
                name: "BOOKCONFIRMATION_CC",
                columns: table => new
                {
                    BOOKCNFCC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOOKCNFCC_MAINID = table.Column<long>(type: "bigint", nullable: false),
                    BOOKCNF_BUCODE = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BOOKCNF_CCCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_SUBACCCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_PRODUCTCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_LOCSEGMENT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_ALLLPER = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKCONFIRMATION_CC", x => x.BOOKCNFCC_ID);
                });

            migrationBuilder.CreateTable(
                name: "BOOKCONFIRMATION_MAIN",
                columns: table => new
                {
                    BOOKCNF_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_MODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_BOOKID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_REFID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    BOOKCNF_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    BOOKCNF_ENDDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    BOOKCNF_BOOKTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_STATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_ADMUNIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_PAYBATCHNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_CONTRACTID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_VENDORID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_TRIPCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKCONFIRMATION_MAIN", x => x.BOOKCNF_ID);
                });

            migrationBuilder.CreateTable(
                name: "BOOKREQUEST_MAIN",
                columns: table => new
                {
                    BOOKMAIN_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_TPSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_TPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_EMPSYSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_THROUGH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_ADMINID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_REMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_APPSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_CNFSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKMAIN_PROOF = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKMAIN_FOODPREF = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKMAIN_BUDCOST = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKMAIN_ENTBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKMAIN_ENTON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    BOOKMAIN_EMPCALID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKMAIN_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKREQUEST_MAIN", x => x.BOOKMAIN_ID);
                });

            migrationBuilder.CreateTable(
                name: "BOOKREQUEST_CAB",
                columns: table => new
                {
                    BOOKCAB_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_MAINID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_PICKUPLOC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_DROPLOC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_PICKUPDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKCAB_CARTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCAB_PREFERENCE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_TRIPTYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_ADDRESS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_CNFNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCAB_NATURE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCAB_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKREQUEST_CAB", x => x.BOOKCAB_ID);
                    table.ForeignKey(
                        name: "FK_BOOKREQUEST_CAB_BOOKREQUEST_MAIN_BOOKCAB_MAINID",
                        column: x => x.BOOKCAB_MAINID,
                        principalTable: "BOOKREQUEST_MAIN",
                        principalColumn: "BOOKMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOKREQUEST_CC",
                columns: table => new
                {
                    BOOKCC_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCC_MAINID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCC_BUCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCC_CCCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCC_SUBACCCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCC_PRODUCTCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCC_LOCSEGMENT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCC_ALLLPER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKREQUEST_CC", x => x.BOOKCC_ID);
                    table.ForeignKey(
                        name: "FK_BOOKREQUEST_CC_BOOKREQUEST_MAIN_BOOKCC_MAINID",
                        column: x => x.BOOKCC_MAINID,
                        principalTable: "BOOKREQUEST_MAIN",
                        principalColumn: "BOOKMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOKREQUEST_CONFIRMATION",
                columns: table => new
                {
                    BOOKCNF_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_MODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_BOOKID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_REFID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKCNF_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKCNF_ENDDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKCNF_COST = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_CLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_VENDORID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_GHSITEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_CABCONFID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_REFUNDCOST = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_CANCELDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKCNF_DEBITMEMOBATCH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_CREDITMEMOBATCH = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_ADMREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_CONFIRMEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_VENDORSLF = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_ATTACHMENT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_APPROVALSTS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_ENTID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_OLDREQID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKCNF_AIRLINEVNDID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_AIRPNRNUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BOOKCNF_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKREQUEST_CONFIRMATION", x => x.BOOKCNF_ID);
                    table.ForeignKey(
                        name: "FK_BOOKREQUEST_CONFIRMATION_BOOKREQUEST_MAIN_BOOKCNF_BOOKID",
                        column: x => x.BOOKCNF_BOOKID,
                        principalTable: "BOOKREQUEST_MAIN",
                        principalColumn: "BOOKMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOKREQUEST_OTHERS",
                columns: table => new
                {
                    BOOKOTH_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKOTH_BOOKID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKOTH_FOR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKOTH_GENDER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKOTH_AGE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKOTH_CONTACTNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKOTH_APPROVEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKOTH_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKREQUEST_OTHERS", x => x.BOOKOTH_ID);
                    table.ForeignKey(
                        name: "FK_BOOKREQUEST_OTHERS_BOOKREQUEST_MAIN_BOOKOTH_BOOKID",
                        column: x => x.BOOKOTH_BOOKID,
                        principalTable: "BOOKREQUEST_MAIN",
                        principalColumn: "BOOKMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOKREQUEST_STAY",
                columns: table => new
                {
                    BOOKSTY_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKSTY_MAINID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKSTY_CITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKSTY_CITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKSTY_CHECKINDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKSTY_CHECKOUTDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKSTY_CNFNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKSTY_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKSTY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKREQUEST_STAY", x => x.BOOKSTY_ID);
                    table.ForeignKey(
                        name: "FK_BOOKREQUEST_STAY_BOOKREQUEST_MAIN_BOOKSTY_MAINID",
                        column: x => x.BOOKSTY_MAINID,
                        principalTable: "BOOKREQUEST_MAIN",
                        principalColumn: "BOOKMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOKREQUEST_TICKET",
                columns: table => new
                {
                    BOOKTKT_ID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_MAINID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_MODEID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_CLASSID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    BOOKTKT_STARTTIME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_STARTCITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_STARTCITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_ENDCITYID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_ENDCITY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_CNFNO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_APPSTATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_LASTMODIFIEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_BUDGETCOST = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_ADMREMARKS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_SPECIALSANCTION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_SPLREASON = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BOOKTKT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKREQUEST_TICKET", x => x.BOOKTKT_ID);
                    table.ForeignKey(
                        name: "FK_BOOKREQUEST_TICKET_BOOKREQUEST_MAIN_BOOKTKT_MAINID",
                        column: x => x.BOOKTKT_MAINID,
                        principalTable: "BOOKREQUEST_MAIN",
                        principalColumn: "BOOKMAIN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOOKREQUEST_CAB_BOOKCAB_MAINID",
                table: "BOOKREQUEST_CAB",
                column: "BOOKCAB_MAINID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKREQUEST_CC_BOOKCC_MAINID",
                table: "BOOKREQUEST_CC",
                column: "BOOKCC_MAINID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKREQUEST_CONFIRMATION_BOOKCNF_BOOKID",
                table: "BOOKREQUEST_CONFIRMATION",
                column: "BOOKCNF_BOOKID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKREQUEST_OTHERS_BOOKOTH_BOOKID",
                table: "BOOKREQUEST_OTHERS",
                column: "BOOKOTH_BOOKID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKREQUEST_STAY_BOOKSTY_MAINID",
                table: "BOOKREQUEST_STAY",
                column: "BOOKSTY_MAINID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKREQUEST_TICKET_BOOKTKT_MAINID",
                table: "BOOKREQUEST_TICKET",
                column: "BOOKTKT_MAINID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOOKCONF_CAB");

            migrationBuilder.DropTable(
                name: "BOOKCONF_STAY");

            migrationBuilder.DropTable(
                name: "BOOKCONF_TICKET");

            migrationBuilder.DropTable(
                name: "BOOKCONFIRMATION_CC");

            migrationBuilder.DropTable(
                name: "BOOKCONFIRMATION_MAIN");

            migrationBuilder.DropTable(
                name: "BOOKREQUEST_CAB");

            migrationBuilder.DropTable(
                name: "BOOKREQUEST_CC");

            migrationBuilder.DropTable(
                name: "BOOKREQUEST_CONFIRMATION");

            migrationBuilder.DropTable(
                name: "BOOKREQUEST_OTHERS");

            migrationBuilder.DropTable(
                name: "BOOKREQUEST_STAY");

            migrationBuilder.DropTable(
                name: "BOOKREQUEST_TICKET");

            migrationBuilder.DropTable(
                name: "BOOKREQUEST_MAIN");
        }
    }
}
