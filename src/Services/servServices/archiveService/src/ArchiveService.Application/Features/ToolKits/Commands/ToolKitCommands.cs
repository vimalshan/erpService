using MediatR;

namespace ArchiveService.Application.Features.ToolKits.Commands;

public record CreateToolKitCommand(
    string? KitCode,
    string? AppPassword,
    string? InstPassword,
    string? ImeiNo,
    string? EngineerId,
    string? Flag,
    string? EnteredBy) : IRequest<long>;

public record UpdateToolKitFlagCommand(
    long Id,
    string? Flag,
    string? ChangedBy) : IRequest<bool>;
