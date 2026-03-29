using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixMedicineCreditPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MEDICINE_CREDIT",
                table: "MEDICINE_CREDIT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MEDICINE_CREDIT",
                table: "MEDICINE_CREDIT",
                columns: new[] { "MD_COM_COD", "MD_TRN_COD" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MEDICINE_CREDIT",
                table: "MEDICINE_CREDIT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MEDICINE_CREDIT",
                table: "MEDICINE_CREDIT",
                column: "MD_COM_COD");
        }
    }
}
