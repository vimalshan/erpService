using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOOK_MAIN",
                columns: table => new
                {
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOOKING_APPNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BOOKING_TITLE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LOCATION_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BOOKING_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BOOKING_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "DRAFT"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOK_MAIN", x => x.BOOKING_ID);
                });

            migrationBuilder.CreateTable(
                name: "BOOK_ATTENDEES",
                columns: table => new
                {
                    ATTENDEE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: false),
                    ATTENDEE_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    ATTENDEE_SERIAL = table.Column<int>(type: "int", nullable: false),
                    ATTENDANCE_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "REGISTERED"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOK_ATTENDEES", x => x.ATTENDEE_ID);
                    table.ForeignKey(
                        name: "FK_BOOK_ATTENDEES_BOOK_MAIN_BOOKING_ID",
                        column: x => x.BOOKING_ID,
                        principalTable: "BOOK_MAIN",
                        principalColumn: "BOOKING_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOOK_REC",
                columns: table => new
                {
                    BOOK_REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOCATION_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    REC_DETAILS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REC_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVE"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOK_REC", x => x.BOOK_REC_ID);
                    table.ForeignKey(
                        name: "FK_BOOK_REC_BOOK_MAIN_BOOKING_ID",
                        column: x => x.BOOKING_ID,
                        principalTable: "BOOK_MAIN",
                        principalColumn: "BOOKING_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOOK_ATTENDEES_ATTENDEE_SYSID",
                table: "BOOK_ATTENDEES",
                column: "ATTENDEE_SYSID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOK_ATTENDEES_BOOKING_ID",
                table: "BOOK_ATTENDEES",
                column: "BOOKING_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOK_ATTENDEES_BOOKING_ID_ATTENDEE_SYSID",
                table: "BOOK_ATTENDEES",
                columns: new[] { "BOOKING_ID", "ATTENDEE_SYSID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BOOK_MAIN_BOOKING_APPNO",
                table: "BOOK_MAIN",
                column: "BOOKING_APPNO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BOOK_REC_BOOKING_ID",
                table: "BOOK_REC",
                column: "BOOKING_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BOOK_REC_LOCATION_CODE",
                table: "BOOK_REC",
                column: "LOCATION_CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOOK_ATTENDEES");

            migrationBuilder.DropTable(
                name: "BOOK_REC");

            migrationBuilder.DropTable(
                name: "BOOK_MAIN");
        }
    }
}
