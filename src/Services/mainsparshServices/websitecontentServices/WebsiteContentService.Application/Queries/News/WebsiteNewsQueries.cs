namespace WebsiteContentService.Application.Queries.News;

using AutoMapper;
using MediatR;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Domain.Repositories;

public record GetAllWebsiteNewsQuery : IRequest<IEnumerable<WebsiteNewsDto>>;
public record GetWebsiteNewsByIdQuery(long NewsId) : IRequest<WebsiteNewsDto>;
public record GetWebsiteNewsByCategoryQuery(string Category) : IRequest<IEnumerable<WebsiteNewsDto>>;
public record GetPublishedWebsiteNewsQuery : IRequest<IEnumerable<WebsiteNewsDto>>;
public record GetFeaturedWebsiteNewsQuery : IRequest<IEnumerable<WebsiteNewsDto>>;

public class GetAllWebsiteNewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllWebsiteNewsQuery, IEnumerable<WebsiteNewsDto>>
{
    public async Task<IEnumerable<WebsiteNewsDto>> Handle(GetAllWebsiteNewsQuery request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetAllAsync(ct);
        return mapper.Map<IEnumerable<WebsiteNewsDto>>(news);
    }
}

public class GetWebsiteNewsByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetWebsiteNewsByIdQuery, WebsiteNewsDto>
{
    public async Task<WebsiteNewsDto> Handle(GetWebsiteNewsByIdQuery request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetByIdAsync(request.NewsId, ct)
            ?? throw new KeyNotFoundException($"News with ID {request.NewsId} not found.");

        return mapper.Map<WebsiteNewsDto>(news);
    }
}

public class GetWebsiteNewsByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetWebsiteNewsByCategoryQuery, IEnumerable<WebsiteNewsDto>>
{
    public async Task<IEnumerable<WebsiteNewsDto>> Handle(GetWebsiteNewsByCategoryQuery request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetByCategoryAsync(request.Category, ct);
        return mapper.Map<IEnumerable<WebsiteNewsDto>>(news);
    }
}

public class GetPublishedWebsiteNewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPublishedWebsiteNewsQuery, IEnumerable<WebsiteNewsDto>>
{
    public async Task<IEnumerable<WebsiteNewsDto>> Handle(GetPublishedWebsiteNewsQuery request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetPublishedAsync(ct);
        return mapper.Map<IEnumerable<WebsiteNewsDto>>(news);
    }
}

public class GetFeaturedWebsiteNewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetFeaturedWebsiteNewsQuery, IEnumerable<WebsiteNewsDto>>
{
    public async Task<IEnumerable<WebsiteNewsDto>> Handle(GetFeaturedWebsiteNewsQuery request, CancellationToken ct)
    {
        var news = await unitOfWork.WebsiteNews.GetFeaturedAsync(ct);
        return mapper.Map<IEnumerable<WebsiteNewsDto>>(news);
    }
}
