using SciTransactional.Domain.Common;
using SciTransactional.Domain.Events;

namespace SciTransactional.Domain.Entities;

public sealed class SparshNavigationEntity : AggregateRoot<long>
{
    public string UserId { get; private set; } = string.Empty;
    public long UserNum { get; private set; }
    public string? RandomNum { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    public string SciId { get; private set; } = string.Empty;
    public string? StatusFlag { get; private set; }

    private SparshNavigationEntity() { }

    public static SparshNavigationEntity Create(
        long requestNum, string userId, long userNum,
        string? randomNum, string sciId, string? statusFlag)
    {
        var entity = new SparshNavigationEntity
        {
            Id = requestNum,
            UserId = userId,
            UserNum = userNum,
            RandomNum = randomNum,
            UpdatedDate = DateTime.UtcNow,
            SciId = sciId,
            StatusFlag = statusFlag ?? "N"
        };
        entity.AddDomainEvent(new NavigationCreatedEvent(requestNum, userId, sciId));
        return entity;
    }

    public void UpdateStatus(string newStatus)
    {
        StatusFlag = newStatus;
        UpdatedDate = DateTime.UtcNow;
        AddDomainEvent(new NavigationStatusChangedEvent(Id, newStatus));
    }
}
