using MediatR;

namespace LovService.Application.Commands.LovType;

public record CreateLovTypeCommand(long LovTypeId, string LovTypeName) : IRequest<long>;

public record UpdateLovTypeCommand(long LovTypeId, string LovTypeName) : IRequest<bool>;

public record DeleteLovTypeCommand(long LovTypeId) : IRequest<bool>;
