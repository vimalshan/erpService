using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtherService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOG_DD_CAT_DEV_DETAIL",
                columns: table => new
                {
                    CT_APP_ID = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    CT_APP_NUM = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    CT_REQ_NUM = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    CT_QTN_NUM = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    CT_ANS_SRL = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    CT_ENT_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    CT_DESC = table.Column<string>(type: "VARCHAR(400)", maxLength: 400, nullable: true),
                    CT_NEED = table.Column<string>(type: "VARCHAR(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG_DD_CAT_DEV_DETAIL", x => new { x.CT_APP_ID, x.CT_APP_NUM });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOG_DD_CAT_DEV_DETAIL");
        }
    }
}
