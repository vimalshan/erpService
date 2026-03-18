using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConditionalMasters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PayeeAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PayeePAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TaxRegime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "Old"),
                    FinancialYear = table.Column<int>(type: "int", nullable: false),
                    TotalExemption = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    TotalExemptionCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    TotalDeduction = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    TotalDeductionCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionalMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxMarginalDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeSystemId = table.Column<long>(type: "bigint", nullable: false),
                    FinancialYear = table.Column<int>(type: "int", nullable: false),
                    GrossIncome = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    GrossIncomeCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    StandardDeduction = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    StandardDeductionCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    TaxableIncome = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    TaxableIncomeCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    CalculatedTax = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    CalculatedTaxCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    Exemptions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxMarginalDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxDeduction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DeductionAmount = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    DeductionCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConditionalMasterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDeduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxDeduction_ConditionalMasters_ConditionalMasterId",
                        column: x => x.ConditionalMasterId,
                        principalTable: "ConditionalMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxExemption",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExemptionAmount = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    ExemptionCurrency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConditionalMasterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxExemption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxExemption_ConditionalMasters_ConditionalMasterId",
                        column: x => x.ConditionalMasterId,
                        principalTable: "ConditionalMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreatedAt",
                table: "ConditionalMasters",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_IsActive",
                table: "ConditionalMasters",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_PayeeId_FinancialYear",
                table: "ConditionalMasters",
                columns: new[] { "PayeeId", "FinancialYear" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxDeduction_ConditionalMasterId",
                table: "TaxDeduction",
                column: "ConditionalMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxExemption_ConditionalMasterId",
                table: "TaxExemption",
                column: "ConditionalMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatedAt",
                table: "TaxMarginalDetails",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSystemId_FinancialYear",
                table: "TaxMarginalDetails",
                columns: new[] { "EmployeeSystemId", "FinancialYear" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxDeduction");

            migrationBuilder.DropTable(
                name: "TaxExemption");

            migrationBuilder.DropTable(
                name: "TaxMarginalDetails");

            migrationBuilder.DropTable(
                name: "ConditionalMasters");
        }
    }
}
