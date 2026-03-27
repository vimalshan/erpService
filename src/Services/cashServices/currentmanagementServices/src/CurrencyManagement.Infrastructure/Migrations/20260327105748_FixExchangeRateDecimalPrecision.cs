using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixExchangeRateDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CURRATE_RATE",
                table: "DEAL_CURRATES",
                type: "DECIMAL(19,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(19,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CURRATE_RATE",
                table: "DEAL_CURRATES",
                type: "DECIMAL(19,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(19,6)");
        }
    }
}
