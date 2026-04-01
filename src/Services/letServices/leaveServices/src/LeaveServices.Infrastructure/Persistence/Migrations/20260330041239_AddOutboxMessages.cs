using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveServices.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OUTBOX_MESSAGES",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EVENT_TYPE = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ROUTING_KEY = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PAYLOAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROCESSED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RETRY_COUNT = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ERROR = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTBOX_MESSAGES", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_OUTBOX_PROCESSED",
                table: "OUTBOX_MESSAGES",
                column: "PROCESSED_ON");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OUTBOX_MESSAGES");
        }
    }
}
