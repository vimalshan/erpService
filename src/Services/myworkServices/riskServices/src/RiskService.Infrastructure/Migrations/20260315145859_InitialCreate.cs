using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RISK_FREQUENCYMAP",
                columns: table => new
                {
                    FREQ_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FREQ_RATINGID = table.Column<long>(type: "bigint", nullable: false),
                    FREQ_MONITORCODE = table.Column<string>(type: "char(3)", nullable: false),
                    FREQ_CODE = table.Column<string>(type: "char(1)", nullable: false),
                    FREQ_MONTH = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    FREQ_DAY = table.Column<int>(type: "int", nullable: false),
                    FREQ_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    FREQ_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FREQ_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    FREQ_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_FREQUENCYMAP", x => x.FREQ_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISK_FUNCTIONMAST",
                columns: table => new
                {
                    FUNCTION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FUNCTION_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FUNCTION_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    FUNCTION_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FUNCTION_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    FUNCTION_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_FUNCTIONMAST", x => x.FUNCTION_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISK_SELFASSDET",
                columns: table => new
                {
                    ASS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ASS_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    ASS_TYPEREFID = table.Column<long>(type: "bigint", nullable: false),
                    ASS_MONBY = table.Column<string>(type: "char(3)", nullable: false),
                    ASS_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ASS_MEETINGFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    ASS_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    ASS_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ASS_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ASS_REVIEWFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    ASS_NEWFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    ASS_NEWLIST = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ASS_MITFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    ASS_MITLIST = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ASS_APPSTATUS = table.Column<string>(type: "char(1)", nullable: false),
                    ASS_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ASS_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_SELFASSDET", x => x.ASS_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISKDIVISION_MASTER",
                columns: table => new
                {
                    RISKDIVISION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RISKDIVISION_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RISKDIVISION_HRMSBUSID = table.Column<long>(type: "bigint", nullable: false),
                    RISKDIVISION_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RISKDIVISION_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RISKDIVISION_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    RISKDIVISION_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKDIVISION_MASTER", x => x.RISKDIVISION_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISKIMPACT_MASTER",
                columns: table => new
                {
                    IMPACT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMPACT_RANK = table.Column<long>(type: "bigint", nullable: false),
                    IMPACT_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IMPACT_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    IMPACT_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IMPACT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    IMPACT_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKIMPACT_MASTER", x => x.IMPACT_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISKPROB_MASTER",
                columns: table => new
                {
                    PROB_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROB_RANK = table.Column<long>(type: "bigint", nullable: false),
                    PROB_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PROB_OCC = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PROB_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    PROB_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROB_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    PROB_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKPROB_MASTER", x => x.PROB_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISKRATING_MASTER",
                columns: table => new
                {
                    RATING_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RATING_RANK = table.Column<long>(type: "bigint", nullable: false),
                    RATING_FROM = table.Column<long>(type: "bigint", nullable: false),
                    RATING_TO = table.Column<long>(type: "bigint", nullable: false),
                    RATING_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RATING_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RATING_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RATING_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    RATING_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKRATING_MASTER", x => x.RATING_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISKRESP_MASTER",
                columns: table => new
                {
                    RESP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RESP_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RESP_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RESP_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RESP_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    RESP_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKRESP_MASTER", x => x.RESP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISKTYPE_MASTER",
                columns: table => new
                {
                    TYPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TYPE_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TYPE_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    TYPE_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TYPE_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    TYPE_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKTYPE_MASTER", x => x.TYPE_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISKUNIT_CHAMPMAP",
                columns: table => new
                {
                    CHAMP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CHAMP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    CHAMP_TYPE = table.Column<string>(type: "char(1)", nullable: false),
                    CHAMP_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    CHAMP_BUSID = table.Column<long>(type: "bigint", nullable: false),
                    CHAMP_DIVISIONID = table.Column<long>(type: "bigint", nullable: false),
                    CHAMP_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    CHAMP_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CHAMP_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CHAMP_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    CHAMP_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKUNIT_CHAMPMAP", x => x.CHAMP_ID);
                });

            migrationBuilder.CreateTable(
                name: "RISK_EVENTASSDET",
                columns: table => new
                {
                    EVENTASS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EVENTASS_ASSID = table.Column<long>(type: "bigint", nullable: false),
                    EVENTASS_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    EVENTASS_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EVENTASS_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_EVENTASSDET", x => x.EVENTASS_ID);
                    table.ForeignKey(
                        name: "FK_RISK_EVENTASSDET_RISK_SELFASSDET_EVENTASS_ASSID",
                        column: x => x.EVENTASS_ASSID,
                        principalTable: "RISK_SELFASSDET",
                        principalColumn: "ASS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_SELFASSCOMMENT",
                columns: table => new
                {
                    COM_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ASS_ID = table.Column<long>(type: "bigint", nullable: false),
                    RISKID = table.Column<long>(name: "RISK ID", type: "bigint", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    UpdatedOn = table.Column<long>(name: "Updated On", type: "bigint", nullable: false),
                    UpdatedBy = table.Column<DateTime>(name: "Updated By", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_SELFASSCOMMENT", x => x.COM_ID);
                    table.ForeignKey(
                        name: "FK_RISK_SELFASSCOMMENT_RISK_SELFASSDET_ASS_ID",
                        column: x => x.ASS_ID,
                        principalTable: "RISK_SELFASSDET",
                        principalColumn: "ASS_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_DIVISIONFUNCTIONMAP",
                columns: table => new
                {
                    DFM_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DFM_DIVISIONID = table.Column<long>(type: "bigint", nullable: false),
                    DFM_FUNCTIONID = table.Column<long>(type: "bigint", nullable: false),
                    DFM_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    DFM_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DFM_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    DFM_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_DIVISIONFUNCTIONMAP", x => x.DFM_MAPID);
                    table.ForeignKey(
                        name: "FK_RISK_DIVISIONFUNCTIONMAP_RISKDIVISION_MASTER_DFM_DIVISIONID",
                        column: x => x.DFM_DIVISIONID,
                        principalTable: "RISKDIVISION_MASTER",
                        principalColumn: "RISKDIVISION_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RISK_DIVISIONFUNCTIONMAP_RISK_FUNCTIONMAST_DFM_FUNCTIONID",
                        column: x => x.DFM_FUNCTIONID,
                        principalTable: "RISK_FUNCTIONMAST",
                        principalColumn: "FUNCTION_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISKDIVISIONUNIT_MAP",
                columns: table => new
                {
                    DIVUNIT_MAPID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DIVUNIT_DIVISIONID = table.Column<long>(type: "bigint", nullable: false),
                    DIVUNIT_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    DIVUNIT_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    DIVUNIT_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DIVUNIT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    DIVUNIT_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISKDIVISIONUNIT_MAP", x => x.DIVUNIT_MAPID);
                    table.ForeignKey(
                        name: "FK_RISKDIVISIONUNIT_MAP_RISKDIVISION_MASTER_DIVUNIT_DIVISIONID",
                        column: x => x.DIVUNIT_DIVISIONID,
                        principalTable: "RISKDIVISION_MASTER",
                        principalColumn: "RISKDIVISION_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_MASTER",
                columns: table => new
                {
                    RISK_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RISK_APPLICABLETO = table.Column<string>(type: "char(1)", nullable: false),
                    RISK_ORGID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_BUSID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_DIVISIONID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_FUNCTIONID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_EVENTTITLE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RISK_DESC = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    RISK_TYPEID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_IMPACTID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_PROBID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_RATEID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_RESIMPACTID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_RESPROBID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_RESRATEID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_RESPID = table.Column<long>(type: "bigint", nullable: false),
                    RISK_MITFLAG = table.Column<string>(type: "char(1)", nullable: false),
                    RISK_OWNER = table.Column<long>(type: "bigint", nullable: false),
                    RISK_APPSTATUS = table.Column<string>(type: "char(1)", nullable: false),
                    RISK_CANCELDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RISK_CANCELREASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RISK_ASSESSMENTID = table.Column<long>(type: "bigint", nullable: true),
                    RISK_REVIMPACTID = table.Column<long>(type: "bigint", nullable: true),
                    RISK_REVPROBID = table.Column<long>(type: "bigint", nullable: true),
                    RISK_REVRISKRATID = table.Column<long>(type: "bigint", nullable: true),
                    RISK_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RISK_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RISK_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    RISK_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_MASTER", x => x.RISK_ID);
                    table.ForeignKey(
                        name: "FK_RISK_MASTER_RISKIMPACT_MASTER_RISK_IMPACTID",
                        column: x => x.RISK_IMPACTID,
                        principalTable: "RISKIMPACT_MASTER",
                        principalColumn: "IMPACT_ID");
                    table.ForeignKey(
                        name: "FK_RISK_MASTER_RISKPROB_MASTER_RISK_PROBID",
                        column: x => x.RISK_PROBID,
                        principalTable: "RISKPROB_MASTER",
                        principalColumn: "PROB_ID");
                    table.ForeignKey(
                        name: "FK_RISK_MASTER_RISKRATING_MASTER_RISK_RATEID",
                        column: x => x.RISK_RATEID,
                        principalTable: "RISKRATING_MASTER",
                        principalColumn: "RATING_ID");
                    table.ForeignKey(
                        name: "FK_RISK_MASTER_RISKRESP_MASTER_RISK_RESPID",
                        column: x => x.RISK_RESPID,
                        principalTable: "RISKRESP_MASTER",
                        principalColumn: "RESP_ID");
                    table.ForeignKey(
                        name: "FK_RISK_MASTER_RISKTYPE_MASTER_RISK_TYPEID",
                        column: x => x.RISK_TYPEID,
                        principalTable: "RISKTYPE_MASTER",
                        principalColumn: "TYPE_ID");
                });

            migrationBuilder.CreateTable(
                name: "RISK_APPDET",
                columns: table => new
                {
                    APP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APP_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    APP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    APP_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    APP_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    APP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    APP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    APP_TYPE = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_APPDET", x => x.APP_ID);
                    table.ForeignKey(
                        name: "FK_RISK_APPDET_RISK_MASTER_APP_RISKID",
                        column: x => x.APP_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_CAUSES",
                columns: table => new
                {
                    ROOT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ROOT_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    ROOT_DESC = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ROOT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ROOT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_CAUSES", x => x.ROOT_ID);
                    table.ForeignKey(
                        name: "FK_RISK_CAUSES_RISK_MASTER_ROOT_RISKID",
                        column: x => x.ROOT_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_CONTROLS",
                columns: table => new
                {
                    CONTROL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CONTROL_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    CONTROL_DESC = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CONTROL_FILENAME = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CONTROL_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    CONTROL_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CONTROL_IMPACTREDPER = table.Column<long>(type: "bigint", nullable: true),
                    CONTROL_PROBREDPER = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_CONTROLS", x => x.CONTROL_ID);
                    table.ForeignKey(
                        name: "FK_RISK_CONTROLS_RISK_MASTER_CONTROL_RISKID",
                        column: x => x.CONTROL_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_EVENT",
                columns: table => new
                {
                    EVENT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EVENT_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    EVENT_DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EVENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EVENT_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    EVENT_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_EVENT", x => x.EVENT_ID);
                    table.ForeignKey(
                        name: "FK_RISK_EVENT_RISK_MASTER_EVENT_RISKID",
                        column: x => x.EVENT_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_FUNCTIONDET",
                columns: table => new
                {
                    FUNDET_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FUNDET_RiskID = table.Column<long>(type: "bigint", nullable: false),
                    FUNDET_FUNCTIONID = table.Column<long>(type: "bigint", nullable: false),
                    FUNDET_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    FUNDET_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_FUNCTIONDET", x => x.FUNDET_ID);
                    table.ForeignKey(
                        name: "FK_RISK_FUNCTIONDET_RISK_FUNCTIONMAST_FUNDET_FUNCTIONID",
                        column: x => x.FUNDET_FUNCTIONID,
                        principalTable: "RISK_FUNCTIONMAST",
                        principalColumn: "FUNCTION_ID");
                    table.ForeignKey(
                        name: "FK_RISK_FUNCTIONDET_RISK_MASTER_FUNDET_RiskID",
                        column: x => x.FUNDET_RiskID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_IMPACT",
                columns: table => new
                {
                    IMPMAP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMPMAP_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    IMPMAP_DESC = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IMPMAP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    IMPMAP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_IMPACT", x => x.IMPMAP_ID);
                    table.ForeignKey(
                        name: "FK_RISK_IMPACT_RISK_MASTER_IMPMAP_RISKID",
                        column: x => x.IMPMAP_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_MITIGATION",
                columns: table => new
                {
                    MIT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MIT_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    MIT_ACTION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    MIT_ORGDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MIT_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MIT_OWNER = table.Column<long>(type: "bigint", nullable: false),
                    MIT_REVIEWER = table.Column<long>(type: "bigint", nullable: false),
                    MIT_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    MIT_PROBRED = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MIT_IMPACTRED = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MIT_APPEMPSYSID = table.Column<long>(type: "bigint", nullable: true),
                    MIT_ATTACHMENT = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MIT_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    MIT_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MIT_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    MIT_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_MITIGATION", x => x.MIT_ID);
                    table.ForeignKey(
                        name: "FK_RISK_MITIGATION_RISK_MASTER_MIT_RISKID",
                        column: x => x.MIT_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_MONITOR",
                columns: table => new
                {
                    RISKMON_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RISKMON_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    RISKMON_BY = table.Column<string>(type: "char(3)", nullable: false),
                    RISKMON_REVFREQUENCY = table.Column<string>(type: "char(1)", nullable: false),
                    RISKMON_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    RISKMON_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_MONITOR", x => x.RISKMON_ID);
                    table.ForeignKey(
                        name: "FK_RISK_MONITOR_RISK_MASTER_RISKMON_RISKID",
                        column: x => x.RISKMON_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_UNITDET",
                columns: table => new
                {
                    HRUDET_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRUDET_RISKID = table.Column<long>(type: "bigint", nullable: false),
                    HRUDET_RISKUNITID = table.Column<long>(type: "bigint", nullable: false),
                    HRUDET_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    HRUDET_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_UNITDET", x => x.HRUDET_ID);
                    table.ForeignKey(
                        name: "FK_RISK_UNITDET_RISK_MASTER_HRUDET_RISKID",
                        column: x => x.HRUDET_RISKID,
                        principalTable: "RISK_MASTER",
                        principalColumn: "RISK_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_MITIGATIONACTION",
                columns: table => new
                {
                    ACTION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACTION_MITID = table.Column<long>(type: "bigint", nullable: false),
                    ACTION_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ACTION_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    ACTION_REVDUEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ACTION_APPSTATUS = table.Column<string>(type: "char(1)", nullable: false),
                    ACTION_COMMENTS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ACTION_COMPLETIONDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ACTION_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ACTION_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ACTION_MODIFIEDBY = table.Column<long>(type: "bigint", nullable: true),
                    ACTION_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_MITIGATIONACTION", x => x.ACTION_ID);
                    table.ForeignKey(
                        name: "FK_RISK_MITIGATIONACTION_RISK_MITIGATION_ACTION_MITID",
                        column: x => x.ACTION_MITID,
                        principalTable: "RISK_MITIGATION",
                        principalColumn: "MIT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RISK_MITAPPDET",
                columns: table => new
                {
                    APP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APP_ACTIONID = table.Column<long>(type: "bigint", nullable: false),
                    APP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    APP_STATUS = table.Column<string>(type: "char(1)", nullable: false),
                    APP_REMARKS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    APP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    APP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RISK_MITAPPDET", x => x.APP_ID);
                    table.ForeignKey(
                        name: "FK_RISK_MITAPPDET_RISK_MITIGATIONACTION_APP_ACTIONID",
                        column: x => x.APP_ACTIONID,
                        principalTable: "RISK_MITIGATIONACTION",
                        principalColumn: "ACTION_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RISK_APPDET_APP_RISKID",
                table: "RISK_APPDET",
                column: "APP_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_CAUSES_ROOT_RISKID",
                table: "RISK_CAUSES",
                column: "ROOT_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_CONTROLS_CONTROL_RISKID",
                table: "RISK_CONTROLS",
                column: "CONTROL_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_DIVISIONFUNCTIONMAP_DFM_DIVISIONID",
                table: "RISK_DIVISIONFUNCTIONMAP",
                column: "DFM_DIVISIONID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_DIVISIONFUNCTIONMAP_DFM_FUNCTIONID",
                table: "RISK_DIVISIONFUNCTIONMAP",
                column: "DFM_FUNCTIONID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_EVENT_EVENT_RISKID",
                table: "RISK_EVENT",
                column: "EVENT_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_EVENTASSDET_EVENTASS_ASSID",
                table: "RISK_EVENTASSDET",
                column: "EVENTASS_ASSID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_FUNCTIONDET_FUNDET_FUNCTIONID",
                table: "RISK_FUNCTIONDET",
                column: "FUNDET_FUNCTIONID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_FUNCTIONDET_FUNDET_RiskID",
                table: "RISK_FUNCTIONDET",
                column: "FUNDET_RiskID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_IMPACT_IMPMAP_RISKID",
                table: "RISK_IMPACT",
                column: "IMPMAP_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MASTER_RISK_IMPACTID",
                table: "RISK_MASTER",
                column: "RISK_IMPACTID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MASTER_RISK_PROBID",
                table: "RISK_MASTER",
                column: "RISK_PROBID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MASTER_RISK_RATEID",
                table: "RISK_MASTER",
                column: "RISK_RATEID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MASTER_RISK_RESPID",
                table: "RISK_MASTER",
                column: "RISK_RESPID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MASTER_RISK_TYPEID",
                table: "RISK_MASTER",
                column: "RISK_TYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MITAPPDET_APP_ACTIONID",
                table: "RISK_MITAPPDET",
                column: "APP_ACTIONID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MITIGATION_MIT_RISKID",
                table: "RISK_MITIGATION",
                column: "MIT_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MITIGATIONACTION_ACTION_MITID",
                table: "RISK_MITIGATIONACTION",
                column: "ACTION_MITID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_MONITOR_RISKMON_RISKID",
                table: "RISK_MONITOR",
                column: "RISKMON_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_SELFASSCOMMENT_ASS_ID",
                table: "RISK_SELFASSCOMMENT",
                column: "ASS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RISK_UNITDET_HRUDET_RISKID",
                table: "RISK_UNITDET",
                column: "HRUDET_RISKID");

            migrationBuilder.CreateIndex(
                name: "IX_RISKDIVISIONUNIT_MAP_DIVUNIT_DIVISIONID",
                table: "RISKDIVISIONUNIT_MAP",
                column: "DIVUNIT_DIVISIONID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RISK_APPDET");

            migrationBuilder.DropTable(
                name: "RISK_CAUSES");

            migrationBuilder.DropTable(
                name: "RISK_CONTROLS");

            migrationBuilder.DropTable(
                name: "RISK_DIVISIONFUNCTIONMAP");

            migrationBuilder.DropTable(
                name: "RISK_EVENT");

            migrationBuilder.DropTable(
                name: "RISK_EVENTASSDET");

            migrationBuilder.DropTable(
                name: "RISK_FREQUENCYMAP");

            migrationBuilder.DropTable(
                name: "RISK_FUNCTIONDET");

            migrationBuilder.DropTable(
                name: "RISK_IMPACT");

            migrationBuilder.DropTable(
                name: "RISK_MITAPPDET");

            migrationBuilder.DropTable(
                name: "RISK_MONITOR");

            migrationBuilder.DropTable(
                name: "RISK_SELFASSCOMMENT");

            migrationBuilder.DropTable(
                name: "RISK_UNITDET");

            migrationBuilder.DropTable(
                name: "RISKDIVISIONUNIT_MAP");

            migrationBuilder.DropTable(
                name: "RISKUNIT_CHAMPMAP");

            migrationBuilder.DropTable(
                name: "RISK_FUNCTIONMAST");

            migrationBuilder.DropTable(
                name: "RISK_MITIGATIONACTION");

            migrationBuilder.DropTable(
                name: "RISK_SELFASSDET");

            migrationBuilder.DropTable(
                name: "RISKDIVISION_MASTER");

            migrationBuilder.DropTable(
                name: "RISK_MITIGATION");

            migrationBuilder.DropTable(
                name: "RISK_MASTER");

            migrationBuilder.DropTable(
                name: "RISKIMPACT_MASTER");

            migrationBuilder.DropTable(
                name: "RISKPROB_MASTER");

            migrationBuilder.DropTable(
                name: "RISKRATING_MASTER");

            migrationBuilder.DropTable(
                name: "RISKRESP_MASTER");

            migrationBuilder.DropTable(
                name: "RISKTYPE_MASTER");
        }
    }
}
