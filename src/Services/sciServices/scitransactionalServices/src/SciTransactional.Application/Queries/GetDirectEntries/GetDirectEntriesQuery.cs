using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetDirectEntries;

public sealed record GetDirectEntriesQuery : IRequest<IReadOnlyList<DirectEntryDto>>;

public sealed class GetDirectEntriesQueryHandler(
    IDirectEntryRepository repository, IMapper mapper)
    : IRequestHandler<GetDirectEntriesQuery, IReadOnlyList<DirectEntryDto>>
{
    public async Task<IReadOnlyList<DirectEntryDto>> Handle(
        GetDirectEntriesQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<DirectEntryDto>>(entities);
    }
}
