using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprovalGroup.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APGROUP_MAST",
                columns: table => new
                {
                    GROUP_ID = table.Column<long>(type: "bigint", nullable: false),
                    GROUP_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GROUP_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GROUP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    GROUP_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    GROUP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GROUP_PRIORITYID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APGROUP_MAST", x => x.GROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PULLMATRIX_DET",
                columns: table => new
                {
                    MAT_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAT_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    MAT_PAYBY = table.Column<string>(type: "char(2)", nullable: false),
                    MAT_FLAG = table.Column<string>(type: "char(1)", nullable: false),
                    MAT_MAINCAT = table.Column<long>(type: "bigint", nullable: false),
                    MAT_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    MAT_MAXNOS = table.Column<long>(type: "bigint", nullable: false),
                    MAT_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MAT_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MAT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MAT_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PULLMATRIX_DET", x => x.MAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "APGROUP_MAP",
                columns: table => new
                {
                    MAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_GROUPID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_PAYBYSPECIFIC = table.Column<int>(type: "int", nullable: false),
                    MAP_BUSPECIFIC = table.Column<int>(type: "int", nullable: false),
                    MAP_MAINCAT = table.Column<long>(type: "bigint", nullable: false),
                    MAP_SUBCAT = table.Column<long>(type: "bigint", nullable: false),
                    MAP_CURRENCY = table.Column<string>(type: "char(1)", nullable: true),
                    MAP_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MAP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MAP_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    MAP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APGROUP_MAP", x => x.MAP_ID);
                    table.ForeignKey(
                        name: "FK_APGROUP_MAP_APGROUP_MAST_MAP_GROUPID",
                        column: x => x.MAP_GROUPID,
                        principalTable: "APGROUP_MAST",
                        principalColumn: "GROUP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "APGROUP_USERMAP",
                columns: table => new
                {
                    MAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_GROUPID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_USERID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_EFFECTIVEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MAP_CLOSUREDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    MAP_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MAP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    MAP_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    MAP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APGROUP_USERMAP", x => x.MAP_ID);
                    table.ForeignKey(
                        name: "FK_APGROUP_USERMAP_APGROUP_MAST_MAP_GROUPID",
                        column: x => x.MAP_GROUPID,
                        principalTable: "APGROUP_MAST",
                        principalColumn: "GROUP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "APGROUP_MAINCATMAP",
                columns: table => new
                {
                    MAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_GROUPMAPID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_MAINCAT = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APGROUP_MAINCATMAP", x => x.MAP_ID);
                    table.ForeignKey(
                        name: "FK_APGROUP_MAINCATMAP_APGROUP_MAP_MAP_GROUPMAPID",
                        column: x => x.MAP_GROUPMAPID,
                        principalTable: "APGROUP_MAP",
                        principalColumn: "MAP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "APGROUP_PAYBY",
                columns: table => new
                {
                    MAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_GROUPMAPID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_PAYBY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APGROUP_PAYBY", x => x.MAP_ID);
                    table.ForeignKey(
                        name: "FK_APGROUP_PAYBY_APGROUP_MAP_MAP_GROUPMAPID",
                        column: x => x.MAP_GROUPMAPID,
                        principalTable: "APGROUP_MAP",
                        principalColumn: "MAP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "APGROUP_UNITMAP",
                columns: table => new
                {
                    MAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_GROUMAPID = table.Column<long>(type: "bigint", nullable: false),
                    MAP_BUID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APGROUP_UNITMAP", x => x.MAP_ID);
                    table.ForeignKey(
                        name: "FK_APGROUP_UNITMAP_APGROUP_MAP_MAP_GROUMAPID",
                        column: x => x.MAP_GROUMAPID,
                        principalTable: "APGROUP_MAP",
                        principalColumn: "MAP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APGROUP_MAINCATMAP_MAP_GROUPMAPID",
                table: "APGROUP_MAINCATMAP",
                column: "MAP_GROUPMAPID");

            migrationBuilder.CreateIndex(
                name: "IX_APGROUP_MAP_MAP_GROUPID",
                table: "APGROUP_MAP",
                column: "MAP_GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_APGROUP_PAYBY_MAP_GROUPMAPID",
                table: "APGROUP_PAYBY",
                column: "MAP_GROUPMAPID");

            migrationBuilder.CreateIndex(
                name: "IX_APGROUP_UNITMAP_MAP_GROUMAPID",
                table: "APGROUP_UNITMAP",
                column: "MAP_GROUMAPID");

            migrationBuilder.CreateIndex(
                name: "IX_APGROUP_USERMAP_MAP_GROUPID",
                table: "APGROUP_USERMAP",
                column: "MAP_GROUPID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APGROUP_MAINCATMAP");

            migrationBuilder.DropTable(
                name: "APGROUP_PAYBY");

            migrationBuilder.DropTable(
                name: "APGROUP_UNITMAP");

            migrationBuilder.DropTable(
                name: "APGROUP_USERMAP");

            migrationBuilder.DropTable(
                name: "PULLMATRIX_DET");

            migrationBuilder.DropTable(
                name: "APGROUP_MAP");

            migrationBuilder.DropTable(
                name: "APGROUP_MAST");
        }
    }
}
