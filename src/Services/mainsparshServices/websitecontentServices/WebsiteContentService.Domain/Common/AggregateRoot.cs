namespace WebsiteContentService.Domain.Common;

public abstract class AggregateRoot : Entity
{
    public int Version { get; set; }

    protected AggregateRoot()
    {
        Version = 1;
    }
}
