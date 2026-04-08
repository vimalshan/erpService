using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetAllAdvanceLicenses;

public sealed record GetAllAdvanceLicensesQuery : IRequest<IReadOnlyList<AdvanceLicenseDto>>;

public sealed class GetAllAdvanceLicensesQueryHandler(
    IAdvanceLicenseRepository repository, IMapper mapper)
    : IRequestHandler<GetAllAdvanceLicensesQuery, IReadOnlyList<AdvanceLicenseDto>>
{
    public async Task<IReadOnlyList<AdvanceLicenseDto>> Handle(
        GetAllAdvanceLicensesQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<AdvanceLicenseDto>>(entities);
    }
}
