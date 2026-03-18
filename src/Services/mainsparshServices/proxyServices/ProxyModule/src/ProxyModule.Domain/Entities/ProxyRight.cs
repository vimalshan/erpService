using ProxyModule.Domain.Events;
using ProxyModule.Domain.Exceptions;
using ProxyModule.Domain.ValueObjects;

namespace ProxyModule.Domain.Entities;

public class ProxyRight : BaseEntity
{
    public long ProxyId { get; private set; }
    public long ProxyUserId { get; private set; }
    public long DelegatedUserId { get; private set; }
    public DateTime ProxyStartDate { get; private set; }
    public DateTime? ProxyEndDate { get; private set; }
    public string ProxyType { get; private set; } = default!;
    public string ProxyStatus { get; private set; } = "A";
    public string? Scope { get; private set; }
    public string? Notes { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private ProxyRight() { } // EF Core constructor

    public static ProxyRight Create(
        long proxyUserId,
        long delegatedUserId,
        DateTime proxyStartDate,
        DateTime? proxyEndDate,
        string proxyType,
        string? scope,
        string? notes,
        long createdBy)
    {
        if (proxyUserId == delegatedUserId)
            throw new ProxyDomainException("Proxy user and delegated user cannot be the same.");

        if (proxyEndDate.HasValue && proxyEndDate.Value < proxyStartDate)
            throw new ProxyDomainException("Proxy end date cannot be before start date.");

        // Validate value objects
        var type = ValueObjects.ProxyType.From(proxyType);
        var scopeVo = ValueObjects.ProxyScope.From(scope);

        var entity = new ProxyRight
        {
            ProxyUserId = proxyUserId,
            DelegatedUserId = delegatedUserId,
            ProxyStartDate = proxyStartDate,
            ProxyEndDate = proxyEndDate,
            ProxyType = type.Value,
            ProxyStatus = "A",
            Scope = scopeVo.Value,
            Notes = notes,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        entity.AddDomainEvent(new ProxyRightCreatedEvent(
            entity.ProxyId, proxyUserId, delegatedUserId, type.Value));

        return entity;
    }

    public void Update(
        DateTime? proxyStartDate,
        DateTime? proxyEndDate,
        string? proxyType,
        string? scope,
        string? notes,
        long updatedBy)
    {
        if (proxyStartDate.HasValue) ProxyStartDate = proxyStartDate.Value;
        if (proxyEndDate.HasValue)
        {
            if (proxyEndDate.Value < ProxyStartDate)
                throw new ProxyDomainException("Proxy end date cannot be before start date.");
            ProxyEndDate = proxyEndDate.Value;
        }

        if (!string.IsNullOrWhiteSpace(proxyType))
            ProxyType = ValueObjects.ProxyType.From(proxyType).Value;

        if (!string.IsNullOrWhiteSpace(scope))
            Scope = ValueObjects.ProxyScope.From(scope).Value;

        if (notes is not null) Notes = notes;

        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new ProxyRightUpdatedEvent(ProxyId));
    }

    public void Deactivate(long updatedBy)
    {
        if (ProxyStatus == "I")
            throw new ProxyDomainException("Proxy right is already inactive.");

        ProxyStatus = "I";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new ProxyRightDeactivatedEvent(ProxyId, ProxyUserId, DelegatedUserId));
    }

    public void Activate(long updatedBy)
    {
        if (ProxyStatus == "A")
            throw new ProxyDomainException("Proxy right is already active.");

        ProxyStatus = "A";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public bool IsCurrentlyActive =>
        ProxyStatus == "A" &&
        ProxyStartDate <= DateTime.UtcNow &&
        (!ProxyEndDate.HasValue || ProxyEndDate.Value >= DateTime.UtcNow);
}
