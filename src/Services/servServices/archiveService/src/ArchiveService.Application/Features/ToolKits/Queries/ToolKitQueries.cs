using ArchiveService.Application.DTOs;
using MediatR;

namespace ArchiveService.Application.Features.ToolKits.Queries;

public record GetToolKitByIdQuery(long Id) : IRequest<ToolKitDto?>;

public record GetToolKitsPagedQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<ToolKitDto>>;

public record GetToolKitsByEngineerQuery(string EngineerId) : IRequest<IReadOnlyList<ToolKitDto>>;
