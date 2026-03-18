using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOAN_DOCUMENTS",
                columns: table => new
                {
                    LOANDOC_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOANDOC_LOANID = table.Column<long>(type: "bigint", nullable: false),
                    LOANDOC_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    LOANDOC_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOANDOC_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_DOCUMENTS", x => x.LOANDOC_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_DOCUMENTS_LOANID",
                table: "LOAN_DOCUMENTS",
                column: "LOANDOC_LOANID");

            migrationBuilder.CreateIndex(
                name: "IX_LOAN_DOCUMENTS_TYPEID",
                table: "LOAN_DOCUMENTS",
                column: "LOANDOC_TYPEID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOAN_DOCUMENTS");
        }
    }
}
