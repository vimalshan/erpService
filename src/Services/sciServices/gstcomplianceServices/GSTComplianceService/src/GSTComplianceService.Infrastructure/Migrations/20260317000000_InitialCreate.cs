using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSTComplianceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate :  Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GST_SUPPLIER",
                columns: table => new
                {
                    SUPPLIER_NUMBER = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUPPLIER_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EMAIL_ADDRESS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PAN_NO = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GST_SUPPLIER", x => x.SUPPLIER_NUMBER);
                });

            migrationBuilder.CreateTable(
                name: "GST_MAIN",
                columns: table => new
                {
                    GST_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GST_TYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GST_PANNO = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GST_EMAILID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_MOBILENO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GST_CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    GST_MODIFIEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GST_VENDORID = table.Column<long>(type: "bigint", nullable: true),
                    GST_VENDORNAMEFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GST_VENDORNAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_VENDCONST = table.Column<int>(type: "int", nullable: true),
                    GST_VENDADDFLAG = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GST_VENDADDLINE1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_VENDADDLINE2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDADDLINE3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDADDLINE4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDCITY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDCITYNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDSTATE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDPINCODE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_REGISTRATIONTYPE = table.Column<int>(type: "int", nullable: false),
                    GST_CONTACTNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_CONTACTEMAILID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_CONTACTMOBILENO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GST_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_STATUS = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GST_DIGITALFLAG = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GST_GSTNCOPY = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_ENTEREDBYFLA = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    GST_ENTEREDBY = table.Column<long>(type: "bigint", nullable: true),
                    GST_SCREENTYPE = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GST_MAIN", x => x.GST_ID);
                    table.UniqueConstraint("AK_GST_MAIN_GST_PANNO", x => x.GST_PANNO);
                });

            migrationBuilder.CreateTable(
                name: "GST_HSNDET",
                columns: table => new
                {
                    GSTHSN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GSTHSN_GSTID = table.Column<long>(type: "bigint", nullable: false),
                    GSTHSN_PRODUCTNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GSTHSN_HSNCODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GSTHSN_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GST_HSNDET", x => x.GSTHSN_ID);
                    table.ForeignKey(
                        name: "FK_GST_HSNDET_GST_MAIN_GSTHSN_GSTID",
                        column: x => x.GSTHSN_GSTID,
                        principalTable: "GST_MAIN",
                        principalColumn: "GST_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GST_SERVDET",
                columns: table => new
                {
                    GSTSAC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GSTSAC_GSTID = table.Column<long>(type: "bigint", nullable: false),
                    GSTSAC_SERVICENAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GSTSAC_SACCODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GSTSAC_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GST_SERVDET", x => x.GSTSAC_ID);
                    table.ForeignKey(
                        name: "FK_GST_SERVDET_GST_MAIN_GSTSAC_GSTID",
                        column: x => x.GSTSAC_GSTID,
                        principalTable: "GST_MAIN",
                        principalColumn: "GST_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GST_STATEREGDET",
                columns: table => new
                {
                    GST_TINID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GST_ID = table.Column<long>(type: "bigint", nullable: false),
                    GST_STATE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GST_ADDRESS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_VENDCITY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDCITYNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_VENDPINCODE = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    GST_TINNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GST_EXCNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GST_SERNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GST_GSTINNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GST_ARNNO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GST_ARNCOPY = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_ARNTEMPFILE = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GST_CONTACTPERSON = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_EMAILID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GST_MOBILENO = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GST_REMARKS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GST_STATEREGDET", x => x.GST_TINID);
                    table.ForeignKey(
                        name: "FK_GST_STATEREGDET_GST_MAIN_GST_ID",
                        column: x => x.GST_ID,
                        principalTable: "GST_MAIN",
                        principalColumn: "GST_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GST_HSNDET_GSTHSN_GSTID",
                table: "GST_HSNDET",
                column: "GSTHSN_GSTID");

            migrationBuilder.CreateIndex(
                name: "IX_GST_SERVDET_GSTSAC_GSTID",
                table: "GST_SERVDET",
                column: "GSTSAC_GSTID");

            migrationBuilder.CreateIndex(
                name: "IX_GST_STATEREGDET_GST_ID",
                table: "GST_STATEREGDET",
                column: "GST_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GST_HSNDET");

            migrationBuilder.DropTable(
                name: "GST_SERVDET");

            migrationBuilder.DropTable(
                name: "GST_STATEREGDET");

            migrationBuilder.DropTable(
                name: "GST_SUPPLIER");

            migrationBuilder.DropTable(
                name: "GST_MAIN");
        }
    }
}
