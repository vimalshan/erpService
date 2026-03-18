using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LovService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ITEMDATA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CATNAME = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ITEMNAME = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    MAKE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    UOM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PRICE = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITEMDATA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_TYPE",
                columns: table => new
                {
                    LOV_TYPE_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOV_TYPE_NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TYPE", x => x.LOV_TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_MASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOV_TYPE_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOV_NAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LOV_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    LOV_UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MASTER", x => x.LOV_ID);
                    table.ForeignKey(
                        name: "FK_LOV_MASTER_TYPE",
                        column: x => x.LOV_TYPE_ID,
                        principalTable: "LOV_TYPE",
                        principalColumn: "LOV_TYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_LOV_MASTER_TYPEID",
                table: "LOV_MASTER",
                column: "LOV_TYPE_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITEMDATA");

            migrationBuilder.DropTable(
                name: "LOV_MASTER");

            migrationBuilder.DropTable(
                name: "LOV_TYPE");
        }
    }
}
