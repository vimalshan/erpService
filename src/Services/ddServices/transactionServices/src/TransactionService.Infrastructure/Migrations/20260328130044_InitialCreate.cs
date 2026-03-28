using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemandMasters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    DemandDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DemandStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletionRemarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CompletedBy = table.Column<long>(type: "bigint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaaBudgets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    YearId = table.Column<long>(type: "bigint", nullable: false),
                    BudgetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaBudgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaaLevels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LevelAmount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LevelReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LevelMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LevelMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LevelEffDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LevelCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LevelUpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    LevelUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaaMailTriggers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuarterId = table.Column<long>(type: "bigint", nullable: false),
                    EmpSysId = table.Column<long>(type: "bigint", nullable: false),
                    MailId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TriggeredBy = table.Column<long>(type: "bigint", nullable: false),
                    TriggeredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaMailTriggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaaPeriods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<long>(type: "bigint", nullable: false),
                    QuarterNo = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PeriodOpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodCloseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CircularGenOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CircularGenBy = table.Column<long>(type: "bigint", nullable: true),
                    ReminderLetOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FormOpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppraiserLastDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewerLastDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BhrLastDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UhrLastDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaaRecommends",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<long>(type: "bigint", nullable: false),
                    PeriodId = table.Column<long>(type: "bigint", nullable: false),
                    EmpSysId = table.Column<long>(type: "bigint", nullable: false),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    CtcAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumCap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EligibilityAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecommendAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InitiativeTaken = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Results = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    AddRemarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<long>(type: "bigint", nullable: false),
                    RejectionBy = table.Column<long>(type: "bigint", nullable: true),
                    RejectionOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecommendBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RecommendSubmitBy = table.Column<long>(type: "bigint", nullable: true),
                    RecommendSubmitOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewerSubmitBy = table.Column<long>(type: "bigint", nullable: true),
                    ReviewerSubmitOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BhrSubmitBy = table.Column<long>(type: "bigint", nullable: true),
                    BhrSubmitOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChrSubmitBy = table.Column<long>(type: "bigint", nullable: true),
                    ChrSubmitOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionRemarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FinalLevel = table.Column<long>(type: "bigint", nullable: true),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InitiativeLetter = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ResultsLetter = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    UhrSubmitBy = table.Column<long>(type: "bigint", nullable: true),
                    UhrSubmitOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecommendSignId = table.Column<long>(type: "bigint", nullable: true),
                    RecommendSignId2 = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaRecommends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaaRecommends_SaaLevels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "SaaLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaaRecommends_SaaPeriods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "SaaPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaaSubmits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodId = table.Column<long>(type: "bigint", nullable: false),
                    BusId = table.Column<long>(type: "bigint", nullable: false),
                    BhrFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ChrFlag = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BhrUpdBy = table.Column<long>(type: "bigint", nullable: false),
                    BhrUpdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BhrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChrUpdBy = table.Column<long>(type: "bigint", nullable: true),
                    ChrUpdOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaaSubmits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaaSubmits_SaaPeriods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "SaaPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandMasters_DemandStatus",
                table: "DemandMasters",
                column: "DemandStatus");

            migrationBuilder.CreateIndex(
                name: "IX_DemandMasters_DepartmentId",
                table: "DemandMasters",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandMasters_Priority",
                table: "DemandMasters",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_SaaBudgets_BusinessId_YearId",
                table: "SaaBudgets",
                columns: new[] { "BusinessId", "YearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaaBudgets_YearId",
                table: "SaaBudgets",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaMailTriggers_EmpSysId",
                table: "SaaMailTriggers",
                column: "EmpSysId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaMailTriggers_QuarterId",
                table: "SaaMailTriggers",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaPeriods_Status",
                table: "SaaPeriods",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SaaPeriods_YearId",
                table: "SaaPeriods",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaRecommends_EmpSysId",
                table: "SaaRecommends",
                column: "EmpSysId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaRecommends_LevelId",
                table: "SaaRecommends",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaRecommends_PeriodId",
                table: "SaaRecommends",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaRecommends_Status",
                table: "SaaRecommends",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SaaSubmits_PeriodId",
                table: "SaaSubmits",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_SaaSubmits_PeriodId_BusId",
                table: "SaaSubmits",
                columns: new[] { "PeriodId", "BusId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandMasters");

            migrationBuilder.DropTable(
                name: "SaaBudgets");

            migrationBuilder.DropTable(
                name: "SaaMailTriggers");

            migrationBuilder.DropTable(
                name: "SaaRecommends");

            migrationBuilder.DropTable(
                name: "SaaSubmits");

            migrationBuilder.DropTable(
                name: "SaaLevels");

            migrationBuilder.DropTable(
                name: "SaaPeriods");
        }
    }
}
