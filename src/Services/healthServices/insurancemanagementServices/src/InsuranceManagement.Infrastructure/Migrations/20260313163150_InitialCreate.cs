using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsurancePlans",
                columns: table => new
                {
                    InsurancePlanId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlanDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PremiumRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MinPremium = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    MaxPremium = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    CoverageDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePlans", x => x.InsurancePlanId);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceEnrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpSysId = table.Column<long>(type: "bigint", nullable: false),
                    InsurancePlanId = table.Column<long>(type: "bigint", nullable: false),
                    COVERAGE_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthlyPremium = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    ENROLLMENT_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceEnrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_InsuranceEnrollments_InsurancePlans_InsurancePlanId",
                        column: x => x.InsurancePlanId,
                        principalTable: "InsurancePlans",
                        principalColumn: "InsurancePlanId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceClaims",
                columns: table => new
                {
                    ClaimId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpSysId = table.Column<long>(type: "bigint", nullable: false),
                    EnrollmentId = table.Column<long>(type: "bigint", nullable: false),
                    InsurancePlanId = table.Column<long>(type: "bigint", nullable: false),
                    CLAIM_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CLAIM_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    REIMBURSABLE_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    APPROVED_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HospitalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClaimRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CLAIM_STATUS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceClaims", x => x.ClaimId);
                    table.ForeignKey(
                        name: "FK_InsuranceClaims_InsuranceEnrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "InsuranceEnrollments",
                        principalColumn: "EnrollmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_CLAIM_STATUS",
                table: "InsuranceClaims",
                column: "CLAIM_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_CreatedOn",
                table: "InsuranceClaims",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_EmpSysId",
                table: "InsuranceClaims",
                column: "EmpSysId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceClaims_EnrollmentId",
                table: "InsuranceClaims",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEnrollments_EmpSysId",
                table: "InsuranceEnrollments",
                column: "EmpSysId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEnrollments_ENROLLMENT_STATUS",
                table: "InsuranceEnrollments",
                column: "ENROLLMENT_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceEnrollments_InsurancePlanId",
                table: "InsuranceEnrollments",
                column: "InsurancePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlans_IsActive",
                table: "InsurancePlans",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePlans_IsActive_CreatedOn",
                table: "InsurancePlans",
                columns: new[] { "IsActive", "CreatedOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceClaims");

            migrationBuilder.DropTable(
                name: "InsuranceEnrollments");

            migrationBuilder.DropTable(
                name: "InsurancePlans");
        }
    }
}
