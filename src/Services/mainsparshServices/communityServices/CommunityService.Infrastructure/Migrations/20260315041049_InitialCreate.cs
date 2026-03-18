using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMMUNITY_MAST",
                columns: table => new
                {
                    COMMUNITY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COMMUNITY_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    COMMUNITY_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    COMMUNITY_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COMMUNITY_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    COMMUNITY_ICON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    COMMUNITY_BANNER = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PRIVACY_LEVEL = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "'PUBLIC'"),
                    OWNER_ID = table.Column<long>(type: "bigint", nullable: false),
                    APPROVER_ID = table.Column<long>(type: "bigint", nullable: true),
                    COMMUNITY_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "'ACTIVE'"),
                    MEMBER_COUNT = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMMUNITY_MAST", x => x.COMMUNITY_ID);
                });

            migrationBuilder.CreateTable(
                name: "COMMUNITY_MEMBERS",
                columns: table => new
                {
                    MEMBER_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COMMUNITY_ID = table.Column<long>(type: "bigint", nullable: false),
                    USER_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    MEMBER_ROLE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValueSql: "'MEMBER'"),
                    JOIN_DATE = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LEAVE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MEMBER_STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValueSql: "'ACTIVE'"),
                    CONTRIBUTION_COUNT = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    COMMUNITY_ID1 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMMUNITY_MEMBERS", x => x.MEMBER_ID);
                    table.ForeignKey(
                        name: "FK_COMMUNITY_MEMBERS_COMMUNITY_MAST_COMMUNITY_ID1",
                        column: x => x.COMMUNITY_ID1,
                        principalTable: "COMMUNITY_MAST",
                        principalColumn: "COMMUNITY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MAST_CODE",
                table: "COMMUNITY_MAST",
                column: "COMMUNITY_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MAST_OWNER",
                table: "COMMUNITY_MAST",
                column: "OWNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MAST_PRIVACY",
                table: "COMMUNITY_MAST",
                column: "PRIVACY_LEVEL");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MAST_STATUS",
                table: "COMMUNITY_MAST",
                column: "COMMUNITY_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MAST_TYPE",
                table: "COMMUNITY_MAST",
                column: "COMMUNITY_TYPE");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MEMBERS_COMMUNITY_ID",
                table: "COMMUNITY_MEMBERS",
                column: "COMMUNITY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MEMBERS_COMMUNITY_ID1",
                table: "COMMUNITY_MEMBERS",
                column: "COMMUNITY_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MEMBERS_ROLE",
                table: "COMMUNITY_MEMBERS",
                column: "MEMBER_ROLE");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MEMBERS_STATUS",
                table: "COMMUNITY_MEMBERS",
                column: "MEMBER_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_COMMUNITY_MEMBERS_USER_SYSID",
                table: "COMMUNITY_MEMBERS",
                column: "USER_SYSID");

            migrationBuilder.CreateIndex(
                name: "UC_COMMUNITY_USER",
                table: "COMMUNITY_MEMBERS",
                columns: new[] { "COMMUNITY_ID", "USER_SYSID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMMUNITY_MEMBERS");

            migrationBuilder.DropTable(
                name: "COMMUNITY_MAST");
        }
    }
}
