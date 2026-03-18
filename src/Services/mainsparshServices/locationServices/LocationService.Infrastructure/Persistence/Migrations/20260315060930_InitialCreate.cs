using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOCATION_CONTACT",
                columns: table => new
                {
                    LOCATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOCATION_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LOCATION_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LOCATION_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValueSql: "'A'"),
                    LOCATION_ADDRESS = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    CITY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    STATE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PIN_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    COUNTRY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PHONE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONTACT_PERSON = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CREATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCATION_CONTACT", x => x.LOCATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "ROOM_MAST",
                columns: table => new
                {
                    ROOM_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    ROOM_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ROOM_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ROOM_CAPACITY = table.Column<int>(type: "int", nullable: true),
                    ROOM_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FLOOR_NUMBER = table.Column<int>(type: "int", nullable: true),
                    ROOM_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValueSql: "'A'"),
                    CREATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOM_MAST", x => x.ROOM_ID);
                    table.ForeignKey(
                        name: "FK_ROOM_MAST_LOCATION_CONTACT_LOCATION_ID",
                        column: x => x.LOCATION_ID,
                        principalTable: "LOCATION_CONTACT",
                        principalColumn: "LOCATION_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ROOM_RESOURCE",
                columns: table => new
                {
                    RESOURCE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROOM_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOCATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    RESOURCE_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RESOURCE_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RESOURCE_TYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RESOURCE_QUANTITY = table.Column<int>(type: "int", nullable: true),
                    RESOURCE_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValueSql: "'A'"),
                    CREATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROOM_RESOURCE", x => x.RESOURCE_ID);
                    table.ForeignKey(
                        name: "FK_ROOM_RESOURCE_ROOM_MAST_ROOM_ID",
                        column: x => x.ROOM_ID,
                        principalTable: "ROOM_MAST",
                        principalColumn: "ROOM_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOCATION_CONTACT_CODE",
                table: "LOCATION_CONTACT",
                column: "LOCATION_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_LOCATION_CONTACT_STATUS",
                table: "LOCATION_CONTACT",
                column: "LOCATION_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_ROOM_MAST_CODE",
                table: "ROOM_MAST",
                column: "ROOM_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_ROOM_MAST_LOCATION_ID",
                table: "ROOM_MAST",
                column: "LOCATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOM_MAST_TYPE",
                table: "ROOM_MAST",
                column: "ROOM_TYPE");

            migrationBuilder.CreateIndex(
                name: "UC_ROOM_CODE_LOCATION",
                table: "ROOM_MAST",
                columns: new[] { "LOCATION_ID", "ROOM_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ROOM_RESOURCE_LOCATION_ID",
                table: "ROOM_RESOURCE",
                column: "LOCATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOM_RESOURCE_ROOM_ID",
                table: "ROOM_RESOURCE",
                column: "ROOM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ROOM_RESOURCE_TYPE",
                table: "ROOM_RESOURCE",
                column: "RESOURCE_TYPE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROOM_RESOURCE");

            migrationBuilder.DropTable(
                name: "ROOM_MAST");

            migrationBuilder.DropTable(
                name: "LOCATION_CONTACT");
        }
    }
}
