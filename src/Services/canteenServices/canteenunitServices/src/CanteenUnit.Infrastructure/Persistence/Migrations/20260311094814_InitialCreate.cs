using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CanteenUnit.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CANTEEN_MASTER",
                columns: table => new
                {
                    CN_COM_COD = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    CN_CAN_NUM = table.Column<long>(type: "bigint", nullable: false),
                    CN_CAN_FRO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CN_CAN_TO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CN_LIV_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CN_ENT_USR = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    CN_ENT_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CN_REM_MRK = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_MASTER", x => x.CN_COM_COD);
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_MASTER_CAT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CN_COM_COD = table.Column<long>(type: "bigint", nullable: true),
                    CN_CAN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    CN_GRD_TYP = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_MASTER_CAT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_MASTER_GRADECAT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CN_CAN_SEQ = table.Column<long>(type: "bigint", nullable: true),
                    CN_COM_COD = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    CN_CAN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    CN_CAN_FRO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CN_CAN_TO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CN_LIV_FLG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CN_GRD_CAT = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_MASTER_GRADECAT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_UNIT_ACCESS",
                columns: table => new
                {
                    UN_UNT_ACC = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UN_COM_COD = table.Column<long>(type: "bigint", nullable: true),
                    UN_USR_ID = table.Column<long>(type: "bigint", nullable: true),
                    UN_ENT_USR = table.Column<long>(type: "bigint", nullable: true),
                    UN_ENT_ON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UN_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_UNIT_ACCESS", x => x.UN_UNT_ACC);
                });

            migrationBuilder.CreateTable(
                name: "CANTEEN_UNIT_MASTER",
                columns: table => new
                {
                    UN_COM_COD = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                    UN_UNT_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UNT_UNT_REF = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UN_MAX_VAL = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    IN_MIN_VAL = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                    UN_SIT_ID = table.Column<long>(type: "bigint", nullable: true),
                    UN_HRMS_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CANTEEN_UNIT_MASTER", x => x.UN_COM_COD);
                });

            migrationBuilder.CreateTable(
                name: "GEN_COUNTER",
                columns: table => new
                {
                    GN_TRN_TYP = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    GN_TRN_NUM = table.Column<long>(type: "bigint", nullable: true),
                    GN_TRN_DES = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GEN_COUNTER", x => x.GN_TRN_TYP);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CANTEEN_MASTER_CN_CAN_NUM",
                table: "CANTEEN_MASTER",
                column: "CN_CAN_NUM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CANTEEN_MASTER");

            migrationBuilder.DropTable(
                name: "CANTEEN_MASTER_CAT");

            migrationBuilder.DropTable(
                name: "CANTEEN_MASTER_GRADECAT");

            migrationBuilder.DropTable(
                name: "CANTEEN_UNIT_ACCESS");

            migrationBuilder.DropTable(
                name: "CANTEEN_UNIT_MASTER");

            migrationBuilder.DropTable(
                name: "GEN_COUNTER");
        }
    }
}
