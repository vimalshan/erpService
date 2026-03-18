using MediatR;
using ReferenceService.Application.DTOs;

namespace ReferenceService.Application.Queries.LovValue;

/// <summary>
/// Query to get a LOV Value by ID.
/// </summary>
public record GetLovValueByIdQuery(int Id) : IRequest<LovValueDto?>;

/// <summary>
/// Query to get LOV Value by type ID and code.
/// </summary>
public record GetLovValueByCodeQuery(int TypeId, string Code) : IRequest<LovValueDto?>;
