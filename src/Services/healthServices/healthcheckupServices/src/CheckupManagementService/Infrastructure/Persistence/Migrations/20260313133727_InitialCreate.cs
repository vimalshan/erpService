using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckupManagementService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHECKUP_MAST",
                columns: table => new
                {
                    CompanyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CheckupCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CheckupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Flag = table.Column<string>(type: "char(1)", nullable: true),
                    CheckupMasterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckupType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHECKUP_MAST", x => new { x.CompanyCode, x.CheckupCode });
                });

            migrationBuilder.CreateTable(
                name: "CHKUP_PRE_MAIN",
                columns: table => new
                {
                    EmployeeNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    HealthNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhysicalHandicapDescription = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProposedDesignation = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IdentificationMarks = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FinalRemarks = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FitPhysical = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    FitFinal = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CheckupDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CHKUP_SYMP_MAST",
                columns: table => new
                {
                    SymptomId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SymptomName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SymptomFlag = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHKUP_SYMP_MAST", x => x.SymptomId);
                });

            migrationBuilder.CreateTable(
                name: "FIELD_TYP_MAST",
                columns: table => new
                {
                    FieldTypeCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FieldTypeName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ControlSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIELD_TYP_MAST", x => x.FieldTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "HEALTH_COUNTER",
                columns: table => new
                {
                    CompanyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CounterCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CounterValue = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEALTH_COUNTER", x => new { x.CompanyCode, x.CounterCode });
                });

            migrationBuilder.CreateTable(
                name: "HEALTH_DYN_DET",
                columns: table => new
                {
                    HealthNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CheckupCode = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ControlSourceId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DynamicValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SystemDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "HLTH_CHKCARD_SUB",
                columns: table => new
                {
                    HealthNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SymptomId = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FlagYesNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SymptomValue = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EmployeeNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "HLTH_CHKUP_CARD",
                columns: table => new
                {
                    HealthNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmployeeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PersonalDetails = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ScreeningDetails = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AdviceRemark1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DoctorDate1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdviceFollowup1 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AdviceRemark2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DoctorDate2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdviceFollowup2 = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckupMasterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CardStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TEST_MAST",
                columns: table => new
                {
                    TestCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CheckboxFlag = table.Column<string>(type: "char(1)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloseFlag = table.Column<string>(type: "char(1)", nullable: true),
                    RangeValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TestGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TEST_MAST", x => x.TestCode);
                });

            migrationBuilder.CreateTable(
                name: "HEALTH_MAIN",
                columns: table => new
                {
                    HealthNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CheckupDate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntryEmployeeNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CheckupCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TextField2 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TextField3 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TextField4 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TextField5 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HealthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckupMasterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BMI = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BloodPressure = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HeartRate = table.Column<int>(type: "int", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EyeVision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorBlindness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hearing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LungsXRay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ECG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallFitness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalClearance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEALTH_MAIN", x => x.HealthNumber);
                    table.ForeignKey(
                        name: "FK_HEALTH_MAIN_CHECKUP_MAST_CompanyCode_CheckupCode",
                        columns: x => new { x.CompanyCode, x.CheckupCode },
                        principalTable: "CHECKUP_MAST",
                        principalColumns: new[] { "CompanyCode", "CheckupCode" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CHKUP_PFI_HIST",
                columns: table => new
                {
                    HealthNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmployeeNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SymptomId = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YesNoFlag = table.Column<string>(type: "char(1)", nullable: true),
                    ImmunizationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TestValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHKUP_PFI_HIST", x => new { x.HealthNumber, x.EmployeeNumber, x.SymptomId });
                    table.ForeignKey(
                        name: "FK_CHKUP_PFI_HIST_CHKUP_SYMP_MAST_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "CHKUP_SYMP_MAST",
                        principalColumn: "SymptomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHKUP_OTHERS",
                columns: table => new
                {
                    CompanyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CheckupCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherSerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FieldLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MandatoryFlag = table.Column<string>(type: "char(1)", nullable: true),
                    FieldTypeCode = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FieldTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckupOthersId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckupMasterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicineAllergy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PastSurgery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentMedicines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LifestyleHabits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherComments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHKUP_OTHERS", x => new { x.CompanyCode, x.CheckupCode, x.OtherSerialNumber });
                    table.ForeignKey(
                        name: "FK_CHKUP_OTHERS_CHECKUP_MAST_CompanyCode_CheckupCode",
                        columns: x => new { x.CompanyCode, x.CheckupCode },
                        principalTable: "CHECKUP_MAST",
                        principalColumns: new[] { "CompanyCode", "CheckupCode" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHKUP_OTHERS_FIELD_TYP_MAST_FieldTypeCode",
                        column: x => x.FieldTypeCode,
                        principalTable: "FIELD_TYP_MAST",
                        principalColumn: "FieldTypeCode",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CHKUP_TEST",
                columns: table => new
                {
                    SerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CheckupCode = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TestCode = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrderNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CheckboxFlag = table.Column<string>(type: "char(1)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloseFlag = table.Column<string>(type: "char(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHKUP_TEST", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_CHKUP_TEST_CHECKUP_MAST_CompanyCode_CheckupCode",
                        columns: x => new { x.CompanyCode, x.CheckupCode },
                        principalTable: "CHECKUP_MAST",
                        principalColumns: new[] { "CompanyCode", "CheckupCode" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHKUP_TEST_TEST_MAST_TestCode",
                        column: x => x.TestCode,
                        principalTable: "TEST_MAST",
                        principalColumn: "TestCode",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HEALTH_ENTRY_LOV",
                columns: table => new
                {
                    TestCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ListOfValueText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEALTH_ENTRY_LOV", x => new { x.TestCode, x.ListOfValueText });
                    table.ForeignKey(
                        name: "FK_HEALTH_ENTRY_LOV_TEST_MAST_TestCode",
                        column: x => x.TestCode,
                        principalTable: "TEST_MAST",
                        principalColumn: "TestCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HEALTH_MINMAX_VAL",
                columns: table => new
                {
                    TestCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SingleValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinText = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaxText = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LovText = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEALTH_MINMAX_VAL", x => new { x.TestCode, x.TypeCode, x.UnitCode });
                    table.ForeignKey(
                        name: "FK_HEALTH_MINMAX_VAL_TEST_MAST_TestCode",
                        column: x => x.TestCode,
                        principalTable: "TEST_MAST",
                        principalColumn: "TestCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HEALTH_SUB",
                columns: table => new
                {
                    HealthNumber = table.Column<int>(type: "int", nullable: false),
                    TestCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TestRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidationFlag = table.Column<string>(type: "char(1)", nullable: true),
                    TextField2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TextField3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TextField4 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TextField5 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DoctorRemarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthSubId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalRange = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HEALTH_SUB", x => new { x.HealthNumber, x.TestCode });
                    table.ForeignKey(
                        name: "FK_HEALTH_SUB_HEALTH_MAIN_HealthNumber",
                        column: x => x.HealthNumber,
                        principalTable: "HEALTH_MAIN",
                        principalColumn: "HealthNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHKUP_OTHERS_LOV",
                columns: table => new
                {
                    ListOfValueSerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CheckupCode = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherSerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ListOfValueDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CheckupOtherCompanyCode = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    CheckupOtherCheckupCode = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CheckupOtherOtherSerialNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHKUP_OTHERS_LOV", x => x.ListOfValueSerialNumber);
                    table.ForeignKey(
                        name: "FK_CHKUP_OTHERS_LOV_CHKUP_OTHERS_CheckupOtherCompanyCode_CheckupOtherCheckupCode_CheckupOtherOtherSerialNumber",
                        columns: x => new { x.CheckupOtherCompanyCode, x.CheckupOtherCheckupCode, x.CheckupOtherOtherSerialNumber },
                        principalTable: "CHKUP_OTHERS",
                        principalColumns: new[] { "CompanyCode", "CheckupCode", "OtherSerialNumber" });
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CHECKUP_MAST_CM_CHK_COD",
                table: "CHECKUP_MAST",
                column: "CheckupCode");

            migrationBuilder.CreateIndex(
                name: "IX_CHKUP_OTHERS_FieldTypeCode",
                table: "CHKUP_OTHERS",
                column: "FieldTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_CHKUP_OTHERS_LOV_CheckupOtherCompanyCode_CheckupOtherCheckupCode_CheckupOtherOtherSerialNumber",
                table: "CHKUP_OTHERS_LOV",
                columns: new[] { "CheckupOtherCompanyCode", "CheckupOtherCheckupCode", "CheckupOtherOtherSerialNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_CHKUP_PFI_HIST_SymptomId",
                table: "CHKUP_PFI_HIST",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_CHKUP_TEST_CompanyCode_CheckupCode",
                table: "CHKUP_TEST",
                columns: new[] { "CompanyCode", "CheckupCode" });

            migrationBuilder.CreateIndex(
                name: "IX_CHKUP_TEST_TestCode",
                table: "CHKUP_TEST",
                column: "TestCode");

            migrationBuilder.CreateIndex(
                name: "IDX_HEALTH_MAIN_HM_EMP_NUM",
                table: "HEALTH_MAIN",
                column: "EmployeeNumber");

            migrationBuilder.CreateIndex(
                name: "IDX_HEALTH_MAIN_HM_HLT_NUM",
                table: "HEALTH_MAIN",
                column: "HealthNumber");

            migrationBuilder.CreateIndex(
                name: "IX_HEALTH_MAIN_CompanyCode_CheckupCode",
                table: "HEALTH_MAIN",
                columns: new[] { "CompanyCode", "CheckupCode" });

            migrationBuilder.CreateIndex(
                name: "IDX_HEALTH_SUB_HM_HLT_NUM",
                table: "HEALTH_SUB",
                column: "HealthNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHKUP_OTHERS_LOV");

            migrationBuilder.DropTable(
                name: "CHKUP_PFI_HIST");

            migrationBuilder.DropTable(
                name: "CHKUP_PRE_MAIN");

            migrationBuilder.DropTable(
                name: "CHKUP_TEST");

            migrationBuilder.DropTable(
                name: "HEALTH_COUNTER");

            migrationBuilder.DropTable(
                name: "HEALTH_DYN_DET");

            migrationBuilder.DropTable(
                name: "HEALTH_ENTRY_LOV");

            migrationBuilder.DropTable(
                name: "HEALTH_MINMAX_VAL");

            migrationBuilder.DropTable(
                name: "HEALTH_SUB");

            migrationBuilder.DropTable(
                name: "HLTH_CHKCARD_SUB");

            migrationBuilder.DropTable(
                name: "HLTH_CHKUP_CARD");

            migrationBuilder.DropTable(
                name: "CHKUP_OTHERS");

            migrationBuilder.DropTable(
                name: "CHKUP_SYMP_MAST");

            migrationBuilder.DropTable(
                name: "TEST_MAST");

            migrationBuilder.DropTable(
                name: "HEALTH_MAIN");

            migrationBuilder.DropTable(
                name: "FIELD_TYP_MAST");

            migrationBuilder.DropTable(
                name: "CHECKUP_MAST");
        }
    }
}
