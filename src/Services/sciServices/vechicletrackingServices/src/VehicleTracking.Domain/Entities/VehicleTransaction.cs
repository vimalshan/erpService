using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class VehicleTransaction : BaseEntity
{
    public long TrackingNumber { get; set; }
    public long? VehicleSerial { get; set; }
    public string? PartyName { get; set; }
    public DateTime? ReportDate { get; set; }
    public long? PurposeCode { get; set; }
    public long? PreviousStage { get; set; }
    public DateTime? PreviousDate { get; set; }
    public decimal? CurrentStage { get; set; }
    public string? GateName { get; set; }
    public string? TransactionNumber { get; set; }
    public string? ProductCode { get; set; }
    public decimal? ProductQuantity { get; set; }
    public string? StageComment { get; set; }
    public string? DriverName { get; set; }
    public string? DriverCell { get; set; }
    public decimal? TyreWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public char? VehicleStatus { get; set; }
    public string? LogEntryUser { get; set; }
    public string? LogEntryNumber { get; set; }
    public DateTime? LogEntryDate { get; set; }
    public long? MainPurpose { get; set; }
    public string? SupplierCode { get; set; }

    public VehicleMaster? Vehicle { get; set; }
    public PurposeMaster? Purpose { get; set; }
}
