namespace WebsiteContentService.Application.DTOs;

public record WebsitePageDto(
    long PageId,
    string PageCode,
    string PageTitle,
    string? PageContent,
    string? MetaDescription,
    string? MetaKeywords,
    int? PageOrder,
    long? ParentPageId,
    string IsPublished,
    DateTime? PublishedDate,
    string PageStatus,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn);

public record WebsiteNewsDto(
    long NewsId,
    string NewsTitle,
    string NewsContent,
    string? NewsSummary,
    string? NewsCategory,
    string? FeaturedImage,
    string IsFeatured,
    string IsPublished,
    DateTime? PublishedDate,
    DateTime? PublishStartDate,
    DateTime? PublishEndDate,
    string NewsStatus,
    int ViewCount,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn);
