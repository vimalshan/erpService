using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LET_MAIN09",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LET_ID = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_DD_REQNO = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_DEV_SOURCE = table.Column<decimal>(type: "numeric(38,0)", nullable: true),
                    LET_SPECIFIC_NEED = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_INDICATOR = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_DEV_AREA = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_EXPECTEDPOST_TRAINING = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_BHRSTATUS = table.Column<char>(type: "char(1)", nullable: true),
                    LET_REVIEWER_COMMENTS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_APP_OPINION = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_APPR_COMMENTS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_MODIFIEDBY = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_EMPLOYEE_ID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LET_CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LET_UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LET_VERSION = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LET_MAIN09", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LET_FEEDBACK",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LET_SRL = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_DDREQNO = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_SPECIFIC_NEED = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_TRAINING = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LET_FEEDBACK_STATUS = table.Column<char>(type: "char(1)", nullable: true),
                    LET_APPRAISEE_COMMENTS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_APPRAISER_COMMENTS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_REVIEWER_COMMENTS = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_APPR_NEEDSTATUS = table.Column<char>(type: "char(1)", nullable: true),
                    LET_APPR_POSTTRAINING = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LET_MODIFIEDBY = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_FEEDBACK_CREATED_AT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LET_FEEDBACK_UPDATED_AT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LET_FEEDBACK_VERSION = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LET_FEEDBACK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DD_CAT_DEV_DETAIL",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CT_REQ_NUM = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    CT_QTN_NUM = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    CT_ANS_SRL = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    CT_APP_ID = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CT_APP_NUM = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    CT_ENT_DAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CT_DESC = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CT_NEED = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DD_CAT_DEV_DETAIL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LET_SUB09",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LET_MODID = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_RECORD_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LET_DD_REQNO = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_DEVELOPMEN_MODE = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_TRAINING_ID = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_TRAINING_DET = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LET_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LET_DEVELOPMENTID = table.Column<decimal>(type: "numeric(38,0)", nullable: false),
                    LET_FINALREVIEW = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LET_SUB09", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LET_SUB09_LET_MAIN09_LET_RECORD_ID",
                        column: x => x.LET_RECORD_ID,
                        principalTable: "LET_MAIN09",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LET_SUB09_LET_RECORD_ID",
                table: "LET_SUB09",
                column: "LET_RECORD_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LET_SUB09");

            migrationBuilder.DropTable(
                name: "LET_FEEDBACK");

            migrationBuilder.DropTable(
                name: "DD_CAT_DEV_DETAIL");

            migrationBuilder.DropTable(
                name: "LET_MAIN09");
        }
    }
}
