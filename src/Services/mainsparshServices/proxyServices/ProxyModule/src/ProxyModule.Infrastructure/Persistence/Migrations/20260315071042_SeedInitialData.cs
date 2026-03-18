using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProxyModule.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PROXY_RIGHTS",
                columns: new[] { "PROXY_ID", "CREATED_BY", "CREATED_ON", "DELEGATED_USER_ID", "NOTES", "PROXY_END_DATE", "PROXY_START_DATE", "PROXY_STATUS", "PROXY_TYPE", "PROXY_USER_ID", "SCOPE", "UPDATED_BY", "UPDATED_ON" },
                values: new object[,]
                {
                    { 1L, 1L, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 101L, "Approval delegation for Q1 reviews", new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "A", "APPROVAL", 100L, "DEPARTMENT", null, null },
                    { 2L, 1L, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 201L, "Submission delegation during leave", new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "A", "SUBMISSION", 200L, "ALL", null, null },
                    { 3L, 1L, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 301L, "Permanent full proxy for branch office", null, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "A", "FULL", 300L, "LOCATION", null, null },
                    { 4L, 1L, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 401L, "Temporary read-only access for audit", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "A", "READONLY", 400L, "SPECIFIC", null, null },
                    { 5L, 1L, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 501L, "Long-term approval proxy for annual cycle", new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "A", "APPROVAL", 500L, "ALL", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PROXY_RIGHTS",
                keyColumn: "PROXY_ID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PROXY_RIGHTS",
                keyColumn: "PROXY_ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "PROXY_RIGHTS",
                keyColumn: "PROXY_ID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "PROXY_RIGHTS",
                keyColumn: "PROXY_ID",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "PROXY_RIGHTS",
                keyColumn: "PROXY_ID",
                keyValue: 5L);
        }
    }
}
