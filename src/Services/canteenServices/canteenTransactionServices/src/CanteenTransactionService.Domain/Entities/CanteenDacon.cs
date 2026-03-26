using CanteenTransactionService.Domain.Common;
using CanteenTransactionService.Domain.Events;

namespace CanteenTransactionService.Domain.Entities;

/// <summary>Maps to CANTEEDN_DACON table — canteen daily consumption/contribution records.</summary>
public sealed class CanteenDacon : AggregateRoot
{
    public long? SerialNumber { get; private set; }           // CN_SRL_NUM
    public long? CompanyCode { get; private set; }            // CN_COM_COD
    public long EmployeeSysId { get; private set; }           // CN_SYS_ID
    public string? EmployeeType { get; private set; }         // CN_EMP_TYP (1 char: R=Regular)
    public string? SwipeDate { get; private set; }            // CN_SWP_DAT
    public long? ItemCode { get; private set; }               // CN_ITM_COD
    public string? ItemType { get; private set; }             // CN_ITM_TYP (1 char: M=Meal)
    public decimal? EmployeeContribution { get; private set; }// CN_EE_CON
    public decimal? EmployerContribution { get; private set; }// CN_ER_CON
    public string? CanteenNumber { get; private set; }        // CN_CAN_NUM
    public long? ItemQuantity { get; private set; }           // CN_ITM_QTY
    public long? EntryUser { get; private set; }              // CN_ENT_USR
    public string? EntryDate { get; private set; }            // CN_ENT_DAT
    public string? FlexField1 { get; private set; }           // CN_FLEX1
    public string? GradeCategory { get; private set; }        // CN_GRD_CAT

    private CanteenDacon() { }

    public static CanteenDacon Record(
        long serialNumber,
        long companyCode,
        long employeeSysId,
        string employeeType,
        string swipeDate,
        long itemCode,
        string itemType,
        decimal employeeContribution,
        decimal employerContribution,
        string? canteenNumber,
        long itemQuantity,
        long entryUser,
        string? gradeCategory)
    {
        var entity = new CanteenDacon
        {
            SerialNumber = serialNumber,
            CompanyCode = companyCode,
            EmployeeSysId = employeeSysId,
            EmployeeType = employeeType?.Length > 1 ? employeeType[..1] : employeeType,
            SwipeDate = swipeDate,
            ItemCode = itemCode,
            ItemType = itemType?.Length > 1 ? itemType[..1] : itemType,
            EmployeeContribution = employeeContribution,
            EmployerContribution = employerContribution,
            CanteenNumber = canteenNumber,
            ItemQuantity = itemQuantity,
            EntryUser = entryUser,
            EntryDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            GradeCategory = gradeCategory?.Length > 3 ? gradeCategory[..3] : gradeCategory
        };

        entity.AddDomainEvent(new CanteenTransactionRecordedEvent(
            serialNumber, employeeSysId, itemCode, employeeContribution, employerContribution));
        entity.IncrementVersion();
        return entity;
    }

    public void Cancel()
    {
        AddDomainEvent(new TransactionCancelledEvent(SerialNumber ?? 0, EmployeeSysId));
        IncrementVersion();
    }
}
