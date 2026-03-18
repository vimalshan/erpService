using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompensationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMP_GRADE",
                columns: table => new
                {
                    GRADE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRADE_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GRADE_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GRADE_LEVEL = table.Column<int>(type: "int", nullable: false),
                    BASE_SALARY = table.Column<decimal>(type: "numeric(19,2)", precision: 19, scale: 2, nullable: false),
                    HRA_PERCENTAGE = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DA_PERCENTAGE = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    GRADE_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    EFFECTIVE_FROM = table.Column<DateTime>(type: "date", nullable: false),
                    EFFECTIVE_TO = table.Column<DateTime>(type: "date", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMP_GRADE", x => x.GRADE_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMP_GRADE_STATUS",
                table: "COMP_GRADE",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_COMP_GRADE_LEVEL",
                table: "COMP_GRADE",
                column: "GRADE_LEVEL");

            migrationBuilder.CreateIndex(
                name: "IX_COMP_GRADE_EFFECTIVE",
                table: "COMP_GRADE",
                columns: new[] { "EFFECTIVE_FROM", "EFFECTIVE_TO" });

            migrationBuilder.CreateIndex(
                name: "IX_COMP_GRADE_CODE",
                table: "COMP_GRADE",
                column: "GRADE_CODE",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMP_GRADE");
        }
    }
}
