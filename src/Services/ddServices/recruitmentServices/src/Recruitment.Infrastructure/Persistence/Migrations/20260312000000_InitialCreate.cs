using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruitment.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecruitmentCycles",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecruitmentCycleNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CycleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CycleYear = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentCycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentParameters",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecruitmentCycleNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParameterNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecruitmentCycleNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RoleDetails = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CadreCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrincipalAccount = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BusinessCode = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AP_SPARSH_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    AP_SPARSH_PIN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentJobDesciption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Achievements = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ReasonForJoining = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Strength = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Awards = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CrtMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DomainMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrtDocumentPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DomainDocumentPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationStatusHistories",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SerialNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationStatusHistories_Applications_ApplicationNumber",
                        column: x => x.ApplicationNumber,
                        principalTable: "Applications",
                        principalColumn: "ApplicationNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseDetails",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    PassOutYear = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDetails_Applications_ApplicationNumber",
                        column: x => x.ApplicationNumber,
                        principalTable: "Applications",
                        principalColumn: "ApplicationNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SteeringCommitteeAssessments",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApplicationNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommitteeMemberPin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParameterNo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mark = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteeringCommitteeAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SteeringCommitteeAssessments_Applications_ApplicationNumber",
                        column: x => x.ApplicationNumber,
                        principalTable: "Applications",
                        principalColumn: "ApplicationNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobId",
                table: "Applications",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusHistories_ApplicationNumber",
                table: "ApplicationStatusHistories",
                column: "ApplicationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDetails_ApplicationNumber",
                table: "CourseDetails",
                column: "ApplicationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_SteeringCommitteeAssessments_ApplicationNumber",
                table: "SteeringCommitteeAssessments",
                column: "ApplicationNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentParameters");

            migrationBuilder.DropTable(
                name: "ApplicationStatusHistories");

            migrationBuilder.DropTable(
                name: "CourseDetails");

            migrationBuilder.DropTable(
                name: "RecruitmentCycles");

            migrationBuilder.DropTable(
                name: "SteeringCommitteeAssessments");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
