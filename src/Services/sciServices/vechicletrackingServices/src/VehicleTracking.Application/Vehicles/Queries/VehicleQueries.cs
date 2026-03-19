using MediatR;
using VehicleTracking.Application.DTOs;

namespace VehicleTracking.Application.Vehicles.Queries;

public record GetVehicleByIdQuery(long SerialNumber) : IRequest<VehicleMasterDto?>;
public record GetAllVehiclesQuery : IRequest<IEnumerable<VehicleMasterDto>>;
public record GetVehicleStagesQuery(long TrackingNumber) : IRequest<IEnumerable<VehicleStageDto>>;
public record GetVehicleTransactionsQuery(long TrackingNumber) : IRequest<IEnumerable<VehicleTransactionDto>>;
public record GetActiveTransactionsQuery : IRequest<IEnumerable<VehicleTransactionDto>>;
public record GetVehicleInvoicesQuery(long TrackingNumber) : IRequest<IEnumerable<VehicleInvoiceDto>>;
public record GetAllStagesQuery : IRequest<IEnumerable<StageMasterDto>>;
public record GetAllPurposesQuery : IRequest<IEnumerable<PurposeMasterDto>>;
public record GetPurposeWithStagesQuery(long PurposeCode) : IRequest<PurposeMasterDto?>;
public record GetDecisionFlagsQuery(long TrackingNumber) : IRequest<IEnumerable<DecisionFlagDto>>;
public record GetWeightInfoQuery(long TrackingNumber) : IRequest<WeightInfoDto?>;
