using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourServices.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TOUR_PACKAGE",
                columns: table => new
                {
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TOUR_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DESTINATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    START_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    END_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    TOUR_PACKAGE_COST = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    MAX_PARTICIPANTS = table.Column<int>(type: "int", nullable: false),
                    TOUR_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_BY = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOUR_PACKAGE", x => x.TOUR_ID);
                });

            migrationBuilder.CreateTable(
                name: "TOUR_REGISTRATION",
                columns: table => new
                {
                    REGISTRATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: false),
                    PARTICIPANT_ID = table.Column<long>(type: "bigint", nullable: false),
                    REGISTRATION_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    REGISTRATION_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_BY = table.Column<long>(type: "bigint", nullable: true),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TOUR_REGISTRATION", x => x.REGISTRATION_ID);
                    table.ForeignKey(
                        name: "FK_TOUR_REG_PACKAGE",
                        column: x => x.TOUR_ID,
                        principalTable: "TOUR_PACKAGE",
                        principalColumn: "TOUR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TOUR_REGISTRATION_TOUR_ID",
                table: "TOUR_REGISTRATION",
                column: "TOUR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TOUR_REGISTRATION");

            migrationBuilder.DropTable(
                name: "TOUR_PACKAGE");
        }
    }
}
