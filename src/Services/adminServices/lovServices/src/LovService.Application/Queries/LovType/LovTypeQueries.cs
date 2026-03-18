using LovService.Application.DTOs;
using MediatR;

namespace LovService.Application.Queries.LovType;

public record GetAllLovTypesQuery : IRequest<IEnumerable<LovTypeDto>>;

public record GetLovTypeByIdQuery(long LovTypeId) : IRequest<LovTypeDto?>;
