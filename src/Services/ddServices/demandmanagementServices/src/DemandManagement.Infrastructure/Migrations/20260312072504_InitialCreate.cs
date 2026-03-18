using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemandManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DEMAND_MASTER",
                columns: table => new
                {
                    DEMAND_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEMAND_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DEPARTMENT_ID = table.Column<long>(type: "bigint", nullable: false),
                    DEMAND_DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    REQUIRED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PRIORITY = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DEMAND_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APPROVAL_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    APPROVED_BY = table.Column<long>(type: "bigint", nullable: true),
                    APPROVAL_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    COMPLETION_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    COMPLETED_BY = table.Column<long>(type: "bigint", nullable: true),
                    COMPLETION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEMAND_MASTER", x => x.DEMAND_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DEMAND_MASTER");
        }
    }
}
