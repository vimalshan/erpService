using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccidentManagementService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACC_CONTRCT_LST",
                columns: table => new
                {
                    ACL_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ACL_CONT_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ACL_CONT_ID = table.Column<long>(type: "bigint", nullable: false),
                    ACL_STATUS = table.Column<int>(type: "int", nullable: false, defaultValue: 65),
                    ACL_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACC_CONTRCT_LST", x => x.ACL_ID);
                });

            migrationBuilder.CreateTable(
                name: "ACC_PERS_INJ",
                columns: table => new
                {
                    API_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    API_PERS_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    API_SRL_NUM = table.Column<long>(type: "bigint", nullable: false),
                    API_EMP_STATUS = table.Column<int>(type: "int", nullable: false),
                    API_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACC_PERS_INJ", x => x.API_ID);
                });

            migrationBuilder.CreateTable(
                name: "ACCIDENT_SEVERITY",
                columns: table => new
                {
                    SEVERITY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SEVERITY_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SEVERITY_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SEVERITY_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCIDENT_SEVERITY", x => x.SEVERITY_ID);
                });

            migrationBuilder.CreateTable(
                name: "ACCIDENT_STATUS",
                columns: table => new
                {
                    STATUS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STATUS_CODE = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    STATUS_NAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    STATUS_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCIDENT_STATUS", x => x.STATUS_ID);
                });

            migrationBuilder.CreateTable(
                name: "AccidentContractors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractorId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(1)", nullable: false, defaultValue: "A"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccidentContractors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CATEGORY_INJURY",
                columns: table => new
                {
                    CAT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CAT_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CAT_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORY_INJURY", x => x.CAT_ID);
                });

            migrationBuilder.CreateTable(
                name: "DailyAccidentFIRs",
                columns: table => new
                {
                    AccidentNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractorId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeDepartment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccidentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccidentLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NatureOfInjury = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BodyPartAffected = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShiftName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MedicalCentreName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TreatmentGiven = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MedicalCentreReceivingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EnteredUserID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EnteredUserNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EnteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InjuryCategoryCode = table.Column<long>(type: "bigint", nullable: false),
                    NatureOfInjuryCode = table.Column<long>(type: "bigint", nullable: false),
                    PreventiveMeasures = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CauseOfIncident = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShiftInChargePersonName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyAccidentFIRs", x => x.AccidentNumber);
                });

            migrationBuilder.CreateTable(
                name: "DoctorAttendants",
                columns: table => new
                {
                    DoctorAttendantId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(1)", nullable: false, defaultValue: "D"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAttendants", x => x.DoctorAttendantId);
                });

            migrationBuilder.CreateTable(
                name: "NATURE_INJURY",
                columns: table => new
                {
                    NATURE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NATURE_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NATURE_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NATURE_INJURY", x => x.NATURE_ID);
                });

            migrationBuilder.CreateTable(
                name: "NaturesOfInjury",
                columns: table => new
                {
                    NatureId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NatureName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturesOfInjury", x => x.NatureId);
                });

            migrationBuilder.CreateTable(
                name: "PersonalInjuries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PersonInjuredName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeStatus = table.Column<string>(type: "nvarchar(1)", nullable: false, defaultValue: "S"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalInjuries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DAILY_ACC_FIR",
                columns: table => new
                {
                    DAF_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DAF_ACC_NUM = table.Column<long>(type: "bigint", nullable: false),
                    DAF_COM_COD = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DAF_EMP_NUM = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DAF_EMP_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DAF_EMP_DEPT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DAF_CONT_ID = table.Column<long>(type: "bigint", nullable: true),
                    DAF_CONT_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DAF_PERS_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DAF_EMP_STATUS = table.Column<int>(type: "int", nullable: false),
                    DAF_SRL_NUM = table.Column<long>(type: "bigint", nullable: true),
                    DAF_ACC_LOC = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DAF_ACC_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DAF_CAU_INC = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DAF_PRV_MES = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DAF_CAT_INJ = table.Column<long>(type: "bigint", nullable: false),
                    DAF_NAT_INJ = table.Column<long>(type: "bigint", nullable: false),
                    DAF_BODY_PART = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DAF_NATURE_INJ = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DAF_MEDCENTRE_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DAF_MEDCENTRE_DAT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DAF_TRT_GIVEN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DAF_SHIFT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DAF_SHFTINCHRG_NAM = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DAF_SEVERITY_ID = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    DAF_STATUS_ID = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    DAF_ENT_USR = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DAF_ENT_NUM = table.Column<long>(type: "bigint", nullable: false),
                    DAF_ENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InjuryDetailsInjuryCategoryId = table.Column<long>(name: "InjuryDetails.InjuryCategoryId", type: "bigint", nullable: true),
                    InjuryDetailsInjuryNatureId = table.Column<long>(name: "InjuryDetails.InjuryNatureId", type: "bigint", nullable: true),
                    DAF_GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAILY_ACC_FIR", x => x.DAF_ID);
                    table.ForeignKey(
                        name: "FK_DAILY_ACC_FIR_ACCIDENT_SEVERITY_DAF_SEVERITY_ID",
                        column: x => x.DAF_SEVERITY_ID,
                        principalTable: "ACCIDENT_SEVERITY",
                        principalColumn: "SEVERITY_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAILY_ACC_FIR_ACCIDENT_STATUS_DAF_STATUS_ID",
                        column: x => x.DAF_STATUS_ID,
                        principalTable: "ACCIDENT_STATUS",
                        principalColumn: "STATUS_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAILY_ACC_FIR_CATEGORY_INJURY_InjuryDetails.InjuryCategoryId",
                        column: x => x.InjuryDetailsInjuryCategoryId,
                        principalTable: "CATEGORY_INJURY",
                        principalColumn: "CAT_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAILY_ACC_FIR_NATURE_INJURY_InjuryDetails.InjuryNatureId",
                        column: x => x.InjuryDetailsInjuryNatureId,
                        principalTable: "NATURE_INJURY",
                        principalColumn: "NATURE_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACC_CONTRCT_LST_ACL_CONT_ID",
                table: "ACC_CONTRCT_LST",
                column: "ACL_CONT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ACC_CONTRCT_LST_ACL_GUID",
                table: "ACC_CONTRCT_LST",
                column: "ACL_GUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACC_PERS_INJ_API_GUID",
                table: "ACC_PERS_INJ",
                column: "API_GUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACC_PERS_INJ_API_SRL_NUM",
                table: "ACC_PERS_INJ",
                column: "API_SRL_NUM");

            migrationBuilder.CreateIndex(
                name: "IX_ACCIDENT_SEVERITY_SEVERITY_CODE",
                table: "ACCIDENT_SEVERITY",
                column: "SEVERITY_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACCIDENT_SEVERITY_SEVERITY_GUID",
                table: "ACCIDENT_SEVERITY",
                column: "SEVERITY_GUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACCIDENT_STATUS_STATUS_CODE",
                table: "ACCIDENT_STATUS",
                column: "STATUS_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ACCIDENT_STATUS_STATUS_GUID",
                table: "ACCIDENT_STATUS",
                column: "STATUS_GUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORY_INJURY_CAT_GUID",
                table: "CATEGORY_INJURY",
                column: "CAT_GUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_DAILY_ACC_FIR_DAF_ACC_DAT",
                table: "DAILY_ACC_FIR",
                column: "DAF_ENT_DATE");

            migrationBuilder.CreateIndex(
                name: "IDX_DAILY_ACC_FIR_DAF_COM_COD",
                table: "DAILY_ACC_FIR",
                column: "DAF_COM_COD");

            migrationBuilder.CreateIndex(
                name: "IX_DAILY_ACC_FIR_DAF_ACC_NUM",
                table: "DAILY_ACC_FIR",
                column: "DAF_ACC_NUM",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DAILY_ACC_FIR_DAF_GUID",
                table: "DAILY_ACC_FIR",
                column: "DAF_GUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DAILY_ACC_FIR_DAF_SEVERITY_ID",
                table: "DAILY_ACC_FIR",
                column: "DAF_SEVERITY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DAILY_ACC_FIR_DAF_STATUS_ID",
                table: "DAILY_ACC_FIR",
                column: "DAF_STATUS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DAILY_ACC_FIR_InjuryDetails.InjuryCategoryId",
                table: "DAILY_ACC_FIR",
                column: "InjuryDetails.InjuryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DAILY_ACC_FIR_InjuryDetails.InjuryNatureId",
                table: "DAILY_ACC_FIR",
                column: "InjuryDetails.InjuryNatureId");

            migrationBuilder.CreateIndex(
                name: "IDX_DAF_ACC_DAT",
                table: "DailyAccidentFIRs",
                column: "AccidentDateTime");

            migrationBuilder.CreateIndex(
                name: "IDX_DAF_COM_COD",
                table: "DailyAccidentFIRs",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IDX_DAF_EMP_NUM",
                table: "DailyAccidentFIRs",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_NATURE_INJURY_NATURE_GUID",
                table: "NATURE_INJURY",
                column: "NATURE_GUID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACC_CONTRCT_LST");

            migrationBuilder.DropTable(
                name: "ACC_PERS_INJ");

            migrationBuilder.DropTable(
                name: "AccidentContractors");

            migrationBuilder.DropTable(
                name: "DAILY_ACC_FIR");

            migrationBuilder.DropTable(
                name: "DailyAccidentFIRs");

            migrationBuilder.DropTable(
                name: "DoctorAttendants");

            migrationBuilder.DropTable(
                name: "NaturesOfInjury");

            migrationBuilder.DropTable(
                name: "PersonalInjuries");

            migrationBuilder.DropTable(
                name: "ACCIDENT_SEVERITY");

            migrationBuilder.DropTable(
                name: "ACCIDENT_STATUS");

            migrationBuilder.DropTable(
                name: "CATEGORY_INJURY");

            migrationBuilder.DropTable(
                name: "NATURE_INJURY");
        }
    }
}
