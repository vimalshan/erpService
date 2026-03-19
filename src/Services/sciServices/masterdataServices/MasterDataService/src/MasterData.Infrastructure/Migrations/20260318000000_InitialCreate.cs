using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterData.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMPANY_UNITMASTER",
                columns: table => new
                {
                    COMPANY_UNIT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COMPANY_UNIT_CODE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    COMPANY_UNIT_NAME = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPANY_UNITMASTER", x => x.COMPANY_UNIT_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOCATION_MASTER",
                columns: table => new
                {
                    LOCATION_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOCATION_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCATION_MASTER", x => x.LOCATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "SUPPLIER_MASTER",
                columns: table => new
                {
                    SU_CUS_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SU_CUS_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SU_CUS_DET = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SU_ENT_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SU_ENT_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SU_ENT_NUM = table.Column<decimal>(type: "numeric(38,0)", precision: 38, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPPLIER_MASTER", x => x.SU_CUS_COD);
                });

            migrationBuilder.CreateTable(
                name: "ORA_STATEMASTER",
                columns: table => new
                {
                    ORA_STATECODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ORA_STATENAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_STATEMASTER", x => x.ORA_STATECODE);
                });

            migrationBuilder.CreateTable(
                name: "ORA_CITYMASTER",
                columns: table => new
                {
                    ORA_CITYCODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ORA_CITYNAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ORA_STATECODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_CITYMASTER", x => x.ORA_CITYCODE);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMPANY_UNITMASTER_Code",
                table: "COMPANY_UNITMASTER",
                column: "COMPANY_UNIT_CODE",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMPANY_UNITMASTER");

            migrationBuilder.DropTable(
                name: "LOCATION_MASTER");

            migrationBuilder.DropTable(
                name: "SUPPLIER_MASTER");

            migrationBuilder.DropTable(
                name: "ORA_STATEMASTER");

            migrationBuilder.DropTable(
                name: "ORA_CITYMASTER");
        }
    }
}
