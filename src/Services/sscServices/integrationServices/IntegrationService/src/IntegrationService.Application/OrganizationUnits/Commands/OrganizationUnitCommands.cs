using IntegrationService.Application.DTOs;
using MediatR;

namespace IntegrationService.Application.OrganizationUnits.Commands;

public record CreateOrganizationUnitCommand(string OuId, string OuName, string BuId) : IRequest<OrganizationUnitDto>;
public record UpdateOrganizationUnitCommand(string OuId, string OuName, string BuId) : IRequest<OrganizationUnitDto>;
public record DeleteOrganizationUnitCommand(string OuId) : IRequest<bool>;
