using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TRAVEL_INSURANCE",
                columns: table => new
                {
                    IN_COM_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    IN_PLN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    IN_INS_TYP = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    IN_PASS_NUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IN_ISS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IN_VIS_PLC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IN_VIS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IN_NOM_NAM1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IN_NOM_NAM2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IN_INS_STS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    IN_CRT_NUM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IN_UPD_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IN_UPD_UID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IN_UPD_UNUM = table.Column<long>(type: "bigint", nullable: true),
                    IN_REM_MRK = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IN_FLX_FLD1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IN_FLX_FLD2 = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    IN_FLX_FLD3 = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    IN_FLX_FLD4 = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_INSURANCE", x => new { x.IN_COM_COD, x.IN_PLN_NUM });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TRAVEL_INSURANCE");
        }
    }
}
