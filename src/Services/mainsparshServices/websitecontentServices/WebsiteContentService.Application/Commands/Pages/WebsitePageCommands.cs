namespace WebsiteContentService.Application.Commands.Pages;

using MediatR;
using WebsiteContentService.Application.DTOs;

public record CreateWebsitePageCommand(
    string PageCode,
    string PageTitle,
    string? PageContent,
    string? MetaDescription,
    string? MetaKeywords,
    int? PageOrder,
    long? ParentPageId,
    long CreatedBy) : IRequest<WebsitePageDto>;

public record UpdateWebsitePageCommand(
    long PageId,
    string PageTitle,
    string? PageContent,
    string? MetaDescription,
    string? MetaKeywords,
    int? PageOrder,
    long? ParentPageId,
    long UpdatedBy) : IRequest<WebsitePageDto>;

public record PublishWebsitePageCommand(
    long PageId,
    long UpdatedBy) : IRequest<WebsitePageDto>;

public record ChangeWebsitePageStatusCommand(
    long PageId,
    string NewStatus,
    long UpdatedBy) : IRequest<bool>;
