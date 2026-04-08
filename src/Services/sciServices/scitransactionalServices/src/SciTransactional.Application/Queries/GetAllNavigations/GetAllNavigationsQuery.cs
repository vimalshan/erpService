using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetAllNavigations;

public sealed record GetAllNavigationsQuery : IRequest<IReadOnlyList<NavigationDto>>;

public sealed class GetAllNavigationsQueryHandler(
    INavigationRepository repository, IMapper mapper)
    : IRequestHandler<GetAllNavigationsQuery, IReadOnlyList<NavigationDto>>
{
    public async Task<IReadOnlyList<NavigationDto>> Handle(
        GetAllNavigationsQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<NavigationDto>>(entities);
    }
}
