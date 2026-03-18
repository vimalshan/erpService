using MediatR;
using Masters.Application.DTOs;

namespace Masters.Application.Queries;

// Get LOV Master by ID
public record GetLovMasterByIdQuery(
    long LovId
) : IRequest<LovMasterDto?>;

// Get all LOV Masters
public record GetAllLovMastersQuery() : IRequest<IEnumerable<LovMasterDto>>;

// Get LOV Masters by Type
public record GetLovMastersByTypeQuery(
    string LovType
) : IRequest<IEnumerable<LovMasterDto>>;
