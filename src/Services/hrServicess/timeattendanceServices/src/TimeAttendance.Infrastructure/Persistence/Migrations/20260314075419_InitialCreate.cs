using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeAttendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ABSENTEEISM_DET",
                columns: table => new
                {
                    ABS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ABS_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    ABS_YEAR = table.Column<int>(type: "int", nullable: false),
                    ABS_MONTH = table.Column<int>(type: "int", nullable: false),
                    ABS_TOTMANDAYS = table.Column<long>(type: "bigint", nullable: false),
                    ABS_ABSMANDAYS = table.Column<long>(type: "bigint", nullable: false),
                    ABS_GRADECAT = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    ABS_FUNCTIONID = table.Column<long>(type: "bigint", nullable: false),
                    ABS_AGEID = table.Column<long>(type: "bigint", nullable: false),
                    ABS_EXPERIENCEID = table.Column<long>(type: "bigint", nullable: false),
                    ABS_GENDER = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    ABS_INTEXPID = table.Column<long>(type: "bigint", nullable: false),
                    ABS_TOTEXPID = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CREATED_BY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    LAST_MODIFIED_AT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LAST_MODIFIED_BY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ABSENTEEISM_DET", x => x.ABS_ID);
                });

            migrationBuilder.CreateTable(
                name: "ABSMIS",
                columns: table => new
                {
                    ABSID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UNTID = table.Column<int>(type: "int", nullable: true),
                    CID = table.Column<int>(type: "int", nullable: true),
                    DID = table.Column<long>(type: "bigint", nullable: true),
                    SYSID = table.Column<long>(type: "bigint", nullable: true),
                    GRD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    PLD = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    PDS = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    WOFF = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    LWOP = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    NPH = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    COF = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    BKL = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    APL = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    PNL = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    SWP = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    OND = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    MNTH = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    LOGSYSID = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    LWOPP = table.Column<decimal>(type: "DECIMAL(38,2)", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CREATED_BY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    LAST_MODIFIED_AT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LAST_MODIFIED_BY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ABSMIS", x => x.ABSID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ABSENTEEISM_DET_UNIT_PERIOD",
                table: "ABSENTEEISM_DET",
                columns: new[] { "ABS_UNITID", "ABS_YEAR", "ABS_MONTH" });

            migrationBuilder.CreateIndex(
                name: "IX_ABSMIS_UNIT_MONTH",
                table: "ABSMIS",
                columns: new[] { "UNTID", "MNTH" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ABSENTEEISM_DET");

            migrationBuilder.DropTable(
                name: "ABSMIS");
        }
    }
}
