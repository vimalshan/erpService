using AutoMapper;
using MediatR;
using VehicleTracking.Application.DTOs;
using VehicleTracking.Application.Vehicles.Queries;
using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Application.Vehicles.Handlers;

public class GetVehicleByIdHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetVehicleByIdQuery, VehicleMasterDto?>
{
    public async Task<VehicleMasterDto?> Handle(GetVehicleByIdQuery request, CancellationToken ct)
    {
        var vehicle = await uow.VehicleMasters.GetByIdAsync(request.SerialNumber, ct);
        return vehicle is null ? null : mapper.Map<VehicleMasterDto>(vehicle);
    }
}

public class GetAllVehiclesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleMasterDto>>
{
    public async Task<IEnumerable<VehicleMasterDto>> Handle(GetAllVehiclesQuery request, CancellationToken ct)
    {
        var vehicles = await uow.VehicleMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<VehicleMasterDto>>(vehicles);
    }
}

public class GetVehicleStagesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetVehicleStagesQuery, IEnumerable<VehicleStageDto>>
{
    public async Task<IEnumerable<VehicleStageDto>> Handle(GetVehicleStagesQuery request, CancellationToken ct)
    {
        var stages = await uow.VehicleStages.GetByTrackingNumberAsync(request.TrackingNumber, ct);
        return mapper.Map<IEnumerable<VehicleStageDto>>(stages);
    }
}

public class GetVehicleTransactionsHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetVehicleTransactionsQuery, IEnumerable<VehicleTransactionDto>>
{
    public async Task<IEnumerable<VehicleTransactionDto>> Handle(GetVehicleTransactionsQuery request, CancellationToken ct)
    {
        var transactions = await uow.VehicleTransactions.GetByTrackingNumberAsync(request.TrackingNumber, ct);
        return mapper.Map<IEnumerable<VehicleTransactionDto>>(transactions);
    }
}

public class GetActiveTransactionsHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetActiveTransactionsQuery, IEnumerable<VehicleTransactionDto>>
{
    public async Task<IEnumerable<VehicleTransactionDto>> Handle(GetActiveTransactionsQuery request, CancellationToken ct)
    {
        var transactions = await uow.VehicleTransactions.GetActiveTransactionsAsync(ct);
        return mapper.Map<IEnumerable<VehicleTransactionDto>>(transactions);
    }
}

public class GetVehicleInvoicesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetVehicleInvoicesQuery, IEnumerable<VehicleInvoiceDto>>
{
    public async Task<IEnumerable<VehicleInvoiceDto>> Handle(GetVehicleInvoicesQuery request, CancellationToken ct)
    {
        var invoices = await uow.VehicleInvoices.GetByTrackingNumberAsync(request.TrackingNumber, ct);
        return mapper.Map<IEnumerable<VehicleInvoiceDto>>(invoices);
    }
}

public class GetAllStagesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllStagesQuery, IEnumerable<StageMasterDto>>
{
    public async Task<IEnumerable<StageMasterDto>> Handle(GetAllStagesQuery request, CancellationToken ct)
    {
        var stages = await uow.StageMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<StageMasterDto>>(stages);
    }
}

public class GetAllPurposesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetAllPurposesQuery, IEnumerable<PurposeMasterDto>>
{
    public async Task<IEnumerable<PurposeMasterDto>> Handle(GetAllPurposesQuery request, CancellationToken ct)
    {
        var purposes = await uow.PurposeMasters.GetAllAsync(ct);
        return mapper.Map<IEnumerable<PurposeMasterDto>>(purposes);
    }
}

public class GetPurposeWithStagesHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetPurposeWithStagesQuery, PurposeMasterDto?>
{
    public async Task<PurposeMasterDto?> Handle(GetPurposeWithStagesQuery request, CancellationToken ct)
    {
        var purpose = await uow.PurposeMasters.GetWithStagesAsync(request.PurposeCode, ct);
        return purpose is null ? null : mapper.Map<PurposeMasterDto>(purpose);
    }
}

public class GetDecisionFlagsHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetDecisionFlagsQuery, IEnumerable<DecisionFlagDto>>
{
    public async Task<IEnumerable<DecisionFlagDto>> Handle(GetDecisionFlagsQuery request, CancellationToken ct)
    {
        var flags = await uow.DecisionFlags.GetByTrackingNumberAsync(request.TrackingNumber, ct);
        return mapper.Map<IEnumerable<DecisionFlagDto>>(flags);
    }
}

public class GetWeightInfoHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<GetWeightInfoQuery, WeightInfoDto?>
{
    public async Task<WeightInfoDto?> Handle(GetWeightInfoQuery request, CancellationToken ct)
    {
        var weight = await uow.WeightInfos.GetByTrackingNumberAsync(request.TrackingNumber, ct);
        return weight is null ? null : mapper.Map<WeightInfoDto>(weight);
    }
}
