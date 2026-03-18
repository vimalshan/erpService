using SwipeTransactionService.Domain.Common;
using SwipeTransactionService.Domain.Events;
using SwipeTransactionService.Domain.ValueObjects;

namespace SwipeTransactionService.Domain.Entities;

/// <summary>
/// Aggregate root representing an uploaded canteen swipe card transaction.
/// Maps to CANTEEN_SWIPE_CARD_UPLOAD table.
/// </summary>
public sealed class SwipeCardUpload : BaseEntity
{
    public long CompanyCode { get; private set; }
    public string EmployeeNumber { get; private set; } = default!;
    public DateTime SwipeTime { get; private set; }
    public long ItemCode { get; private set; }
    public long ItemQuantity { get; private set; }
    public DateTime BatchDate { get; private set; }
    public long BatchNumber { get; private set; }
    public long SerialNumber { get; private set; }
    public DateTime EntryDate { get; private set; }
    public char CanteenNumber { get; private set; }
    public string GateNumber { get; private set; } = default!;
    public char UpdateStatus { get; private set; }
    public string? FlexField1 { get; private set; }
    public string? FlexField2 { get; private set; }
    public decimal? FlexField3 { get; private set; }
    public DateTime? FlexField4 { get; private set; }
    public string? FlexField5 { get; private set; }

    private SwipeCardUpload() { }

    public static SwipeCardUpload Create(
        long companyCode,
        string employeeNumber,
        DateTime swipeTime,
        long itemCode,
        long itemQuantity,
        long batchNumber,
        long serialNumber,
        char canteenNumber,
        string gateNumber)
    {
        var upload = new SwipeCardUpload
        {
            CompanyCode = companyCode,
            EmployeeNumber = employeeNumber,
            SwipeTime = swipeTime,
            ItemCode = itemCode,
            ItemQuantity = itemQuantity,
            BatchDate = DateTime.UtcNow.Date,
            BatchNumber = batchNumber,
            SerialNumber = serialNumber,
            EntryDate = DateTime.UtcNow,
            CanteenNumber = canteenNumber,
            GateNumber = gateNumber,
            UpdateStatus = 'P'
        };

        upload.RaiseDomainEvent(new SwipeTransactionRecordedEvent(
            companyCode, employeeNumber, swipeTime, itemCode, itemQuantity));

        return upload;
    }

    public void MarkAsProcessed() => UpdateStatus = 'Y';
    public void MarkAsFailed() => UpdateStatus = 'F';
}
