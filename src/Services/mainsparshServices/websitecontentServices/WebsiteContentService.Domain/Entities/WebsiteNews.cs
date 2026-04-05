namespace WebsiteContentService.Domain.Entities;

using WebsiteContentService.Domain.Common;
using WebsiteContentService.Domain.Events;
using WebsiteContentService.Domain.ValueObjects;

public class WebsiteNews : AggregateRoot
{
    public string NewsTitle { get; private set; } = null!;
    public string NewsContent { get; private set; } = null!;
    public string? NewsSummary { get; private set; }
    public string? NewsCategory { get; private set; }
    public string? FeaturedImage { get; private set; }
    public PublishFlag IsFeatured { get; private set; } = null!;
    public PublishFlag IsPublished { get; private set; } = null!;
    public DateTime? PublishedDate { get; private set; }
    public DateTime? PublishStartDate { get; private set; }
    public DateTime? PublishEndDate { get; private set; }
    public PublishStatus NewsStatus { get; private set; } = null!;
    public int ViewCount { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private WebsiteNews() { }

    public static WebsiteNews Create(
        string newsTitle,
        string newsContent,
        string? newsSummary,
        string? newsCategory,
        string? featuredImage,
        DateTime? publishStartDate,
        DateTime? publishEndDate,
        long createdBy)
    {
        var news = new WebsiteNews
        {
            NewsTitle = newsTitle,
            NewsContent = newsContent,
            NewsSummary = newsSummary,
            NewsCategory = newsCategory,
            FeaturedImage = featuredImage,
            IsFeatured = PublishFlag.No,
            IsPublished = PublishFlag.No,
            PublishStartDate = publishStartDate,
            PublishEndDate = publishEndDate,
            NewsStatus = PublishStatus.Draft,
            ViewCount = 0,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        news.AddDomainEvent(new WebsiteNewsCreatedEvent(
            Guid.NewGuid(), newsTitle, newsCategory, createdBy));

        return news;
    }

    public void Update(
        string newsTitle,
        string newsContent,
        string? newsSummary,
        string? newsCategory,
        string? featuredImage,
        DateTime? publishStartDate,
        DateTime? publishEndDate,
        long updatedBy)
    {
        NewsTitle = newsTitle;
        NewsContent = newsContent;
        NewsSummary = newsSummary;
        NewsCategory = newsCategory;
        FeaturedImage = featuredImage;
        PublishStartDate = publishStartDate;
        PublishEndDate = publishEndDate;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WebsiteNewsUpdatedEvent(
            Guid.NewGuid(), newsTitle, updatedBy));
    }

    public void Publish(long updatedBy)
    {
        IsPublished = PublishFlag.Yes;
        PublishedDate = DateTime.UtcNow;
        NewsStatus = PublishStatus.Published;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WebsiteNewsPublishedEvent(
            Guid.NewGuid(), NewsTitle, updatedBy));
    }

    public void Unpublish(long updatedBy)
    {
        IsPublished = PublishFlag.No;
        NewsStatus = PublishStatus.Draft;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Archive(long updatedBy)
    {
        NewsStatus = PublishStatus.Archived;
        IsPublished = PublishFlag.No;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void SetFeatured(bool featured, long updatedBy)
    {
        IsFeatured = PublishFlag.FromBool(featured);
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void IncrementViewCount()
    {
        ViewCount++;
    }

    public void ChangeStatus(string newStatus, long updatedBy)
    {
        NewsStatus = PublishStatus.Create(newStatus);
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
