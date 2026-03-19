using IntegrationService.Application.DTOs;
using MediatR;

namespace IntegrationService.Application.OrganizationUnits.Queries;

public record GetOrganizationUnitByIdQuery(string OuId) : IRequest<OrganizationUnitDto?>;
public record GetAllOrganizationUnitsQuery : IRequest<IEnumerable<OrganizationUnitDto>>;
