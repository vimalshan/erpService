using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LovService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOV_TYPEMAST",
                columns: table => new
                {
                    LOV_TYPEID = table.Column<int>(type: "int", nullable: false),
                    LOV_TYPENAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LOV_CATEGORY = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOV_ORGID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TYPEMAST", x => x.LOV_TYPEID);
                });

            migrationBuilder.CreateTable(
                name: "PROGRAMLOV_MAST",
                columns: table => new
                {
                    PRLOV_TYPECODE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PRLOV_CODE = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    PRLOV_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROGRAMLOV_MAST", x => new { x.PRLOV_CODE, x.PRLOV_TYPECODE });
                });

            migrationBuilder.CreateTable(
                name: "LOV_MASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOV_TYPEID = table.Column<int>(type: "int", nullable: false),
                    LOV_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    LOV_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LOV_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOV_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOV_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MASTER", x => x.LOV_ID);
                    table.ForeignKey(
                        name: "FK_LOV_MASTER_LOV_TYPEMAST_LOV_TYPEID",
                        column: x => x.LOV_TYPEID,
                        principalTable: "LOV_TYPEMAST",
                        principalColumn: "LOV_TYPEID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_LOV_MASTER_LOV_TYPEID",
                table: "LOV_MASTER",
                column: "LOV_TYPEID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOV_MASTER");

            migrationBuilder.DropTable(
                name: "PROGRAMLOV_MAST");

            migrationBuilder.DropTable(
                name: "LOV_TYPEMAST");
        }
    }
}
