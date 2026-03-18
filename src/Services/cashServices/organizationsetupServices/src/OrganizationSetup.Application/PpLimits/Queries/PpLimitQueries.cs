using MediatR;
using OrganizationSetup.Application.DTOs;

namespace OrganizationSetup.Application.PpLimits.Queries;

public sealed record GetPpLimitsByOrgAndYearQuery(long OrgId, int FinYear) : IRequest<IEnumerable<PpLimitDto>>;
public sealed record GetPpLimitByIdQuery(long LimitId) : IRequest<PpLimitDto?>;
