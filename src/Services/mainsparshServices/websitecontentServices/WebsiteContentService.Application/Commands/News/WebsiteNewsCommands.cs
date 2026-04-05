namespace WebsiteContentService.Application.Commands.News;

using MediatR;
using WebsiteContentService.Application.DTOs;

public record CreateWebsiteNewsCommand(
    string NewsTitle,
    string NewsContent,
    string? NewsSummary,
    string? NewsCategory,
    string? FeaturedImage,
    DateTime? PublishStartDate,
    DateTime? PublishEndDate,
    long CreatedBy) : IRequest<WebsiteNewsDto>;

public record UpdateWebsiteNewsCommand(
    long NewsId,
    string NewsTitle,
    string NewsContent,
    string? NewsSummary,
    string? NewsCategory,
    string? FeaturedImage,
    DateTime? PublishStartDate,
    DateTime? PublishEndDate,
    long UpdatedBy) : IRequest<WebsiteNewsDto>;

public record PublishWebsiteNewsCommand(
    long NewsId,
    long UpdatedBy) : IRequest<WebsiteNewsDto>;

public record ArchiveWebsiteNewsCommand(
    long NewsId,
    long UpdatedBy) : IRequest<bool>;

public record SetNewsFeaturedCommand(
    long NewsId,
    bool IsFeatured,
    long UpdatedBy) : IRequest<WebsiteNewsDto>;
