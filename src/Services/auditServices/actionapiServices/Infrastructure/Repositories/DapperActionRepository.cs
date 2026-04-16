using ActionService.Domain.Entities;
using ActionService.Domain.Interfaces;
using ActionService.Data;
using Dapper;

namespace ActionService.Infrastructure.Repositories;

public class DapperActionRepository : IActionRepository
{
    private readonly DapperContext _context;

    public DapperActionRepository(DapperContext context) => _context = context;

    public async Task<ActionItem?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ActionItem>(
            "SELECT id as Id, action as Action, dueDate as DueDate, highPriority as HighPriority, " +
            "message as Message, language as Language, service as Service, site as Site, " +
            "entityType as EntityType, entityId as EntityId, subject as Subject, snowLink as SnowLink " +
            "FROM Actions WHERE id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<ActionItem>> GetAllAsync(CancellationToken ct = default)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<ActionItem>(
            "SELECT id as Id, action as Action, dueDate as DueDate, highPriority as HighPriority, " +
            "message as Message, language as Language, service as Service, site as Site, " +
            "entityType as EntityType, entityId as EntityId, subject as Subject, snowLink as SnowLink " +
            "FROM Actions");
    }

    public async Task<IEnumerable<ActionItem>> GetByEntityAsync(string entityType, int entityId, CancellationToken ct = default)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<ActionItem>(
            "SELECT id as Id, action as Action, dueDate as DueDate, highPriority as HighPriority, " +
            "message as Message, language as Language, service as Service, site as Site, " +
            "entityType as EntityType, entityId as EntityId, subject as Subject, snowLink as SnowLink " +
            "FROM Actions WHERE entityType = @EntityType AND entityId = @EntityId",
            new { EntityType = entityType, EntityId = entityId });
    }

    public async Task<ActionItem> AddAsync(ActionItem entity, CancellationToken ct = default)
    {
        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(
            "INSERT INTO Actions (action, dueDate, highPriority, message, language, service, site, entityType, entityId, subject, snowLink) " +
            "VALUES (@Action, @DueDate, @HighPriority, @Message, @Language, @Service, @Site, @EntityType, @EntityId, @Subject, @SnowLink); " +
            "SELECT CAST(SCOPE_IDENTITY() as int)", entity);
        entity.Id = id;
        return entity;
    }

    public async Task UpdateAsync(ActionItem entity, CancellationToken ct = default)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE Actions SET action=@Action, dueDate=@DueDate, highPriority=@HighPriority, " +
            "message=@Message, language=@Language, service=@Service, site=@Site, " +
            "entityType=@EntityType, entityId=@EntityId, subject=@Subject, snowLink=@SnowLink WHERE id=@Id", entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Actions WHERE id = @Id", new { Id = id });
    }
}
