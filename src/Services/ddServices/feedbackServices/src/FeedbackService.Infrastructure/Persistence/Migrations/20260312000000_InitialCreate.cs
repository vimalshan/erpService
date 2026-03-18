using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APP_FEEDBACKMAIN",
                columns: table => new
                {
                    FB_FEEDBACKID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    FB_REQUESTNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    FB_APPRSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    FB_STATUS = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true),
                    FB_REMARKS = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_FEEDBACKMAIN", x => x.FB_FEEDBACKID);
                });

            migrationBuilder.CreateTable(
                name: "APP_FEEDBACKSUB",
                columns: table => new
                {
                    FB_FEEDBACKID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    FB_QTNNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    FB_ANSNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_FEEDBACKSUB", x => new { x.FB_FEEDBACKID, x.FB_QTNNO });
                    table.ForeignKey(
                        name: "FK_APP_FEEDBACKSUB_APP_FEEDBACKMAIN_FB_FEEDBACKID",
                        column: x => x.FB_FEEDBACKID,
                        principalTable: "APP_FEEDBACKMAIN",
                        principalColumn: "FB_FEEDBACKID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOV_FEEDBACK",
                columns: table => new
                {
                    DD_FEEDBACKID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DD_FEEDBACKNAME = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_APP_FEEDBACKSUB_FB_FEEDBACKID",
                table: "APP_FEEDBACKSUB",
                column: "FB_FEEDBACKID");

            migrationBuilder.CreateIndex(
                name: "IX_APP_FEEDBACKMAIN_FB_REQUESTNO",
                table: "APP_FEEDBACKMAIN",
                column: "FB_REQUESTNO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APP_FEEDBACKSUB");

            migrationBuilder.DropTable(
                name: "LOV_FEEDBACK");

            migrationBuilder.DropTable(
                name: "APP_FEEDBACKMAIN");
        }
    }
}
