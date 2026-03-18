using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Masters.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOV_TYPEMASTER",
                columns: table => new
                {
                    LOV_TYPECODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    LOV_TYPENAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TYPEMASTER", x => x.LOV_TYPECODE);
                });

            migrationBuilder.CreateTable(
                name: "LOV_MASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOV_TYPE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    LOV_NAME = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MASTER", x => x.LOV_ID);
                    table.ForeignKey(
                        name: "FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE",
                        column: x => x.LOV_TYPE,
                        principalTable: "LOV_TYPEMASTER",
                        principalColumn: "LOV_TYPECODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_LOV_MASTER_LOV_TYPE",
                table: "LOV_MASTER",
                column: "LOV_TYPE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOV_MASTER");

            migrationBuilder.DropTable(
                name: "LOV_TYPEMASTER");
        }
    }
}
