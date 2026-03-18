using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingModule.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MEETTYPE_MAST",
                columns: table => new
                {
                    MEETTYPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MEETTYPE_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MEETTYPE_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MEETTYPE_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MEETTYPE_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEETTYPE_MAST", x => x.MEETTYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "SRF_MEETINGSCH",
                columns: table => new
                {
                    MEETING_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MEETTYPE_ID = table.Column<long>(type: "bigint", nullable: false),
                    MEETING_TITLE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MEETING_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    MEETING_LOCATION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MEETING_DURATION = table.Column<int>(type: "int", nullable: true),
                    ORGANIZER_ID = table.Column<long>(type: "bigint", nullable: false),
                    MEETING_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "SCHEDULED"),
                    NOTES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRF_MEETINGSCH", x => x.MEETING_ID);
                    table.ForeignKey(
                        name: "FK_SRF_MEETINGSCH_MEETTYPE",
                        column: x => x.MEETTYPE_ID,
                        principalTable: "MEETTYPE_MAST",
                        principalColumn: "MEETTYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SRF_POLL_DETAIL",
                columns: table => new
                {
                    POLL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MEETING_ID = table.Column<long>(type: "bigint", nullable: false),
                    POLL_QUESTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    POLL_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    POLL_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVE"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRF_POLL_DETAIL", x => x.POLL_ID);
                    table.ForeignKey(
                        name: "FK_SRF_POLL_DETAIL_MEETINGSCH",
                        column: x => x.MEETING_ID,
                        principalTable: "SRF_MEETINGSCH",
                        principalColumn: "MEETING_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MEETTYPE_MAST_MEETTYPE_CODE",
                table: "MEETTYPE_MAST",
                column: "MEETTYPE_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MEETTYPE_MAST_MEETTYPE_STATUS",
                table: "MEETTYPE_MAST",
                column: "MEETTYPE_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_MEETINGSCH_MEETING_DATE",
                table: "SRF_MEETINGSCH",
                column: "MEETING_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_MEETINGSCH_MEETING_STATUS",
                table: "SRF_MEETINGSCH",
                column: "MEETING_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_MEETINGSCH_MEETTYPE_ID",
                table: "SRF_MEETINGSCH",
                column: "MEETTYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_POLL_DETAIL_MEETING_ID",
                table: "SRF_POLL_DETAIL",
                column: "MEETING_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_POLL_DETAIL_POLL_STATUS",
                table: "SRF_POLL_DETAIL",
                column: "POLL_STATUS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SRF_POLL_DETAIL");

            migrationBuilder.DropTable(
                name: "SRF_MEETINGSCH");

            migrationBuilder.DropTable(
                name: "MEETTYPE_MAST");
        }
    }
}
