using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PROJ_ACCESS",
                columns: table => new
                {
                    PROJACC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJACC_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    PROJACC_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    PROJACC_DEPID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJ_ACCESS", x => x.PROJACC_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJDEP_MAST",
                columns: table => new
                {
                    PROJDEP_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROJDEP_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROJDEP_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROJDEP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJDEP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJDEP_MAST", x => x.PROJDEP_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJECTCAT_MASTER",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CATEGORY_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CATEGORY_TEAMID = table.Column<long>(type: "bigint", nullable: false),
                    CATEGORY_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CATEGORY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECTCAT_MASTER", x => x.CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJFUNC_MAST",
                columns: table => new
                {
                    PROJFUNC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJFUNC_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROJFUNC_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJFUNC_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJFUNC_MAST", x => x.PROJFUNC_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJLOC_MAST",
                columns: table => new
                {
                    LOC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LOC_NAME = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LOC_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    LOC_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJLOC_MAST", x => x.LOC_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJPROC_MAST",
                columns: table => new
                {
                    PROC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROC_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROC_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROC_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJPROC_MAST", x => x.PROC_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJTYPE_CATEGORYMAST",
                columns: table => new
                {
                    PROJCAT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJCAT_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROJCAT_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJCAT_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJTYPE_CATEGORYMAST", x => x.PROJCAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJTYPE_FINYEARSEQ",
                columns: table => new
                {
                    PROJTYPE_ID = table.Column<long>(type: "bigint", nullable: false),
                    PROJTYPE_YEAR = table.Column<int>(type: "int", nullable: false),
                    PROJTYPE_SEQ = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJTYPE_FINYEARSEQ", x => x.PROJTYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_MASTER",
                columns: table => new
                {
                    PROJECT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJECT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PROJECT_CATEGORYID = table.Column<long>(type: "bigint", nullable: false),
                    PROJECT_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJECT_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROJECT_TEAMID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROJECT_LISTALL = table.Column<string>(type: "char(1)", nullable: false),
                    PROJECT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJECT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_MASTER", x => x.PROJECT_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_MASTER_PROJECTCAT_MASTER_PROJECT_CATEGORYID",
                        column: x => x.PROJECT_CATEGORYID,
                        principalTable: "PROJECTCAT_MASTER",
                        principalColumn: "CATEGORY_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJFUNCEMP_MAP",
                columns: table => new
                {
                    PROJFUNCEMP_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJFUNCEMP_MAPFUNCID = table.Column<long>(type: "bigint", nullable: false),
                    PROJFUNCEMP_MAPEMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    PROJFUNCEMP_LIVEFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    PROJFUNCEMP_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJFUNCEMP_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJFUNCEMP_MAP", x => x.PROJFUNCEMP_MAPID);
                    table.ForeignKey(
                        name: "FK_PROJFUNCEMP_MAP_PROJFUNC_MAST_PROJFUNCEMP_MAPFUNCID",
                        column: x => x.PROJFUNCEMP_MAPFUNCID,
                        principalTable: "PROJFUNC_MAST",
                        principalColumn: "PROJFUNC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJTYPE_MAST",
                columns: table => new
                {
                    PROJTYPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJTYPE_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROJTYPE_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROJTYPE_DEPID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROJTYPE_CATID = table.Column<long>(type: "bigint", nullable: false),
                    PROJTYPE_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROJTYPE_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJTYPE_MAST", x => x.PROJTYPE_ID);
                    table.ForeignKey(
                        name: "FK_PROJTYPE_MAST_PROJTYPE_CATEGORYMAST_PROJTYPE_CATID",
                        column: x => x.PROJTYPE_CATID,
                        principalTable: "PROJTYPE_CATEGORYMAST",
                        principalColumn: "PROJCAT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_EMPMAP",
                columns: table => new
                {
                    PROJEMP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJEMP_PROJECTID = table.Column<long>(type: "bigint", nullable: false),
                    PROJEMP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    PROJEMP_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJEMP_CLOSEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJEMP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJEMP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_EMPMAP", x => x.PROJEMP_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_EMPMAP_PROJECT_MASTER_PROJEMP_PROJECTID",
                        column: x => x.PROJEMP_PROJECTID,
                        principalTable: "PROJECT_MASTER",
                        principalColumn: "PROJECT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_MAIN",
                columns: table => new
                {
                    PROJ_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJ_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PROJ_CHARTERNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROJ_LEADERID = table.Column<long>(type: "bigint", nullable: false),
                    PROJ_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    PROJ_LOCID = table.Column<long>(type: "bigint", nullable: false),
                    PROJ_PROCESSID = table.Column<long>(type: "bigint", nullable: false),
                    PROJ_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJ_ENDDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJ_ESTENDDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJ_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    PROJ_REVNO = table.Column<int>(type: "int", nullable: false),
                    PROJ_VERNO = table.Column<int>(type: "int", nullable: false),
                    PROJ_OBJID = table.Column<long>(type: "bigint", nullable: true),
                    PROJ_OBJDESC = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_TARGETPROD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PROJ_TARGETPRODREM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_TARGETSPECFILE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_TARGETSPECREM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_TARGETYIELDFILE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_TARGETYIELDREM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_NOTES = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PROJ_ACTUALPROD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PROJ_ACTUALPRODREM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_ACTUALSPECFILE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_ACTUALSPECREM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_ACTUALYIELDFILE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_ACTUALYIELDREM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROJ_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROJ_APPEMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROJ_PLANFILE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJ_TARGETLBL1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROJ_TARGETLBL2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROJ_TARGETLBL3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PROJ_PPTXFILE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_MAIN", x => x.PROJ_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_MAIN_PROJLOC_MAST_PROJ_LOCID",
                        column: x => x.PROJ_LOCID,
                        principalTable: "PROJLOC_MAST",
                        principalColumn: "LOC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROJECT_MAIN_PROJPROC_MAST_PROJ_PROCESSID",
                        column: x => x.PROJ_PROCESSID,
                        principalTable: "PROJPROC_MAST",
                        principalColumn: "PROC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROJECT_MAIN_PROJTYPE_MAST_PROJ_TYPEID",
                        column: x => x.PROJ_TYPEID,
                        principalTable: "PROJTYPE_MAST",
                        principalColumn: "PROJTYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJTYPE_DELMAP",
                columns: table => new
                {
                    DEL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEL_PROJTYPEID = table.Column<long>(type: "bigint", nullable: false),
                    DEL_DESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DEL_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DEL_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJTYPE_DELMAP", x => x.DEL_ID);
                    table.ForeignKey(
                        name: "FK_PROJTYPE_DELMAP_PROJTYPE_MAST_DEL_PROJTYPEID",
                        column: x => x.DEL_PROJTYPEID,
                        principalTable: "PROJTYPE_MAST",
                        principalColumn: "PROJTYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJTYPE_OBJMAP",
                columns: table => new
                {
                    OBJ_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OBJ_PROJTYPEID = table.Column<long>(type: "bigint", nullable: false),
                    OBJ_DESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OBJ_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    OBJ_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJTYPE_OBJMAP", x => x.OBJ_ID);
                    table.ForeignKey(
                        name: "FK_PROJTYPE_OBJMAP_PROJTYPE_MAST_OBJ_PROJTYPEID",
                        column: x => x.OBJ_PROJTYPEID,
                        principalTable: "PROJTYPE_MAST",
                        principalColumn: "PROJTYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJTYPE_SCOPEMAP",
                columns: table => new
                {
                    SCOPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCOPE_PROJTYPEID = table.Column<long>(type: "bigint", nullable: false),
                    SCOPE_DESC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SCOPE_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SCOPE_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJTYPE_SCOPEMAP", x => x.SCOPE_ID);
                    table.ForeignKey(
                        name: "FK_PROJTYPE_SCOPEMAP_PROJTYPE_MAST_SCOPE_PROJTYPEID",
                        column: x => x.SCOPE_PROJTYPEID,
                        principalTable: "PROJTYPE_MAST",
                        principalColumn: "PROJTYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJTYPEFUNC_MAP",
                columns: table => new
                {
                    PROJTYPEFUNC_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJTYPEFUNC_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    PROJTYPEFUNC_FUNCID = table.Column<long>(type: "bigint", nullable: false),
                    PROJTYPEFUNC_ADDLNO = table.Column<long>(type: "bigint", nullable: false),
                    PROJTYPEFUNC_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJTYPEFUNC_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJTYPEFUNC_MAP", x => x.PROJTYPEFUNC_MAPID);
                    table.ForeignKey(
                        name: "FK_PROJTYPEFUNC_MAP_PROJFUNC_MAST_PROJTYPEFUNC_FUNCID",
                        column: x => x.PROJTYPEFUNC_FUNCID,
                        principalTable: "PROJFUNC_MAST",
                        principalColumn: "PROJFUNC_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROJTYPEFUNC_MAP_PROJTYPE_MAST_PROJTYPEFUNC_TYPEID",
                        column: x => x.PROJTYPEFUNC_TYPEID,
                        principalTable: "PROJTYPE_MAST",
                        principalColumn: "PROJTYPE_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_ADDLDEL",
                columns: table => new
                {
                    PROJADLDEL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJADLDEL_PROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJADLDEL_DESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_ADDLDEL", x => x.PROJADLDEL_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_ADDLDEL_PROJECT_MAIN_PROJADLDEL_PROJID",
                        column: x => x.PROJADLDEL_PROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_ADDLSCOPE",
                columns: table => new
                {
                    PROJADSCOPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJADSCOPE_PROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJADSCOPE_DESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_ADDLSCOPE", x => x.PROJADSCOPE_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_ADDLSCOPE_PROJECT_MAIN_PROJADSCOPE_PROJID",
                        column: x => x.PROJADSCOPE_PROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_APPRDETAILS",
                columns: table => new
                {
                    PROJ_APPRID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJ_APPRPROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJ_APPRTYPE = table.Column<string>(type: "char(1)", nullable: false),
                    PROJ_APPRSENTON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJ_APPEMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    PROJ_APPRAPPDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJ_APPRSTATUS = table.Column<string>(type: "char(1)", nullable: false),
                    PROJ_APPREMARKS = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PROJ_APPRDROPREMARKS = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_APPRDETAILS", x => x.PROJ_APPRID);
                    table.ForeignKey(
                        name: "FK_PROJECT_APPRDETAILS_PROJECT_MAIN_PROJ_APPRPROJID",
                        column: x => x.PROJ_APPRPROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_DEL",
                columns: table => new
                {
                    PROJDEL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJDEL_PROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJDEL_DELID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_DEL", x => x.PROJDEL_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_DEL_PROJECT_MAIN_PROJDEL_PROJID",
                        column: x => x.PROJDEL_PROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_HOLD",
                columns: table => new
                {
                    PROJHOLD_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJHOLD_PROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJHOLD_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    PROJHOLD_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJHOLD_REASON = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PROJHOLD_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJHOLD_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_HOLD", x => x.PROJHOLD_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_HOLD_PROJECT_MAIN_PROJHOLD_PROJID",
                        column: x => x.PROJHOLD_PROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_MEMBERS",
                columns: table => new
                {
                    PROJMEM_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJMEM_PROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJMEM_FUNCID = table.Column<long>(type: "bigint", nullable: false),
                    PROJMEM_EMPSYSID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_MEMBERS", x => x.PROJMEM_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_MEMBERS_PROJECT_MAIN_PROJMEM_PROJID",
                        column: x => x.PROJMEM_PROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROJECT_MEMBERS_PROJFUNC_MAST_PROJMEM_FUNCID",
                        column: x => x.PROJMEM_FUNCID,
                        principalTable: "PROJFUNC_MAST",
                        principalColumn: "PROJFUNC_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_SCOPE",
                columns: table => new
                {
                    PROJSCOPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJSCOPE_PROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJSCOPE_SCOPEID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_SCOPE", x => x.PROJSCOPE_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_SCOPE_PROJECT_MAIN_PROJSCOPE_PROJID",
                        column: x => x.PROJSCOPE_PROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT_STATUS",
                columns: table => new
                {
                    PROJSTATUS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJSTATUS_PROJID = table.Column<long>(type: "bigint", nullable: false),
                    PROJSTATUS_FILE = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PROJSTATUS_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJSTATUS_REM = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PROJSTATUS_REVNO = table.Column<long>(type: "bigint", nullable: false),
                    PROJSTATUS_VERNO = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT_STATUS", x => x.PROJSTATUS_ID);
                    table.ForeignKey(
                        name: "FK_PROJECT_STATUS_PROJECT_MAIN_PROJSTATUS_PROJID",
                        column: x => x.PROJSTATUS_PROJID,
                        principalTable: "PROJECT_MAIN",
                        principalColumn: "PROJ_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_ADDLDEL_PROJADLDEL_PROJID",
                table: "PROJECT_ADDLDEL",
                column: "PROJADLDEL_PROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_ADDLSCOPE_PROJADSCOPE_PROJID",
                table: "PROJECT_ADDLSCOPE",
                column: "PROJADSCOPE_PROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_APPRDETAILS_PROJ_APPRPROJID",
                table: "PROJECT_APPRDETAILS",
                column: "PROJ_APPRPROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_DEL_PROJDEL_PROJID",
                table: "PROJECT_DEL",
                column: "PROJDEL_PROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_EMPMAP_PROJEMP_PROJECTID",
                table: "PROJECT_EMPMAP",
                column: "PROJEMP_PROJECTID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_HOLD_PROJHOLD_PROJID",
                table: "PROJECT_HOLD",
                column: "PROJHOLD_PROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_MAIN_PROJ_LOCID",
                table: "PROJECT_MAIN",
                column: "PROJ_LOCID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_MAIN_PROJ_PROCESSID",
                table: "PROJECT_MAIN",
                column: "PROJ_PROCESSID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_MAIN_PROJ_TYPEID",
                table: "PROJECT_MAIN",
                column: "PROJ_TYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_MASTER_PROJECT_CATEGORYID",
                table: "PROJECT_MASTER",
                column: "PROJECT_CATEGORYID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_MEMBERS_PROJMEM_FUNCID",
                table: "PROJECT_MEMBERS",
                column: "PROJMEM_FUNCID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_MEMBERS_PROJMEM_PROJID",
                table: "PROJECT_MEMBERS",
                column: "PROJMEM_PROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_SCOPE_PROJSCOPE_PROJID",
                table: "PROJECT_SCOPE",
                column: "PROJSCOPE_PROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_STATUS_PROJSTATUS_PROJID",
                table: "PROJECT_STATUS",
                column: "PROJSTATUS_PROJID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJFUNCEMP_MAP_PROJFUNCEMP_MAPFUNCID",
                table: "PROJFUNCEMP_MAP",
                column: "PROJFUNCEMP_MAPFUNCID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJTYPE_DELMAP_DEL_PROJTYPEID",
                table: "PROJTYPE_DELMAP",
                column: "DEL_PROJTYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJTYPE_MAST_PROJTYPE_CATID",
                table: "PROJTYPE_MAST",
                column: "PROJTYPE_CATID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJTYPE_OBJMAP_OBJ_PROJTYPEID",
                table: "PROJTYPE_OBJMAP",
                column: "OBJ_PROJTYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJTYPE_SCOPEMAP_SCOPE_PROJTYPEID",
                table: "PROJTYPE_SCOPEMAP",
                column: "SCOPE_PROJTYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJTYPEFUNC_MAP_PROJTYPEFUNC_FUNCID",
                table: "PROJTYPEFUNC_MAP",
                column: "PROJTYPEFUNC_FUNCID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJTYPEFUNC_MAP_PROJTYPEFUNC_TYPEID",
                table: "PROJTYPEFUNC_MAP",
                column: "PROJTYPEFUNC_TYPEID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PROJ_ACCESS");

            migrationBuilder.DropTable(
                name: "PROJDEP_MAST");

            migrationBuilder.DropTable(
                name: "PROJECT_ADDLDEL");

            migrationBuilder.DropTable(
                name: "PROJECT_ADDLSCOPE");

            migrationBuilder.DropTable(
                name: "PROJECT_APPRDETAILS");

            migrationBuilder.DropTable(
                name: "PROJECT_DEL");

            migrationBuilder.DropTable(
                name: "PROJECT_EMPMAP");

            migrationBuilder.DropTable(
                name: "PROJECT_HOLD");

            migrationBuilder.DropTable(
                name: "PROJECT_MEMBERS");

            migrationBuilder.DropTable(
                name: "PROJECT_SCOPE");

            migrationBuilder.DropTable(
                name: "PROJECT_STATUS");

            migrationBuilder.DropTable(
                name: "PROJFUNCEMP_MAP");

            migrationBuilder.DropTable(
                name: "PROJTYPE_DELMAP");

            migrationBuilder.DropTable(
                name: "PROJTYPE_FINYEARSEQ");

            migrationBuilder.DropTable(
                name: "PROJTYPE_OBJMAP");

            migrationBuilder.DropTable(
                name: "PROJTYPE_SCOPEMAP");

            migrationBuilder.DropTable(
                name: "PROJTYPEFUNC_MAP");

            migrationBuilder.DropTable(
                name: "PROJECT_MASTER");

            migrationBuilder.DropTable(
                name: "PROJECT_MAIN");

            migrationBuilder.DropTable(
                name: "PROJFUNC_MAST");

            migrationBuilder.DropTable(
                name: "PROJECTCAT_MASTER");

            migrationBuilder.DropTable(
                name: "PROJLOC_MAST");

            migrationBuilder.DropTable(
                name: "PROJPROC_MAST");

            migrationBuilder.DropTable(
                name: "PROJTYPE_MAST");

            migrationBuilder.DropTable(
                name: "PROJTYPE_CATEGORYMAST");
        }
    }
}
