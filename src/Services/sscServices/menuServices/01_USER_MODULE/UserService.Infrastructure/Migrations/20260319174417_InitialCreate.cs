using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER_MAST",
                columns: table => new
                {
                    USER_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    USER_PASSWORD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    USER_EMAILID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USER_SPARSHUSERID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    USER_HREMPSYSID = table.Column<long>(type: "bigint", nullable: true),
                    USER_EFFECTIVE_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    USER_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    USER_ENTEREDBY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_MAST", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "USER_LOCATIONMAP",
                columns: table => new
                {
                    LOC_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOC_USERID = table.Column<long>(type: "bigint", nullable: false),
                    LOC_ID = table.Column<int>(type: "int", nullable: false),
                    LOC_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    LOC_CREATEDBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_LOCATIONMAP", x => x.LOC_MAPID);
                    table.ForeignKey(
                        name: "FK_USER_LOCATIONMAP_USER_MAST_LOC_USERID",
                        column: x => x.LOC_USERID,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_ORGMAP",
                columns: table => new
                {
                    ORG_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ORG_USERID = table.Column<long>(type: "bigint", nullable: false),
                    ORG_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ORG_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ORG_CREATEDBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ORGMAP", x => x.ORG_MAPID);
                    table.ForeignKey(
                        name: "FK_USER_ORGMAP_USER_MAST_ORG_USERID",
                        column: x => x.ORG_USERID,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLEMAP",
                columns: table => new
                {
                    ROLE_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROLE_USERID = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_ID = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_DEFFLAG = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ROLE_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ROLE_CREATEDBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLEMAP", x => x.ROLE_MAPID);
                    table.ForeignKey(
                        name: "FK_USER_ROLEMAP_USER_MAST_ROLE_USERID",
                        column: x => x.ROLE_USERID,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_USER_LOCATIONMAP_LOC_USERID",
                table: "USER_LOCATIONMAP",
                column: "LOC_USERID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ORGMAP_ORG_USERID",
                table: "USER_ORGMAP",
                column: "ORG_USERID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLEMAP_ROLE_USERID",
                table: "USER_ROLEMAP",
                column: "ROLE_USERID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER_LOCATIONMAP");

            migrationBuilder.DropTable(
                name: "USER_ORGMAP");

            migrationBuilder.DropTable(
                name: "USER_ROLEMAP");

            migrationBuilder.DropTable(
                name: "USER_MAST");
        }
    }
}
