using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appraisals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    StatusDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumericDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialPeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FinancialStartYear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinancialEndYear = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeNumber = table.Column<long>(type: "bigint", nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    GradeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    DDType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CompletionFlag = table.Column<char>(type: "char(1)", nullable: true),
                    StatusCode = table.Column<char>(type: "char(1)", nullable: true),
                    PinNumber = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appraisals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DDRatings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserPinNumber = table.Column<long>(type: "bigint", nullable: true),
                    BusinessCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Rating1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating4 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating5 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating1Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating2Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating4Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating3Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Rating5Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UniversitCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessUnitCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DDRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppraisalGoals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    FromUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ToUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppraiserRemarks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CandidateRemarks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weightage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinancialStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinancialEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PinNumber = table.Column<long>(type: "bigint", nullable: true),
                    AppraisalStatus = table.Column<char>(type: "char(1)", nullable: true),
                    Achievement = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Difference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModificationSerialNo = table.Column<long>(type: "bigint", nullable: true),
                    ExpenseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoalFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountabilityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraisalGoals_Appraisals_RequestNumber",
                        column: x => x.RequestNumber,
                        principalTable: "Appraisals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppraiseePerformances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<long>(type: "bigint", nullable: false),
                    PerformanceSerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PerformanceRating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitActual = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceRemarks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    AssessmentWeightage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CandidateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppraIserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppraIsalNumber = table.Column<long>(type: "bigint", nullable: true),
                    CandidateRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitMeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceRating1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceRemark1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformanceRatingValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PerfRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MeanRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AppPerfRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AppMeanRating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MeanRemarks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    AppMeanRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraiseePerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraiseePerformances_Appraisals_RequestNumber",
                        column: x => x.RequestNumber,
                        principalTable: "Appraisals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalGoals_RequestNumber",
                table: "AppraisalGoals",
                column: "RequestNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AppraiseePerformances_RequestNumber",
                table: "AppraiseePerformances",
                column: "RequestNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_RequestNumber",
                table: "Appraisals",
                column: "RequestNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_UserId",
                table: "Appraisals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DDRatings_BusinessCode",
                table: "DDRatings",
                column: "BusinessCode");

            migrationBuilder.CreateIndex(
                name: "IX_DDRatings_UserId",
                table: "DDRatings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppraisalGoals");

            migrationBuilder.DropTable(
                name: "AppraiseePerformances");

            migrationBuilder.DropTable(
                name: "DDRatings");

            migrationBuilder.DropTable(
                name: "Appraisals");
        }
    }
}
