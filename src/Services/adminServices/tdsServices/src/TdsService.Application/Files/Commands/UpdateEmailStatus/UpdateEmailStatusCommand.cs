using MediatR;

namespace TdsService.Application.Files.Commands.UpdateEmailStatus;

public sealed record UpdateEmailStatusCommand(long FileId) : IRequest;
