using MediatR;
using LovService.Application.DTOs;
using LovService.Domain.Interfaces;

namespace LovService.Application.Features.LovTypeMast.Queries;

public sealed class GetLovTypeByIdQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetLovTypeByIdQuery, LovTypeMastDto?>
{
    public async Task<LovTypeMastDto?> Handle(GetLovTypeByIdQuery q, CancellationToken ct)
    {
        var e = await uow.LovTypeMasts.GetByIdAsync(q.LovTypeId, ct);
        return e == null ? null
            : new LovTypeMastDto(e.LovTypeId, e.LovTypeName, e.LovCategory.Value.ToString(), e.LovOrgId);
    }
}

public sealed class GetAllLovTypesQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetAllLovTypesQuery, IEnumerable<LovTypeMastDto>>
{
    public async Task<IEnumerable<LovTypeMastDto>> Handle(GetAllLovTypesQuery q, CancellationToken ct)
    {
        var items = q.OrgId.HasValue
            ? await uow.LovTypeMasts.GetByOrgIdAsync(q.OrgId.Value, ct)
            : await uow.LovTypeMasts.GetAllAsync(ct);

        return items.Select(e => new LovTypeMastDto(e.LovTypeId, e.LovTypeName, e.LovCategory.Value.ToString(), e.LovOrgId));
    }
}
