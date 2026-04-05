namespace WebsiteContentService.Domain.Entities;

using WebsiteContentService.Domain.Common;
using WebsiteContentService.Domain.Events;
using WebsiteContentService.Domain.ValueObjects;

public class WebsitePage : AggregateRoot
{
    public PageCode PageCode { get; private set; } = null!;
    public string PageTitle { get; private set; } = null!;
    public string? PageContent { get; private set; }
    public string? MetaDescription { get; private set; }
    public string? MetaKeywords { get; private set; }
    public int? PageOrder { get; private set; }
    public long? ParentPageId { get; private set; }
    public PublishFlag IsPublished { get; private set; } = null!;
    public DateTime? PublishedDate { get; private set; }
    public PublishStatus PageStatus { get; private set; } = null!;
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private WebsitePage() { }

    public static WebsitePage Create(
        string pageCode,
        string pageTitle,
        string? pageContent,
        string? metaDescription,
        string? metaKeywords,
        int? pageOrder,
        long? parentPageId,
        long createdBy)
    {
        var page = new WebsitePage
        {
            PageCode = PageCode.Create(pageCode),
            PageTitle = pageTitle,
            PageContent = pageContent,
            MetaDescription = metaDescription,
            MetaKeywords = metaKeywords,
            PageOrder = pageOrder,
            ParentPageId = parentPageId,
            IsPublished = PublishFlag.No,
            PageStatus = PublishStatus.Active,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        page.AddDomainEvent(new WebsitePageCreatedEvent(
            Guid.NewGuid(), pageCode, pageTitle, createdBy));

        return page;
    }

    public void Update(
        string pageTitle,
        string? pageContent,
        string? metaDescription,
        string? metaKeywords,
        int? pageOrder,
        long? parentPageId,
        long updatedBy)
    {
        PageTitle = pageTitle;
        PageContent = pageContent;
        MetaDescription = metaDescription;
        MetaKeywords = metaKeywords;
        PageOrder = pageOrder;
        ParentPageId = parentPageId;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WebsitePageUpdatedEvent(
            Guid.NewGuid(), PageCode.Value, pageTitle, updatedBy));
    }

    public void Publish(long updatedBy)
    {
        IsPublished = PublishFlag.Yes;
        PublishedDate = DateTime.UtcNow;
        PageStatus = PublishStatus.Active;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WebsitePagePublishedEvent(
            Guid.NewGuid(), PageCode.Value, updatedBy));
    }

    public void Unpublish(long updatedBy)
    {
        IsPublished = PublishFlag.No;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void ChangeStatus(string newStatus, long updatedBy)
    {
        PageStatus = PublishStatus.Create(newStatus);
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
