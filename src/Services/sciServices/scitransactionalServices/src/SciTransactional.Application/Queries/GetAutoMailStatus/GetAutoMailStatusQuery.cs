using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetAutoMailStatus;

public sealed record GetAutoMailStatusQuery : IRequest<IReadOnlyList<AutoMailStatusDto>>;

public sealed class GetAutoMailStatusQueryHandler(
    IAutoMailRepository repository, IMapper mapper)
    : IRequestHandler<GetAutoMailStatusQuery, IReadOnlyList<AutoMailStatusDto>>
{
    public async Task<IReadOnlyList<AutoMailStatusDto>> Handle(
        GetAutoMailStatusQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllStatusAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<AutoMailStatusDto>>(entities);
    }
}
