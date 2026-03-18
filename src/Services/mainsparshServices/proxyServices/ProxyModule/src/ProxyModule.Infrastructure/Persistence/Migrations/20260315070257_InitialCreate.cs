using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyModule.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PROXY_RIGHTS",
                columns: table => new
                {
                    PROXY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROXY_USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    DELEGATED_USER_ID = table.Column<long>(type: "bigint", nullable: false),
                    PROXY_START_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    PROXY_END_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    PROXY_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROXY_STATUS = table.Column<string>(type: "char(1)", nullable: false, defaultValue: "A"),
                    SCOPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROXY_RIGHTS", x => x.PROXY_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PROXY_RIGHTS_DATES",
                table: "PROXY_RIGHTS",
                columns: new[] { "PROXY_START_DATE", "PROXY_END_DATE" });

            migrationBuilder.CreateIndex(
                name: "IX_PROXY_RIGHTS_DELEGATED_USER_ID",
                table: "PROXY_RIGHTS",
                column: "DELEGATED_USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROXY_RIGHTS_PROXY_USER_ID",
                table: "PROXY_RIGHTS",
                column: "PROXY_USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PROXY_RIGHTS_STATUS",
                table: "PROXY_RIGHTS",
                column: "PROXY_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_PROXY_RIGHTS_TYPE",
                table: "PROXY_RIGHTS",
                column: "PROXY_TYPE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PROXY_RIGHTS");
        }
    }
}
