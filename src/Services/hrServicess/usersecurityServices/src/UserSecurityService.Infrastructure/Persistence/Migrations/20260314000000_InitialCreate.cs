using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserSecurityService.Infrastructure.Persistence.Migrations;

/// <summary>InitialCreate migration — creates all UserSecurity tables.</summary>
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "USER_PROFILE_PFS",
            columns: table => new
            {
                EM_USR_ID = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                EM_EMP_NUM = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                EM_UNT_COD = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                EM_NICK_NAM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: false),
                EM_USR_TYP = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                EM_EML_FLG = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                EM_OEML_ID = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                EM_PEML_ID = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                EM_EFF_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                EM_CLS_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                EM_USR_PASS = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                EM_EMP_NAM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                EM_DOB_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                EM_PHT_PTH = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                EM_DIV_NAM = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                EM_JOB_COD = table.Column<long>(type: "BIGINT", nullable: true),
                EM_PIN_NUM = table.Column<decimal>(type: "DECIMAL(20,0)", nullable: true),
                EM_OLD_NUM = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                EM_EMP_DSG = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                EM_FRS_NAM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                EM_MID_NAM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                EM_LST_NAM = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                EM_CUR_BUS = table.Column<string>(type: "char(9)", maxLength: 9, nullable: true),
                EM_REP_UNT = table.Column<string>(type: "char(3)", maxLength: 3, nullable: true),
                EM_CUR_GRD = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                EM_PRO_DAT = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                EM_CUR_LOC = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true),
                EM_TIM_UNT = table.Column<string>(type: "char(3)", maxLength: 3, nullable: true),
                EM_CTC_AMT = table.Column<decimal>(type: "DECIMAL(19,0)", nullable: true),
                EM_EMP_SEX = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true),
                EM_APP_USR = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                EM_WRK_FLG = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true),
                EM_SIG_PTH = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                EM_OUTLOOK = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true),
                EM_REGSTATUS = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_USER_PROFILE_PFS", x => x.EM_USR_ID));

        migrationBuilder.CreateTable(
            name: "USER_APPSMAP",
            columns: table => new
            {
                USER_EMPSYSID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_APPS = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                USER_EFFDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                USER_CLSDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                USER_MODIFIEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_MODIFIEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                USER_HRROLEID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_CREATEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                USER_CREATEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                USER_REMARKS = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_USER_APPSMAP", x => x.USER_EMPSYSID));

        migrationBuilder.CreateTable(
            name: "USER_CALENDERMAP",
            columns: table => new
            {
                USER_ROLEID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                CALENDAR_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                MODIFIEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                MODIFIEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_USER_CALENDERMAP", x => x.USER_ROLEID));

        migrationBuilder.CreateTable(
            name: "USER_MENUMAP",
            columns: table => new
            {
                USER_ROLEID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_MENUID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_MODIFIEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_MODIFIEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_USER_MENUMAP", x => x.USER_ROLEID));

        migrationBuilder.CreateTable(
            name: "USER_UNITMAP",
            columns: table => new
            {
                ROLE_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_APPS = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                ROLE_EMPSYSID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_ORGID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_UNITALL = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                ROLE_UNITID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                ROLE_MENUGROUPID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                ROLE_TYPE = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                ROLE_EFFDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                ROLE_CLSDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                ROLE_MODIFIEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_MODIFIEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                ROLE_REMARKS = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                ROLE_VTCENTRY = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true)
            },
            constraints: table => { });

        migrationBuilder.CreateTable(
            name: "USER_UNITMAPLOG",
            columns: table => new
            {
                ROLE_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_APPS = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                ROLE_EMPSYSID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_ORGID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_UNITALL = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                ROLE_UNITID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                ROLE_MENUGROUPID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: true),
                ROLE_TYPE = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                ROLE_EFFDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                ROLE_CLSDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                ROLE_MODIFIEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                ROLE_MODIFIEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                ROLE_REMARKS = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                ROLE_VTCENTRY = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true),
                LOG_CREATEDBY = table.Column<decimal>(type: "DECIMAL(22,0)", nullable: false),
                LOG_CREATEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
            },
            constraints: table => { });

        migrationBuilder.CreateTable(
            name: "USER_CALENDERMAP_LOG",
            columns: table => new
            {
                USER_ROLEID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                CALENDAR_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                CLSDATE = table.Column<DateTime>(type: "DATETIME2(3)", nullable: true),
                MODIFIEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                MODIFIEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                LOGCREATED_BY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                LOGCREATED_ON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
            },
            constraints: table => { });

        migrationBuilder.CreateTable(
            name: "USER_MENUMAP_LOG",
            columns: table => new
            {
                USER_ROLEID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_MENUID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_MODIFIEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                USER_MODIFIEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false),
                LOG_CREATEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                LOG_CREATEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
            },
            constraints: table => { });

        migrationBuilder.CreateTable(
            name: "EMP_PASSWORDCHANGE",
            columns: table => new
            {
                EPWD_ID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                EPWD_EMPSYSID = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                EPWD_CREATEDBY = table.Column<decimal>(type: "DECIMAL(38,0)", nullable: false),
                EPWD_CREATEDON = table.Column<DateTime>(type: "DATETIME2(3)", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_EMP_PASSWORDCHANGE", x => x.EPWD_ID));

        // Seed: default admin user (password = "Admin@1234!" — regenerate in production)
        migrationBuilder.Sql(@"
            IF NOT EXISTS (SELECT 1 FROM USER_PROFILE_PFS WHERE EM_USR_ID = 'admin')
            BEGIN
                INSERT INTO USER_PROFILE_PFS
                    (EM_USR_ID, EM_EMP_NUM, EM_UNT_COD, EM_NICK_NAM, EM_USR_TYP,
                     EM_EML_FLG, EM_EFF_DAT, EM_USR_PASS, EM_EMP_NAM, EM_REGSTATUS)
                VALUES
                    ('admin', 1, 'HQ', 'Administrator', 'A',
                     'Y', '2026-01-01', 'seed_hash_placeholder', 'System Administrator', 'A')
            END");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("EMP_PASSWORDCHANGE");
        migrationBuilder.DropTable("USER_MENUMAP_LOG");
        migrationBuilder.DropTable("USER_CALENDERMAP_LOG");
        migrationBuilder.DropTable("USER_UNITMAPLOG");
        migrationBuilder.DropTable("USER_UNITMAP");
        migrationBuilder.DropTable("USER_MENUMAP");
        migrationBuilder.DropTable("USER_CALENDERMAP");
        migrationBuilder.DropTable("USER_APPSMAP");
        migrationBuilder.DropTable("USER_PROFILE_PFS");
    }
}
