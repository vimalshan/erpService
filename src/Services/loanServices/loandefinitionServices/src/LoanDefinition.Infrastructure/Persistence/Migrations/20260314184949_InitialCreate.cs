using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanDefinition.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOAN_ACCMAST",
                columns: table => new
                {
                    LOAN_ACID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOAN_TYPE = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_GRADETYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LOAN_ACCODE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    LOAN_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_ACCMAST", x => x.LOAN_ACID);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_FESTIVALS",
                columns: table => new
                {
                    LOANFEST_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOANFEST_DESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LOANFEST_STRDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANFEST_ENDDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANFEST_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOANFEST_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANFEST_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOANFEST_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_FESTIVALS", x => x.LOANFEST_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_PRQ",
                columns: table => new
                {
                    LOAN_PRQID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOAN_CLASSID = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LOAN_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOAN_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOAN_ITINTRATE = table.Column<int>(type: "int", nullable: false),
                    LOAN_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOAN_MINAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_PRQ", x => x.LOAN_PRQID);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_TYPEMASTER",
                columns: table => new
                {
                    LOAN_TYPE = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LOAN_CATEGORY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LOAN_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOAN_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_TYPEMASTER", x => x.LOAN_TYPE);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_MASTER",
                columns: table => new
                {
                    LOAN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOAN_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    LOAN_PURPOSE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LOAN_APPLYToUNIT = table.Column<int>(type: "int", nullable: false),
                    LOAN_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_APPLYToCONFIRMEMP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_GRADECATAGORY = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LOAN_APPLYToALLGRADE = table.Column<int>(type: "int", nullable: false),
                    LOAN_GRADEID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_MINIMUMLIMIT = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_MAXIMUMLIMIT = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_AUTOPAYONCOMPLETION = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_ALLOWFORCECLOSE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_ALLOWMULTIPLENOS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_ONCONFIRMATION = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_CHECKENTITLEMENT = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_RECOVERABLE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_APPLICATIONNOS = table.Column<int>(type: "int", nullable: false),
                    LOAN_CHECKNETPAYPERCENTAGE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_BKDINTERESTRATEREVISION = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_SUBCLASSAVAILABLE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_ITCLASS = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    LOAN_DOCUMENTREQUIRED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_DOCUMENTUPLOADREQUIRED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_SLFAPPALLOWED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_EMPSPECIFICRATESALLOWED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_HRAPPROVAL = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOAN_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOAN_COMFACTOR = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_INTFREQUENCY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_RECTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LOAN_BULKUPLOADALLOWED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_PRNRECEDID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_INTRECEDID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_PRNPAYEDID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_POLICYFILENAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LOAN_GUARANTORREQUIRED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_CHKBASICENTITLEMENT = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_ALLOWADDLLOAN = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_ADDITONALLOANNO = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_CURRECOVERY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_REPUNITAPPLICABLE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_REPUNITID = table.Column<int>(type: "int", nullable: false),
                    LOAN_FLEXIFIRSTINSDATE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOAN_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_MASTER", x => x.LOAN_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_MASTER_LOAN_TYPEMASTER_LOAN_TYPEID",
                        column: x => x.LOAN_TYPEID,
                        principalTable: "LOAN_TYPEMASTER",
                        principalColumn: "LOAN_TYPE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_FESTIVALMAP",
                columns: table => new
                {
                    LOANFESTMAP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOANFESTMAP_LOANID = table.Column<long>(type: "bigint", nullable: false),
                    LOANFESTMAP_FESTIVALID = table.Column<long>(type: "bigint", nullable: false),
                    LOANFESTMAP_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOANFESTMAP_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_FESTIVALMAP", x => x.LOANFESTMAP_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_FESTIVALMAP_LOAN_FESTIVALS_LOANFESTMAP_FESTIVALID",
                        column: x => x.LOANFESTMAP_FESTIVALID,
                        principalTable: "LOAN_FESTIVALS",
                        principalColumn: "LOANFEST_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOAN_FESTIVALMAP_LOAN_MASTER_LOANFESTMAP_LOANID",
                        column: x => x.LOANFESTMAP_LOANID,
                        principalTable: "LOAN_MASTER",
                        principalColumn: "LOAN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_INTRATEMAST",
                columns: table => new
                {
                    LOANINT_RATEID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOANINT_LOANID = table.Column<long>(type: "bigint", nullable: false),
                    LOANINT_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANINT_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOANINT_RATE = table.Column<int>(type: "int", nullable: false),
                    LOANINT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOANINT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANINT_EMIAMT = table.Column<long>(type: "bigint", nullable: false),
                    LOANINT_INSNOS = table.Column<int>(type: "int", nullable: false),
                    LOANINT_RANGESPECIFIC = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_INTRATEMAST", x => x.LOANINT_RATEID);
                    table.ForeignKey(
                        name: "FK_LOAN_INTRATEMAST_LOAN_MASTER_LOANINT_LOANID",
                        column: x => x.LOANINT_LOANID,
                        principalTable: "LOAN_MASTER",
                        principalColumn: "LOAN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_SUBCLASS",
                columns: table => new
                {
                    SUBCLASS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUBCLASS_LOANID = table.Column<long>(type: "bigint", nullable: false),
                    SUBCLASS_DESC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SUBCLASS_IT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    SUBCLASS_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SUBCLASS_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SUBCLASS_PRNRECEDID = table.Column<long>(type: "bigint", nullable: true),
                    SUBCLASS_INTRECEDID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_SUBCLASS", x => x.SUBCLASS_ID);
                    table.ForeignKey(
                        name: "FK_LOAN_SUBCLASS_LOAN_MASTER_SUBCLASS_LOANID",
                        column: x => x.SUBCLASS_LOANID,
                        principalTable: "LOAN_MASTER",
                        principalColumn: "LOAN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOANLIMITRANGE_MAST",
                columns: table => new
                {
                    LOANLIMITRANGE_RATEID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOANLIMITRANGE_LOANID = table.Column<long>(type: "bigint", nullable: false),
                    LOANLIMITRANGE_MINYEAR = table.Column<long>(type: "bigint", nullable: false),
                    LOANLIMITRANGE_MAXYEAR = table.Column<long>(type: "bigint", nullable: false),
                    LOANLIMITRANGE_LOANAMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    LOANLIMITRANGE_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANLIMITRANGE_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LOANLIMITRANGE_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOANLIMITRANGE_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANLIMITRANGE_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOANLIMITRANGE_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOANLIMITRANGE_INTRATE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOANLIMITRANGE_ADDLMINVALUE = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOANLIMITRANGE_MAST", x => x.LOANLIMITRANGE_RATEID);
                    table.ForeignKey(
                        name: "FK_LOANLIMITRANGE_MAST_LOAN_MASTER_LOANLIMITRANGE_LOANID",
                        column: x => x.LOANLIMITRANGE_LOANID,
                        principalTable: "LOAN_MASTER",
                        principalColumn: "LOAN_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_FESTIVALMAP_LOANFESTMAP_FESTIVALID",
                table: "LOAN_FESTIVALMAP",
                column: "LOANFESTMAP_FESTIVALID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_FESTIVALMAP_LOANFESTMAP_LOANID",
                table: "LOAN_FESTIVALMAP",
                column: "LOANFESTMAP_LOANID");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_INTRATEMAST_LOANINT_LOANID",
                table: "LOAN_INTRATEMAST",
                column: "LOANINT_LOANID");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_MASTER_LOAN_TYPEID",
                table: "LOAN_MASTER",
                column: "LOAN_TYPEID");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_PRQ_LOAN_CLASSID",
                table: "LOAN_PRQ",
                column: "LOAN_CLASSID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_SUBCLASS_SUBCLASS_LOANID",
                table: "LOAN_SUBCLASS",
                column: "SUBCLASS_LOANID");

            migrationBuilder.CreateIndex(
                name: "IX_LOANLIMITRANGE_MAST_LOANLIMITRANGE_LOANID",
                table: "LOANLIMITRANGE_MAST",
                column: "LOANLIMITRANGE_LOANID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOAN_ACCMAST");

            migrationBuilder.DropTable(
                name: "LOAN_FESTIVALMAP");

            migrationBuilder.DropTable(
                name: "LOAN_INTRATEMAST");

            migrationBuilder.DropTable(
                name: "LOAN_PRQ");

            migrationBuilder.DropTable(
                name: "LOAN_SUBCLASS");

            migrationBuilder.DropTable(
                name: "LOANLIMITRANGE_MAST");

            migrationBuilder.DropTable(
                name: "LOAN_FESTIVALS");

            migrationBuilder.DropTable(
                name: "LOAN_MASTER");

            migrationBuilder.DropTable(
                name: "LOAN_TYPEMASTER");
        }
    }
}
