using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileExpenseManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MOBEXP_DET",
                columns: table => new
                {
                    MOBEXP_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.seq_MOBEXP_Id"),
                    MOBEXP_TPID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MOBEXP_CATID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MOBEXP_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MOBEXP_COMMENT = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MOBEXP_AMOUNT = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    MOBEXP_CURRID = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MOBEXP_ENTEREDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MOBEXP_ENTEREDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MOBEXP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    MOBEXP_MODIFIEDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MOBEXP_DELETEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    MOBEXP_DELETEDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MOBEXP_ISDELETED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOBEXP_DET", x => x.MOBEXP_ID);
                });

            migrationBuilder.CreateTable(
                name: "MOBEXP_FILE",
                columns: table => new
                {
                    MOBEXPPHT_ID = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.seq_MOBEXP_File_Id"),
                    MOBEXPPHT_EXPID = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MOBEXPPHT_FILENAME = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MOBEXPPHT_FILEDATA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MOBEXPPHT_FILESIZE = table.Column<long>(type: "bigint", nullable: false),
                    MOBEXPPHT_CONTENTTYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MOBEXPPHT_UPLOADEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MOBEXPPHT_UPLOADEDBY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MOBEXPPHT_BLOBPATH = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MOBEXPPHT_ISDELETED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOBEXP_FILE", x => x.MOBEXPPHT_ID);
                    table.ForeignKey(
                        name: "FK_MOBEXP_FILE_MOBEXP_DET_MOBEXPPHT_EXPID",
                        column: x => x.MOBEXPPHT_EXPID,
                        principalTable: "MOBEXP_DET",
                        principalColumn: "MOBEXP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MOBEXP_CATID",
                table: "MOBEXP_DET",
                column: "MOBEXP_CATID");

            migrationBuilder.CreateIndex(
                name: "IX_MOBEXP_DATE",
                table: "MOBEXP_DET",
                column: "MOBEXP_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_MOBEXP_ENTEREDBY",
                table: "MOBEXP_DET",
                column: "MOBEXP_ENTEREDBY");

            migrationBuilder.CreateIndex(
                name: "IX_MOBEXP_ISDELETED",
                table: "MOBEXP_DET",
                column: "MOBEXP_ISDELETED");

            migrationBuilder.CreateIndex(
                name: "IX_MOBEXP_TPID",
                table: "MOBEXP_DET",
                column: "MOBEXP_TPID");

            migrationBuilder.CreateIndex(
                name: "IX_MOBEXP_FILE_EXPID",
                table: "MOBEXP_FILE",
                column: "MOBEXPPHT_EXPID");

            migrationBuilder.CreateIndex(
                name: "IX_MOBEXP_FILE_ISDELETED",
                table: "MOBEXP_FILE",
                column: "MOBEXPPHT_ISDELETED");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MOBEXP_FILE");

            migrationBuilder.DropTable(
                name: "MOBEXP_DET");
        }
    }
}
