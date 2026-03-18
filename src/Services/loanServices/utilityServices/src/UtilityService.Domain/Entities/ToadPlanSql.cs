using UtilityService.Domain.Common;
using UtilityService.Domain.Events;
using UtilityService.Domain.ValueObjects;

namespace UtilityService.Domain.Entities;

public class ToadPlanSql : BaseEntity
{
    public string? Username { get; private set; }
    public StatementId StatementId { get; private set; } = null!;
    public DateTime? Timestamp { get; private set; }
    public string? Statement { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ToadPlanSql() { }

    public static ToadPlanSql Create(
        string? username,
        string statementId,
        string? statement,
        DateTime? timestamp = null)
    {
        var entity = new ToadPlanSql
        {
            Username = username,
            StatementId = StatementId.Create(statementId),
            Statement = statement,
            Timestamp = timestamp ?? DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        entity.AddDomainEvent(new ToadPlanCreatedEvent(entity.StatementId.Value, entity.Username));
        return entity;
    }

    public void Update(string? username, string? statement, DateTime? timestamp)
    {
        Username = username;
        Statement = statement;
        Timestamp = timestamp;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ToadPlanUpdatedEvent(StatementId.Value, username));
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new ToadPlanDeletedEvent(StatementId.Value));
    }
}
