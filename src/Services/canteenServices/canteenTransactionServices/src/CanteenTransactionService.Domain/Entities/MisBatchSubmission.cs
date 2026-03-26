using CanteenTransactionService.Domain.Common;
using CanteenTransactionService.Domain.Events;

namespace CanteenTransactionService.Domain.Entities;

/// <summary>Maps to CANTEEN_MIS_SBT table — MIS batch submission records.</summary>
public sealed class MisBatchSubmission : AggregateRoot
{
    public long CompanyCode { get; private set; }      // CN_COM_COD
    public string EmployeeNumber { get; private set; } = default!; // CN_EMP_NUM
    public DateTime SwipeTime { get; private set; }    // CN_SWP_TIM
    public long ItemCode { get; private set; }         // CN_ITM_COD
    public long ItemQuantity { get; private set; }     // CN_ITM_QTN
    public DateTime BatchDate { get; private set; }    // CN_BAT_DAT
    public long BatchNumber { get; private set; }      // CN_BAT_NUM
    public long SerialNumber { get; private set; }     // CN_SRL_NUM
    public DateTime EntryDate { get; private set; }    // CN_ENT_DAT
    public string CanteenNumber { get; private set; } = default!; // CN_CAN_NUM (1 char)
    public string GateNumber { get; private set; } = default!;    // CN_GAT_NUM (3 char)
    public string UpdateStatus { get; private set; } = default!;  // CN_UPD_STS (1 char)
    public string? FlexField1 { get; private set; }    // CN_FLX_FLD1
    public string? FlexField2 { get; private set; }    // CN_FLX_FLD2
    public decimal? FlexField3 { get; private set; }   // CN_FLX_FLD3
    public DateTime? FlexField4 { get; private set; }  // CN_FLX_FLD4
    public string? FlexField5 { get; private set; }    // CN_FLX_FLD5

    private MisBatchSubmission() { }

    public static MisBatchSubmission Create(
        long companyCode,
        string employeeNumber,
        DateTime swipeTime,
        long itemCode,
        long itemQuantity,
        DateTime batchDate,
        long batchNumber,
        long serialNumber,
        string canteenNumber,
        string gateNumber)
    {
        var entity = new MisBatchSubmission
        {
            CompanyCode = companyCode,
            EmployeeNumber = employeeNumber.Length > 20 ? employeeNumber[..20] : employeeNumber,
            SwipeTime = swipeTime,
            ItemCode = itemCode,
            ItemQuantity = itemQuantity,
            BatchDate = batchDate,
            BatchNumber = batchNumber,
            SerialNumber = serialNumber,
            EntryDate = DateTime.UtcNow,
            CanteenNumber = canteenNumber.Length > 1 ? canteenNumber[..1] : canteenNumber,
            GateNumber = gateNumber.Length > 3 ? gateNumber[..3] : gateNumber,
            UpdateStatus = "P" // Pending
        };

        entity.AddDomainEvent(new MisBatchSubmittedEvent(batchNumber, companyCode, employeeNumber));
        entity.IncrementVersion();
        return entity;
    }

    public void MarkAsProcessed()
    {
        UpdateStatus = "Y";
        UpdatedAt = DateTime.UtcNow;
        IncrementVersion();
    }

    public void MarkAsFailed()
    {
        UpdateStatus = "F";
        UpdatedAt = DateTime.UtcNow;
        IncrementVersion();
    }
}
