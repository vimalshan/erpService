using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterDataService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMP_FINYEAR",
                columns: table => new
                {
                    AC_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AC_STR_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AC_END_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AC_CLS_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: false),
                    AC_REMARKS = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    AC_INT_FLG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    AC_EMP_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    AC_EMP_DESG = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true),
                    AC_BAT_NUM = table.Column<long>(type: "bigint", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMP_FINYEAR", x => x.AC_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "COMP_MONTH",
                columns: table => new
                {
                    AC_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    AC_MNT_NAM = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMP_MONTH", x => x.AC_SRL_NUM);
                });

            migrationBuilder.CreateTable(
                name: "CONFIGURATION",
                columns: table => new
                {
                    CONFIG_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CONFIG_KEY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CONFIG_VALUE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CONFIG_TYPE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CONFIG_DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONFIGURATION", x => x.CONFIG_ID);
                });

            migrationBuilder.CreateTable(
                name: "FUND_TYPE_MASTER",
                columns: table => new
                {
                    FUND_TYPECODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    FUND_TYPENAME = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FUND_TYPE_MASTER", x => x.FUND_TYPECODE);
                });

            migrationBuilder.CreateTable(
                name: "INVCAT_LIMIT",
                columns: table => new
                {
                    INVCAT_LIMITID = table.Column<int>(type: "int", nullable: false),
                    INVCAT_ID = table.Column<int>(type: "int", nullable: false),
                    INVCAT_MAXPER = table.Column<int>(type: "int", nullable: false),
                    INVCAT_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INVCAT_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVCAT_LIMIT", x => x.INVCAT_LIMITID);
                });

            migrationBuilder.CreateTable(
                name: "INVCATGRP_MAST",
                columns: table => new
                {
                    INVGRP_ID = table.Column<int>(type: "int", nullable: false),
                    INVGRP_SHTNAME = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    INVGRP_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVCATGRP_MAST", x => x.INVGRP_ID);
                });

            migrationBuilder.CreateTable(
                name: "LOV_MASTER",
                columns: table => new
                {
                    LOV_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    LOV_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LOV_DESC = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LOV_VALUE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LOV_CATEGORY = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LOV_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOV_MASTER", x => x.LOV_ID);
                });

            migrationBuilder.CreateTable(
                name: "PF_FINYEARRULES",
                columns: table => new
                {
                    PF_FINYEAR_CODE = table.Column<long>(type: "bigint", nullable: false),
                    PF_FINYEAR_RULES = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PF_FINYEARRULES", x => x.PF_FINYEAR_CODE);
                });

            migrationBuilder.CreateTable(
                name: "PF_HRIS",
                columns: table => new
                {
                    COM_COD = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    EMP_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    PIN_NUM = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PF_HRIS", x => new { x.COM_COD, x.EMP_NUM });
                });

            migrationBuilder.CreateTable(
                name: "PF_MAIN_ACCOUNT",
                columns: table => new
                {
                    MAIN_ACC_COD = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    MAIN_ACC_NAM = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PF_MAIN_ACCOUNT", x => x.MAIN_ACC_COD);
                });

            migrationBuilder.CreateTable(
                name: "RATE_TYPE_MASTER",
                columns: table => new
                {
                    RATE_TYPE_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    RATE_TYPE_NAME = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RATE_TYPE_MASTER", x => x.RATE_TYPE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "ROLE_MASTER",
                columns: table => new
                {
                    ROLE_CODE = table.Column<long>(type: "bigint", nullable: false),
                    ROLE_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    ROLE_DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ROLE_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, defaultValue: "A"),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE_MASTER", x => x.ROLE_CODE);
                });

            migrationBuilder.CreateTable(
                name: "STATUS_MASTER",
                columns: table => new
                {
                    STATUS_TYPE = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    STATUS_CODE = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    STATUS_NAME = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATUS_MASTER", x => new { x.STATUS_TYPE, x.STATUS_CODE });
                });

            migrationBuilder.CreateTable(
                name: "INVGRP_LIMIT",
                columns: table => new
                {
                    INVGRP_LIMITID = table.Column<int>(type: "int", nullable: false),
                    INVGRP_ID = table.Column<int>(type: "int", nullable: false),
                    INVGRP_MAXPER = table.Column<int>(type: "int", nullable: false),
                    INVGRP_EFFDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    INVGRP_CLSDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    INVGRP_RANGE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVGRP_LIMIT", x => x.INVGRP_LIMITID);
                    table.ForeignKey(
                        name: "FK_INVGRP_LIMIT_INVCATGRP_MAST_INVGRP_ID",
                        column: x => x.INVGRP_ID,
                        principalTable: "INVCATGRP_MAST",
                        principalColumn: "INVGRP_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PF_MAIN_SUB",
                columns: table => new
                {
                    MAIN_ACC_COD = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                    SUB_ACC_COD = table.Column<decimal>(type: "decimal(38,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PF_MAIN_SUB", x => new { x.MAIN_ACC_COD, x.SUB_ACC_COD });
                    table.ForeignKey(
                        name: "FK_PF_MAIN_SUB_PF_MAIN_ACCOUNT_MAIN_ACC_COD",
                        column: x => x.MAIN_ACC_COD,
                        principalTable: "PF_MAIN_ACCOUNT",
                        principalColumn: "MAIN_ACC_COD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RATE_MASTER",
                columns: table => new
                {
                    RT_TRUST_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: false),
                    RATE_ID = table.Column<int>(type: "int", nullable: false),
                    RT_RATE_TYPE_CODE = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    RATE_EFF_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RATE_CLS_DATE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RATE_VALUE = table.Column<decimal>(type: "decimal(19,0)", nullable: true),
                    RATE_DEL_FLAG = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    RT_REWRK_STS = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RATE_MASTER", x => new { x.RT_TRUST_CODE, x.RATE_ID });
                    table.ForeignKey(
                        name: "FK_RATE_MASTER_RATE_TYPE_MASTER_RT_RATE_TYPE_CODE",
                        column: x => x.RT_RATE_TYPE_CODE,
                        principalTable: "RATE_TYPE_MASTER",
                        principalColumn: "RATE_TYPE_CODE");
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CONFIG_KEY",
                table: "CONFIGURATION",
                column: "CONFIG_KEY",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_INVGRP_LIMIT_INVGRP_ID",
                table: "INVGRP_LIMIT",
                column: "INVGRP_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_LOV_MASTER_CODE",
                table: "LOV_MASTER",
                columns: new[] { "LOV_CATEGORY", "LOV_CODE" });

            migrationBuilder.CreateIndex(
                name: "IDX_RATE_MASTER_TYPE",
                table: "RATE_MASTER",
                columns: new[] { "RT_RATE_TYPE_CODE", "RATE_EFF_DATE" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "COMP_FINYEAR");

            migrationBuilder.DropTable(
                name: "COMP_MONTH");

            migrationBuilder.DropTable(
                name: "CONFIGURATION");

            migrationBuilder.DropTable(
                name: "FUND_TYPE_MASTER");

            migrationBuilder.DropTable(
                name: "INVCAT_LIMIT");

            migrationBuilder.DropTable(
                name: "INVGRP_LIMIT");

            migrationBuilder.DropTable(
                name: "LOV_MASTER");

            migrationBuilder.DropTable(
                name: "PF_FINYEARRULES");

            migrationBuilder.DropTable(
                name: "PF_HRIS");

            migrationBuilder.DropTable(
                name: "PF_MAIN_SUB");

            migrationBuilder.DropTable(
                name: "RATE_MASTER");

            migrationBuilder.DropTable(
                name: "ROLE_MASTER");

            migrationBuilder.DropTable(
                name: "STATUS_MASTER");

            migrationBuilder.DropTable(
                name: "INVCATGRP_MAST");

            migrationBuilder.DropTable(
                name: "PF_MAIN_ACCOUNT");

            migrationBuilder.DropTable(
                name: "RATE_TYPE_MASTER");
        }
    }
}
