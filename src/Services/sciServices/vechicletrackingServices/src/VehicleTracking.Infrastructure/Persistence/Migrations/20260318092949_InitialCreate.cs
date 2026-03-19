using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleTracking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DECISION_FLAG",
                columns: table => new
                {
                    DF_TRC_NUM = table.Column<long>(type: "bigint", nullable: false),
                    DF_PUR_COD = table.Column<long>(type: "bigint", nullable: false),
                    DF_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    DF_STG_DEC = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DF_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DF_REF_NUM = table.Column<long>(type: "bigint", nullable: true),
                    DF_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    DF_REMARK = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DECISION_FLAG", x => new { x.DF_TRC_NUM, x.DF_PUR_COD, x.DF_STG_COD });
                });

            migrationBuilder.CreateTable(
                name: "PURPOSE_MAST",
                columns: table => new
                {
                    PR_PRP_COD = table.Column<long>(type: "bigint", nullable: false),
                    PR_PRP_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PR_TRN_TYP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    PR_PRP_CAT = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PR_LST_STG = table.Column<long>(type: "bigint", nullable: true),
                    PR_PAR_PRP = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PURPOSE_MAST", x => x.PR_PRP_COD);
                });

            migrationBuilder.CreateTable(
                name: "SPARSH_NAVIGATION",
                columns: table => new
                {
                    SN_REQ_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SN_USR_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SN_USR_NUM = table.Column<long>(type: "bigint", nullable: false),
                    SN_RAN_NUM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SN_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    SN_SCI_ID = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SN_STS_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPARSH_NAVIGATION", x => x.SN_REQ_NUM);
                });

            migrationBuilder.CreateTable(
                name: "STAGE_DECISION",
                columns: table => new
                {
                    SD_PUR_COD = table.Column<long>(type: "bigint", nullable: false),
                    SD_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    SD_OPT_NAM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SD_OPT_ID = table.Column<long>(type: "bigint", nullable: true),
                    SD_STG_NEXT = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAGE_DECISION", x => new { x.SD_PUR_COD, x.SD_STG_COD, x.SD_OPT_NAM });
                });

            migrationBuilder.CreateTable(
                name: "STAGE_FLEX",
                columns: table => new
                {
                    PS_PRP_COD = table.Column<long>(type: "bigint", nullable: false),
                    PS_STG_SRL = table.Column<long>(type: "bigint", nullable: false),
                    PS_FLX_NUM = table.Column<long>(type: "bigint", nullable: false),
                    PS_FLX_DES = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PS_LOV_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PS_LOV_TYP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PS_FLX_TYP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAGE_FLEX", x => x.PS_PRP_COD);
                });

            migrationBuilder.CreateTable(
                name: "STAGE_MAST",
                columns: table => new
                {
                    ST_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    ST_OPT_NAM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ST_UPD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ST_UPD_NUM = table.Column<long>(type: "bigint", nullable: false),
                    ST_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STAGE_MAST", x => x.ST_STG_COD);
                });

            migrationBuilder.CreateTable(
                name: "VEHICLE_DIRECT_ENTRY",
                columns: table => new
                {
                    VDE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VDE_TRK_NUM = table.Column<long>(type: "bigint", nullable: false),
                    VDE_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    VDE_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICLE_DIRECT_ENTRY", x => x.VDE_ID);
                });

            migrationBuilder.CreateTable(
                name: "VEHICLE_INVOICE",
                columns: table => new
                {
                    IN_TRK_NUM = table.Column<long>(type: "bigint", nullable: false),
                    IN_REF_NUM = table.Column<long>(type: "bigint", nullable: false),
                    IN_INV_SRL = table.Column<long>(type: "bigint", nullable: false),
                    IN_ORC_INV = table.Column<long>(type: "bigint", nullable: true),
                    IN_CHN_INV = table.Column<long>(type: "bigint", nullable: false),
                    IN_CUS_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    IN_CAN_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IN_MOD_NUM = table.Column<long>(type: "bigint", nullable: false),
                    IN_MOD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    IN_MOD_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICLE_INVOICE", x => new { x.IN_TRK_NUM, x.IN_REF_NUM, x.IN_INV_SRL });
                });

            migrationBuilder.CreateTable(
                name: "VEHICLE_MAST",
                columns: table => new
                {
                    VH_SRL_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VH_REG_NUM1 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    VH_REG_NUM2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    VH_REG_NUM3 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    VH_REG_NUM4 = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    VH_REG_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    VH_LOG_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    VH_LOG_NUM = table.Column<long>(type: "bigint", nullable: true),
                    VH_LOG_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    VH_UPD_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    VH_UPD_NUM = table.Column<long>(type: "bigint", nullable: false),
                    VH_UPD_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICLE_MAST", x => x.VH_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "WEIGHT_INFO",
                columns: table => new
                {
                    WI_TRK_NUM = table.Column<long>(type: "bigint", nullable: false),
                    WI_TYR_WGT = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    WI_GRS_WGT = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    WI_NET_WGT = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WEIGHT_INFO", x => x.WI_TRK_NUM);
                });

            migrationBuilder.CreateTable(
                name: "PURPOSE_PRODUCT",
                columns: table => new
                {
                    PP_PRO_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PP_PUR_COD = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PURPOSE_PRODUCT", x => new { x.PP_PRO_COD, x.PP_PUR_COD });
                    table.ForeignKey(
                        name: "FK_PURPOSE_PRODUCT_PURPOSE_MAST_PP_PUR_COD",
                        column: x => x.PP_PUR_COD,
                        principalTable: "PURPOSE_MAST",
                        principalColumn: "PR_PRP_COD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PURPOSE_STAGE",
                columns: table => new
                {
                    PS_PRP_COD = table.Column<long>(type: "bigint", nullable: false),
                    PS_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    PS_STG_SRL = table.Column<long>(type: "bigint", nullable: false),
                    PS_FLX_FLD = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PS_PRL_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PS_ROL_COD = table.Column<long>(type: "bigint", nullable: false),
                    PS_BOL_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PS_BOL_DES = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PS_TRU_STG = table.Column<long>(type: "bigint", nullable: true),
                    PS_FAL_STG = table.Column<long>(type: "bigint", nullable: true),
                    PS_REM_MRK = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PS_LOW_LIMIT = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    PS_HIGH_LIMIT = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    PS_TARGET_TIME = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PURPOSE_STAGE", x => new { x.PS_PRP_COD, x.PS_STG_COD });
                    table.ForeignKey(
                        name: "FK_PURPOSE_STAGE_PURPOSE_MAST_PS_PRP_COD",
                        column: x => x.PS_PRP_COD,
                        principalTable: "PURPOSE_MAST",
                        principalColumn: "PR_PRP_COD",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PURPOSE_STAGE_STAGE_MAST_PS_STG_COD",
                        column: x => x.PS_STG_COD,
                        principalTable: "STAGE_MAST",
                        principalColumn: "ST_STG_COD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VEHICLE_STAGE",
                columns: table => new
                {
                    ST_TRN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    ST_TRK_NUM = table.Column<long>(type: "bigint", nullable: false),
                    ST_STG_SRL = table.Column<long>(type: "bigint", nullable: false),
                    ST_ENT_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    ST_ENT_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ST_ENT_NUM = table.Column<long>(type: "bigint", nullable: false),
                    ST_LEV_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    ST_ROL_COD = table.Column<long>(type: "bigint", nullable: false),
                    ST_DEC_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ST_CAN_STS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ST_TIM_TKN = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    VT_STG_COD = table.Column<long>(type: "bigint", nullable: false),
                    ST_STG_COM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VT_DEL_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: false),
                    VT_DEL_USR = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    VT_DEL_NUM = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICLE_STAGE", x => new { x.ST_TRN_NUM, x.ST_TRK_NUM, x.ST_STG_SRL });
                    table.ForeignKey(
                        name: "FK_VEHICLE_STAGE_STAGE_MAST_VT_STG_COD",
                        column: x => x.VT_STG_COD,
                        principalTable: "STAGE_MAST",
                        principalColumn: "ST_STG_COD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VEHICLE_TRAN",
                columns: table => new
                {
                    TR_TRK_NUM = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TR_VEH_SRL = table.Column<long>(type: "bigint", nullable: true),
                    TR_PTY_NAM = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TR_REP_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    TR_PRP_COD = table.Column<long>(type: "bigint", nullable: true),
                    TR_STG_PRV = table.Column<long>(type: "bigint", nullable: true),
                    TR_PRV_DAT = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    TR_STG_CUR = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    TR_GAT_NAM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TR_TRN_NUM = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TR_PRO_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TR_PRO_QTY = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    TR_STG_COM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TR_DRV_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TR_DRV_CELL = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    TR_TYR_WGT = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    TR_GRS_WGT = table.Column<decimal>(type: "decimal(38,0)", precision: 38, scale: 0, nullable: true),
                    TR_VEH_STS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    TR_LOG_ENT_USR = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_LOG_ENT_NUM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TR_LOG_ENT_DATE = table.Column<DateTime>(type: "datetime2(3)", precision: 3, nullable: true),
                    TR_MAIN_PURPOSE = table.Column<long>(type: "bigint", nullable: true),
                    TR_SUP_COD = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VEHICLE_TRAN", x => x.TR_TRK_NUM);
                    table.ForeignKey(
                        name: "FK_VEHICLE_TRAN_PURPOSE_MAST_TR_PRP_COD",
                        column: x => x.TR_PRP_COD,
                        principalTable: "PURPOSE_MAST",
                        principalColumn: "PR_PRP_COD");
                    table.ForeignKey(
                        name: "FK_VEHICLE_TRAN_VEHICLE_MAST_TR_VEH_SRL",
                        column: x => x.TR_VEH_SRL,
                        principalTable: "VEHICLE_MAST",
                        principalColumn: "VH_SRL_NUM");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PURPOSE_PRODUCT_PP_PUR_COD",
                table: "PURPOSE_PRODUCT",
                column: "PP_PUR_COD");

            migrationBuilder.CreateIndex(
                name: "IX_PURPOSE_STAGE_PS_STG_COD",
                table: "PURPOSE_STAGE",
                column: "PS_STG_COD");

            migrationBuilder.CreateIndex(
                name: "IX_VEHICLE_STAGE_VT_STG_COD",
                table: "VEHICLE_STAGE",
                column: "VT_STG_COD");

            migrationBuilder.CreateIndex(
                name: "IX_VEHICLE_TRAN_TR_PRP_COD",
                table: "VEHICLE_TRAN",
                column: "TR_PRP_COD");

            migrationBuilder.CreateIndex(
                name: "IX_VEHICLE_TRAN_TR_VEH_SRL",
                table: "VEHICLE_TRAN",
                column: "TR_VEH_SRL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DECISION_FLAG");

            migrationBuilder.DropTable(
                name: "PURPOSE_PRODUCT");

            migrationBuilder.DropTable(
                name: "PURPOSE_STAGE");

            migrationBuilder.DropTable(
                name: "SPARSH_NAVIGATION");

            migrationBuilder.DropTable(
                name: "STAGE_DECISION");

            migrationBuilder.DropTable(
                name: "STAGE_FLEX");

            migrationBuilder.DropTable(
                name: "VEHICLE_DIRECT_ENTRY");

            migrationBuilder.DropTable(
                name: "VEHICLE_INVOICE");

            migrationBuilder.DropTable(
                name: "VEHICLE_STAGE");

            migrationBuilder.DropTable(
                name: "VEHICLE_TRAN");

            migrationBuilder.DropTable(
                name: "WEIGHT_INFO");

            migrationBuilder.DropTable(
                name: "STAGE_MAST");

            migrationBuilder.DropTable(
                name: "PURPOSE_MAST");

            migrationBuilder.DropTable(
                name: "VEHICLE_MAST");
        }
    }
}
