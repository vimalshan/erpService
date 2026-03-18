using MediatR;
using Masters.Application.DTOs;

namespace Masters.Application.Commands;

// Create LOV Type Master
public record CreateLovTypeMasterCommand(
    string LovTypeCode,
    string LovTypeName
) : IRequest<LovTypeMasterDto>;

// Update LOV Type Master
public record UpdateLovTypeMasterCommand(
    string LovTypeCode,
    string LovTypeName
) : IRequest<LovTypeMasterDto>;

// Delete LOV Type Master
public record DeleteLovTypeMasterCommand(
    string LovTypeCode
) : IRequest<bool>;
