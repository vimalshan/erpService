namespace WebsiteContentService.Domain.Events;

using WebsiteContentService.Domain.Common;

public class WebsitePageCreatedEvent(Guid aggregateId, string pageCode, string pageTitle, long createdBy)
    : DomainEvent(aggregateId)
{
    public string PageCode { get; } = pageCode;
    public string PageTitle { get; } = pageTitle;
    public long CreatedBy { get; } = createdBy;
}

public class WebsitePageUpdatedEvent(Guid aggregateId, string pageCode, string pageTitle, long updatedBy)
    : DomainEvent(aggregateId)
{
    public string PageCode { get; } = pageCode;
    public string PageTitle { get; } = pageTitle;
    public long UpdatedBy { get; } = updatedBy;
}

public class WebsitePagePublishedEvent(Guid aggregateId, string pageCode, long publishedBy)
    : DomainEvent(aggregateId)
{
    public string PageCode { get; } = pageCode;
    public long PublishedBy { get; } = publishedBy;
}
