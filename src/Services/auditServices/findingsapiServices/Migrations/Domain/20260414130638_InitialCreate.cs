using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindingsAPI.Gateway.Migrations.Domain
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FindingCategories",
                columns: table => new
                {
                    FindingCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingCategories", x => x.FindingCategoryId);
                    table.ForeignKey(
                        name: "FK_FindingCategories_FindingCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "FindingCategories",
                        principalColumn: "FindingCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FindingStatuses",
                columns: table => new
                {
                    FindingStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsClosedStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingStatuses", x => x.FindingStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Findings",
                columns: table => new
                {
                    FindingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FindingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuditId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FindingType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FindingStatusId = table.Column<int>(type: "int", nullable: false),
                    FindingCategoryId = table.Column<int>(type: "int", nullable: true),
                    IdentifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    IdentifiedBy = table.Column<int>(type: "int", nullable: true),
                    AssignedTo = table.Column<int>(type: "int", nullable: true),
                    Evidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RootCause = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectiveAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreventiveAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Findings", x => x.FindingId);
                    table.ForeignKey(
                        name: "FK_Findings_FindingCategories_FindingCategoryId",
                        column: x => x.FindingCategoryId,
                        principalTable: "FindingCategories",
                        principalColumn: "FindingCategoryId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Findings_FindingStatuses_FindingStatusId",
                        column: x => x.FindingStatusId,
                        principalTable: "FindingStatuses",
                        principalColumn: "FindingStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FindingClauses",
                columns: table => new
                {
                    FindingClauseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FindingId = table.Column<int>(type: "int", nullable: false),
                    ClauseId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingClauses", x => x.FindingClauseId);
                    table.ForeignKey(
                        name: "FK_FindingClauses_Findings_FindingId",
                        column: x => x.FindingId,
                        principalTable: "Findings",
                        principalColumn: "FindingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FindingFocusAreas",
                columns: table => new
                {
                    FindingFocusAreaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FindingId = table.Column<int>(type: "int", nullable: false),
                    FocusAreaId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingFocusAreas", x => x.FindingFocusAreaId);
                    table.ForeignKey(
                        name: "FK_FindingFocusAreas_Findings_FindingId",
                        column: x => x.FindingId,
                        principalTable: "Findings",
                        principalColumn: "FindingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FindingResponses",
                columns: table => new
                {
                    FindingResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FindingId = table.Column<int>(type: "int", nullable: false),
                    ResponseText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedBy = table.Column<int>(type: "int", nullable: false),
                    IsSubmittedToDNV = table.Column<bool>(type: "bit", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReviewComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedBy = table.Column<int>(type: "int", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingResponses", x => x.FindingResponseId);
                    table.ForeignKey(
                        name: "FK_FindingResponses_Findings_FindingId",
                        column: x => x.FindingId,
                        principalTable: "Findings",
                        principalColumn: "FindingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FindingCategories_CategoryCode",
                table: "FindingCategories",
                column: "CategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingCategories_CategoryName",
                table: "FindingCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingCategories_ParentCategoryId",
                table: "FindingCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingClauses_FindingId_ClauseId",
                table: "FindingClauses",
                columns: new[] { "FindingId", "ClauseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingFocusAreas_FindingId_FocusAreaId",
                table: "FindingFocusAreas",
                columns: new[] { "FindingId", "FocusAreaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingResponses_FindingId",
                table: "FindingResponses",
                column: "FindingId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingResponses_IsActive",
                table: "FindingResponses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FindingResponses_IsSubmittedToDNV",
                table: "FindingResponses",
                column: "IsSubmittedToDNV");

            migrationBuilder.CreateIndex(
                name: "IX_FindingResponses_RespondedBy",
                table: "FindingResponses",
                column: "RespondedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FindingResponses_ResponseDate",
                table: "FindingResponses",
                column: "ResponseDate");

            migrationBuilder.CreateIndex(
                name: "IX_FindingResponses_Status",
                table: "FindingResponses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_AssignedTo",
                table: "Findings",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_AuditId",
                table: "Findings",
                column: "AuditId");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_DueDate",
                table: "Findings",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_FindingCategoryId",
                table: "Findings",
                column: "FindingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_FindingNumber",
                table: "Findings",
                column: "FindingNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Findings_FindingStatusId",
                table: "Findings",
                column: "FindingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_FindingType",
                table: "Findings",
                column: "FindingType");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_IsActive",
                table: "Findings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_Severity",
                table: "Findings",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_Findings_SiteId",
                table: "Findings",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_FindingStatuses_StatusCode",
                table: "FindingStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FindingStatuses_StatusName",
                table: "FindingStatuses",
                column: "StatusName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FindingClauses");

            migrationBuilder.DropTable(
                name: "FindingFocusAreas");

            migrationBuilder.DropTable(
                name: "FindingResponses");

            migrationBuilder.DropTable(
                name: "Findings");

            migrationBuilder.DropTable(
                name: "FindingCategories");

            migrationBuilder.DropTable(
                name: "FindingStatuses");
        }
    }
}
