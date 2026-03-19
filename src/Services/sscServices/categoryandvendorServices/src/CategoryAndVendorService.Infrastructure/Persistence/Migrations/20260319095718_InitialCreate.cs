using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CategoryAndVendorService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MAINCAT_MAST",
                columns: table => new
                {
                    MAINCAT_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAINCAT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MAINCAT_PRIORITY = table.Column<long>(type: "bigint", nullable: false),
                    MAINCAT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MAINCAT_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    MAINCAT_DEFSUBCATID = table.Column<long>(type: "bigint", nullable: true),
                    MAINCAT_AVGRESTIME = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAINCAT_MAST", x => x.MAINCAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "SUPDOC_COUNTER",
                columns: table => new
                {
                    SUPDOC_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SUPDOC_NO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SUPDOC_DET",
                columns: table => new
                {
                    SUP_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    SUP_DOCCAT = table.Column<long>(type: "bigint", nullable: false),
                    SUP_INVDOCID = table.Column<long>(type: "bigint", nullable: false),
                    SUP_DOCKEY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SUP_DOCSTATUS = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    SUP_PBGNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SUP_PBGSTART = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    SUP_PBGEXPDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    SUP_AMOUNT = table.Column<long>(type: "bigint", nullable: true),
                    SUP_RECDUE = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPDOC_DET", x => x.SUP_DOCID);
                });

            migrationBuilder.CreateTable(
                name: "VENDOR_DOCDET",
                columns: table => new
                {
                    VNDDOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    VNDDOC_VENDORID = table.Column<long>(type: "bigint", nullable: false),
                    VNDDOC_SITEID = table.Column<long>(type: "bigint", nullable: false),
                    VNDDOC_BUID = table.Column<long>(type: "bigint", nullable: false),
                    VNDDOC_INFCAT = table.Column<long>(type: "bigint", nullable: false),
                    VNDDOC_REMARKS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    VNDDOC_DOCFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VNDDOC_DOCTYPE = table.Column<long>(type: "bigint", nullable: true),
                    VNDDOC_DOCREFNO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VNDDOC_VALIDFROM = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    VNDDOC_VALIDTO = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    VNDDOC_ACTIVESTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VNDDOC_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    VNDDOC_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    VNDDOC_APPSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VNDDOC_APPREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VNDDOC_APPROVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    VNDDOC_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDOR_DOCDET", x => x.VNDDOC_ID);
                });

            migrationBuilder.CreateTable(
                name: "SUBCAT_MAST",
                columns: table => new
                {
                    SUBCAT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_MAINID = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SUBCAT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBCAT_MAST", x => x.SUBCAT_ID);
                    table.ForeignKey(
                        name: "FK_SUBCAT_MAST_MAINCAT_MAST_SUBCAT_MAINID",
                        column: x => x.SUBCAT_MAINID,
                        principalTable: "MAINCAT_MAST",
                        principalColumn: "MAINCAT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SUPDOC_ATT",
                columns: table => new
                {
                    SUPDOC_ATTID = table.Column<long>(type: "bigint", nullable: false),
                    SUPDOC_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    SUPDOC_INVDOCID = table.Column<long>(type: "bigint", nullable: false),
                    SUPDOC_REFFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPDOC_ATT", x => x.SUPDOC_ATTID);
                    table.ForeignKey(
                        name: "FK_SUPDOC_ATT_SUPDOC_DET_SUPDOC_DOCID",
                        column: x => x.SUPDOC_DOCID,
                        principalTable: "SUPDOC_DET",
                        principalColumn: "SUP_DOCID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VENDOR_DOCFILE",
                columns: table => new
                {
                    VNDFILE_ID = table.Column<long>(type: "bigint", nullable: false),
                    VNDFILE_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    VNDFILE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VNDFILE_PATH = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VENDOR_DOCFILE", x => x.VNDFILE_ID);
                    table.ForeignKey(
                        name: "FK_VENDOR_DOCFILE_VENDOR_DOCDET_VNDFILE_DOCID",
                        column: x => x.VNDFILE_DOCID,
                        principalTable: "VENDOR_DOCDET",
                        principalColumn: "VNDDOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SUBCAT_MAST_SUBCAT_MAINID",
                table: "SUBCAT_MAST",
                column: "SUBCAT_MAINID");

            migrationBuilder.CreateIndex(
                name: "IX_SUPDOC_ATT_SUPDOC_DOCID",
                table: "SUPDOC_ATT",
                column: "SUPDOC_DOCID");

            migrationBuilder.CreateIndex(
                name: "IX_VENDOR_DOCFILE_VNDFILE_DOCID",
                table: "VENDOR_DOCFILE",
                column: "VNDFILE_DOCID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SUBCAT_MAST");

            migrationBuilder.DropTable(
                name: "SUPDOC_ATT");

            migrationBuilder.DropTable(
                name: "SUPDOC_COUNTER");

            migrationBuilder.DropTable(
                name: "VENDOR_DOCFILE");

            migrationBuilder.DropTable(
                name: "MAINCAT_MAST");

            migrationBuilder.DropTable(
                name: "SUPDOC_DET");

            migrationBuilder.DropTable(
                name: "VENDOR_DOCDET");
        }
    }
}
