using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContractService.Migrations.Domain
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    TotalValue = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "USD"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    SignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignedByClient = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SignedByDNV = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContractPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Terms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AutoRenewal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                });

            migrationBuilder.CreateTable(
                name: "ContractServices",
                columns: table => new
                {
                    ContractServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "USD"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Active"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractServices", x => x.ContractServiceId);
                    table.ForeignKey(
                        name: "FK_ContractServices_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractSites",
                columns: table => new
                {
                    ContractSiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Active"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSites", x => x.ContractSiteId);
                    table.ForeignKey(
                        name: "FK_ContractSites_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractNumber",
                table: "Contracts",
                column: "ContractNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_EndDate",
                table: "Contracts",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_IsActive",
                table: "Contracts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_StartDate",
                table: "Contracts",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Status",
                table: "Contracts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_ContractId",
                table: "ContractServices",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_ContractId_ServiceId",
                table: "ContractServices",
                columns: new[] { "ContractId", "ServiceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_IsActive",
                table: "ContractServices",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ContractServices_ServiceId",
                table: "ContractServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSites_ContractId",
                table: "ContractSites",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSites_ContractId_SiteId",
                table: "ContractSites",
                columns: new[] { "ContractId", "SiteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractSites_IsActive",
                table: "ContractSites",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSites_SiteId",
                table: "ContractSites",
                column: "SiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractServices");

            migrationBuilder.DropTable(
                name: "ContractSites");

            migrationBuilder.DropTable(
                name: "Contracts");
        }
    }
}
