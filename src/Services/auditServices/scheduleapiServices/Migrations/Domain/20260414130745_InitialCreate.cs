using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleService.Migrations.Domain
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditSiteAudits",
                columns: table => new
                {
                    AuditSiteAuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    AuditTypeId = table.Column<int>(type: "int", nullable: false),
                    AuditNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "scheduled"),
                    LeadAuditorId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReportPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CertificateIssued = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditSiteAudits", x => x.AuditSiteAuditId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditSiteAudits_AuditId",
                table: "AuditSiteAudits",
                column: "AuditId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSiteAudits_AuditNumber",
                table: "AuditSiteAudits",
                column: "AuditNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditSiteAudits_LeadAuditorId",
                table: "AuditSiteAudits",
                column: "LeadAuditorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSiteAudits_ScheduledDate",
                table: "AuditSiteAudits",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSiteAudits_SiteId",
                table: "AuditSiteAudits",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditSiteAudits_Status",
                table: "AuditSiteAudits",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditSiteAudits");
        }
    }
}
