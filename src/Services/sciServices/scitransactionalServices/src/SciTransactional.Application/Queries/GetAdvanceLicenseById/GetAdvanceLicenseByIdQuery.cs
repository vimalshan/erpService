using AutoMapper;
using MediatR;
using SciTransactional.Application.DTOs;
using SciTransactional.Domain.Interfaces;

namespace SciTransactional.Application.Queries.GetAdvanceLicenseById;

public sealed record GetAdvanceLicenseByIdQuery(long LicenseId) : IRequest<AdvanceLicenseDto?>;

public sealed class GetAdvanceLicenseByIdQueryHandler(
    IAdvanceLicenseRepository repository, IMapper mapper)
    : IRequestHandler<GetAdvanceLicenseByIdQuery, AdvanceLicenseDto?>
{
    public async Task<AdvanceLicenseDto?> Handle(
        GetAdvanceLicenseByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.LicenseId, cancellationToken);
        return entity is null ? null : mapper.Map<AdvanceLicenseDto>(entity);
    }
}
