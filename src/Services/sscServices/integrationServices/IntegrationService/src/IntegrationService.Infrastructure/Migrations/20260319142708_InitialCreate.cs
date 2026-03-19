using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORA_OU_BUMAP",
                columns: table => new
                {
                    OU_ID = table.Column<long>(type: "bigint", nullable: false),
                    OU_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ORA_OUMAST",
                columns: table => new
                {
                    OU_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    OU_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OU_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_OUMAST", x => x.OU_ID);
                });

            migrationBuilder.CreateTable(
                name: "ORA_POMAST",
                columns: table => new
                {
                    PO_SEQID = table.Column<long>(type: "bigint", nullable: false),
                    PO_OUID = table.Column<long>(type: "bigint", nullable: false),
                    PO_ID = table.Column<long>(type: "bigint", nullable: false),
                    PO_NO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PO_VENDORSITEID = table.Column<long>(type: "bigint", nullable: false),
                    PO_DUEDAYS = table.Column<long>(type: "bigint", nullable: false),
                    PO_DUE_DAY_MONTHOFF = table.Column<long>(type: "bigint", nullable: false),
                    PO_MONTHFORWARD = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_POMAST", x => x.PO_SEQID);
                });

            migrationBuilder.CreateTable(
                name: "ORA_VENDORMAST",
                columns: table => new
                {
                    VENDOR_ID = table.Column<int>(type: "int", nullable: false),
                    VENDOR_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VENDOR_CODE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_VENDORMAST", x => x.VENDOR_ID);
                });

            migrationBuilder.CreateTable(
                name: "ORA_MRCMAST",
                columns: table => new
                {
                    MRC_SEQID = table.Column<long>(type: "bigint", nullable: false),
                    MRC_POID = table.Column<long>(type: "bigint", nullable: false),
                    MRC_NO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    MRC_SEQNO = table.Column<long>(type: "bigint", nullable: true),
                    MRC_RECDATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    MRC_VENDID = table.Column<long>(type: "bigint", nullable: true),
                    MRC_VENSITEID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_MRCMAST", x => x.MRC_SEQID);
                    table.ForeignKey(
                        name: "FK_ORA_MRCMAST_ORA_POMAST_MRC_POID",
                        column: x => x.MRC_POID,
                        principalTable: "ORA_POMAST",
                        principalColumn: "PO_SEQID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORA_VENDORSITEMAST",
                columns: table => new
                {
                    VENDOR_SITEID = table.Column<long>(type: "bigint", nullable: false),
                    VENDOR_ID = table.Column<int>(type: "int", nullable: false),
                    VENDOR_SITECODE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VENDOR_OUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_VENDORSITEMAST", x => x.VENDOR_SITEID);
                    table.ForeignKey(
                        name: "FK_ORA_VENDORSITEMAST_ORA_VENDORMAST_VENDOR_ID",
                        column: x => x.VENDOR_ID,
                        principalTable: "ORA_VENDORMAST",
                        principalColumn: "VENDOR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ORA_VENDORSITEBUMAP",
                columns: table => new
                {
                    VENDOR_SITEID = table.Column<long>(type: "bigint", nullable: false),
                    VENDOR_BUID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORA_VENDORSITEBUMAP", x => x.VENDOR_SITEID);
                    table.ForeignKey(
                        name: "FK_ORA_VENDORSITEBUMAP_ORA_VENDORSITEMAST_VENDOR_SITEID",
                        column: x => x.VENDOR_SITEID,
                        principalTable: "ORA_VENDORSITEMAST",
                        principalColumn: "VENDOR_SITEID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ORA_MRCMAST_MRC_POID",
                table: "ORA_MRCMAST",
                column: "MRC_POID");

            migrationBuilder.CreateIndex(
                name: "IX_ORA_POMAST_PO_ID",
                table: "ORA_POMAST",
                column: "PO_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ORA_VENDORSITEMAST_VENDOR_ID",
                table: "ORA_VENDORSITEMAST",
                column: "VENDOR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORA_MRCMAST");

            migrationBuilder.DropTable(
                name: "ORA_OU_BUMAP");

            migrationBuilder.DropTable(
                name: "ORA_OUMAST");

            migrationBuilder.DropTable(
                name: "ORA_VENDORSITEBUMAP");

            migrationBuilder.DropTable(
                name: "ORA_POMAST");

            migrationBuilder.DropTable(
                name: "ORA_VENDORSITEMAST");

            migrationBuilder.DropTable(
                name: "ORA_VENDORMAST");
        }
    }
}
