using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeTransactionsService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AA_EMP_PROBATION",
                columns: table => new
                {
                    PROB_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROB_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROB_DUEDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROB_DDREQUESTNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROB_FINSTATUS = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    PROB_REVIEWDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROB_NXTREVIEWDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    PROB_CONFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AA_EMP_PROBATION", x => x.PROB_ID);
                });

            migrationBuilder.CreateTable(
                name: "ALERTGRP_MASTER",
                columns: table => new
                {
                    ALGRP_ID = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                    ALGRP_NAME = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ALGRP_TYPE = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    ALGRP_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALGRP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALGRP_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ALGRP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTGRP_MASTER", x => x.ALGRP_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMP_GRADE",
                columns: table => new
                {
                    GRADE_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    GRADE_TRANID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    GRADE_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    GRADE_EFF_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GRADE_CLS_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GRADE_REMARKS = table.Column<string>(type: "varchar(65)", unicode: false, maxLength: 65, nullable: true),
                    GRADE_LIVFLAG = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    GRADE_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GRADE_UPDATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GRADE_PROBATION = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_GRADE", x => x.GRADE_EMP_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "EMP_GRADECHANGE",
                columns: table => new
                {
                    EMP_GRADECHANGEID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_OLDGRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_NEWGRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    EMP_STATUS = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    EMP_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EMP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    EMP_APPROVEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EMP_APPROVEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_GRADECHANGE", x => x.EMP_GRADECHANGEID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_MAIN",
                columns: table => new
                {
                    EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_PIN_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_APP_DATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    EMP_APP_UNIT = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    EMP_APP_GRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_APP_POSITION = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_APP_POSITIONDESC = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    EMP_FRS_NAME = table.Column<string>(type: "varchar(65)", unicode: false, maxLength: 65, nullable: false),
                    EMP_MID_NAME = table.Column<string>(type: "varchar(65)", unicode: false, maxLength: 65, nullable: true),
                    EMP_LST_NAME = table.Column<string>(type: "varchar(65)", unicode: false, maxLength: 65, nullable: true),
                    EMP_GENDER = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    EMP_DOB_RECORD = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    EMP_OFFERSTATUS = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    EMP_OEMAIL_ID = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    EMP_PEMAIL_ID = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    EMP_MOBILE_NO = table.Column<string>(type: "varchar(65)", unicode: false, maxLength: 65, nullable: true),
                    EMP_LEAD_ROLE = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: false),
                    EMP_PROBDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    EMP_PROB_FLAG = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    EMP_CONFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    EMP_APPUNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_CREATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    EMP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    EMP_UPDATED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EMP_UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_MAIN", x => x.EMP_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "STATIONERY_ITEM_IMAGE",
                columns: table => new
                {
                    IMAGE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ITEM_REFERENCE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    BLOB_NAME = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: false),
                    CONTENT_TYPE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    UPLOADED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    UPLOADED_ON_UTC = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATIONERY_ITEM_IMAGE", x => x.IMAGE_ID);
                });

            migrationBuilder.CreateTable(
                name: "ALERTGRP_EMPMAP",
                columns: table => new
                {
                    ALMAP_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALMAP_GRPID = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                    ALMAP_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALMAP_EMAILID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ALMAP_ORGID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALMAP_UNITID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALMAP_CALENDARID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ALMAP_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALMAP_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    ALMAP_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ALMAP_CREATEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    ALMAP_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ALMAP_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTGRP_EMPMAP", x => x.ALMAP_ID);
                    table.ForeignKey(
                        name: "FK_ALERTGRP_EMPMAP_ALERTGRP_MASTER_ALMAP_GRPID",
                        column: x => x.ALMAP_GRPID,
                        principalTable: "ALERTGRP_MASTER",
                        principalColumn: "ALGRP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERTGRP_EMPMAP_ALMAP_GRPID",
                table: "ALERTGRP_EMPMAP",
                column: "ALMAP_GRPID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AA_EMP_PROBATION");

            migrationBuilder.DropTable(
                name: "ALERTGRP_EMPMAP");

            migrationBuilder.DropTable(
                name: "EMP_GRADE");

            migrationBuilder.DropTable(
                name: "EMP_GRADECHANGE");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_MAIN");

            migrationBuilder.DropTable(
                name: "STATIONERY_ITEM_IMAGE");

            migrationBuilder.DropTable(
                name: "ALERTGRP_MASTER");
        }
    }
}
