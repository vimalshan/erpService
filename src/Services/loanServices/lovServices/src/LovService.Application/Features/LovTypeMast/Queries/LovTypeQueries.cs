using MediatR;
using LovService.Application.DTOs;

namespace LovService.Application.Features.LovTypeMast.Queries;

public record GetLovTypeByIdQuery(int LovTypeId) : IRequest<LovTypeMastDto?>;
public record GetAllLovTypesQuery(int? OrgId = null) : IRequest<IEnumerable<LovTypeMastDto>>;
