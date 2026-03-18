using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObjectiveService.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Employees",
            columns: table => new
            {
                Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                UserId = table.Column<string>(maxLength: 50, nullable: false),
                PinNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                EmployeeSysId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Department = table.Column<string>(maxLength: 100, nullable: true),
                Status = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "A"),
                CreatedDate = table.Column<DateTime>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Employees", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ControlPoints",
            columns: table => new
            {
                Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                EmployeeSysId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                DDYearId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Source = table.Column<string>(maxLength: 5, nullable: false),
                RefId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                SerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Description = table.Column<string>(maxLength: 4000, nullable: false),
                Category = table.Column<string>(maxLength: 100, nullable: false),
                UnitOfMeasurement = table.Column<string>(maxLength: 65, nullable: false),
                UnitFrom = table.Column<string>(maxLength: 50, nullable: false),
                UnitTo = table.Column<string>(maxLength: 50, nullable: false),
                VersionNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Weightage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                AccountabilityId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                ModifiedDate = table.Column<DateTime>(nullable: true),
                Status = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "A")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControlPoints", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Goals",
            columns: table => new
            {
                Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                UserId = table.Column<string>(maxLength: 50, nullable: false),
                PinNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                PeriodFrom = table.Column<DateTime>(nullable: false),
                PeriodTo = table.Column<DateTime>(nullable: false),
                ReferenceNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                FormFlag = table.Column<string>(maxLength: 1, nullable: false),
                NextReviewDate = table.Column<DateTime>(nullable: true),
                ClosureDate = table.Column<DateTime>(nullable: true),
                Status = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "N"),
                AppraiserRemarks = table.Column<string>(maxLength: 4000, nullable: true),
                HasAttachment = table.Column<bool>(nullable: false, defaultValue: false),
                AttachmentUrl = table.Column<string>(maxLength: 500, nullable: true),
                CreatedDate = table.Column<DateTime>(nullable: false),
                ModifiedDate = table.Column<DateTime>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Goals", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GoalSubGoals",
            columns: table => new
            {
                Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                GoalId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Description = table.Column<string>(maxLength: 4000, nullable: false),
                UnitFrom = table.Column<string>(maxLength: 20, nullable: true),
                UnitTo = table.Column<string>(maxLength: 20, nullable: true),
                Achievement = table.Column<string>(maxLength: 4000, nullable: true),
                Difference = table.Column<string>(maxLength: 4000, nullable: true),
                ExpectationCode = table.Column<string>(maxLength: 3, nullable: true),
                GoalFlag = table.Column<string>(maxLength: 3, nullable: true),
                ModificationSerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                UnitOfMeasurement = table.Column<string>(maxLength: 65, nullable: true),
                Category = table.Column<string>(maxLength: 100, nullable: true),
                Remarks = table.Column<string>(maxLength: 4000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GoalSubGoals", x => x.Id);
                table.ForeignKey(
                    name: "FK_GoalSubGoals_Goals_GoalId",
                    column: x => x.GoalId,
                    principalTable: "Goals",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ControlPointRequests",
            columns: table => new
            {
                Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                EmployeeSysId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                DDYearId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CreatedOn = table.Column<DateTime>(nullable: false),
                SubmittedOn = table.Column<DateTime>(nullable: true),
                Status = table.Column<string>(maxLength: 1, nullable: false, defaultValue: "N"),
                Remarks = table.Column<string>(maxLength: 500, nullable: true),
                SubordinateFlag = table.Column<string>(maxLength: 1, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControlPointRequests", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ControlPointRequestDetails",
            columns: table => new
            {
                Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ControlPointRequestId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ControlPointId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                DDYearId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                EmployeeSysId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Source = table.Column<string>(maxLength: 5, nullable: false),
                ReferenceId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                SerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Description = table.Column<string>(maxLength: 4000, nullable: false),
                Category = table.Column<string>(maxLength: 100, nullable: false),
                UnitOfMeasurement = table.Column<string>(maxLength: 65, nullable: false),
                UnitFrom = table.Column<string>(maxLength: 50, nullable: false),
                UnitTo = table.Column<string>(maxLength: 50, nullable: false),
                VersionNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Weightage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                AccountabilityId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                ModifiedDate = table.Column<DateTime>(nullable: false),
                AppStatus = table.Column<string>(maxLength: 1, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControlPointRequestDetails", x => x.Id);
                table.ForeignKey(
                    name: "FK_ControlPointRequestDetails_ControlPointRequests_ControlPointRequestId",
                    column: x => x.ControlPointRequestId,
                    principalTable: "ControlPointRequests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ControlPointRequestApprovals",
            columns: table => new
            {
                Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ControlPointRequestId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                ApproverSysId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Status = table.Column<string>(maxLength: 1, nullable: true),
                Remarks = table.Column<string>(maxLength: 1000, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ControlPointRequestApprovals", x => x.Id);
                table.ForeignKey(
                    name: "FK_ControlPointRequestApprovals_ControlPointRequests_ControlPointRequestId",
                    column: x => x.ControlPointRequestId,
                    principalTable: "ControlPointRequests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        // Indexes
        migrationBuilder.CreateIndex("IX_Employees_UserId", "Employees", "UserId", unique: true);
        migrationBuilder.CreateIndex("IX_Employees_EmployeeSysId", "Employees", "EmployeeSysId", unique: true);
        migrationBuilder.CreateIndex("IX_ControlPoints_EmployeeSysId_DDYearId", "ControlPoints", new[] { "EmployeeSysId", "DDYearId" });
        migrationBuilder.CreateIndex("IX_Goals_UserId_PeriodFrom_PeriodTo", "Goals", new[] { "UserId", "PeriodFrom", "PeriodTo" });
        migrationBuilder.CreateIndex("IX_GoalSubGoals_GoalId", "GoalSubGoals", "GoalId");
        migrationBuilder.CreateIndex("IX_ControlPointRequestDetails_ControlPointRequestId", "ControlPointRequestDetails", "ControlPointRequestId");
        migrationBuilder.CreateIndex("IX_ControlPointRequestApprovals_ControlPointRequestId", "ControlPointRequestApprovals", "ControlPointRequestId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("ControlPointRequestApprovals");
        migrationBuilder.DropTable("ControlPointRequestDetails");
        migrationBuilder.DropTable("ControlPointRequests");
        migrationBuilder.DropTable("GoalSubGoals");
        migrationBuilder.DropTable("Goals");
        migrationBuilder.DropTable("ControlPoints");
        migrationBuilder.DropTable("Employees");
    }
}
