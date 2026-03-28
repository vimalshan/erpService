using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RightCode = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RightDescription = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialInputMasters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialInputId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    YearId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EmployeeSysId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AppraisalSysId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CreatedBy = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialInputMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecialInputs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialInputId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    YearId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EmployeeSysId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    AppraisalSysId = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    Inputs = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialInputs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackerRights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    PinNumber = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    TrackerMode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    BusinessCode = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TrackerRights = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    VtcRights = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    RepresentingUnit = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    LetRight = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CarRight = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackerRights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRights",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    PinNumber = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    RightCode = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    BusinessCode = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    RightMode = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRights", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rights_RightCode",
                table: "Rights",
                column: "RightCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialInputMasters_SpecialInputId",
                table: "SpecialInputMasters",
                column: "SpecialInputId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialInputs_EmployeeSysId",
                table: "SpecialInputs",
                column: "EmployeeSysId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialInputs_SpecialInputId",
                table: "SpecialInputs",
                column: "SpecialInputId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackerRights_BusinessCode",
                table: "TrackerRights",
                column: "BusinessCode");

            migrationBuilder.CreateIndex(
                name: "IX_TrackerRights_UserId",
                table: "TrackerRights",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRights_RightCode",
                table: "UserRights",
                column: "RightCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserRights_UserId",
                table: "UserRights",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rights");

            migrationBuilder.DropTable(
                name: "SpecialInputMasters");

            migrationBuilder.DropTable(
                name: "SpecialInputs");

            migrationBuilder.DropTable(
                name: "TrackerRights");

            migrationBuilder.DropTable(
                name: "UserRights");
        }
    }
}
