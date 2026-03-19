using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilingAndArchiveService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FILE_MASTER",
                columns: table => new
                {
                    FILE_ID = table.Column<long>(type: "bigint", nullable: false),
                    FILE_ORGID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FILE_YEAR = table.Column<long>(type: "bigint", nullable: false),
                    FILE_NO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FILE_STATUS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    FILE_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FILE_PODNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FILE_COURIERNAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FILE_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    FILE_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    FILE_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    FILE_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    FILE_DISPATCHEDON = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    FILE_DISPATCHEDBY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILE_MASTER", x => x.FILE_ID);
                });

            migrationBuilder.CreateTable(
                name: "FILING_COUNTER",
                columns: table => new
                {
                    FILING_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FILE_COUNT = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILING_COUNTER", x => x.FILING_BUID);
                });

            migrationBuilder.CreateTable(
                name: "FILING_DOC_PRINT",
                columns: table => new
                {
                    DOC_SEQ = table.Column<long>(type: "bigint", nullable: false),
                    DOC_KEY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DOC_FILENO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FILING_DOC_PRINT", x => x.DOC_SEQ);
                });

            migrationBuilder.CreateTable(
                name: "FILINGDOC_ERROR_LIST",
                columns: table => new
                {
                    DOC_KEY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    SYS_ID = table.Column<long>(type: "bigint", nullable: true),
                    ACCOUNTING_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    FLAG = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SNO = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_FILE_MASTER_ORG_FILENO",
                table: "FILE_MASTER",
                columns: new[] { "FILE_ORGID", "FILE_NO" });

            migrationBuilder.CreateIndex(
                name: "IX_FILE_MASTER_STATUS",
                table: "FILE_MASTER",
                column: "FILE_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_FILE_MASTER_YEAR",
                table: "FILE_MASTER",
                column: "FILE_YEAR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FILE_MASTER");

            migrationBuilder.DropTable(
                name: "FILING_COUNTER");

            migrationBuilder.DropTable(
                name: "FILING_DOC_PRINT");

            migrationBuilder.DropTable(
                name: "FILINGDOC_ERROR_LIST");
        }
    }
}
