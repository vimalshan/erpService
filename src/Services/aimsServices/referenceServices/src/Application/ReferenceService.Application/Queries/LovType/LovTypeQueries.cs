using MediatR;
using ReferenceService.Application.DTOs;

namespace ReferenceService.Application.Queries.LovType;

/// <summary>
/// Query to get all LOV Types with their values.
/// </summary>
public record GetAllLovTypesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResponse<LovTypeDto>>;

/// <summary>
/// Query to get a LOV Type by ID.
/// </summary>
public record GetLovTypeByIdQuery(int Id) : IRequest<LovTypeDto?>;

/// <summary>
/// Query to get LOV Type by name.
/// </summary>
public record GetLovTypeByNameQuery(string TypeName) : IRequest<LovTypeDto?>;

/// <summary>
/// Query to get all LOV Values for a specific type.
/// </summary>
public record GetLovValuesByTypeQuery(int TypeId) : IRequest<List<LovValueDto>>;
