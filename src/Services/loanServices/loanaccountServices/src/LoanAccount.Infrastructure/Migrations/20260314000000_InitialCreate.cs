using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanAccount.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanMains",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanNo = table.Column<long>(type: "bigint", nullable: false),
                    LoanAppId = table.Column<long>(type: "bigint", nullable: false),
                    EmpSysId = table.Column<long>(type: "bigint", nullable: false),
                    LoanId = table.Column<long>(type: "bigint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    OldPrincipalAdjustment = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    PrincipalOutstanding = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    DisbursementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecoveryMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstInstallmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastInstallmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanClosureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    SubClassId = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GuarantorId = table.Column<long>(type: "bigint", nullable: false),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NewLoanNo = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeWiseInterestRate = table.Column<bool>(type: "bit", nullable: false),
                    CompoundingFactor = table.Column<bool>(type: "bit", nullable: false),
                    InterestFrequency = table.Column<char>(type: "char(1)", nullable: false),
                    EmployeeSpecificInstallmentNos = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeSpecificInstallmentAmount = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: true),
                    AcountDisbursementEDId = table.Column<long>(type: "bigint", nullable: false),
                    PrincipalRecoveryEDId = table.Column<long>(type: "bigint", nullable: false),
                    InterestRecoveryEDId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanMains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanEmployeeInterestRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanNo = table.Column<long>(type: "bigint", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EMIAmount = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    InstallmentNumbers = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanEmployeeInterestRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanInstallments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanNo = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    InstallmentNo = table.Column<long>(type: "bigint", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    PrincipalOutstanding = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    PrincipalAdjustment = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    InterestAdjustment = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    InterestAccrued = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    InterestRecovered = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    PrincipalRecovered = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    InstallmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterestFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanInstallments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanLedgers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanNo = table.Column<long>(type: "bigint", nullable: false),
                    EmpSysId = table.Column<long>(type: "bigint", nullable: false),
                    EmpNo = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DCFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TransactionReferenceNo = table.Column<long>(type: "bigint", nullable: false),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanLedgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanSettlements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanNo = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    InstallmentNo = table.Column<long>(type: "bigint", nullable: false),
                    SettlementType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecoveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecoveryType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(19,0)", precision: 19, scale: 0, nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PayrollBatchId = table.Column<long>(type: "bigint", nullable: true),
                    AdjustmentLoanNo = table.Column<long>(type: "bigint", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanSettlements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_EMPINTRATEMAST_LOANINT_LOANNO",
                table: "LoanEmployeeInterestRates",
                column: "LoanNo");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_INS_LOANINS_LOANNO",
                table: "LoanInstallments",
                column: "LoanNo");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_INS_LOANINS_LOANNO_LOANINS_INSNO",
                table: "LoanInstallments",
                columns: new[] { "LoanNo", "InstallmentNo" });

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_LEDGER_LOAN_NO",
                table: "LoanLedgers",
                column: "LoanNo");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_LEDGER_EMPSYSID",
                table: "LoanLedgers",
                column: "EmpSysId");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_LEDGER_TRANSDATE",
                table: "LoanLedgers",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_MAIN_LOANNO",
                table: "LoanMains",
                column: "LoanNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_MAIN_APPID",
                table: "LoanMains",
                column: "LoanAppId");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_MAIN_EMPSYSID",
                table: "LoanMains",
                column: "EmpSysId");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_MAIN_UNITID",
                table: "LoanMains",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_SETTLEMENT_LOANNO",
                table: "LoanSettlements",
                column: "LoanNo");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_SETTLEMENT_RECDATE",
                table: "LoanSettlements",
                column: "RecoveryDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanEmployeeInterestRates");

            migrationBuilder.DropTable(
                name: "LoanInstallments");

            migrationBuilder.DropTable(
                name: "LoanLedgers");

            migrationBuilder.DropTable(
                name: "LoanMains");

            migrationBuilder.DropTable(
                name: "LoanSettlements");
        }
    }
}
