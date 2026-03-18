using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailNotification.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMAIL_TYPEMAST",
                columns: table => new
                {
                    EMAIL_TYPEID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMAIL_NAME = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EMAIL_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    EMAIL_PRCNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    EMAIL_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMAIL_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMAIL_TYPEMAST", x => x.EMAIL_TYPEID);
                });

            migrationBuilder.CreateTable(
                name: "MAIL_ACCESS",
                columns: table => new
                {
                    MAIL_ACCESSID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAIL_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    MAIL_ORGID = table.Column<long>(type: "bigint", nullable: true),
                    MAIL_BUSINESSID = table.Column<long>(type: "bigint", nullable: true),
                    MAIL_EMPSYSID = table.Column<long>(type: "bigint", nullable: true),
                    MAIL_EMAILID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MAIL_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailTypeAggregateId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MAIL_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MAIL_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAIL_ACCESS", x => x.MAIL_ACCESSID);
                    table.ForeignKey(
                        name: "FK_MAIL_ACCESS_EMAIL_TYPEMAST_EmailTypeAggregateId",
                        column: x => x.EmailTypeAggregateId,
                        principalTable: "EMAIL_TYPEMAST",
                        principalColumn: "EMAIL_TYPEID");
                    table.ForeignKey(
                        name: "FK_MAIL_ACCESS_EMAIL_TYPEMAST_MAIL_TYPEID",
                        column: x => x.MAIL_TYPEID,
                        principalTable: "EMAIL_TYPEMAST",
                        principalColumn: "EMAIL_TYPEID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EMAIL_TYPEMAST_TYPE",
                table: "EMAIL_TYPEMAST",
                column: "EMAIL_TYPE");

            migrationBuilder.CreateIndex(
                name: "IX_MAIL_ACCESS_EMAILID",
                table: "MAIL_ACCESS",
                column: "MAIL_EMAILID");

            migrationBuilder.CreateIndex(
                name: "IX_MAIL_ACCESS_EmailTypeAggregateId",
                table: "MAIL_ACCESS",
                column: "EmailTypeAggregateId");

            migrationBuilder.CreateIndex(
                name: "IX_MAIL_ACCESS_EMPSYSID",
                table: "MAIL_ACCESS",
                column: "MAIL_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_MAIL_ACCESS_ORGID",
                table: "MAIL_ACCESS",
                column: "MAIL_ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_MAIL_ACCESS_TYPEID",
                table: "MAIL_ACCESS",
                column: "MAIL_TYPEID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MAIL_ACCESS");

            migrationBuilder.DropTable(
                name: "EMAIL_TYPEMAST");
        }
    }
}
