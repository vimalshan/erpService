using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOAN_ADDITIONAL",
                columns: table => new
                {
                    LOAN_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    ADDL_LOANNO = table.Column<long>(type: "bigint", nullable: false),
                    ADDL_LOANID = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_ADDITIONAL", x => new { x.LOAN_EMPSYSID, x.ADDL_LOANNO });
                });

            migrationBuilder.CreateTable(
                name: "LOAN_APPLICATION",
                columns: table => new
                {
                    LOAN_APPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOAN_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_APPLIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_APPLIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LOAN_SOURCE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LOAN_AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LOAN_SUBCLASSID = table.Column<long>(type: "bigint", nullable: true),
                    LOAN_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LOAN_APPSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    LOAN_GUARANTOR = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_GUARANTOR2 = table.Column<long>(type: "bigint", nullable: true),
                    LOAN_APRREMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LOAN_REQUIREDBY = table.Column<long>(type: "bigint", nullable: true),
                    LOAN_APPROVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    LOAN_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    LOAN_TENURE = table.Column<int>(type: "int", nullable: true),
                    LOAN_SPLSANCTION = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LOAN_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    LOAN_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_APPLICATION", x => x.LOAN_APPID);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_ADDITIONAL_EMPSYSID",
                table: "LOAN_ADDITIONAL",
                column: "LOAN_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_APPLICATION_EMPSYSID",
                table: "LOAN_APPLICATION",
                column: "LOAN_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IDX_LOAN_APPLICATION_STATUS",
                table: "LOAN_APPLICATION",
                column: "LOAN_APPSTATUS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOAN_ADDITIONAL");

            migrationBuilder.DropTable(
                name: "LOAN_APPLICATION");
        }
    }
}
