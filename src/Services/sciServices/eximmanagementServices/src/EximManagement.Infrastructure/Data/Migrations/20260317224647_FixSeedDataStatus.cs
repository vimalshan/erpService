using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EximManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1001L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1002L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1003L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1004L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1005L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 101L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 102L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 103L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 104L,
                column: "STATUS",
                value: "Y");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 105L,
                column: "STATUS",
                value: "Y");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1001L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1002L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1003L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1004L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCT",
                keyColumn: "PRODUCT_ID",
                keyValue: 1005L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 101L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 102L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 103L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 104L,
                column: "STATUS",
                value: "A");

            migrationBuilder.UpdateData(
                table: "EXIM_PRODUCTGROUP",
                keyColumn: "GROUP_ID",
                keyValue: 105L,
                column: "STATUS",
                value: "A");
        }
    }
}
