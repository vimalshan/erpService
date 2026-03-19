using AutoMapper;
using IntegrationService.Application.DTOs;
using IntegrationService.Domain.Interfaces;
using MediatR;

namespace IntegrationService.Application.OrganizationUnits.Queries;

public class GetOrganizationUnitByIdHandler(
    IOrganizationUnitRepository repository,
    IMapper mapper) : IRequestHandler<GetOrganizationUnitByIdQuery, OrganizationUnitDto?>
{
    public async Task<OrganizationUnitDto?> Handle(GetOrganizationUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var ou = await repository.GetByIdAsync(request.OuId, cancellationToken);
        return ou is null ? null : mapper.Map<OrganizationUnitDto>(ou);
    }
}

public class GetAllOrganizationUnitsHandler(
    IOrganizationUnitRepository repository,
    IMapper mapper) : IRequestHandler<GetAllOrganizationUnitsQuery, IEnumerable<OrganizationUnitDto>>
{
    public async Task<IEnumerable<OrganizationUnitDto>> Handle(GetAllOrganizationUnitsQuery request, CancellationToken cancellationToken)
    {
        var ous = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IEnumerable<OrganizationUnitDto>>(ous);
    }
}
