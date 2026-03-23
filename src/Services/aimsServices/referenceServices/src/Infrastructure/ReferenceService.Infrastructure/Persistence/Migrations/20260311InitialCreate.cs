using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReferenceService.Infrastructure.Persistence.Migrations
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
                    LOV_TYPENAME = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    LOV_DESCRIPTION = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    LOV_TYPESEQ = table.Column<int>(type: "int", nullable: false),
                    LOV_STATUS = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LOV_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOV_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TYPEMAST", x => x.LOV_TYPEID);
                    table.UniqueConstraint("UQ_LOV_TYPENAME", x => x.LOV_TYPENAME);
                });

            migrationBuilder.CreateTable(
                name: "LEAVEFLAG",
                columns: table => new
                {
                    LEAVEFLAG_ID = table.Column<int>(type: "int", nullable: false),
                    LEAVEFLAG_CODE = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    LEAVEFLAG_DESCRIPTION = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    LEAVEFLAG_TYPE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LEAVEFLAG_STATUS = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LEAVEFLAG_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LEAVEFLAG_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVEFLAG", x => x.LEAVEFLAG_ID);
                });

            migrationBuilder.CreateTable(
                name: "PERMISSION_RULES",
                columns: table => new
                {
                    PERM_ID = table.Column<int>(type: "int", nullable: false),
                    PERM_RESOURCEID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PERM_ACTION = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PERM_DESCRIPTION = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    PERM_APPCODE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PERM_STATUS = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    PERM_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PERM_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISSION_RULES", x => x.PERM_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_MAST",
                columns: table => new
                {
                    LOV_ID = table.Column<int>(type: "int", nullable: false),
                    LOV_TYPEID = table.Column<int>(type: "int", nullable: false),
                    LOV_CODE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LOV_DESCRIPTION = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    LOV_LONGDESCRIPTION = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    LOV_SEQUENCE = table.Column<int>(type: "int", nullable: false),
                    LOV_STATUS = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LOV_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOV_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MAST", x => x.LOV_ID);
                    table.ForeignKey(
                        name: "FK_LOV_TYPEID",
                        column: x => x.LOV_TYPEID,
                        principalTable: "LOV_TYPEMAST",
                        principalColumn: "LOV_TYPEID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOV_TYPEMAST_STATUS",
                table: "LOV_TYPEMAST",
                column: "LOV_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_LEAVEFLAG_CODE",
                table: "LEAVEFLAG",
                column: "LEAVEFLAG_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_PERMISSION",
                table: "PERMISSION_RULES",
                columns: new[] { "PERM_RESOURCEID", "PERM_ACTION", "PERM_APPCODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LOV_MAST_TYPEID",
                table: "LOV_MAST",
                column: "LOV_TYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_LOV_MAST_CODE",
                table: "LOV_MAST",
                column: "LOV_CODE");

            migrationBuilder.CreateIndex(
                name: "UQ_LOV_CODE",
                table: "LOV_MAST",
                columns: new[] { "LOV_TYPEID", "LOV_CODE" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LEAVEFLAG");

            migrationBuilder.DropTable(
                name: "LOV_MAST");

            migrationBuilder.DropTable(
                name: "PERMISSION_RULES");

            migrationBuilder.DropTable(
                name: "LOV_TYPEMAST");
        }
    }
}
