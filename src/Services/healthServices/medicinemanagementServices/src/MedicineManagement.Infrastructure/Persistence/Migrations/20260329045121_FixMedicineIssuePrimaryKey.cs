using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixMedicineIssuePrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MD_TRN_NUM",
                table: "MEDICINE_ISSUE",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MD_COM_COD",
                table: "MEDICINE_ISSUE",
                type: "CHAR(3)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "CHAR(3)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MEDICINE_ISSUE",
                table: "MEDICINE_ISSUE",
                columns: new[] { "MD_COM_COD", "MD_TRN_NUM" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MEDICINE_ISSUE",
                table: "MEDICINE_ISSUE");

            migrationBuilder.AlterColumn<string>(
                name: "MD_TRN_NUM",
                table: "MEDICINE_ISSUE",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "MD_COM_COD",
                table: "MEDICINE_ISSUE",
                type: "CHAR(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "CHAR(3)");
        }
    }
}
