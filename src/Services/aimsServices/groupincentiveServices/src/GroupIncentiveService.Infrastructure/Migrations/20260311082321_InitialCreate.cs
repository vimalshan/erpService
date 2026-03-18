using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupIncentiveService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group_Master",
                columns: table => new
                {
                    GROUP_ID = table.Column<int>(type: "int", nullable: false),
                    GROUP_NAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GROUP_DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GROUP_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    GROUP_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GROUP_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    GROUP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GROUP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group_Master", x => x.GROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "GROUP_EMPLOYEEMAP",
                columns: table => new
                {
                    GRPEMPMAP_ID = table.Column<long>(type: "bigint", nullable: false),
                    GRPEMPMAP_GROUPID = table.Column<int>(type: "int", nullable: false),
                    GRPEMPMAP_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    GRPEMPMAP_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    GRPEMPMAP_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GRPEMPMAP_ROLE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GRPEMPMAP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GRPEMPMAP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GROUP_EMPLOYEEMAP", x => x.GRPEMPMAP_ID);
                    table.ForeignKey(
                        name: "FK_GROUP_EMPLOYEEMAP_Group_Master_GRPEMPMAP_GROUPID",
                        column: x => x.GRPEMPMAP_GROUPID,
                        principalTable: "Group_Master",
                        principalColumn: "GROUP_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GROUPINCENTIVE_BREAK",
                columns: table => new
                {
                    GRPINCBRK_ID = table.Column<int>(type: "int", nullable: false),
                    GRPINCBRK_GROUPID = table.Column<int>(type: "int", nullable: false),
                    GRPINCBRK_ATTPERCENTAGE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    GRPINCBRK_INCPERCENTAGE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    GRPINCBRK_EFFDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    GRPINCBRK_CLSDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GRPINCBRK_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCBRK_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GROUPINCENTIVE_BREAK", x => x.GRPINCBRK_ID);
                    table.ForeignKey(
                        name: "FK_GROUPINCENTIVE_BREAK_Group_Master_GRPINCBRK_GROUPID",
                        column: x => x.GRPINCBRK_GROUPID,
                        principalTable: "Group_Master",
                        principalColumn: "GROUP_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GROUPINCENTIVE_MAIN",
                columns: table => new
                {
                    GRPINC_ID = table.Column<long>(type: "bigint", nullable: false),
                    GRPINC_GROUPID = table.Column<int>(type: "int", nullable: false),
                    GRPINC_INCMONTH = table.Column<int>(type: "int", nullable: false),
                    GRPINC_INCYEAR = table.Column<int>(type: "int", nullable: false),
                    GRPINC_TOTALAMOUNT = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    GRPINC_APPSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    GRPINC_APPROVEDAMOUNT = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    GRPINC_APPROVER = table.Column<long>(type: "bigint", nullable: true),
                    GRPINC_APPROVALDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                    GRPINC_ENTEREDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    GRPINC_ENTEREDBY = table.Column<long>(type: "bigint", nullable: false),
                    GRPINC_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GRPINC_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GROUPINCENTIVE_MAIN", x => x.GRPINC_ID);
                    table.ForeignKey(
                        name: "FK_GROUPINCENTIVE_MAIN_Group_Master_GRPINC_GROUPID",
                        column: x => x.GRPINC_GROUPID,
                        principalTable: "Group_Master",
                        principalColumn: "GROUP_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GROUPINCENTIVE_APPROVAL",
                columns: table => new
                {
                    GRPINCAPP_ID = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCAPP_MAINID = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCAPP_APPROVER = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCAPP_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    GRPINCAPP_REMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GRPINCAPP_APPROVALDATE = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                    GRPINCAPP_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCAPP_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GROUPINCENTIVE_APPROVAL", x => x.GRPINCAPP_ID);
                    table.ForeignKey(
                        name: "FK_GROUPINCENTIVE_APPROVAL_GROUPINCENTIVE_MAIN_GRPINCAPP_MAINID",
                        column: x => x.GRPINCAPP_MAINID,
                        principalTable: "GROUPINCENTIVE_MAIN",
                        principalColumn: "GRPINC_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GROUPINCENTIVE_DET",
                columns: table => new
                {
                    GRPINCDET_ID = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCDET_MAINID = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCDET_EMPSYSID = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCDET_ALLOCPERCENTAGE = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    GRPINCDET_ALLOCAMOUNT = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    GRPINCDET_APPROVEDAMOUNT = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    GRPINCDET_APPSTATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    GRPINCDET_LASTMODIFIEDBY = table.Column<long>(type: "bigint", nullable: false),
                    GRPINCDET_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GROUPINCENTIVE_DET", x => x.GRPINCDET_ID);
                    table.ForeignKey(
                        name: "FK_GROUPINCENTIVE_DET_GROUPINCENTIVE_MAIN_GRPINCDET_MAINID",
                        column: x => x.GRPINCDET_MAINID,
                        principalTable: "GROUPINCENTIVE_MAIN",
                        principalColumn: "GRPINC_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GROUP_EMPLOYEEMAP_EMPSYSID",
                table: "GROUP_EMPLOYEEMAP",
                column: "GRPEMPMAP_EMPSYSID");

            migrationBuilder.CreateIndex(
                name: "IX_GROUP_EMPLOYEEMAP_GROUPID",
                table: "GROUP_EMPLOYEEMAP",
                column: "GRPEMPMAP_GROUPID");

            migrationBuilder.CreateIndex(
                name: "UQ_GRPEMPMAP",
                table: "GROUP_EMPLOYEEMAP",
                columns: new[] { "GRPEMPMAP_GROUPID", "GRPEMPMAP_EMPSYSID", "GRPEMPMAP_EFFDATE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Group_Master_STATUS",
                table: "Group_Master",
                column: "GROUP_STATUS");

            migrationBuilder.CreateIndex(
                name: "UQ_GROUP_NAME",
                table: "Group_Master",
                column: "GROUP_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GROUPINCENTIVE_APPROVAL_GRPINCAPP_MAINID",
                table: "GROUPINCENTIVE_APPROVAL",
                column: "GRPINCAPP_MAINID");

            migrationBuilder.CreateIndex(
                name: "IX_GROUPINCENTIVE_BREAK_GRPINCBRK_GROUPID",
                table: "GROUPINCENTIVE_BREAK",
                column: "GRPINCBRK_GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_GROUPINCENTIVE_DET_APPSTATUS",
                table: "GROUPINCENTIVE_DET",
                column: "GRPINCDET_APPSTATUS");

            migrationBuilder.CreateIndex(
                name: "IX_GROUPINCENTIVE_DET_MAINID",
                table: "GROUPINCENTIVE_DET",
                column: "GRPINCDET_MAINID");

            migrationBuilder.CreateIndex(
                name: "UQ_GRPINCDET",
                table: "GROUPINCENTIVE_DET",
                columns: new[] { "GRPINCDET_MAINID", "GRPINCDET_EMPSYSID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GROUPINCENTIVE_MAIN_APPSTATUS",
                table: "GROUPINCENTIVE_MAIN",
                column: "GRPINC_APPSTATUS");

            migrationBuilder.CreateIndex(
                name: "IX_GROUPINCENTIVE_MAIN_GROUPID",
                table: "GROUPINCENTIVE_MAIN",
                column: "GRPINC_GROUPID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GROUP_EMPLOYEEMAP");

            migrationBuilder.DropTable(
                name: "GROUPINCENTIVE_APPROVAL");

            migrationBuilder.DropTable(
                name: "GROUPINCENTIVE_BREAK");

            migrationBuilder.DropTable(
                name: "GROUPINCENTIVE_DET");

            migrationBuilder.DropTable(
                name: "GROUPINCENTIVE_MAIN");

            migrationBuilder.DropTable(
                name: "Group_Master");
        }
    }
}
