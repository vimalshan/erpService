namespace WebsiteContentService.API.GraphQL;

using HotChocolate;
using MediatR;
using WebsiteContentService.Application.DTOs;
using WebsiteContentService.Application.Queries.News;
using WebsiteContentService.Application.Queries.Pages;

public class WebsiteContentQuery
{
    [GraphQLDescription("Get all website pages")]
    public async Task<IEnumerable<WebsitePageDto>> GetWebsitePages([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllWebsitePagesQuery(), ct);

    [GraphQLDescription("Get published website pages")]
    public async Task<IEnumerable<WebsitePageDto>> GetPublishedWebsitePages([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPublishedWebsitePagesQuery(), ct);

    [GraphQLDescription("Get website page by ID")]
    public async Task<WebsitePageDto?> GetWebsitePageById([Service] IMediator mediator, long pageId, CancellationToken ct)
    {
        try { return await mediator.Send(new GetWebsitePageByIdQuery(pageId), ct); }
        catch (KeyNotFoundException) { return null; }
    }

    [GraphQLDescription("Get website page by code")]
    public async Task<WebsitePageDto?> GetWebsitePageByCode([Service] IMediator mediator, string pageCode, CancellationToken ct)
    {
        try { return await mediator.Send(new GetWebsitePageByCodeQuery(pageCode), ct); }
        catch (KeyNotFoundException) { return null; }
    }

    [GraphQLDescription("Get all website news")]
    public async Task<IEnumerable<WebsiteNewsDto>> GetWebsiteNews([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllWebsiteNewsQuery(), ct);

    [GraphQLDescription("Get published website news")]
    public async Task<IEnumerable<WebsiteNewsDto>> GetPublishedWebsiteNews([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPublishedWebsiteNewsQuery(), ct);

    [GraphQLDescription("Get featured website news")]
    public async Task<IEnumerable<WebsiteNewsDto>> GetFeaturedWebsiteNews([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetFeaturedWebsiteNewsQuery(), ct);

    [GraphQLDescription("Get website news by ID")]
    public async Task<WebsiteNewsDto?> GetWebsiteNewsById([Service] IMediator mediator, long newsId, CancellationToken ct)
    {
        try { return await mediator.Send(new GetWebsiteNewsByIdQuery(newsId), ct); }
        catch (KeyNotFoundException) { return null; }
    }

    [GraphQLDescription("Get website news by category")]
    public async Task<IEnumerable<WebsiteNewsDto>> GetWebsiteNewsByCategory([Service] IMediator mediator, string category, CancellationToken ct)
        => await mediator.Send(new GetWebsiteNewsByCategoryQuery(category), ct);
}
