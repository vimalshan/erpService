using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER_POLICY",
                columns: table => new
                {
                    POLICY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    POLICY_CODE = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    POLICY_TYPE = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    DATA_RETENTION_DAYS = table.Column<int>(type: "int", nullable: true),
                    SESSION_TIMEOUT_MINS = table.Column<int>(type: "int", nullable: true),
                    MAX_LOGIN_ATTEMPTS = table.Column<int>(type: "int", nullable: true),
                    POLICY_STATUS = table.Column<string>(type: "CHAR(1)", nullable: false, defaultValue: "A"),
                    EFFECTIVE_FROM = table.Column<DateOnly>(type: "date", nullable: false),
                    EFFECTIVE_TO = table.Column<DateOnly>(type: "date", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_POLICY", x => x.POLICY_ID);
                });

            migrationBuilder.CreateTable(
                name: "WEBSITE_CON_MAILID",
                columns: table => new
                {
                    CONTACT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    PRIMARY_EMAIL = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    SECONDARY_EMAIL = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    PHONE = table.Column<string>(type: "VARCHAR(20)", nullable: true),
                    MOBILE = table.Column<string>(type: "VARCHAR(20)", nullable: true),
                    WEBSITE = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    SOCIAL_MEDIA = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    NEWSLETTER_OPT_IN = table.Column<string>(type: "CHAR(1)", nullable: false, defaultValue: "Y"),
                    CONTACT_STATUS = table.Column<string>(type: "CHAR(1)", nullable: false, defaultValue: "A"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                    UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                    UPDATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WEBSITE_CON_MAILID", x => x.CONTACT_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_PROFILEHIST",
                columns: table => new
                {
                    HIST_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POLICY_ID = table.Column<long>(type: "bigint", nullable: false),
                    USER_SYSID = table.Column<long>(type: "bigint", nullable: false),
                    PROFILE_FIELD = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    OLD_VALUE = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    NEW_VALUE = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    CHAR_REASON = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    CHANGED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CHANGED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_PROFILEHIST", x => x.HIST_ID);
                    table.ForeignKey(
                        name: "FK_USER_PROFILEHIST_USER_POLICY_POLICY_ID",
                        column: x => x.POLICY_ID,
                        principalTable: "USER_POLICY",
                        principalColumn: "POLICY_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "USER_POLICY",
                columns: new[] { "POLICY_ID", "CREATED_BY", "CREATED_ON", "DATA_RETENTION_DAYS", "EFFECTIVE_FROM", "EFFECTIVE_TO", "MAX_LOGIN_ATTEMPTS", "POLICY_CODE", "POLICY_STATUS", "POLICY_TYPE", "SESSION_TIMEOUT_MINS", "UPDATED_BY", "UPDATED_ON", "USER_SYSID" },
                values: new object[,]
                {
                    { -4L, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 730, new DateOnly(2025, 1, 1), null, 3, "ACCESS_CONTROL_ADMIN", "A", "ACCESS_CONTROL", 15, null, null, 1004L },
                    { -3L, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateOnly(2025, 1, 1), null, null, "PREFERENCES_DARK", "A", "PREFERENCES", null, null, null, 1003L },
                    { -2L, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 180, new DateOnly(2025, 1, 1), null, 3, "NOTIFICATION_EMAIL", "A", "NOTIFICATION", 60, null, null, 1002L },
                    { -1L, 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 365, new DateOnly(2025, 1, 1), null, 5, "SECURITY_DEFAULT", "A", "SECURITY", 30, null, null, 1001L }
                });

            migrationBuilder.InsertData(
                table: "WEBSITE_CON_MAILID",
                columns: new[] { "CONTACT_ID", "CONTACT_STATUS", "CREATED_BY", "CREATED_ON", "MOBILE", "NEWSLETTER_OPT_IN", "PHONE", "PRIMARY_EMAIL", "SECONDARY_EMAIL", "SOCIAL_MEDIA", "UPDATED_BY", "UPDATED_ON", "USER_SYSID", "WEBSITE" },
                values: new object[,]
                {
                    { -3L, "A", 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "+91-9000000003", "Y", null, "user3@sparsh.local", null, "@user3_sparsh", null, null, 1003L, null },
                    { -2L, "A", 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "+91-9000000002", "N", "+91-22-12345678", "user2@sparsh.local", null, null, null, null, 1002L, null },
                    { -1L, "A", 1L, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "+91-9000000001", "Y", null, "admin@sparsh.local", "admin-backup@sparsh.local", null, null, null, 1001L, "https://sparsh.local" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_USER_POLICY_STATUS",
                table: "USER_POLICY",
                column: "POLICY_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_USER_POLICY_TYPE",
                table: "USER_POLICY",
                column: "POLICY_TYPE");

            migrationBuilder.CreateIndex(
                name: "UQ_USER_POLICY_USER_SYSID",
                table: "USER_POLICY",
                column: "USER_SYSID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_PROFILEHIST_DATE",
                table: "USER_PROFILEHIST",
                column: "CHANGED_ON");

            migrationBuilder.CreateIndex(
                name: "IX_USER_PROFILEHIST_POLICY_ID",
                table: "USER_PROFILEHIST",
                column: "POLICY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_PROFILEHIST_USER_SYSID",
                table: "USER_PROFILEHIST",
                column: "USER_SYSID");

            migrationBuilder.CreateIndex(
                name: "IX_WEBSITE_CON_MAILID_EMAIL",
                table: "WEBSITE_CON_MAILID",
                column: "PRIMARY_EMAIL");

            migrationBuilder.CreateIndex(
                name: "IX_WEBSITE_CON_MAILID_STATUS",
                table: "WEBSITE_CON_MAILID",
                column: "CONTACT_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_WEBSITE_CON_MAILID_USER_SYSID",
                table: "WEBSITE_CON_MAILID",
                column: "USER_SYSID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER_PROFILEHIST");

            migrationBuilder.DropTable(
                name: "WEBSITE_CON_MAILID");

            migrationBuilder.DropTable(
                name: "USER_POLICY");
        }
    }
}
