using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WORK_ORDER",
                columns: table => new
                {
                    WORK_ORDER_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WORK_ORDER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    WORK_ORDER_DESCRIPTION = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    DUE_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    ASSIGNED_TO = table.Column<long>(type: "bigint", nullable: false),
                    WORK_ORDER_STATUS = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORK_ORDER", x => x.WORK_ORDER_ID);
                });

            migrationBuilder.CreateTable(
                name: "WORK_TASK",
                columns: table => new
                {
                    TASK_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WORK_ORDER_ID = table.Column<long>(type: "bigint", nullable: false),
                    TASK_NAME = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ASSIGNED_TO = table.Column<long>(type: "bigint", nullable: false),
                    ESTIMATED_HOURS = table.Column<int>(type: "int", nullable: false),
                    ACTUAL_HOURS = table.Column<int>(type: "int", nullable: true),
                    TASK_STATUS = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    COMPLETION_REMARKS = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    COMPLETED_BY = table.Column<long>(type: "bigint", nullable: true),
                    COMPLETED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WORK_TASK", x => x.TASK_ID);
                    table.ForeignKey(
                        name: "FK_WORK_TASK_WORK_ORDER",
                        column: x => x.WORK_ORDER_ID,
                        principalTable: "WORK_ORDER",
                        principalColumn: "WORK_ORDER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_WORK_ORDER_STATUS",
                table: "WORK_ORDER",
                column: "WORK_ORDER_STATUS");

            migrationBuilder.CreateIndex(
                name: "IDX_WORK_TASK_STATUS",
                table: "WORK_TASK",
                column: "TASK_STATUS");

            migrationBuilder.CreateIndex(
                name: "IDX_WORK_TASK_WORK_ORDER_ID",
                table: "WORK_TASK",
                column: "WORK_ORDER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WORK_TASK");

            migrationBuilder.DropTable(
                name: "WORK_ORDER");
        }
    }
}
