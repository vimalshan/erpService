using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteContentService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WEBSITE_NEWS",
                columns: table => new
                {
                    NEWS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NEWS_TITLE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NEWS_CONTENT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NEWS_SUMMARY = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NEWS_CATEGORY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FEATURED_IMAGE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IS_FEATURED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "N"),
                    IS_PUBLISHED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "N"),
                    PUBLISHED_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PUBLISH_START_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PUBLISH_END_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    NEWS_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "DRAFT"),
                    VIEW_COUNT = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    VERSION = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WEBSITE_NEWS", x => x.NEWS_ID);
                });

            migrationBuilder.CreateTable(
                name: "WEBSITE_PAGES",
                columns: table => new
                {
                    PAGE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PAGE_CODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PAGE_TITLE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PAGE_CONTENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    META_DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    META_KEYWORDS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PAGE_ORDER = table.Column<int>(type: "int", nullable: true),
                    PARENT_PAGE_ID = table.Column<long>(type: "bigint", nullable: true),
                    IS_PUBLISHED = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "N"),
                    PUBLISHED_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    PAGE_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVE"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    VERSION = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WEBSITE_PAGES", x => x.PAGE_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WEBSITE_NEWS_CATEGORY",
                table: "WEBSITE_NEWS",
                column: "NEWS_CATEGORY");

            migrationBuilder.CreateIndex(
                name: "IX_WEBSITE_NEWS_DATE",
                table: "WEBSITE_NEWS",
                column: "PUBLISHED_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_WEBSITE_PAGES_PAGE_CODE",
                table: "WEBSITE_PAGES",
                column: "PAGE_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WEBSITE_PAGES_PARENT",
                table: "WEBSITE_PAGES",
                column: "PARENT_PAGE_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WEBSITE_NEWS");

            migrationBuilder.DropTable(
                name: "WEBSITE_PAGES");
        }
    }
}
