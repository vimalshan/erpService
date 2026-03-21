using ArchiveService.Application.DTOs;
using AutoMapper;
using ArchiveService.Domain.Interfaces;
using MediatR;

namespace ArchiveService.Application.Features.ToolKits.Queries;

public class GetToolKitByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetToolKitByIdQuery, ToolKitDto?>
{
    public async Task<ToolKitDto?> Handle(GetToolKitByIdQuery request, CancellationToken ct)
    {
        var toolkit = await unitOfWork.ToolKits.GetByIdAsync(request.Id, ct);
        return toolkit is null ? null : mapper.Map<ToolKitDto>(toolkit);
    }
}

public class GetToolKitsPagedHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetToolKitsPagedQuery, PagedResult<ToolKitDto>>
{
    public async Task<PagedResult<ToolKitDto>> Handle(GetToolKitsPagedQuery request, CancellationToken ct)
    {
        var toolkits = await unitOfWork.ToolKits.GetAllAsync(request.Page, request.PageSize, ct);
        return new PagedResult<ToolKitDto>
        {
            Items = mapper.Map<IReadOnlyList<ToolKitDto>>(toolkits),
            TotalCount = toolkits.Count,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}

public class GetToolKitsByEngineerHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetToolKitsByEngineerQuery, IReadOnlyList<ToolKitDto>>
{
    public async Task<IReadOnlyList<ToolKitDto>> Handle(GetToolKitsByEngineerQuery request, CancellationToken ct)
    {
        var toolkits = await unitOfWork.ToolKits.GetByEngineerIdAsync(request.EngineerId, ct);
        return mapper.Map<IReadOnlyList<ToolKitDto>>(toolkits);
    }
}
