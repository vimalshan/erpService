using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StipendService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SRF_STIPEND_MASTER",
                columns: table => new
                {
                    STIPEND_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RESEARCH_CATEGORY_ID = table.Column<long>(type: "bigint", nullable: false),
                    SRF_RANK_ID = table.Column<long>(type: "bigint", nullable: false),
                    SRF_MONTHLY_STIPEND = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    ADDITIONAL_ALLOWANCE = table.Column<decimal>(type: "decimal(19,2)", nullable: true),
                    EFFECTIVE_FROM = table.Column<DateTime>(type: "date", nullable: false),
                    EFFECTIVE_TO = table.Column<DateTime>(type: "date", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRF_STIPEND_MASTER", x => x.STIPEND_ID);
                });

            migrationBuilder.CreateTable(
                name: "SRF_STIPEND_DISBURSEMENT",
                columns: table => new
                {
                    DISBURSEMENT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SRF_ID = table.Column<long>(type: "bigint", nullable: false),
                    STIPEND_ID = table.Column<long>(type: "bigint", nullable: false),
                    DISBURSEMENT_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    DISBURSEMENT_AMOUNT = table.Column<decimal>(type: "decimal(19,2)", nullable: false),
                    DISBURSEMENT_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "D"),
                    MONTH_YEAR = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    BANK_REFERENCE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    REFERENCE_NO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRF_STIPEND_DISBURSEMENT", x => x.DISBURSEMENT_ID);
                    table.ForeignKey(
                        name: "FK_SRF_STIPEND_DISBURSE_MASTER",
                        column: x => x.STIPEND_ID,
                        principalTable: "SRF_STIPEND_MASTER",
                        principalColumn: "STIPEND_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_DISBURSEMENT_DATE",
                table: "SRF_STIPEND_DISBURSEMENT",
                column: "DISBURSEMENT_DATE");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_DISBURSEMENT_MONTH",
                table: "SRF_STIPEND_DISBURSEMENT",
                column: "MONTH_YEAR");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_DISBURSEMENT_SRF_ID",
                table: "SRF_STIPEND_DISBURSEMENT",
                column: "SRF_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_DISBURSEMENT_STATUS",
                table: "SRF_STIPEND_DISBURSEMENT",
                column: "DISBURSEMENT_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_DISBURSEMENT_STIPEND_ID",
                table: "SRF_STIPEND_DISBURSEMENT",
                column: "STIPEND_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_MASTER_CATEGORY",
                table: "SRF_STIPEND_MASTER",
                column: "RESEARCH_CATEGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_MASTER_EFFECTIVE",
                table: "SRF_STIPEND_MASTER",
                columns: new[] { "EFFECTIVE_FROM", "EFFECTIVE_TO" });

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_MASTER_RANK",
                table: "SRF_STIPEND_MASTER",
                column: "SRF_RANK_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SRF_STIPEND_MASTER_STATUS",
                table: "SRF_STIPEND_MASTER",
                column: "STATUS");

            migrationBuilder.CreateIndex(
                name: "UC_STIPEND_CATEGORY_RANK",
                table: "SRF_STIPEND_MASTER",
                columns: new[] { "RESEARCH_CATEGORY_ID", "SRF_RANK_ID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SRF_STIPEND_DISBURSEMENT");

            migrationBuilder.DropTable(
                name: "SRF_STIPEND_MASTER");
        }
    }
}
