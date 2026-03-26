using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReferenceDataService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOV_MASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<string>(type: "char(3)", nullable: false),
                    LOV_TYPE = table.Column<string>(type: "char(3)", nullable: true),
                    LOV_NAME = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MASTER", x => x.LOV_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_TYPEMASTER",
                columns: table => new
                {
                    LOV_TYPECODE = table.Column<string>(type: "char(3)", nullable: false),
                    LOV_TYPENAME = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TYPEMASTER", x => x.LOV_TYPECODE);
                });

            migrationBuilder.CreateTable(
                name: "PATHTOSQLSERVER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COM_COD = table.Column<string>(type: "char(3)", nullable: true),
                    SERVER_NAME = table.Column<string>(type: "varchar(20)", nullable: true),
                    DATABASE_NAME = table.Column<string>(type: "varchar(20)", nullable: true),
                    USER_ID = table.Column<string>(type: "varchar(10)", nullable: true),
                    DBPASSWORD = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATHTOSQLSERVER", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOV_MASTER");

            migrationBuilder.DropTable(
                name: "LOV_TYPEMASTER");

            migrationBuilder.DropTable(
                name: "PATHTOSQLSERVER");
        }
    }
}
