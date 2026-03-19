using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubMembershipService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLUB_MASTER",
                columns: table => new
                {
                    CLUB_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLUB_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CLUB_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_BY = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLUB_MASTER", x => x.CLUB_ID);
                });

            migrationBuilder.CreateTable(
                name: "CLUB_ACTIVITY",
                columns: table => new
                {
                    ACTIVITY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLUB_ID = table.Column<long>(type: "bigint", nullable: false),
                    ACTIVITY_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ACTIVITY_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    ACTIVITY_BUDGET = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: true),
                    ORGANIZER_ID = table.Column<long>(type: "bigint", nullable: false),
                    ACTIVITY_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_BY = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLUB_ACTIVITY", x => x.ACTIVITY_ID);
                    table.ForeignKey(
                        name: "FK_CLUB_ACTIVITY_CLUB_MASTER_CLUB_ID",
                        column: x => x.CLUB_ID,
                        principalTable: "CLUB_MASTER",
                        principalColumn: "CLUB_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CLUB_MEMBERSHIP",
                columns: table => new
                {
                    MEMBERSHIP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CLUB_ID = table.Column<long>(type: "bigint", nullable: false),
                    MEMBER_ID = table.Column<long>(type: "bigint", nullable: false),
                    JOIN_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    MEMBERSHIP_FEE = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: true),
                    MEMBERSHIP_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_BY = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLUB_MEMBERSHIP", x => x.MEMBERSHIP_ID);
                    table.ForeignKey(
                        name: "FK_CLUB_MEMBERSHIP_CLUB_MASTER_CLUB_ID",
                        column: x => x.CLUB_ID,
                        principalTable: "CLUB_MASTER",
                        principalColumn: "CLUB_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CLUB_ACTIVITY_CLUB_ID",
                table: "CLUB_ACTIVITY",
                column: "CLUB_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CLUB_MEMBERSHIP_CLUB_ID",
                table: "CLUB_MEMBERSHIP",
                column: "CLUB_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLUB_ACTIVITY");

            migrationBuilder.DropTable(
                name: "CLUB_MEMBERSHIP");

            migrationBuilder.DropTable(
                name: "CLUB_MASTER");
        }
    }
}
