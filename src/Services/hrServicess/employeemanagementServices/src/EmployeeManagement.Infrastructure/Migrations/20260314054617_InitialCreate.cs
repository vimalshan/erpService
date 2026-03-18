using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMP_PROBATIONKK",
                columns: table => new
                {
                    PROBATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROBATION_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    PROBATION_UNITID = table.Column<long>(type: "bigint", nullable: false),
                    PROBATION_GRADE = table.Column<long>(type: "bigint", nullable: false),
                    PROBATION_DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROBATION_PROBATIONSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PROBATION_EXTENDED = table.Column<bool>(type: "bit", nullable: false),
                    PROBATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROBATION_SALARYCHANGE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PROBATION_GRADECHANGE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PROBATION_RATING = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PROBATION_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROBATION_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROBATION_LASTMODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    PROBATION_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_PROBATIONKK", x => x.PROBATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMP_RETIRALS",
                columns: table => new
                {
                    RETIRAL_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RETIRAL_TRANID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    RETIRAL_PFAPPLICABLE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    RETIRAL_PFTRUST = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    RETIRAL_PFNO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    RETIRAL_GRATUITYAPP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    RETIRAL_ESIAPP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    RETIRAL_ESINO = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    RETIRAL_EFF_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RETIRAL_CLS_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RETIRAL_UPDATED_BY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    RETIRAL_UPDATED_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMP_RETIRALS", x => x.RETIRAL_EMP_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_ADDRESS",
                columns: table => new
                {
                    ADDRESS_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ADDRESS_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ADDRESS_1 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_2 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_3 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_4 = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_CITY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ADDRESS_CITYOTHERS = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    ADDRESS_PINCODE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ADDRESS_STATE = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    ADDRESS_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ADDRESS_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_ADDRESS", x => x.ADDRESS_EMP_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_CAREER",
                columns: table => new
                {
                    CAREER_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CAREER_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CAREER_BUSINESS = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    CAREER_UNIT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CAREER_FROM = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CAREER_TO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CAREER_EMPNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CAREER_GRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CAREER_GRADEOTH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CAREER_DESIGNATION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CAREER_DIVISION = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CAREER_DIVISIONOTH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CAREER_PROCESS = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CAREER_PROCESSOTH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CAREER_DEPARTMENT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CAREER_DEPARTMENTOTH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CAREER_REASON = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CAREER_MODIFIEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CAREER_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_CAREER", x => x.CAREER_ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_DIARY",
                columns: table => new
                {
                    DIARY_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DIARY_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DIARY_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DIARY_SUBTYPE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    DIARY_DATE = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    DIARY_REASON = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DIARY_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    DIARY_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_DIARY", x => new { x.DIARY_EMP_SYSID, x.DIARY_ID });
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_LANGUAGE",
                columns: table => new
                {
                    LANG_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LANG_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LANG_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    LANG_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    LANG_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_LANGUAGE", x => new { x.LANG_EMP_SYSID, x.LANG_ID });
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_MASTER",
                columns: table => new
                {
                    EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMP_NO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EMP_BUSINESS = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    EMP_UNIT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    EMP_GRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EMP_DESIGNATION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EMP_DIVISION = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EMP_DEPARTMENT = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EMP_POSITION = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EMP_ISACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    EMP_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMP_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    EMP_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EMP_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_MASTER", x => x.EMP_SYSID);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_PROMOTION",
                columns: table => new
                {
                    PROM_NO = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PROM_SOURCE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PROM_REQUESTNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_RECDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROM_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_OLDGRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_NEWGRADE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PROM_OLDPOSITION = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_NEWPOSITION = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_REASON = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_REMARKS = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    PROM_CNFDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PROM_REVISIONSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PROM_INCREMENTNO = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PROM_DESIGNATION = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    PROM_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PROM_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PROM_CREATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                    PROM_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PROM_UPDATEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_PROMOTION", x => x.PROM_NO);
                });

            migrationBuilder.CreateTable(
                name: "EMPLOYEE_QUALIFICATION",
                columns: table => new
                {
                    QUAL_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QUAL_EMP_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    QUAL_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    QUAL_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_YEARFRO = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    QUAL_YEARTO = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    QUAL_INST_CODE = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                    QUAL_INST_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_EDU_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    QUAL_SPE_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    QUAL_SPE_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_PERCENTAGE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    QUAL_DEGREE_CODE = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    QUAL_DEGREE_DESC = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    QUAL_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    QUAL_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPLOYEE_QUALIFICATION", x => x.QUAL_ID);
                });

            migrationBuilder.CreateTable(
                name: "TRANSFER_MAIN",
                columns: table => new
                {
                    TRANSFER_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TRANSFER_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    TRANSFER_OLDUNIT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TRANSFER_NEWUNIT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TRANSFER_OLDUNITID = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                    TRANSFER_NEWUNITID = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                    TRANSFER_REASON = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    TRANSFER_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TRANSFER_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TRANSFER_TYPE = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TRANSFER_STATUS = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TRANSFER_PAYFLAG = table.Column<bool>(type: "bit", nullable: false),
                    TRANSFER_EXPATSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    TRANSFER_CREATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    TRANSFER_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TRANSFER_UPDATEDBY = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    TRANSFER_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSFER_MAIN", x => x.TRANSFER_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMP_PROBATIONKK");

            migrationBuilder.DropTable(
                name: "EMP_RETIRALS");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_ADDRESS");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_CAREER");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_DIARY");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_LANGUAGE");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_MASTER");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_PROMOTION");

            migrationBuilder.DropTable(
                name: "EMPLOYEE_QUALIFICATION");

            migrationBuilder.DropTable(
                name: "TRANSFER_MAIN");
        }
    }
}
