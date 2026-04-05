namespace WebsiteContentService.Application.Queries.Pages;

using AutoMapper;
using MediatR;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Domain.Repositories;

public record GetAllWebsitePagesQuery : IRequest<IEnumerable<WebsitePageDto>>;
public record GetWebsitePageByIdQuery(long PageId) : IRequest<WebsitePageDto>;
public record GetWebsitePageByCodeQuery(string PageCode) : IRequest<WebsitePageDto>;
public record GetPublishedWebsitePagesQuery : IRequest<IEnumerable<WebsitePageDto>>;

public class GetAllWebsitePagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllWebsitePagesQuery, IEnumerable<WebsitePageDto>>
{
    public async Task<IEnumerable<WebsitePageDto>> Handle(GetAllWebsitePagesQuery request, CancellationToken ct)
    {
        var pages = await unitOfWork.WebsitePages.GetAllAsync(ct);
        return mapper.Map<IEnumerable<WebsitePageDto>>(pages);
    }
}

public class GetWebsitePageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetWebsitePageByIdQuery, WebsitePageDto>
{
    public async Task<WebsitePageDto> Handle(GetWebsitePageByIdQuery request, CancellationToken ct)
    {
        var page = await unitOfWork.WebsitePages.GetByIdAsync(request.PageId, ct)
            ?? throw new KeyNotFoundException($"Page with ID {request.PageId} not found.");

        return mapper.Map<WebsitePageDto>(page);
    }
}

public class GetWebsitePageByCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetWebsitePageByCodeQuery, WebsitePageDto>
{
    public async Task<WebsitePageDto> Handle(GetWebsitePageByCodeQuery request, CancellationToken ct)
    {
        var page = await unitOfWork.WebsitePages.GetByCodeAsync(request.PageCode, ct)
            ?? throw new KeyNotFoundException($"Page with code '{request.PageCode}' not found.");

        return mapper.Map<WebsitePageDto>(page);
    }
}

public class GetPublishedWebsitePagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPublishedWebsitePagesQuery, IEnumerable<WebsitePageDto>>
{
    public async Task<IEnumerable<WebsitePageDto>> Handle(GetPublishedWebsitePagesQuery request, CancellationToken ct)
    {
        var pages = await unitOfWork.WebsitePages.GetPublishedAsync(ct);
        return mapper.Map<IEnumerable<WebsitePageDto>>(pages);
    }
}
