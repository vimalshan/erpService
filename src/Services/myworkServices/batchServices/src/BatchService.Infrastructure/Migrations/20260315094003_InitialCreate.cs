using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BatchService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BATCH_MASTER",
                columns: table => new
                {
                    BATCH_ID = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_MONTHNO = table.Column<int>(type: "int", nullable: false),
                    BATCH_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    BATCH_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BATCH_MASTER", x => x.BATCH_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BATCH_MASTER");
        }
    }
}
