using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APPROVAL_WORKFLOW",
                columns: table => new
                {
                    WORKFLOW_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WORKFLOW_CODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ENTITY_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ENTITY_ID = table.Column<long>(type: "bigint", nullable: false),
                    EMPLOYEE_ID = table.Column<long>(type: "bigint", nullable: false),
                    WORKFLOW_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "SUBMITTED"),
                    CURRENT_APPROVAL_LEVEL = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CURRENT_APPROVER_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAX_APPROVAL_LEVELS = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_WORKFLOW", x => x.WORKFLOW_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRANSACTION_LOG",
                columns: table => new
                {
                    LOG_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRANSACTION_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TRANSACTION_ID = table.Column<long>(type: "bigint", nullable: false),
                    ACTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ACTION_BY = table.Column<long>(type: "bigint", nullable: false),
                    ACTION_DATA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PREVIOUS_STATUS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NEW_STATUS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IP_ADDRESS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACTION_LOG", x => x.LOG_ID);
                });

            migrationBuilder.CreateTable(
                name: "APPROVAL_STEP",
                columns: table => new
                {
                    STEP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WORKFLOW_ID = table.Column<long>(type: "bigint", nullable: false),
                    STEP_LEVEL = table.Column<int>(type: "int", nullable: false),
                    APPROVER_ID = table.Column<long>(type: "bigint", nullable: false),
                    STEP_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "PENDING"),
                    STEP_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ACTED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_STEP", x => x.STEP_ID);
                    table.ForeignKey(
                        name: "FK_APPROVAL_STEP_WORKFLOW",
                        column: x => x.WORKFLOW_ID,
                        principalTable: "APPROVAL_WORKFLOW",
                        principalColumn: "WORKFLOW_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_STEP_APPROVER",
                table: "APPROVAL_STEP",
                column: "APPROVER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_STEP_STATUS",
                table: "APPROVAL_STEP",
                column: "STEP_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_STEP_WORKFLOW",
                table: "APPROVAL_STEP",
                column: "WORKFLOW_ID");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_WORKFLOW_APPROVER",
                table: "APPROVAL_WORKFLOW",
                column: "CURRENT_APPROVER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_WORKFLOW_EMPLOYEE",
                table: "APPROVAL_WORKFLOW",
                column: "EMPLOYEE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_WORKFLOW_ENTITY",
                table: "APPROVAL_WORKFLOW",
                columns: new[] { "ENTITY_TYPE", "ENTITY_ID" });

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_WORKFLOW_ENTITY_TYPE",
                table: "APPROVAL_WORKFLOW",
                column: "ENTITY_TYPE");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_WORKFLOW_STATUS",
                table: "APPROVAL_WORKFLOW",
                column: "WORKFLOW_STATUS");

            migrationBuilder.CreateIndex(
                name: "UC_APPROVAL_WORKFLOW_CODE",
                table: "APPROVAL_WORKFLOW",
                column: "WORKFLOW_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOG_ACTION",
                table: "TRANSACTION_LOG",
                column: "ACTION");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOG_ACTION_BY",
                table: "TRANSACTION_LOG",
                column: "ACTION_BY");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOG_CREATED_ON",
                table: "TRANSACTION_LOG",
                column: "CREATED_ON");

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOG_ENTITY",
                table: "TRANSACTION_LOG",
                columns: new[] { "TRANSACTION_TYPE", "TRANSACTION_ID" });

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACTION_LOG_TYPE",
                table: "TRANSACTION_LOG",
                column: "TRANSACTION_TYPE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APPROVAL_STEP");

            migrationBuilder.DropTable(
                name: "TRANSACTION_LOG");

            migrationBuilder.DropTable(
                name: "APPROVAL_WORKFLOW");
        }
    }
}
