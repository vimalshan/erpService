using LovService.Application.DTOs;
using LovService.Application.Interfaces;
using MediatR;

namespace LovService.Application.Queries.LovType;

public class GetAllLovTypesQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetAllLovTypesQuery, IEnumerable<LovTypeDto>>
{
    public async Task<IEnumerable<LovTypeDto>> Handle(GetAllLovTypesQuery request, CancellationToken ct)
    {
        var types = await uow.LovTypes.GetAllAsync(ct);
        return types.Select(t => new LovTypeDto(t.LovTypeId, t.LovTypeName));
    }
}

public class GetLovTypeByIdQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetLovTypeByIdQuery, LovTypeDto?>
{
    public async Task<LovTypeDto?> Handle(GetLovTypeByIdQuery request, CancellationToken ct)
    {
        var type = await uow.LovTypes.GetByIdAsync(request.LovTypeId, ct);
        return type is null ? null : new LovTypeDto(type.LovTypeId, type.LovTypeName);
    }
}
