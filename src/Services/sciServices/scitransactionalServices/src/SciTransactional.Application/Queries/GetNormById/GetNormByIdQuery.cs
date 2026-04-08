using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetNormById;

public sealed record GetNormByIdQuery(long NormNo) : IRequest<NormsMainDto?>;

public sealed class GetNormByIdQueryHandler(
    INormsRepository repository, IMapper mapper)
    : IRequestHandler<GetNormByIdQuery, NormsMainDto?>
{
    public async Task<NormsMainDto?> Handle(
        GetNormByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.NormNo, cancellationToken);
        if (entity is null) return null;

        var dto = mapper.Map<NormsMainDto>(entity);
        var details = await repository.GetDetailsByNormNoAsync(request.NormNo, cancellationToken);
        return dto with { Details = mapper.Map<IReadOnlyList<NormsMasterDto>>(details) };
    }
}
