using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BatchAndEnvelopeService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BATCH_MAIN",
                columns: table => new
                {
                    BATCH_ID = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_LOCATIONID = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_RECEIVEDBY = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_RECEIVEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_PODNO = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    BATCH_SUMMARYFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BATCH_CANCELBY = table.Column<long>(type: "bigint", nullable: true),
                    BATCH_CANCELDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCH_CONFIRMEDBY = table.Column<long>(type: "bigint", nullable: true),
                    BATCH_CONFIRMEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCH_COURIERNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BATCH_SCANFLAG = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BATCH_MAIN", x => x.BATCH_ID);
                });

            migrationBuilder.CreateTable(
                name: "ENV_MAIN",
                columns: table => new
                {
                    ENV_ID = table.Column<long>(type: "bigint", nullable: false),
                    ENV_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ENV_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ENV_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ENV_RECEIVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    ENV_RECEIVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ENV_SUMMARYFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ENV_CANCELLEDBY = table.Column<long>(type: "bigint", nullable: true),
                    ENV_CANCELLEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ENV_CONFIRMEDBY = table.Column<long>(type: "bigint", nullable: true),
                    ENV_CONFIRMEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ENV_SCANLOTNO = table.Column<long>(type: "bigint", nullable: true),
                    ENV_LOCID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENV_MAIN", x => x.ENV_ID);
                });

            migrationBuilder.CreateTable(
                name: "SCAN_LOTMAST",
                columns: table => new
                {
                    SCAN_LOTNO = table.Column<long>(type: "bigint", nullable: false),
                    SCAN_USERID = table.Column<long>(type: "bigint", nullable: false),
                    SCAN_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SCAN_DEVICENO = table.Column<int>(type: "int", nullable: false),
                    SCAN_CLOSEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCAN_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SCAN_DEVICEID = table.Column<long>(type: "bigint", nullable: true),
                    SCAN_FLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCAN_LOTMAST", x => x.SCAN_LOTNO);
                });

            migrationBuilder.CreateTable(
                name: "BATCH_DET",
                columns: table => new
                {
                    BATCH_DETID = table.Column<int>(type: "int", nullable: false),
                    BATCH_ID = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_ENVID = table.Column<int>(type: "int", nullable: false),
                    BATCH_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    BATCH_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_RECEIVEFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BATCH_RECEIVEDBY = table.Column<long>(type: "bigint", nullable: true),
                    BATCH_RECEIVEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BATCH_CANCELDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BATCH_CANCELBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BATCH_DET", x => x.BATCH_DETID);
                    table.ForeignKey(
                        name: "FK_BATCH_DET_BATCH_MAIN_BATCH_ID",
                        column: x => x.BATCH_ID,
                        principalTable: "BATCH_MAIN",
                        principalColumn: "BATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BATCH_RECDET",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false),
                    REC_BATCHID = table.Column<long>(type: "bigint", nullable: false),
                    REC_ENVID = table.Column<long>(type: "bigint", nullable: false),
                    REC_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    REC_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    REC_SCANLOCATIONID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BATCH_RECDET", x => x.REC_ID);
                    table.ForeignKey(
                        name: "FK_BATCH_RECDET_BATCH_MAIN_REC_BATCHID",
                        column: x => x.REC_BATCHID,
                        principalTable: "BATCH_MAIN",
                        principalColumn: "BATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ENV_DET",
                columns: table => new
                {
                    ENV_DETID = table.Column<long>(type: "bigint", nullable: false),
                    ENV_ID = table.Column<long>(type: "bigint", nullable: false),
                    ENV_TYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ENV_DOCID = table.Column<int>(type: "int", nullable: false),
                    ENV_CREATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ENV_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ENV_RECEIVEFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ENV_RECEIVEDBY = table.Column<long>(type: "bigint", nullable: false),
                    ENV_RECEIVEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ENV_CANCELDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ENV_CANCELBY = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENV_DET", x => x.ENV_DETID);
                    table.ForeignKey(
                        name: "FK_ENV_DET_ENV_MAIN_ENV_ID",
                        column: x => x.ENV_ID,
                        principalTable: "ENV_MAIN",
                        principalColumn: "ENV_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ENV_RECDET",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false),
                    REC_ENVID = table.Column<long>(type: "bigint", nullable: false),
                    REC_DOCID = table.Column<long>(type: "bigint", nullable: false),
                    REC_UPDATEDBY = table.Column<long>(type: "bigint", nullable: false),
                    REC_UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    REC_ENVTYPE = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    REC_SCANLOCATIONID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENV_RECDET", x => x.REC_ID);
                    table.ForeignKey(
                        name: "FK_ENV_RECDET_ENV_MAIN_REC_ENVID",
                        column: x => x.REC_ENVID,
                        principalTable: "ENV_MAIN",
                        principalColumn: "ENV_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BATCH_DET_BATCH_ID",
                table: "BATCH_DET",
                column: "BATCH_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BATCH_RECDET_REC_BATCHID",
                table: "BATCH_RECDET",
                column: "REC_BATCHID");

            migrationBuilder.CreateIndex(
                name: "IX_ENV_DET_ENV_ID",
                table: "ENV_DET",
                column: "ENV_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ENV_RECDET_REC_ENVID",
                table: "ENV_RECDET",
                column: "REC_ENVID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BATCH_DET");

            migrationBuilder.DropTable(
                name: "BATCH_RECDET");

            migrationBuilder.DropTable(
                name: "ENV_DET");

            migrationBuilder.DropTable(
                name: "ENV_RECDET");

            migrationBuilder.DropTable(
                name: "SCAN_LOTMAST");

            migrationBuilder.DropTable(
                name: "BATCH_MAIN");

            migrationBuilder.DropTable(
                name: "ENV_MAIN");
        }
    }
}
