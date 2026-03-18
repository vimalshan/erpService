using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReimbursementService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "REIM_TRAN",
                columns: table => new
                {
                    REIM_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REIM_REF_NO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    REIM_TYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    REIM_AMOUNT = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    REIM_CURRENCY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "INR"),
                    REIM_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    EXPENSE_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LOCATION = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    REIM_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "'DRAFT'"),
                    APPROVAL_LEVEL = table.Column<int>(type: "int", nullable: true),
                    APPROVED_BY = table.Column<long>(type: "bigint", nullable: true),
                    APPROVED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    REJECTION_REASON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAYMENT_DATE = table.Column<DateOnly>(type: "date", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REIM_TRAN", x => x.REIM_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_REIM_TRAN_DATE",
                table: "REIM_TRAN",
                column: "REIM_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_REIM_TRAN_EMP_SYSID",
                table: "REIM_TRAN",
                column: "EMP_SYSID");

            migrationBuilder.CreateIndex(
                name: "IX_REIM_TRAN_REIM_REF_NO",
                table: "REIM_TRAN",
                column: "REIM_REF_NO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_REIM_TRAN_STATUS",
                table: "REIM_TRAN",
                column: "REIM_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_REIM_TRAN_TYPE",
                table: "REIM_TRAN",
                column: "REIM_TYPE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "REIM_TRAN");
        }
    }
}
