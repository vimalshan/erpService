using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetNavigationById;

public sealed record GetNavigationByIdQuery(long RequestNum) : IRequest<NavigationDto?>;

public sealed class GetNavigationByIdQueryHandler(
    INavigationRepository repository, IMapper mapper)
    : IRequestHandler<GetNavigationByIdQuery, NavigationDto?>
{
    public async Task<NavigationDto?> Handle(
        GetNavigationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.RequestNum, cancellationToken);
        return entity is null ? null : mapper.Map<NavigationDto>(entity);
    }
}
