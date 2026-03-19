using MediatR;
using VehicleTracking.Application.DTOs;

namespace VehicleTracking.Application.Vehicles.Commands;

public record RegisterVehicleCommand : IRequest<VehicleMasterDto>
{
    public string RegNum1 { get; init; } = string.Empty;
    public string? RegNum2 { get; init; }
    public string? RegNum3 { get; init; }
    public string RegNum4 { get; init; } = string.Empty;
    public DateTime? RegistrationDate { get; init; }
    public string UpdatedBy { get; init; } = string.Empty;
    public long UpdatedByNum { get; init; }
}

public record UpdateVehicleStageCommand : IRequest<VehicleStageDto>
{
    public long TrackingNumber { get; init; }
    public long VehicleTracker { get; init; }
    public long StageCode { get; init; }
    public char? StageDecision { get; init; }
    public string EnteredBy { get; init; } = string.Empty;
    public long EnteredByNum { get; init; }
}

public record CreateVehicleTransactionCommand : IRequest<VehicleTransactionDto>
{
    public long? VehicleSerial { get; init; }
    public string? PartyName { get; init; }
    public long? PurposeCode { get; init; }
    public string? GateName { get; init; }
    public string? ProductCode { get; init; }
    public decimal? ProductQuantity { get; init; }
    public string? DriverName { get; init; }
    public string? DriverCell { get; init; }
    public decimal? TyreWeight { get; init; }
    public decimal? GrossWeight { get; init; }
    public long? MainPurpose { get; init; }
    public string? SupplierCode { get; init; }
    public string LogEntryUser { get; init; } = string.Empty;
}

public record CreateVehicleInvoiceCommand : IRequest<VehicleInvoiceDto>
{
    public long TrackingNumber { get; init; }
    public long ReferenceNumber { get; init; }
    public long? OriginalInvoice { get; init; }
    public long ChainInvoice { get; init; }
    public string? CustomerCode { get; init; }
    public string ModifiedUser { get; init; } = string.Empty;
    public long ModifiedNumber { get; init; }
}

public record MakeDecisionCommand : IRequest<DecisionFlagDto>
{
    public long TrackingNumber { get; init; }
    public long PurposeCode { get; init; }
    public long StageCode { get; init; }
    public char StageDecision { get; init; }
    public string? Remark { get; init; }
}

public record UpdateWeightInfoCommand : IRequest<WeightInfoDto>
{
    public long TrackingNumber { get; init; }
    public decimal? TyreWeight { get; init; }
    public decimal? GrossWeight { get; init; }
    public decimal? NetWeight { get; init; }
}

public record UpdateVehicleMasterCommand : IRequest<VehicleMasterDto>
{
    public long SerialNumber { get; init; }
    public string RegNum1 { get; init; } = string.Empty;
    public string? RegNum2 { get; init; }
    public string? RegNum3 { get; init; }
    public string RegNum4 { get; init; } = string.Empty;
    public DateTime? RegistrationDate { get; init; }
    public string UpdatedBy { get; init; } = string.Empty;
    public long UpdatedByNum { get; init; }
}

public record CloseVehicleTransactionCommand : IRequest<VehicleTransactionDto>
{
    public long TrackingNumber { get; init; }
    public string LogEntryUser { get; init; } = string.Empty;
}
