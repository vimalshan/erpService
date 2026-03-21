using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DA_BREAKUP",
                columns: table => new
                {
                    DA_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    DA_REQ_ID = table.Column<long>(type: "bigint", nullable: false),
                    DA_FRO_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DA_TO_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DA_TYP_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    DA_HRS = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_BREAKUP", x => x.DA_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "DA_RULE",
                columns: table => new
                {
                    RL_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    RL_BND_ID = table.Column<long>(type: "bigint", nullable: false),
                    RL_CTR_COD = table.Column<long>(type: "bigint", nullable: false),
                    RL_SLF_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    RL_CUR_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    RL_BUD_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    RL_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RL_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_RULE", x => x.RL_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "DA_SUMMARY",
                columns: table => new
                {
                    DA_REQID = table.Column<long>(type: "bigint", nullable: false),
                    DA_ADMHRS = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DA_ADMDYS = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DA_ADMRAT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DA_ADMAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DA_SLFHRS = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DA_SLFDYS = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DA_SLFRAT = table.Column<decimal>(type: "decimal(19,0)", nullable: false),
                    DA_SLFAMT = table.Column<decimal>(type: "decimal(19,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DA_SUMMARY", x => x.DA_REQID);
                });

            migrationBuilder.CreateTable(
                name: "EXP_SETTLEMENT",
                columns: table => new
                {
                    EXP_COD = table.Column<long>(type: "bigint", nullable: true),
                    EXP_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EXP_BUD = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXP_CMP = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXP_SLF = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXP_ANX = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    EXP_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EXP_REM1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EXP_SETTLEMENTRPT",
                columns: table => new
                {
                    EXP_COD = table.Column<long>(type: "bigint", nullable: true),
                    EXP_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EXP_BUD = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    EXP_CMP = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    EXP_SLF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    EXP_ANX = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    EXP_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    REQ_NUM = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RULE_DA",
                columns: table => new
                {
                    RL_COM_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_BND_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_LOC_GRP = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_TYP_COD = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    RL_ADM_SLF = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    RL_CUR_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_DA_TYP = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    RL_BUD_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    RL_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RL_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RULE_MODE",
                columns: table => new
                {
                    RL_COM_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_BND_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_TYP_COD = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    RL_MOD_COD = table.Column<long>(type: "bigint", nullable: true),
                    RL_CLS_TYP = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RL_BUD_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RULE_STAY",
                columns: table => new
                {
                    RL_COM_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_BND_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RL_STY_TYP = table.Column<long>(type: "bigint", nullable: true),
                    RL_BUD_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    RL_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RL_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_CONVEYANCE",
                columns: table => new
                {
                    CONV_SRLNO = table.Column<long>(type: "bigint", nullable: false),
                    CONV_REQNO = table.Column<long>(type: "bigint", nullable: false),
                    CONV_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CONV_PARTICULARS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CONV_MODE = table.Column<long>(type: "bigint", nullable: true),
                    CONV_AMOUNT = table.Column<long>(type: "bigint", nullable: true),
                    CONV_BOOKNUM = table.Column<long>(type: "bigint", nullable: true),
                    CONV_BOOKSTS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_CONVEYANCE", x => new { x.CONV_SRLNO, x.CONV_REQNO });
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_CURRENCY",
                columns: table => new
                {
                    TC_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    TC_SRL_NO = table.Column<int>(type: "int", nullable: false),
                    TC_CUR_COD = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    TC_CSH_AMT = table.Column<long>(type: "bigint", nullable: true),
                    TC_TC_AMT = table.Column<long>(type: "bigint", nullable: true),
                    TC_DNM_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    TC_DNM_TXT = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_CURRENCY", x => new { x.TC_REQ_NUM, x.TC_SRL_NO });
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_EXPENSE",
                columns: table => new
                {
                    TR_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    TR_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    TR_EXP_COD = table.Column<long>(type: "bigint", nullable: true),
                    TR_CUR_TYP = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    TR_ELG_AMT = table.Column<long>(type: "bigint", nullable: true),
                    TR_BUD_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TR_ACT_UNT = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    TR_ACT_SLF = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TR_VAR_AMT = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    TR_EXP_REM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TR_TRN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    TR_EXP_ANX = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_EXPENSE", x => new { x.TR_REQ_NUM, x.TR_SRL_NUM });
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_EXPENSEALL",
                columns: table => new
                {
                    TR_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    TR_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    TR_EXP_SRL = table.Column<long>(type: "bigint", nullable: true),
                    TR_UNT_COD = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    TR_CST_COD = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    TR_ALL_TYP = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    TR_ALL_PER = table.Column<decimal>(type: "decimal(19,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_EXPENSEALL", x => new { x.TR_REQ_NUM, x.TR_SRL_NUM });
                    table.ForeignKey(
                        name: "FK_TRAVEL_EXPENSEALL_TRAVEL_EXPENSE_TR_REQ_NUM_TR_EXP_SRL",
                        columns: x => new { x.TR_REQ_NUM, x.TR_EXP_SRL },
                        principalTable: "TRAVEL_EXPENSE",
                        principalColumns: new[] { "TR_REQ_NUM", "TR_SRL_NUM" });
                });

            migrationBuilder.CreateTable(
                name: "TRAVEL_EXPENSESUB",
                columns: table => new
                {
                    TE_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    TE_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    TE_TYP_EXP = table.Column<long>(type: "bigint", nullable: true),
                    TE_BILL_ATT = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    TE_CIT_NAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TE_TOT_AMT = table.Column<long>(type: "bigint", nullable: true),
                    TE_STS_COD = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    TE_REM_TXT = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TE_BILL_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRAVEL_EXPENSESUB", x => new { x.TE_REQ_NUM, x.TE_SRL_NUM });
                    table.ForeignKey(
                        name: "FK_TRAVEL_EXPENSESUB_TRAVEL_EXPENSE_TE_REQ_NUM_TE_SRL_NUM",
                        columns: x => new { x.TE_REQ_NUM, x.TE_SRL_NUM },
                        principalTable: "TRAVEL_EXPENSE",
                        principalColumns: new[] { "TR_REQ_NUM", "TR_SRL_NUM" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TRAVEL_EXPENSEALL_TR_REQ_NUM_TR_EXP_SRL",
                table: "TRAVEL_EXPENSEALL",
                columns: new[] { "TR_REQ_NUM", "TR_EXP_SRL" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DA_BREAKUP");

            migrationBuilder.DropTable(
                name: "DA_RULE");

            migrationBuilder.DropTable(
                name: "DA_SUMMARY");

            migrationBuilder.DropTable(
                name: "EXP_SETTLEMENT");

            migrationBuilder.DropTable(
                name: "EXP_SETTLEMENTRPT");

            migrationBuilder.DropTable(
                name: "RULE_DA");

            migrationBuilder.DropTable(
                name: "RULE_MODE");

            migrationBuilder.DropTable(
                name: "RULE_STAY");

            migrationBuilder.DropTable(
                name: "TRAVEL_CONVEYANCE");

            migrationBuilder.DropTable(
                name: "TRAVEL_CURRENCY");

            migrationBuilder.DropTable(
                name: "TRAVEL_EXPENSEALL");

            migrationBuilder.DropTable(
                name: "TRAVEL_EXPENSESUB");

            migrationBuilder.DropTable(
                name: "TRAVEL_EXPENSE");
        }
    }
}
