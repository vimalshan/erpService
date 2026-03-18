namespace ApprovalService.Infrastructure.Persistence.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

/// <summary>
/// Initial migration for ApprovalService database
/// </summary>
#nullable disable

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "APPR_MAST",
            columns: table => new
            {
                APPR_ID = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                APPR_CODE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                APPR_NAME = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                APPR_MODULE = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                APPR_LEVEL = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                APPR_STATUS = table.Column<string>(type: "nvarchar(1)", nullable: false),
                CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_APPR_MAST", x => x.APPR_ID);
            });

        migrationBuilder.CreateTable(
            name: "APPROVER_EMP",
            columns: table => new
            {
                APPROVER_ID = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                APPR_ID = table.Column<long>(type: "bigint", nullable: false),
                EMP_SYSID = table.Column<long>(type: "bigint", nullable: false),
                APPROVER_LEVEL = table.Column<int>(type: "int", nullable: false),
                APPROVER_STATUS = table.Column<string>(type: "nvarchar(1)", nullable: false),
                EFFECTIVE_FROM = table.Column<DateTime>(type: "date", nullable: false),
                EFFECTIVE_TO = table.Column<DateTime>(type: "date", nullable: true),
                CREATED_BY = table.Column<long>(type: "bigint", nullable: false),
                CREATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: false, defaultValueSql: "GETDATE()"),
                UPDATED_BY = table.Column<long>(type: "bigint", nullable: true),
                UPDATED_ON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_APPROVER_EMP", x => x.APPROVER_ID);
                table.ForeignKey(
                    name: "FK_APPROVER_EMP_APPR_MAST",
                    column: x => x.APPR_ID,
                    principalTable: "APPR_MAST",
                    principalColumn: "APPR_ID",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_APPROVER_EMP_APPR_ID",
            table: "APPROVER_EMP",
            column: "APPR_ID");

        migrationBuilder.CreateIndex(
            name: "IX_APPROVER_EMP_EMP_SYSID",
            table: "APPROVER_EMP",
            column: "EMP_SYSID");

        migrationBuilder.CreateIndex(
            name: "UQ_APPR_CODE",
            table: "APPR_MAST",
            column: "APPR_CODE",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_APPR_MAST_MODULE",
            table: "APPR_MAST",
            column: "APPR_MODULE");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "APPROVER_EMP");

        migrationBuilder.DropTable(
            name: "APPR_MAST");
    }
}

#nullable restore
