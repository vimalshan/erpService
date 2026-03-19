using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRDocumentService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRDOC_COUNTER",
                columns: table => new
                {
                    DOC_NO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "HRDOC_DET",
                columns: table => new
                {
                    DOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_NO = table.Column<long>(type: "bigint", nullable: false),
                    DOC_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DOC_PAYREFNO = table.Column<long>(type: "bigint", nullable: false),
                    DOC_LOCID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_REMARKS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOC_USERID = table.Column<long>(type: "bigint", nullable: false),
                    DOC_REFNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DOC_REFNAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    DOC_DOCSTATUS = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DOC_SOURCE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DOC_ACTIONSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DOC_ACTIONTAKENON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    DOC_ACTIONTAKENBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DOC_FILEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOC_CANCELFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DOC_CANCELBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DOC_CANCELON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    DOC_PAYBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DOC_REJECTREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDOC_DET", x => x.DOC_ID);
                });

            migrationBuilder.CreateTable(
                name: "HRDOC_RECDET",
                columns: table => new
                {
                    HRREC_ID = table.Column<long>(type: "bigint", nullable: false),
                    HRREC_ENVID = table.Column<long>(type: "bigint", nullable: false),
                    HRREC_HRDOCID = table.Column<long>(type: "bigint", nullable: false),
                    HRREC_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    HRREC_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDOC_RECDET", x => x.HRREC_ID);
                    table.ForeignKey(
                        name: "FK_HRDOC_RECDET_HRDOC_DET_HRREC_HRDOCID",
                        column: x => x.HRREC_HRDOCID,
                        principalTable: "HRDOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRDOC_SSFILELIST",
                columns: table => new
                {
                    FILE_ID = table.Column<long>(type: "bigint", nullable: false),
                    FILE_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    FILE_PATH = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FILE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDOC_SSFILELIST", x => x.FILE_ID);
                    table.ForeignKey(
                        name: "FK_HRDOC_SSFILELIST_HRDOC_DET_FILE_DOCID",
                        column: x => x.FILE_DOCID,
                        principalTable: "HRDOC_DET",
                        principalColumn: "DOC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HRDOC_RECDET_HRREC_HRDOCID",
                table: "HRDOC_RECDET",
                column: "HRREC_HRDOCID");

            migrationBuilder.CreateIndex(
                name: "IX_HRDOC_SSFILELIST_FILE_DOCID",
                table: "HRDOC_SSFILELIST",
                column: "FILE_DOCID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRDOC_COUNTER");

            migrationBuilder.DropTable(
                name: "HRDOC_RECDET");

            migrationBuilder.DropTable(
                name: "HRDOC_SSFILELIST");

            migrationBuilder.DropTable(
                name: "HRDOC_DET");
        }
    }
}
