using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparshTransactional.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SCHOLARSHIP_MASTER",
                columns: table => new
                {
                    SCHOLARSHIP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCHOLARSHIP_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SCHOLARSHIP_DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SCHOLARSHIP_TYPE = table.Column<string>(type: "char(1)", nullable: true),
                    SCHOLARSHIP_COVERAGE_PERCENT = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    SCHOLARSHIP_MAX_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    SCHOLARSHIP_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHOLARSHIP_MASTER", x => x.SCHOLARSHIP_ID);
                });

            migrationBuilder.CreateTable(
                name: "SCHOLARSHIP_APPLICATION",
                columns: table => new
                {
                    APPLICATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMP_STUDENT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SCHOLARSHIP_ID = table.Column<long>(type: "bigint", nullable: false),
                    APPLICATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FAMILY_INCOME = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    APPLICATION_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    APPROVED_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    APPROVED_BY = table.Column<long>(type: "bigint", nullable: true),
                    REJECTION_REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHOLARSHIP_APPLICATION", x => x.APPLICATION_ID);
                    table.ForeignKey(
                        name: "FK_SCHOLARSHIP_APPLICATION_SCHOLARSHIP_MASTER_SCHOLARSHIP_ID",
                        column: x => x.SCHOLARSHIP_ID,
                        principalTable: "SCHOLARSHIP_MASTER",
                        principalColumn: "SCHOLARSHIP_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SCHOLARSHIP_ELIGIBILITY_CRITERIA",
                columns: table => new
                {
                    CRITERIA_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCHOLARSHIP_ID = table.Column<long>(type: "bigint", nullable: false),
                    CRITERIA_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CRITERIA_DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MIN_SCORE = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MAX_FAMILY_INCOME = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    ELIGIBILITY_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHOLARSHIP_ELIGIBILITY_CRITERIA", x => x.CRITERIA_ID);
                    table.ForeignKey(
                        name: "FK_SCHOLARSHIP_ELIGIBILITY_CRITERIA_SCHOLARSHIP_MASTER_SCHOLARSHIP_ID",
                        column: x => x.SCHOLARSHIP_ID,
                        principalTable: "SCHOLARSHIP_MASTER",
                        principalColumn: "SCHOLARSHIP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SCHOLARSHIP_DISBURSEMENT",
                columns: table => new
                {
                    DISBURSEMENT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APPLICATION_ID = table.Column<long>(type: "bigint", nullable: false),
                    STUDENT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SCHOLARSHIP_ID = table.Column<long>(type: "bigint", nullable: false),
                    DISBURSEMENT_AMOUNT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DISBURSEMENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DISBURSEMENT_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    PAYMENT_REFERENCE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCHOLARSHIP_DISBURSEMENT", x => x.DISBURSEMENT_ID);
                    table.ForeignKey(
                        name: "FK_SCHOLARSHIP_DISBURSEMENT_SCHOLARSHIP_APPLICATION_APPLICATION_ID",
                        column: x => x.APPLICATION_ID,
                        principalTable: "SCHOLARSHIP_APPLICATION",
                        principalColumn: "APPLICATION_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APPLICATION_SCHOLARSHIP",
                table: "SCHOLARSHIP_APPLICATION",
                column: "SCHOLARSHIP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_APPLICATION_STATUS",
                table: "SCHOLARSHIP_APPLICATION",
                column: "APPLICATION_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_APPLICATION_STUDENT",
                table: "SCHOLARSHIP_APPLICATION",
                column: "EMP_STUDENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DISBURSEMENT_APPLICATION",
                table: "SCHOLARSHIP_DISBURSEMENT",
                column: "APPLICATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DISBURSEMENT_STATUS",
                table: "SCHOLARSHIP_DISBURSEMENT",
                column: "DISBURSEMENT_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_ELIGIBILITY_SCHOLARSHIP",
                table: "SCHOLARSHIP_ELIGIBILITY_CRITERIA",
                column: "SCHOLARSHIP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SCHOLARSHIP_MASTER_STATUS",
                table: "SCHOLARSHIP_MASTER",
                column: "SCHOLARSHIP_STATUS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SCHOLARSHIP_DISBURSEMENT");

            migrationBuilder.DropTable(
                name: "SCHOLARSHIP_ELIGIBILITY_CRITERIA");

            migrationBuilder.DropTable(
                name: "SCHOLARSHIP_APPLICATION");

            migrationBuilder.DropTable(
                name: "SCHOLARSHIP_MASTER");
        }
    }
}
