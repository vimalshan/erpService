using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComplaintService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMPL_MAIN",
                columns: table => new
                {
                    CM_GROUPID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CM_UNIT_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    CM_GROUP_NAME = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CM_GROUP_DESC = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CM_GROUP_SRC = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CM_BEHALF_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    CM_BEHALF_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CM_REG_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CM_SHIFT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CM_MAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CM_SUBMIT = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CM_REG_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CM_UPDATEDBY = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CM_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPL_MAIN", x => x.CM_GROUPID);
                    table.UniqueConstraint("AK_COMPL_MAIN_CM_GROUP_SRC", x => x.CM_GROUP_SRC);
                });

            migrationBuilder.CreateTable(
                name: "COMPL_DET",
                columns: table => new
                {
                    CD_TICKET_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CD_GROUPID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CD_TYPE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CD_LOCATION = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CD_DEPARTMENT = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CD_PROCESS = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CD_SUBJECT = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CD_DESCRIPTION = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CD_NCR = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    CD_PICTUREPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CD_FILEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CD_TARGET_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CD_CLOSURE_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPL_DET", x => x.CD_TICKET_NUM);
                    table.ForeignKey(
                        name: "FK_COMPL_DET_COMPL_MAIN_CD_GROUPID",
                        column: x => x.CD_GROUPID,
                        principalTable: "COMPL_MAIN",
                        principalColumn: "CM_GROUP_SRC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPL_ACTION",
                columns: table => new
                {
                    CA_ACTION_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CA_TASK_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CA_PRM_RESP = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CA_PRM_ACTBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CA_PRM_ACTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CA_PRM_SOLUTION = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CA_SEC_ESCHRS = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CA_SEC_RESP = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CA_SEC_ACTBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CA_SEC_ACTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CA_SEC_SOLUTION = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CA_FWD_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CA_FWD_RESP = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CA_FWD_ACTBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CA_FWD_ACTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CA_FWD_SOLUTION = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CA_CUR_ESCLEVEL = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CA_CORR_ACTREQ = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    CA_CORR_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CA_CORR_RESP = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CA_CORR_ACTBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CA_CORR_ACTDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CA_CORR_SOLUTION = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CA_REOPEN_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    CA_REOPEN_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CA_TRG_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CA_CLS_DAT = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CA_UPATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPL_ACTION", x => x.CA_ACTION_NUM);
                    table.ForeignKey(
                        name: "FK_COMPL_ACTION_COMPL_DET_CA_TASK_NUM",
                        column: x => x.CA_TASK_NUM,
                        principalTable: "COMPL_DET",
                        principalColumn: "CD_TICKET_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPL_ESC",
                columns: table => new
                {
                    CE_TICKET_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CE_LEVEL_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CE_ESC_NOHRS = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CE_USER_PIN = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CE_EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CE_CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CE_EXCLUDE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    CE_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CE_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPL_ESC", x => new { x.CE_TICKET_NUM, x.CE_LEVEL_NUM });
                    table.ForeignKey(
                        name: "FK_COMPL_ESC_COMPL_DET_CE_TICKET_NUM",
                        column: x => x.CE_TICKET_NUM,
                        principalTable: "COMPL_DET",
                        principalColumn: "CD_TICKET_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPL_TASK",
                columns: table => new
                {
                    CT_TASK_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CT_TICKET_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CT_SCHEDULE_FREQ = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    CT_SCHEDULE_VALUE = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CT_SCHEDULE_TIME = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    CT_SCHEDULE_DAY = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    CT_EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CT_CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CT_UPDATED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CT_UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPL_TASK", x => x.CT_TASK_NUM);
                    table.ForeignKey(
                        name: "FK_COMPL_TASK_COMPL_DET_CT_TICKET_NUM",
                        column: x => x.CT_TICKET_NUM,
                        principalTable: "COMPL_DET",
                        principalColumn: "CD_TICKET_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COMPL_HIST",
                columns: table => new
                {
                    CH_HISTORY_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CH_ACTION_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CH_SERIAL_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CH_FROM = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    CH_TO = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CH_ACTION_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    CH_ACTION_TYPE = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    CH_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CH_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CH_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    CH_FILEPATH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPL_HIST", x => x.CH_HISTORY_NUM);
                    table.ForeignKey(
                        name: "FK_COMPL_HIST_COMPL_ACTION_CH_ACTION_NUM",
                        column: x => x.CH_ACTION_NUM,
                        principalTable: "COMPL_ACTION",
                        principalColumn: "CA_ACTION_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_COMPL_ACTION_CA_TASK_NUM",
                table: "COMPL_ACTION",
                column: "CA_TASK_NUM",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_COMPL_DET_CD_GROUPID",
                table: "COMPL_DET",
                column: "CD_GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_COMPL_HIST_CH_ACTION_NUM",
                table: "COMPL_HIST",
                column: "CH_ACTION_NUM");

            migrationBuilder.CreateIndex(
                name: "IX_COMPL_TASK_CT_TICKET_NUM",
                table: "COMPL_TASK",
                column: "CT_TICKET_NUM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMPL_ESC");

            migrationBuilder.DropTable(
                name: "COMPL_HIST");

            migrationBuilder.DropTable(
                name: "COMPL_TASK");

            migrationBuilder.DropTable(
                name: "COMPL_ACTION");

            migrationBuilder.DropTable(
                name: "COMPL_DET");

            migrationBuilder.DropTable(
                name: "COMPL_MAIN");
        }
    }
}
