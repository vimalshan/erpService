using MediatR;
using LovService.Application.DTOs;
using LovService.Domain.Interfaces;

namespace LovService.Application.Features.LovMaster.Queries;

public sealed class GetLovMasterByIdQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetLovMasterByIdQuery, LovMasterDto?>
{
    public async Task<LovMasterDto?> Handle(GetLovMasterByIdQuery q, CancellationToken ct)
    {
        var e = await uow.LovMasters.GetByIdAsync(q.LovId, ct);
        return e == null ? null
            : new LovMasterDto(e.LovId, e.LovTypeId, e.LovName,
                e.LovCreatedOn, e.LovCreatedBy, e.LovUpdatedBy, e.LovUpdatedOn);
    }
}

public sealed class GetAllLovMastersQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetAllLovMastersQuery, IEnumerable<LovMasterDto>>
{
    public async Task<IEnumerable<LovMasterDto>> Handle(GetAllLovMastersQuery q, CancellationToken ct)
    {
        var items = q.LovTypeId.HasValue
            ? await uow.LovMasters.GetByTypeIdAsync(q.LovTypeId.Value, ct)
            : await uow.LovMasters.GetAllAsync(ct);

        return items.Select(e => new LovMasterDto(
            e.LovId, e.LovTypeId, e.LovName, e.LovCreatedOn,
            e.LovCreatedBy, e.LovUpdatedBy, e.LovUpdatedOn));
    }
}
