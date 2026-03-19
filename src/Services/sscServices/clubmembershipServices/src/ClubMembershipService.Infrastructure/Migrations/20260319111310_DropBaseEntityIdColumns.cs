using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubMembershipService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DropBaseEntityIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CLUB_MEMBERSHIP");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CLUB_MASTER");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CLUB_ACTIVITY");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "CLUB_MEMBERSHIP",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "CLUB_MASTER",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "CLUB_ACTIVITY",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
