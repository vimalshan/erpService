using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetAllNorms;

public sealed record GetAllNormsQuery : IRequest<IReadOnlyList<NormsMainDto>>;

public sealed class GetAllNormsQueryHandler(
    INormsRepository repository, IMapper mapper)
    : IRequestHandler<GetAllNormsQuery, IReadOnlyList<NormsMainDto>>
{
    public async Task<IReadOnlyList<NormsMainDto>> Handle(
        GetAllNormsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<NormsMainDto>>(entities);
    }
}
