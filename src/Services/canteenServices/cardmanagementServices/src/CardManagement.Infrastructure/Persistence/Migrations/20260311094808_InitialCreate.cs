using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CANTEEN_CARD_MAP",
                columns: table => new
                {
                    CC_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    CC_CAN_UNT = table.Column<long>(type: "bigint", nullable: false),
                    CC_CRD_NUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CC_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CC_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CC_UPD_USR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    CC_UPD_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CARD_SETTLEMENT",
                columns: table => new
                {
                    ST_SYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    ST_CAN_UNT = table.Column<long>(type: "bigint", nullable: false),
                    ST_CRD_NUM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ST_SET_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ST_UPD_USR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    ST_UPD_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GUEST_CARD_MASTER",
                columns: table => new
                {
                    GC_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    GC_CRD_SEQ = table.Column<long>(type: "bigint", nullable: false),
                    GC_CRD_NUM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GC_CRD_NAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GC_REP_UNT = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    GC_CRD_DEP = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GC_CRD_TYP = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    GC_ENT_USR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GC_ENT_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GC_EFF_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GC_CLS_DAT = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GUEST_CARD_MASTER", x => x.GC_COM_COD);
                });

            migrationBuilder.CreateTable(
                name: "GUEST_CARD_MASTER_HIS",
                columns: table => new
                {
                    GC_COM_COD = table.Column<long>(type: "bigint", nullable: false),
                    GC_CRD_SEQ = table.Column<long>(type: "bigint", nullable: false),
                    GC_CRD_NUM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GC_CRD_NAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GC_REP_UNT = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    GC_CRD_DEP = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GC_CRD_TYP = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    GC_MOD_USR = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                    GC_MOD_ON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IDX_GUEST_CARD_MASTER_GC_CRD_SEQ",
                table: "GUEST_CARD_MASTER",
                column: "GC_CRD_SEQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CANTEEN_CARD_MAP");

            migrationBuilder.DropTable(
                name: "CARD_SETTLEMENT");

            migrationBuilder.DropTable(
                name: "GUEST_CARD_MASTER");

            migrationBuilder.DropTable(
                name: "GUEST_CARD_MASTER_HIS");
        }
    }
}
