using MediatR;

namespace LovService.Application.Commands.LovMaster;

public record CreateLovMasterCommand(long LovId, long LovTypeId, string LovName, long UpdatedBy) : IRequest<long>;

public record UpdateLovMasterCommand(long LovId, string LovName, long UpdatedBy) : IRequest<bool>;

public record DeleteLovMasterCommand(long LovId) : IRequest<bool>;
