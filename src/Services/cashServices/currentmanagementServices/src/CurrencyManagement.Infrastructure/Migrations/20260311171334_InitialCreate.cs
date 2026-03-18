using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DEAL_CURRMAST",
                columns: table => new
                {
                    CURR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CURR_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CURR_SYMBOL = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CURR_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CURR_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_CURRMAST", x => x.CURR_ID);
                });

            migrationBuilder.CreateTable(
                name: "DEAL_CURRATES",
                columns: table => new
                {
                    CURRATE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CURRATE_FINYEAR = table.Column<long>(type: "bigint", nullable: false),
                    CURRATE_MONTH = table.Column<long>(type: "bigint", nullable: false),
                    CURRATE_FROMCUR = table.Column<long>(type: "bigint", nullable: false),
                    CURRATE_TOCUR = table.Column<long>(type: "bigint", nullable: false),
                    CURRATE_RATE = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: false),
                    CURRATE_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CURRATE_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_CURRATES", x => x.CURRATE_ID);
                    table.ForeignKey(
                        name: "FK_DEAL_CURRATES_DEAL_CURRMAST_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "DEAL_CURRMAST",
                        principalColumn: "CURR_ID");
                });

            migrationBuilder.CreateTable(
                name: "DEAL_ORGCURRMAP",
                columns: table => new
                {
                    ORG_ID = table.Column<long>(type: "bigint", nullable: false),
                    ORG_CURRID = table.Column<long>(type: "bigint", nullable: false),
                    ORG_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ORG_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEAL_ORGCURRMAP", x => new { x.ORG_ID, x.ORG_CURRID });
                    table.ForeignKey(
                        name: "FK_DEAL_ORGCURRMAP_CURRMAST",
                        column: x => x.ORG_CURRID,
                        principalTable: "DEAL_CURRMAST",
                        principalColumn: "CURR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_CURRATES_CurrencyId",
                table: "DEAL_CURRATES",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_CURRATES_FINYEAR_MONTH",
                table: "DEAL_CURRATES",
                columns: new[] { "CURRATE_FINYEAR", "CURRATE_MONTH" });

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_CURRATES_FROMCUR_TOCUR",
                table: "DEAL_CURRATES",
                columns: new[] { "CURRATE_FROMCUR", "CURRATE_TOCUR" });

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_ORGCURRMAP_ORG_CURRID",
                table: "DEAL_ORGCURRMAP",
                column: "ORG_CURRID");

            migrationBuilder.CreateIndex(
                name: "IX_DEAL_ORGCURRMAP_ORG_ID",
                table: "DEAL_ORGCURRMAP",
                column: "ORG_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEAL_CURRATES");

            migrationBuilder.DropTable(
                name: "DEAL_ORGCURRMAP");

            migrationBuilder.DropTable(
                name: "DEAL_CURRMAST");
        }
    }
}
