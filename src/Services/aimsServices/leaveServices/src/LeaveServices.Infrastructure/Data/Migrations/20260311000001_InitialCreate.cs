using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveServices.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LEAVE_MASTER",
                columns: table => new
                {
                    LEAVE_ID               = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_DESCRIPTION      = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    LEAVE_GENDERSPECIFIC   = table.Column<string>(type: "CHAR(1)", nullable: false),
                    LEAVE_APPLICABLEFORALL = table.Column<string>(type: "CHAR(1)", nullable: false),
                    LEAVE_MAXDAYSPL        = table.Column<int>(type: "int", nullable: false),
                    LEAVE_ENCASHABLE       = table.Column<string>(type: "CHAR(1)", nullable: false),
                    LEAVE_CARRYFORWARD     = table.Column<string>(type: "CHAR(1)", nullable: false),
                    LEAVE_LASTMODIFIEDBY   = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_LASTMODIFIEDON   = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_LEAVE_MASTER", x => x.LEAVE_ID));

            migrationBuilder.CreateIndex(
                name: "UQ_LEAVE_DESC",
                table: "LEAVE_MASTER",
                column: "LEAVE_DESCRIPTION",
                unique: true);

            migrationBuilder.CreateTable(
                name: "LEAVE_DETAILS",
                columns: table => new
                {
                    LEAVE_DETAILID        = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_EMPSYSID        = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_APPFROM         = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    LEAVE_APPTO           = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    LEAVE_APPTYPE         = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    LEAVE_ID              = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_TIMEUNITID      = table.Column<int>(type: "int", nullable: false),
                    LEAVE_APPSTATUS       = table.Column<string>(type: "CHAR(1)", nullable: false),
                    LEAVE_APPLIEDDAYS     = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    LEAVE_REASON          = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    LEAVE_ENTEREDON       = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    LEAVE_ENTEREDBY       = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_LASTMODIFIEDBY  = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_LASTMODIFIEDON  = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_DETAILS", x => x.LEAVE_DETAILID);
                    table.ForeignKey(
                        name: "FK_LEAVE_DETAILS_MASTER",
                        column: x => x.LEAVE_ID,
                        principalTable: "LEAVE_MASTER",
                        principalColumn: "LEAVE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LEAVE_CREDIT",
                columns: table => new
                {
                    CREDIT_ID              = table.Column<long>(type: "bigint", nullable: false),
                    CREDIT_EMPSYSID        = table.Column<long>(type: "bigint", nullable: false),
                    CREDIT_LEAVEID         = table.Column<long>(type: "bigint", nullable: false),
                    CREDIT_LEAVEFLAG       = table.Column<string>(type: "CHAR(1)", nullable: false),
                    CREDIT_YEAR            = table.Column<int>(type: "int", nullable: false),
                    CREDIT_OPENING         = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CREDIT_CREDITED        = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CREDIT_UTILIZED        = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CREDIT_CLOSING         = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CREDIT_LASTMODIFIEDBY  = table.Column<long>(type: "bigint", nullable: false),
                    CREDIT_LASTMODIFIEDON  = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_CREDIT", x => x.CREDIT_ID);
                    table.ForeignKey(
                        name: "FK_CREDIT_LEAVE",
                        column: x => x.CREDIT_LEAVEID,
                        principalTable: "LEAVE_MASTER",
                        principalColumn: "LEAVE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ_LEAVE_CREDIT",
                table: "LEAVE_CREDIT",
                columns: new[] { "CREDIT_EMPSYSID", "CREDIT_LEAVEID", "CREDIT_YEAR" },
                unique: true);

            migrationBuilder.CreateTable(
                name: "LEAVE_DETAILSAPR",
                columns: table => new
                {
                    LEAVEAPR_ID              = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEAPR_DETAILID        = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEAPR_APPROVESTATUS   = table.Column<string>(type: "CHAR(1)", nullable: false),
                    LEAVEAPR_REMARKS         = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    LEAVEAPR_APPROVEDON      = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    LEAVEAPR_APPROVEDBY      = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEAPR_LASTMODIFIEDBY  = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEAPR_LASTMODIFIEDON  = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_DETAILSAPR", x => x.LEAVEAPR_ID);
                    table.ForeignKey(
                        name: "FK_LEAVEAPR_DETAILS",
                        column: x => x.LEAVEAPR_DETAILID,
                        principalTable: "LEAVE_DETAILS",
                        principalColumn: "LEAVE_DETAILID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LEAVE_RULES",
                columns: table => new
                {
                    RULE_ID              = table.Column<int>(type: "int", nullable: false),
                    RULE_LEAVEID         = table.Column<long>(type: "bigint", nullable: false),
                    RULE_MAXDAYSINAPPL   = table.Column<int>(type: "int", nullable: false),
                    RULE_MINDAYSINAPPL   = table.Column<int>(type: "int", nullable: false),
                    RULE_MAXYEARLIMIT    = table.Column<int>(type: "int", nullable: false),
                    RULE_CLUBBING        = table.Column<string>(type: "CHAR(1)", nullable: false),
                    RULE_LASTMODIFIEDBY  = table.Column<long>(type: "bigint", nullable: false),
                    RULE_LASTMODIFIEDON  = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_RULES", x => x.RULE_ID);
                    table.ForeignKey(
                        name: "FK_RULE_LEAVE",
                        column: x => x.RULE_LEAVEID,
                        principalTable: "LEAVE_MASTER",
                        principalColumn: "LEAVE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPOFF_ADJUST",
                columns: table => new
                {
                    COMPOFF_ID              = table.Column<long>(type: "bigint", nullable: false),
                    COMPOFF_EMPSYSID        = table.Column<long>(type: "bigint", nullable: false),
                    COMPOFF_COMPOFFDATE     = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                    COMPOFF_USEDDATE        = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    COMPOFF_STATUS          = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    COMPOFF_LASTMODIFIEDBY  = table.Column<long>(type: "bigint", nullable: false),
                    COMPOFF_LASTMODIFIEDON  = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_COMPOFF_ADJUST", x => x.COMPOFF_ID));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "COMPOFF_ADJUST");
            migrationBuilder.DropTable(name: "LEAVE_RULES");
            migrationBuilder.DropTable(name: "LEAVE_DETAILSAPR");
            migrationBuilder.DropTable(name: "LEAVE_CREDIT");
            migrationBuilder.DropTable(name: "LEAVE_DETAILS");
            migrationBuilder.DropTable(name: "LEAVE_MASTER");
        }
    }
}
