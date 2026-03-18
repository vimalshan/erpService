using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveServices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LEAVE_ENCASHMENT",
                columns: table => new
                {
                    ENCASHMENT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMP_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    LEAVE_TYPE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ENCASHMENT_DAYS = table.Column<int>(type: "int", nullable: false),
                    ENCASHMENT_AMOUNT = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    REQUEST_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    ENCASHMENT_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "P"),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MODIFIED_BY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LEAVE_ENCASHMENT", x => x.ENCASHMENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "LET_COUNTERS",
                columns: table => new
                {
                    LT_TYP_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LT_CNT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    LT_CNT_DES = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LET_COUNTERS", x => x.LT_TYP_COD);
                });

            migrationBuilder.CreateTable(
                name: "LET_MAIN",
                columns: table => new
                {
                    REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    FINYEAR_SRLNO = table.Column<int>(type: "int", nullable: false),
                    EMP_USERID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SUP_USERID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    REQ_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LET_MAIN", x => x.REQ_NUM);
                });

            migrationBuilder.CreateTable(
                name: "LET_MODEL",
                columns: table => new
                {
                    LT_SKL_COD = table.Column<long>(type: "bigint", nullable: false),
                    LT_LVL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    LT_FNC_COD = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    LT_JOB_COD = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LET_MODEL", x => new { x.LT_SKL_COD, x.LT_LVL_NUM });
                });

            migrationBuilder.CreateTable(
                name: "LET_SIGID",
                columns: table => new
                {
                    LET_SIGID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SIG_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SIG_DESG = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LOSS_OF_PAY",
                columns: table => new
                {
                    LOP_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMP_SYS_ID = table.Column<long>(type: "bigint", nullable: false),
                    LOP_DAYS = table.Column<int>(type: "int", nullable: false),
                    LOP_MONTH = table.Column<DateOnly>(type: "date", nullable: false),
                    LOP_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MODIFIED_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MODIFIED_BY = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOSS_OF_PAY", x => x.LOP_ID);
                });

            migrationBuilder.CreateTable(
                name: "LET_SUB",
                columns: table => new
                {
                    LS_SRL_NUM = table.Column<int>(type: "int", nullable: false),
                    LS_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    LS_MOD_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LS_MOD_USER = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LS_PREF_MODDEV = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    LS_ACT_TAKEN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_CRS_ID = table.Column<int>(type: "int", nullable: true),
                    LS_TRNPRG_BHR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_IMPBEN_PRO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_MEASURE_CP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_MIDYER_REVNAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_MIDYER_REVDAT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_MIDYER_REVREM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_ANNYER_REVNAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_ANNYER_REVDAT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_ANNYER_REVREM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LS_COMP_DEV = table.Column<int>(type: "int", nullable: true),
                    LS_DOMKNOW_DEV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LS_DOMKNOW_DEV_DET = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LS_PROCES_DEV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LS_PROCES_DEV_DET = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LS_LETSUB_CODE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    LS_REV_TYPE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LET_SUB", x => x.LS_SRL_NUM);
                    table.ForeignKey(
                        name: "FK_LET_SUB_LET_MAIN_LS_REQ_NUM",
                        column: x => x.LS_REQ_NUM,
                        principalTable: "LET_MAIN",
                        principalColumn: "REQ_NUM",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_LEAVE_ENCASHMENT_EMP_ID",
                table: "LEAVE_ENCASHMENT",
                column: "EMP_SYS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LET_SUB_LS_REQ_NUM",
                table: "LET_SUB",
                column: "LS_REQ_NUM");

            migrationBuilder.CreateIndex(
                name: "IDX_LOSS_OF_PAY_EMP_ID",
                table: "LOSS_OF_PAY",
                column: "EMP_SYS_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LEAVE_ENCASHMENT");

            migrationBuilder.DropTable(
                name: "LET_COUNTERS");

            migrationBuilder.DropTable(
                name: "LET_MODEL");

            migrationBuilder.DropTable(
                name: "LET_SIGID");

            migrationBuilder.DropTable(
                name: "LET_SUB");

            migrationBuilder.DropTable(
                name: "LOSS_OF_PAY");

            migrationBuilder.DropTable(
                name: "LET_MAIN");
        }
    }
}
