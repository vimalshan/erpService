using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingDevelopment.Infrastructure.Data.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "INSTITUTE_MASTER",
            columns: table => new
            {
                INSTITUTE_CODE = table.Column<decimal>(type: "decimal(22,0)", nullable: false),
                INSTITUTE_NAME = table.Column<string>(maxLength: 100, nullable: true),
                INSTITUTE_ADD1 = table.Column<string>(maxLength: 100, nullable: true),
                INSTITUTE_ADD2 = table.Column<string>(maxLength: 100, nullable: true),
                INSTITUTE_CITY = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_STATE = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_PIN = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_PHONE = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_FAX = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_EMAIL = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_URL = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_TYPE = table.Column<string>(maxLength: 50, nullable: true),
                INSTITUTE_CAMPUSRECRUIT = table.Column<string>(maxLength: 1, nullable: false),
                INSTITUTE_CLASS = table.Column<string>(maxLength: 3, nullable: true),
                INSTITUTE_MODIFIEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                INSTITUTE_MODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_INSTITUTE_MASTER", x => x.INSTITUTE_CODE));

        migrationBuilder.CreateTable(
            name: "PROGRAMLOV_MAST",
            columns: table => new
            {
                PRLOV_TYPECODE = table.Column<string>(maxLength: 20, nullable: false),
                PRLOV_CODE = table.Column<string>(maxLength: 5, nullable: false),
                PRLOV_NAME = table.Column<string>(maxLength: 200, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_PROGRAMLOV_MAST", x => x.PRLOV_TYPECODE));

        migrationBuilder.CreateTable(
            name: "TRAINING_DET",
            columns: table => new
            {
                TR_ID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                TR_FINYEAR = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                TR_EMPSYSID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                TR_NEED = table.Column<string>(maxLength: 1000, nullable: false),
                TR_GAPS = table.Column<string>(maxLength: 1000, nullable: false),
                TR_MODE = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                TR_PROGRAMID = table.Column<decimal>(type: "decimal(38,0)", nullable: false),
                TR_PROGRAMDESC = table.Column<string>(maxLength: 1000, nullable: false),
                TR_PLANFROM = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                TR_PLANTO = table.Column<DateTime>(type: "datetime2(3)", nullable: false),
                TR_STATUS = table.Column<string>(maxLength: 1, nullable: false),
                TR_ACTFROM = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                TR_ACTTO = table.Column<DateTime>(type: "datetime2(3)", nullable: true),
                TR_INSTITUTEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                TR_INSTITUTEDESC = table.Column<string>(maxLength: 1000, nullable: true),
                TR_TRAINERID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                TR_TRAINERDESC = table.Column<string>(maxLength: 65, nullable: true),
                TR_PLACEID = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                TR_PLACE = table.Column<string>(maxLength: 65, nullable: true),
                TR_COST = table.Column<decimal>(type: "decimal(38,0)", nullable: true),
                TR_DROPREMARKS = table.Column<string>(maxLength: 1000, nullable: true),
                TR_LASTMODIFIEDBY = table.Column<decimal>(type: "decimal(22,0)", nullable: true),
                TR_LASTMODIFIEDON = table.Column<DateTime>(type: "datetime2(3)", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_TRAINING_DET", x => x.TR_ID));

        migrationBuilder.CreateIndex(name: "IX_TRAINING_DET_TR_EMPSYSID", table: "TRAINING_DET", column: "TR_EMPSYSID");
        migrationBuilder.CreateIndex(name: "IX_TRAINING_DET_TR_FINYEAR", table: "TRAINING_DET", column: "TR_FINYEAR");
        migrationBuilder.CreateIndex(name: "IX_TRAINING_DET_TR_STATUS", table: "TRAINING_DET", column: "TR_STATUS");

        // Seed data
        migrationBuilder.InsertData(
            table: "INSTITUTE_MASTER",
            columns: ["INSTITUTE_CODE", "INSTITUTE_NAME", "INSTITUTE_CAMPUSRECRUIT", "INSTITUTE_TYPE"],
            values: new object[] { 1m, "National Institute of Technology", "Y", "Technical" });

        migrationBuilder.InsertData(
            table: "INSTITUTE_MASTER",
            columns: ["INSTITUTE_CODE", "INSTITUTE_NAME", "INSTITUTE_CAMPUSRECRUIT", "INSTITUTE_TYPE"],
            values: new object[] { 2m, "Indian Institute of Management", "N", "Management" });

        migrationBuilder.InsertData(
            table: "PROGRAMLOV_MAST",
            columns: ["PRLOV_TYPECODE", "PRLOV_CODE", "PRLOV_NAME"],
            values: new object[] { "SAFETY", "S001", "Safety Training Program" });

        migrationBuilder.InsertData(
            table: "PROGRAMLOV_MAST",
            columns: ["PRLOV_TYPECODE", "PRLOV_CODE", "PRLOV_NAME"],
            values: new object[] { "TECH", "T001", "Technical Skills Development" });

        migrationBuilder.InsertData(
            table: "PROGRAMLOV_MAST",
            columns: ["PRLOV_TYPECODE", "PRLOV_CODE", "PRLOV_NAME"],
            values: new object[] { "MGMT", "M001", "Management Development Program" });

        migrationBuilder.InsertData(
            table: "PROGRAMLOV_MAST",
            columns: ["PRLOV_TYPECODE", "PRLOV_CODE", "PRLOV_NAME"],
            values: new object[] { "SOFT", "SO01", "Soft Skills Training" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "TRAINING_DET");
        migrationBuilder.DropTable(name: "INSTITUTE_MASTER");
        migrationBuilder.DropTable(name: "PROGRAMLOV_MAST");
    }
}
