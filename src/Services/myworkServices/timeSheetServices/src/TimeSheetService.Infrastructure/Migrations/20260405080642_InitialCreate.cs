using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheetService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TCPROJECT_MASTER",
                columns: table => new
                {
                    PROJECT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROJECT_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PROJECT_CATEGORYID = table.Column<long>(type: "bigint", nullable: false),
                    PROJECT_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJECT_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROJECT_TEAMID = table.Column<long>(type: "bigint", nullable: false),
                    PROJECT_LISTALL = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    PROJECT_OLDPROJID = table.Column<long>(type: "bigint", nullable: true),
                    PROJECT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJECT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCPROJECT_MASTER", x => x.PROJECT_ID);
                });

            migrationBuilder.CreateTable(
                name: "TCPROJECTCAT_MASTER",
                columns: table => new
                {
                    CATEGORY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CATEGORY_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CATEGORY_TEAMID = table.Column<long>(type: "bigint", nullable: false),
                    CATEGORY_OLDCATID = table.Column<long>(type: "bigint", nullable: true),
                    CATEGORY_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CATEGORY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCPROJECTCAT_MASTER", x => x.CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "TCSUBCAT_MASTER",
                columns: table => new
                {
                    SUBCAT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUBCAT_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    SUBCAT_PROJECTID = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_OLDSUBCATID = table.Column<long>(type: "bigint", nullable: true),
                    SUBCAT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCSUBCAT_MASTER", x => x.SUBCAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "TCTIMESHEET_MAIN",
                columns: table => new
                {
                    TIME_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIME_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TIME_IN = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    TIME_OUT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    TIME_HOURS = table.Column<long>(type: "bigint", nullable: false),
                    TIME_REMARKS = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    TIME_ENTRYTYPE = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    TIME_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TIME_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCTIMESHEET_MAIN", x => x.TIME_ID);
                });

            migrationBuilder.CreateTable(
                name: "TIMESHEET_MAIN",
                columns: table => new
                {
                    TIME_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIME_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TIME_IN = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    TIME_OUT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    TIME_HOURS = table.Column<long>(type: "bigint", nullable: false),
                    TIME_REMARKS = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    TIME_ENTRYTYPE = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    TIME_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TIME_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIMESHEET_MAIN", x => x.TIME_ID);
                });

            migrationBuilder.CreateTable(
                name: "TSACTIVITY_MASTER",
                columns: table => new
                {
                    ACTIVITY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACTIVITY_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ACTIVITY_ROLE = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ACTIVITY_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ACTIVITY_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSACTIVITY_MASTER", x => x.ACTIVITY_ID);
                });

            migrationBuilder.CreateTable(
                name: "TSPROJECT_MASTER",
                columns: table => new
                {
                    PROJECT_ID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    PROJECT_GROUP = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PROJECT_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PROJECT_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    PROJECT_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROJECT_TYPE = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    PROJECT_APPID = table.Column<int>(type: "int", nullable: false),
                    PROJECT_APPLYALL = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    PROJECT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROJECT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSPROJECT_MASTER", x => x.PROJECT_ID);
                });

            migrationBuilder.CreateTable(
                name: "TSTIMESHEET_DET",
                columns: table => new
                {
                    TIME_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIME_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    TIME_PROJECTID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_STAGEID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_ACTIVITYID = table.Column<long>(type: "bigint", nullable: false),
                    TIME_HOURS = table.Column<long>(type: "bigint", nullable: false),
                    TIME_REMARKS = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    TIME_MODULEID = table.Column<long>(type: "bigint", nullable: true),
                    TIME_REFID = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    TIME_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TIME_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TIME_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TIME_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSTIMESHEET_DET", x => x.TIME_ID);
                });

            migrationBuilder.CreateTable(
                name: "TCSUBCAT_EMPMAP",
                columns: table => new
                {
                    SUBCAT_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUBCAT_ID = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    SUBCAT_ENDDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SUBCAT_PLANNEDENDDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    SUBCAT_PLANNEDHRS = table.Column<int>(type: "int", nullable: false),
                    TcSubCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    SUBCAT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    SUBCAT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCSUBCAT_EMPMAP", x => x.SUBCAT_MAPID);
                    table.ForeignKey(
                        name: "FK_TCSUBCAT_EMPMAP_TCSUBCAT_MASTER_TcSubCategoryId",
                        column: x => x.TcSubCategoryId,
                        principalTable: "TCSUBCAT_MASTER",
                        principalColumn: "SUBCAT_ID");
                });

            migrationBuilder.CreateTable(
                name: "TCTIMESHEET_DET",
                columns: table => new
                {
                    TIMEDET_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIMEDET_TIMEID = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_HOURS = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_PROJECTID = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_SUBCATID = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_REMARKS = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    TIMEDET_CALLNO = table.Column<long>(type: "bigint", nullable: true),
                    TIMEDET_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCTIMESHEET_DET", x => x.TIMEDET_ID);
                    table.ForeignKey(
                        name: "FK_TCTIMESHEET_DET_MAIN",
                        column: x => x.TIMEDET_TIMEID,
                        principalTable: "TCTIMESHEET_MAIN",
                        principalColumn: "TIME_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TIMESHEET_DET",
                columns: table => new
                {
                    TIMEDET_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TIMEDET_TIMEID = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_HOURS = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_PROJECTID = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_SUBCATID = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_REMARKS = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    TIMEDET_CALLNO = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TIMEDET_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIMESHEET_DET", x => x.TIMEDET_ID);
                    table.ForeignKey(
                        name: "FK_TIMESHEET_DET_MAIN",
                        column: x => x.TIMEDET_TIMEID,
                        principalTable: "TIMESHEET_MAIN",
                        principalColumn: "TIME_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TSSTAGE_MASTER",
                columns: table => new
                {
                    STAGE_ID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    STAGE_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    STAGE_PROJECTID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TsProjectProjectCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    STAGE_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    STAGE_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSSTAGE_MASTER", x => x.STAGE_ID);
                    table.ForeignKey(
                        name: "FK_TSSTAGE_MASTER_TSPROJECT_MASTER_TsProjectProjectCode",
                        column: x => x.TsProjectProjectCode,
                        principalTable: "TSPROJECT_MASTER",
                        principalColumn: "PROJECT_ID");
                });

            migrationBuilder.CreateTable(
                name: "TSSTAGE_EMPMAP",
                columns: table => new
                {
                    STMAP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STMAP_STAGEID = table.Column<long>(type: "bigint", nullable: false),
                    STMAP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    STMAP_HOURS = table.Column<long>(type: "bigint", nullable: false),
                    STMAP_STARTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    STMAP_PLANNEDENDDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    STMAP_CLOSUREDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    TsStageStageCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    STMAP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    STMAP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TSSTAGE_EMPMAP", x => x.STMAP_ID);
                    table.ForeignKey(
                        name: "FK_TSSTAGE_EMPMAP_TSSTAGE_MASTER_TsStageStageCode",
                        column: x => x.TsStageStageCode,
                        principalTable: "TSSTAGE_MASTER",
                        principalColumn: "STAGE_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TCSUBCAT_EMPMAP_TcSubCategoryId",
                table: "TCSUBCAT_EMPMAP",
                column: "TcSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TCTIMESHEET_DET_TIMEDET_TIMEID",
                table: "TCTIMESHEET_DET",
                column: "TIMEDET_TIMEID");

            migrationBuilder.CreateIndex(
                name: "IX_TIMESHEET_DET_TIMEDET_TIMEID",
                table: "TIMESHEET_DET",
                column: "TIMEDET_TIMEID");

            migrationBuilder.CreateIndex(
                name: "IX_TSSTAGE_EMPMAP_TsStageStageCode",
                table: "TSSTAGE_EMPMAP",
                column: "TsStageStageCode");

            migrationBuilder.CreateIndex(
                name: "IX_TSSTAGE_MASTER_TsProjectProjectCode",
                table: "TSSTAGE_MASTER",
                column: "TsProjectProjectCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TCPROJECT_MASTER");

            migrationBuilder.DropTable(
                name: "TCPROJECTCAT_MASTER");

            migrationBuilder.DropTable(
                name: "TCSUBCAT_EMPMAP");

            migrationBuilder.DropTable(
                name: "TCTIMESHEET_DET");

            migrationBuilder.DropTable(
                name: "TIMESHEET_DET");

            migrationBuilder.DropTable(
                name: "TSACTIVITY_MASTER");

            migrationBuilder.DropTable(
                name: "TSSTAGE_EMPMAP");

            migrationBuilder.DropTable(
                name: "TSTIMESHEET_DET");

            migrationBuilder.DropTable(
                name: "TCSUBCAT_MASTER");

            migrationBuilder.DropTable(
                name: "TCTIMESHEET_MAIN");

            migrationBuilder.DropTable(
                name: "TIMESHEET_MAIN");

            migrationBuilder.DropTable(
                name: "TSSTAGE_MASTER");

            migrationBuilder.DropTable(
                name: "TSPROJECT_MASTER");
        }
    }
}
