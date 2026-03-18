using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinyearAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FINYEAR_MASTER",
                columns: table => new
                {
                    FY_ID = table.Column<long>(type: "bigint", nullable: false),
                    FY_NAME = table.Column<string>(type: "varchar(27)", maxLength: 27, nullable: false),
                    FY_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    FY_CLOSEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    FY_UPDATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    FY_UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINYEAR_MASTER", x => x.FY_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_FINYEAR_STARTDATE",
                table: "FINYEAR_MASTER",
                column: "FY_STARTDATE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FINYEAR_MASTER");
        }
    }
}
