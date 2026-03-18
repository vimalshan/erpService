using MediatR;
using Masters.Application.DTOs;

namespace Masters.Application.Commands;

// Create LOV Master
public record CreateLovMasterCommand(
    long LovId,
    string LovType,
    string LovName
) : IRequest<LovMasterDto>;

// Update LOV Master
public record UpdateLovMasterCommand(
    long LovId,
    string LovName
) : IRequest<LovMasterDto>;

// Delete LOV Master
public record DeleteLovMasterCommand(
    long LovId
) : IRequest<bool>;
