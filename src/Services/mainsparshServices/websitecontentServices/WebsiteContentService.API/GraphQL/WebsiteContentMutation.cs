namespace WebsiteContentService.API.GraphQL;

using HotChocolate;
using MediatR;
using WebsiteContentService.Application.Commands.News;
using WebsiteContentService.Application.Commands.Pages;
using WebsiteContentService.Application.DTOs;

public class WebsiteContentMutation
{
    [GraphQLDescription("Create a new website page")]
    public async Task<WebsitePageDto> CreateWebsitePage(
        [Service] IMediator mediator,
        string pageCode, string pageTitle, string? pageContent, string? metaDescription,
        string? metaKeywords, int? pageOrder, long? parentPageId, long createdBy, CancellationToken ct)
        => await mediator.Send(new CreateWebsitePageCommand(pageCode, pageTitle, pageContent, metaDescription,
            metaKeywords, pageOrder, parentPageId, createdBy), ct);

    [GraphQLDescription("Update a website page")]
    public async Task<WebsitePageDto> UpdateWebsitePage(
        [Service] IMediator mediator,
        long pageId, string pageTitle, string? pageContent, string? metaDescription,
        string? metaKeywords, int? pageOrder, long? parentPageId, long updatedBy, CancellationToken ct)
        => await mediator.Send(new UpdateWebsitePageCommand(pageId, pageTitle, pageContent, metaDescription,
            metaKeywords, pageOrder, parentPageId, updatedBy), ct);

    [GraphQLDescription("Publish a website page")]
    public async Task<WebsitePageDto> PublishWebsitePage(
        [Service] IMediator mediator,
        long pageId, long updatedBy, CancellationToken ct)
        => await mediator.Send(new PublishWebsitePageCommand(pageId, updatedBy), ct);

    [GraphQLDescription("Change website page status")]
    public async Task<bool> ChangeWebsitePageStatus(
        [Service] IMediator mediator,
        long pageId, string newStatus, long updatedBy, CancellationToken ct)
    {
        await mediator.Send(new ChangeWebsitePageStatusCommand(pageId, newStatus, updatedBy), ct);
        return true;
    }

    [GraphQLDescription("Create a new website news article")]
    public async Task<WebsiteNewsDto> CreateWebsiteNews(
        [Service] IMediator mediator,
        string newsTitle, string newsContent, string? newsSummary, string? newsCategory, string? featuredImage,
        DateTime? publishStartDate, DateTime? publishEndDate, long createdBy, CancellationToken ct)
        => await mediator.Send(new CreateWebsiteNewsCommand(newsTitle, newsContent, newsSummary, newsCategory,
            featuredImage, publishStartDate, publishEndDate, createdBy), ct);

    [GraphQLDescription("Update a website news article")]
    public async Task<WebsiteNewsDto> UpdateWebsiteNews(
        [Service] IMediator mediator,
        long newsId, string newsTitle, string newsContent, string? newsSummary, string? newsCategory,
        string? featuredImage, DateTime? publishStartDate, DateTime? publishEndDate, long updatedBy,
        CancellationToken ct)
        => await mediator.Send(new UpdateWebsiteNewsCommand(newsId, newsTitle, newsContent, newsSummary,
            newsCategory, featuredImage, publishStartDate, publishEndDate, updatedBy), ct);

    [GraphQLDescription("Publish a website news article")]
    public async Task<WebsiteNewsDto> PublishWebsiteNews(
        [Service] IMediator mediator,
        long newsId, long updatedBy, CancellationToken ct)
        => await mediator.Send(new PublishWebsiteNewsCommand(newsId, updatedBy), ct);

    [GraphQLDescription("Archive a website news article")]
    public async Task<bool> ArchiveWebsiteNews(
        [Service] IMediator mediator,
        long newsId, long updatedBy, CancellationToken ct)
    {
        await mediator.Send(new ArchiveWebsiteNewsCommand(newsId, updatedBy), ct);
        return true;
    }

    [GraphQLDescription("Set featured status for a website news article")]
    public async Task<WebsiteNewsDto> SetFeaturedWebsiteNews(
        [Service] IMediator mediator,
        long newsId, bool isFeatured, long updatedBy, CancellationToken ct)
        => await mediator.Send(new SetNewsFeaturedCommand(newsId, isFeatured, updatedBy), ct);
}
