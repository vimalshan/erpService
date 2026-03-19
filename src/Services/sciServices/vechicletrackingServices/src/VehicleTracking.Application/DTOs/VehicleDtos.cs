namespace VehicleTracking.Application.DTOs;

public record VehicleMasterDto
{
    public long SerialNumber { get; init; }
    public string RegNum1 { get; init; } = string.Empty;
    public string? RegNum2 { get; init; }
    public string? RegNum3 { get; init; }
    public string RegNum4 { get; init; } = string.Empty;
    public string FullRegistration { get; init; } = string.Empty;
    public DateTime? RegistrationDate { get; init; }
    public string? UpdatedBy { get; init; }
    public DateTime UpdatedDate { get; init; }
}

public record VehicleStageDto
{
    public long TransactionNumber { get; init; }
    public long TrackingNumber { get; init; }
    public DateTime EntryDate { get; init; }
    public string? StageName { get; init; }
    public char? DecisionFlag { get; init; }
    public char CancelStatus { get; init; }
    public decimal? TimeTaken { get; init; }
    public string? StageComment { get; init; }
}

public record VehicleTransactionDto
{
    public long TrackingNumber { get; init; }
    public long? VehicleSerial { get; init; }
    public string? PartyName { get; init; }
    public DateTime? ReportDate { get; init; }
    public string? PurposeName { get; init; }
    public string? DriverName { get; init; }
    public string? DriverCell { get; init; }
    public decimal? TyreWeight { get; init; }
    public decimal? GrossWeight { get; init; }
    public char? VehicleStatus { get; init; }
    public string? GateName { get; init; }
    public string? ProductCode { get; init; }
    public decimal? ProductQuantity { get; init; }
}

public record VehicleInvoiceDto
{
    public long TrackingNumber { get; init; }
    public long ReferenceNumber { get; init; }
    public long InvoiceSerial { get; init; }
    public string? CustomerCode { get; init; }
    public char? CancelFlag { get; init; }
    public DateTime ModifiedDate { get; init; }
}

public record StageMasterDto
{
    public long StageCode { get; init; }
    public string OptionName { get; init; } = string.Empty;
}

public record PurposeMasterDto
{
    public long PurposeCode { get; init; }
    public string? PurposeName { get; init; }
    public char? TransactionType { get; init; }
    public string? PurposeCategory { get; init; }
    public List<PurposeStageDto> Stages { get; init; } = [];
}

public record PurposeStageDto
{
    public long PurposeCode { get; init; }
    public long StageCode { get; init; }
    public string? StageName { get; init; }
    public long StageSerial { get; init; }
    public decimal? TargetTime { get; init; }
}

public record DecisionFlagDto
{
    public long TrackingNumber { get; init; }
    public long PurposeCode { get; init; }
    public long StageCode { get; init; }
    public char StageDecision { get; init; }
    public char CancelFlag { get; init; }
    public string? Remark { get; init; }
}

public record WeightInfoDto
{
    public long TrackingNumber { get; init; }
    public decimal? TyreWeight { get; init; }
    public decimal? GrossWeight { get; init; }
    public decimal? NetWeight { get; init; }
}
