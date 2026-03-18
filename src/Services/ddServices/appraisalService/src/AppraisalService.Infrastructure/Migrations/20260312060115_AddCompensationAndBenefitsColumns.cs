using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppraisalService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompensationAndBenefitsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AP_USR_COD",
                table: "DD_APPRAISERASSESS",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_TRG_DEV",
                table: "DD_APPRAISERASSESS",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_SLF_DEV",
                table: "DD_APPRAISERASSESS",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_ROLE",
                table: "DD_APPRAISERASSESS",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_REM_MRK",
                table: "DD_APPRAISERASSESS",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_JOB_DEV",
                table: "DD_APPRAISERASSESS",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_CAN_REM",
                table: "DD_APPRAISERASSESS",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_USR_ID",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_UOM",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_UNT_TO",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_UNT_FRM",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_PER_DES",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_GOL_FLG",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_EXPCOD",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_DIFF",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_CATEGORY",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_CAN_RMK",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_APS_STS",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_APP_RMK",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_ACH",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_VTC_RAT",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_SLT",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_MNM",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_LNM",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_FNM",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_DSG",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_PAYROLL",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_EMP_TYP",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_CEO_NAM",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_CEO_DSG",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_USR_COD",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "AP_SUBORDINATE",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_STS_COD",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_DD_TYPE",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_CAN_REM",
                table: "DD_APPRAISALMAIN",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AP_BASIC_NEW",
                table: "DD_APPRAISALMAIN",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AP_BASIC_OLD",
                table: "DD_APPRAISALMAIN",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AP_BENF_GRAT",
                table: "DD_APPRAISALMAIN",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AP_BENF_PF",
                table: "DD_APPRAISALMAIN",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AP_BENF_SUPER",
                table: "DD_APPRAISALMAIN",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AP_CTC_NEW",
                table: "DD_APPRAISALMAIN",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AP_CTC_OLD",
                table: "DD_APPRAISALMAIN",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AP_EFF_FROM",
                table: "DD_APPRAISALMAIN",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AP_INC_AMOUNT",
                table: "DD_APPRAISALMAIN",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AP_NEWFLEXIPAY",
                table: "DD_APPRAISALMAIN",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_SIG_NAM",
                table: "DD_APPRAISALBAND",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_SIG_DSG",
                table: "DD_APPRAISALBAND",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_FORMFLAG",
                table: "DD_APPRAISALBAND",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_BND_DSG",
                table: "DD_APPRAISALBAND",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_BND_DSC",
                table: "DD_APPRAISALBAND",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_BND_COD",
                table: "DD_APPRAISALBAND",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DD_APPRAISALDETAILS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DD_PIN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    DD_USR_DSG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DD_EMP_TYP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    DD_INC_AMT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DD_BLT_PERCENT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DD_PRMLEVEL = table.Column<long>(type: "bigint", nullable: true),
                    DD_NEWGRADE = table.Column<long>(type: "bigint", nullable: true),
                    DD_PRM_BND = table.Column<long>(type: "bigint", nullable: true),
                    DD_EMPGRADEID = table.Column<long>(type: "bigint", nullable: true),
                    DD_EMPLEVELID = table.Column<long>(type: "bigint", nullable: true),
                    DD_EMPUNITID = table.Column<long>(type: "bigint", nullable: true),
                    DD_YEARID = table.Column<long>(type: "bigint", nullable: true),
                    DD_INCTEMPLATEID = table.Column<long>(type: "bigint", nullable: true),
                    DD_RATETEMPLATEID = table.Column<long>(type: "bigint", nullable: true),
                    DD_LETFILE = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DD_EXPMONTHS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AppraisalMainRequestNumber = table.Column<long>(type: "bigint", nullable: false),
                    AppraisalMainEntityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_APPRAISALDETAILS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DD_APPRAISALDETAILS_DD_APPRAISALMAIN_AppraisalMainEntityId",
                        column: x => x.AppraisalMainEntityId,
                        principalTable: "DD_APPRAISALMAIN",
                        principalColumn: "AP_REQ_NUM");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DD_APPRAISALDETAILS_AppraisalMainEntityId",
                table: "DD_APPRAISALDETAILS",
                column: "AppraisalMainEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DD_APPRAISALDETAILS");

            migrationBuilder.DropColumn(
                name: "AP_BASIC_NEW",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_BASIC_OLD",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_BENF_GRAT",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_BENF_PF",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_BENF_SUPER",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_CTC_NEW",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_CTC_OLD",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_EFF_FROM",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_INC_AMOUNT",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.DropColumn(
                name: "AP_NEWFLEXIPAY",
                table: "DD_APPRAISALMAIN");

            migrationBuilder.AlterColumn<string>(
                name: "AP_USR_COD",
                table: "DD_APPRAISERASSESS",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_TRG_DEV",
                table: "DD_APPRAISERASSESS",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_SLF_DEV",
                table: "DD_APPRAISERASSESS",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_ROLE",
                table: "DD_APPRAISERASSESS",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_REM_MRK",
                table: "DD_APPRAISERASSESS",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_JOB_DEV",
                table: "DD_APPRAISERASSESS",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_CAN_REM",
                table: "DD_APPRAISERASSESS",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_USR_ID",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_UOM",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_UNT_TO",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_UNT_FRM",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_PER_DES",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_GOL_FLG",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_EXPCOD",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_DIFF",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_CATEGORY",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_CAN_RMK",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_APS_STS",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_APP_RMK",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AG_ACH",
                table: "DD_APPRAISEEGOAL_CUR",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_VTC_RAT",
                table: "DD_APPRAISALMAIN",
                type: "varchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_SLT",
                table: "DD_APPRAISALMAIN",
                type: "varchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_MNM",
                table: "DD_APPRAISALMAIN",
                type: "varchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_LNM",
                table: "DD_APPRAISALMAIN",
                type: "varchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_FNM",
                table: "DD_APPRAISALMAIN",
                type: "varchar(65)",
                maxLength: 65,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(65)",
                oldMaxLength: 65,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_USR_DSG",
                table: "DD_APPRAISALMAIN",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_PAYROLL",
                table: "DD_APPRAISALMAIN",
                type: "char(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_EMP_TYP",
                table: "DD_APPRAISALMAIN",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_CEO_NAM",
                table: "DD_APPRAISALMAIN",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_CEO_DSG",
                table: "DD_APPRAISALMAIN",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_USR_COD",
                table: "DD_APPRAISALMAIN",
                type: "varchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "AP_SUBORDINATE",
                table: "DD_APPRAISALMAIN",
                type: "char(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_STS_COD",
                table: "DD_APPRAISALMAIN",
                type: "varchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "AP_DD_TYPE",
                table: "DD_APPRAISALMAIN",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AP_CAN_REM",
                table: "DD_APPRAISALMAIN",
                type: "varchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_SIG_NAM",
                table: "DD_APPRAISALBAND",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_SIG_DSG",
                table: "DD_APPRAISALBAND",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_FORMFLAG",
                table: "DD_APPRAISALBAND",
                type: "char(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_BND_DSG",
                table: "DD_APPRAISALBAND",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_BND_DSC",
                table: "DD_APPRAISALBAND",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DD_BND_COD",
                table: "DD_APPRAISALBAND",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3,
                oldNullable: true);
        }
    }
}
