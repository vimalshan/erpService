using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprovalService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APPR_MAST",
                columns: table => new
                {
                    APPR_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APPR_CODE = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    APPR_DESC = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    APPR_MODULE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    APPR_LEVEL = table.Column<int>(type: "int", nullable: false),
                    APPR_STATUS = table.Column<string>(type: "char(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A"),
                    CREATED_BY = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPR_MAST", x => x.APPR_ID);
                    table.UniqueConstraint("UQ_APPR_CODE", x => x.APPR_CODE);
                    table.CheckConstraint("CK_APPR_LEVEL", "[APPR_LEVEL] > 0");
                });

            migrationBuilder.CreateTable(
                name: "APPROVER_EMP",
                columns: table => new
                {
                    APPROVER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APPR_ID = table.Column<int>(type: "int", nullable: false),
                    EMP_ID = table.Column<int>(type: "int", nullable: false),
                    APPR_LEVEL = table.Column<int>(type: "int", nullable: false),
                    APPR_STATUS = table.Column<string>(type: "char(1)", fixedLength: true, maxLength: 1, nullable: false, defaultValue: "A"),
                    EFF_FROM_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    EFF_TO_DATE = table.Column<DateOnly>(type: "date", nullable: true),
                    CREATED_BY = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVER_EMP", x => x.APPROVER_ID);
                    table.ForeignKey(
                        name: "FK_APPROVER_EMP_APPR_MAST_APPR_ID",
                        column: x => x.APPR_ID,
                        principalTable: "APPR_MAST",
                        principalColumn: "APPR_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.CheckConstraint("CK_APPROVER_LEVEL", "[APPR_LEVEL] > 0");
                });

            // Create Indexes
            migrationBuilder.CreateIndex(
                name: "IX_APPR_MAST_CODE",
                table: "APPR_MAST",
                column: "APPR_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_APPR_MAST_MODULE",
                table: "APPR_MAST",
                column: "APPR_MODULE");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVER_EMP_APPR_ID",
                table: "APPROVER_EMP",
                column: "APPR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVER_EMP_EMP_ID",
                table: "APPROVER_EMP",
                column: "EMP_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APPROVER_EMP");

            migrationBuilder.DropTable(
                name: "APPR_MAST");
        }
    }
}
