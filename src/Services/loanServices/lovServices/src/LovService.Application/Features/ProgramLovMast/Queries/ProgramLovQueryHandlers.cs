using MediatR;
using LovService.Application.DTOs;
using LovService.Domain.Interfaces;

namespace LovService.Application.Features.ProgramLovMast.Queries;

public sealed class GetProgramLovByIdQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetProgramLovByIdQuery, ProgramLovMastDto?>
{
    public async Task<ProgramLovMastDto?> Handle(GetProgramLovByIdQuery q, CancellationToken ct)
    {
        var e = await uow.ProgramLovMasts.GetByIdAsync(q.PrlovTypeCode, q.PrlovCode, ct);
        return e == null ? null
            : new ProgramLovMastDto(e.PrlovTypeCode, e.PrlovCode, e.PrlovName);
    }
}

public sealed class GetAllProgramLovsQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetAllProgramLovsQuery, IEnumerable<ProgramLovMastDto>>
{
    public async Task<IEnumerable<ProgramLovMastDto>> Handle(GetAllProgramLovsQuery q, CancellationToken ct)
    {
        var items = q.PrlovTypeCode != null
            ? await uow.ProgramLovMasts.GetByTypeCodeAsync(q.PrlovTypeCode, ct)
            : await uow.ProgramLovMasts.GetAllAsync(ct);

        return items.Select(e => new ProgramLovMastDto(e.PrlovTypeCode, e.PrlovCode, e.PrlovName));
    }
}
