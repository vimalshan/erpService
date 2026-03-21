using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LookupService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOV_TYPEMASTER",
                columns: table => new
                {
                    LOV_TYPECODE = table.Column<string>(type: "char(3)", nullable: false),
                    LOV_TYPENAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_TYPEMASTER", x => x.LOV_TYPECODE);
                });

            migrationBuilder.CreateTable(
                name: "PANEL_MAST",
                columns: table => new
                {
                    PANEL_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PANEL_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PANEL_MAST", x => x.PANEL_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROCESS_MASTER",
                columns: table => new
                {
                    PROCESS_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROCESS_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PROCESS_LIVFLAG = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROCESS_MASTER", x => x.PROCESS_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_MASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOV_TYPE = table.Column<string>(type: "char(3)", nullable: true),
                    LOV_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MASTER", x => x.LOV_ID);
                    table.ForeignKey(
                        name: "FK_LOV_MASTER_LOV_TYPEMASTER_LOV_TYPE",
                        column: x => x.LOV_TYPE,
                        principalTable: "LOV_TYPEMASTER",
                        principalColumn: "LOV_TYPECODE");
                });

            migrationBuilder.CreateTable(
                name: "UNIT_PROCESS_MAP",
                columns: table => new
                {
                    UP_MAPID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    UP_UNIT_CODE = table.Column<string>(type: "char(3)", nullable: true),
                    UP_PROCESS_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIT_PROCESS_MAP", x => x.UP_MAPID);
                    table.ForeignKey(
                        name: "FK_UNIT_PROCESS_MAP_PROCESS_MASTER_UP_PROCESS_ID",
                        column: x => x.UP_PROCESS_ID,
                        principalTable: "PROCESS_MASTER",
                        principalColumn: "PROCESS_ID");
                });

            migrationBuilder.CreateTable(
                name: "LOV_PANELMAP",
                columns: table => new
                {
                    LP_LOVID = table.Column<long>(type: "bigint", nullable: false),
                    LP_PANELID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LP_FLAG = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_PANELMAP", x => new { x.LP_LOVID, x.LP_PANELID });
                    table.ForeignKey(
                        name: "FK_LOV_PANELMAP_LOV_MASTER_LP_LOVID",
                        column: x => x.LP_LOVID,
                        principalTable: "LOV_MASTER",
                        principalColumn: "LOV_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOV_PANELMAP_PANEL_MAST_LP_PANELID",
                        column: x => x.LP_PANELID,
                        principalTable: "PANEL_MAST",
                        principalColumn: "PANEL_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOV_UNITMAP",
                columns: table => new
                {
                    LU_MAPID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LU_LOVID = table.Column<long>(type: "bigint", nullable: true),
                    LU_UNITCODE = table.Column<string>(type: "char(3)", nullable: true),
                    LU_FLAG = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_UNITMAP", x => x.LU_MAPID);
                    table.ForeignKey(
                        name: "FK_LOV_UNITMAP_LOV_MASTER_LU_LOVID",
                        column: x => x.LU_LOVID,
                        principalTable: "LOV_MASTER",
                        principalColumn: "LOV_ID");
                });

            migrationBuilder.CreateTable(
                name: "UNITLOV_ACCESSMAST",
                columns: table => new
                {
                    UA_ACCESSMASTID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    UA_UNITLOVMAPID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    UA_DEPARTMENTID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    UA_PROCESSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNITLOV_ACCESSMAST", x => x.UA_ACCESSMASTID);
                    table.ForeignKey(
                        name: "FK_UNITLOV_ACCESSMAST_LOV_UNITMAP_UA_UNITLOVMAPID",
                        column: x => x.UA_UNITLOVMAPID,
                        principalTable: "LOV_UNITMAP",
                        principalColumn: "LU_MAPID");
                    table.ForeignKey(
                        name: "FK_UNITLOV_ACCESSMAST_PROCESS_MASTER_UA_PROCESSID",
                        column: x => x.UA_PROCESSID,
                        principalTable: "PROCESS_MASTER",
                        principalColumn: "PROCESS_ID");
                });

            migrationBuilder.CreateTable(
                name: "UNITLOV_ACCESSDET",
                columns: table => new
                {
                    UD_ACCESSDETID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    UD_ACCESSMASTID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    UD_ACCESSTYPE = table.Column<string>(type: "char(2)", nullable: true),
                    UD_EMPSYSID = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UD_ESCDAYS = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    UD_EFF_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UD_CLS_DAT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UD_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    UD_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNITLOV_ACCESSDET", x => x.UD_ACCESSDETID);
                    table.ForeignKey(
                        name: "FK_UNITLOV_ACCESSDET_UNITLOV_ACCESSMAST_UD_ACCESSMASTID",
                        column: x => x.UD_ACCESSMASTID,
                        principalTable: "UNITLOV_ACCESSMAST",
                        principalColumn: "UA_ACCESSMASTID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LOV_MASTER_LOV_TYPE",
                table: "LOV_MASTER",
                column: "LOV_TYPE");

            migrationBuilder.CreateIndex(
                name: "IX_LOV_PANELMAP_LP_PANELID",
                table: "LOV_PANELMAP",
                column: "LP_PANELID");

            migrationBuilder.CreateIndex(
                name: "IX_LOV_UNITMAP_LU_LOVID",
                table: "LOV_UNITMAP",
                column: "LU_LOVID");

            migrationBuilder.CreateIndex(
                name: "IX_UNIT_PROCESS_MAP_UP_PROCESS_ID",
                table: "UNIT_PROCESS_MAP",
                column: "UP_PROCESS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_UNITLOV_ACCESSDET_UD_ACCESSMASTID",
                table: "UNITLOV_ACCESSDET",
                column: "UD_ACCESSMASTID");

            migrationBuilder.CreateIndex(
                name: "IX_UNITLOV_ACCESSMAST_UA_PROCESSID",
                table: "UNITLOV_ACCESSMAST",
                column: "UA_PROCESSID");

            migrationBuilder.CreateIndex(
                name: "IX_UNITLOV_ACCESSMAST_UA_UNITLOVMAPID",
                table: "UNITLOV_ACCESSMAST",
                column: "UA_UNITLOVMAPID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOV_PANELMAP");

            migrationBuilder.DropTable(
                name: "UNIT_PROCESS_MAP");

            migrationBuilder.DropTable(
                name: "UNITLOV_ACCESSDET");

            migrationBuilder.DropTable(
                name: "PANEL_MAST");

            migrationBuilder.DropTable(
                name: "UNITLOV_ACCESSMAST");

            migrationBuilder.DropTable(
                name: "LOV_UNITMAP");

            migrationBuilder.DropTable(
                name: "PROCESS_MASTER");

            migrationBuilder.DropTable(
                name: "LOV_MASTER");

            migrationBuilder.DropTable(
                name: "LOV_TYPEMASTER");
        }
    }
}
