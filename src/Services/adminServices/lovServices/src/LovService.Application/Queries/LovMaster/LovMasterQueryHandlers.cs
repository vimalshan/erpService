using LovService.Application.DTOs;
using LovService.Application.Interfaces;
using MediatR;

namespace LovService.Application.Queries.LovMaster;

public class GetAllLovMastersQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetAllLovMastersQuery, IEnumerable<LovMasterDto>>
{
    public async Task<IEnumerable<LovMasterDto>> Handle(GetAllLovMastersQuery request, CancellationToken ct)
    {
        var masters = await uow.LovMasters.GetAllAsync(ct);
        return masters.Select(m => new LovMasterDto(m.LovId, m.LovTypeId, m.LovName, m.LovUpdatedBy, m.LovUpdatedOn));
    }
}

public class GetLovMasterByIdQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetLovMasterByIdQuery, LovMasterDto?>
{
    public async Task<LovMasterDto?> Handle(GetLovMasterByIdQuery request, CancellationToken ct)
    {
        var master = await uow.LovMasters.GetByIdAsync(request.LovId, ct);
        return master is null ? null : new LovMasterDto(master.LovId, master.LovTypeId, master.LovName, master.LovUpdatedBy, master.LovUpdatedOn);
    }
}

public class GetLovMastersByTypeQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetLovMastersByTypeQuery, IEnumerable<LovMasterDto>>
{
    public async Task<IEnumerable<LovMasterDto>> Handle(GetLovMastersByTypeQuery request, CancellationToken ct)
    {
        var masters = await uow.LovMasters.GetByTypeIdAsync(request.LovTypeId, ct);
        return masters.Select(m => new LovMasterDto(m.LovId, m.LovTypeId, m.LovName, m.LovUpdatedBy, m.LovUpdatedOn));
    }
}
