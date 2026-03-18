using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorServices.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VISITOR_MAIN",
                columns: table => new
                {
                    VISITOR_ID = table.Column<long>(type: "bigint", nullable: false),
                    VISITOR_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VISITOR_IDTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VISITOR_IDNUMBER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VISITOR_PHONENUMBER = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VISITOR_EMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VISITOR_COMPANY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VISITOR_PURPOSE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VISITOR_CHECKINTIME = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    VISITOR_CHECKOUTTIME = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    VISITOR_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VISITOR_WHOMTOVISIT = table.Column<long>(type: "bigint", nullable: false),
                    VISITOR_ENTEREDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    VISITOR_ENTEREDBY = table.Column<long>(type: "bigint", nullable: false),
                    VISITOR_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    VISITOR_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISITOR_MAIN", x => x.VISITOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "VISITOR_APPREQUEST",
                columns: table => new
                {
                    VREQ_ID = table.Column<long>(type: "bigint", nullable: false),
                    VREQ_VISITORID = table.Column<long>(type: "bigint", nullable: false),
                    VREQ_REQUIREDAPPROVERID = table.Column<long>(type: "bigint", nullable: false),
                    VREQ_APPROVALSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VREQ_APPROVALDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    VREQ_APPROVALREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VREQ_REQUESTEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    VREQ_REQUESTEDBY = table.Column<long>(type: "bigint", nullable: false),
                    VREQ_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    VREQ_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    VisitorAggregateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISITOR_APPREQUEST", x => x.VREQ_ID);
                    table.ForeignKey(
                        name: "FK_VISITOR_APPREQUEST_VISITOR_MAIN_VisitorAggregateId",
                        column: x => x.VisitorAggregateId,
                        principalTable: "VISITOR_MAIN",
                        principalColumn: "VISITOR_ID");
                });

            migrationBuilder.CreateTable(
                name: "VISITOR_ITEM",
                columns: table => new
                {
                    ITEM_ID = table.Column<long>(type: "bigint", nullable: false),
                    ITEM_VISITORID = table.Column<long>(type: "bigint", nullable: false),
                    ITEM_DESCRIPTION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ITEM_QUANTITY = table.Column<int>(type: "int", nullable: false),
                    ITEM_MATERIALTYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ITEM_NOTES = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ITEM_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ITEM_ENTEREDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ITEM_ENTEREDBY = table.Column<long>(type: "bigint", nullable: false),
                    VisitorAggregateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISITOR_ITEM", x => x.ITEM_ID);
                    table.ForeignKey(
                        name: "FK_VISITOR_ITEM_VISITOR_MAIN_VisitorAggregateId",
                        column: x => x.VisitorAggregateId,
                        principalTable: "VISITOR_MAIN",
                        principalColumn: "VISITOR_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VISITOR_APPREQUEST_STATUS",
                table: "VISITOR_APPREQUEST",
                column: "VREQ_APPROVALSTATUS");

            migrationBuilder.CreateIndex(
                name: "IX_VISITOR_APPREQUEST_VisitorAggregateId",
                table: "VISITOR_APPREQUEST",
                column: "VisitorAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_VISITOR_APPREQUEST_VISITORID",
                table: "VISITOR_APPREQUEST",
                column: "VREQ_VISITORID");

            migrationBuilder.CreateIndex(
                name: "IX_VISITOR_ITEM_VisitorAggregateId",
                table: "VISITOR_ITEM",
                column: "VisitorAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_VISITOR_ITEM_VISITORID",
                table: "VISITOR_ITEM",
                column: "ITEM_VISITORID");

            migrationBuilder.CreateIndex(
                name: "IX_VISITOR_MAIN_CHECKINTIME",
                table: "VISITOR_MAIN",
                column: "VISITOR_CHECKINTIME");

            migrationBuilder.CreateIndex(
                name: "IX_VISITOR_MAIN_STATUS",
                table: "VISITOR_MAIN",
                column: "VISITOR_STATUS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VISITOR_APPREQUEST");

            migrationBuilder.DropTable(
                name: "VISITOR_ITEM");

            migrationBuilder.DropTable(
                name: "VISITOR_MAIN");
        }
    }
}
