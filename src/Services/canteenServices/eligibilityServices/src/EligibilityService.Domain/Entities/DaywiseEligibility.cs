using EligibilityService.Domain.Common;

namespace EligibilityService.Domain.Entities;

/// <summary>Maps to CANTEEN_DAYWISE_ELIGIBILITY table.</summary>
public class DaywiseEligibility : BaseEntity
{
    public long SerialNumber { get; private set; }       // CN_SRL_NUM (PK)
    public long CompanyCode { get; private set; }        // CN_COM_COD
    public long EmployeeSysId { get; private set; }      // CN_SYS_ID
    public DateTime? AttendanceDate { get; private set; }// CN_ATT_DAT
    public long? ProcessNumber { get; private set; }     // CN_PRC_NUM
    public string? ShiftCode { get; private set; }       // CN_SFT_COD
    public long? ItemCode { get; private set; }          // CN_ITM_COD
    public int? ShiftQuantity { get; private set; }      // CN_SFT_QTY
    public int? BeforeShiftQty { get; private set; }     // CN_SFT_BEF
    public int? AfterShiftQty { get; private set; }      // CN_SFT_AFT
    public long? EnteredUser { get; private set; }       // CN_ENT_USR
    public DateTime? EnteredOn { get; private set; }     // CN_ENT_DAT
    public string? FlexField1 { get; private set; }      // CN_FLEX1
    public string? GradeType { get; private set; }       // CN_GRD_TYP

    private DaywiseEligibility() { }

    public static DaywiseEligibility Create(
        long serialNumber,
        long companyCode,
        long employeeSysId,
        DateTime? attendanceDate,
        long? processNumber,
        string? shiftCode,
        long? itemCode,
        int? shiftQuantity,
        int? beforeShiftQty,
        int? afterShiftQty,
        long? enteredUser,
        string? flexField1,
        string? gradeType)
    {
        return new DaywiseEligibility
        {
            SerialNumber = serialNumber,
            CompanyCode = companyCode,
            EmployeeSysId = employeeSysId,
            AttendanceDate = attendanceDate,
            ProcessNumber = processNumber,
            ShiftCode = shiftCode,
            ItemCode = itemCode,
            ShiftQuantity = shiftQuantity,
            BeforeShiftQty = beforeShiftQty,
            AfterShiftQty = afterShiftQty,
            EnteredUser = enteredUser,
            EnteredOn = DateTime.UtcNow,
            FlexField1 = flexField1,
            GradeType = gradeType
        };
    }
}
