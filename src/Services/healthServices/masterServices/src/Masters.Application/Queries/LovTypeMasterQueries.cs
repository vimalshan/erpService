using MediatR;
using Masters.Application.DTOs;

namespace Masters.Application.Queries;

// Get LOV Type Master by ID
public record GetLovTypeMasterByIdQuery(
    string LovTypeCode
) : IRequest<LovTypeMasterDto?>;

// Get all LOV Type Masters
public record GetAllLovTypeMastersQuery() : IRequest<IEnumerable<LovTypeMasterDto>>;
