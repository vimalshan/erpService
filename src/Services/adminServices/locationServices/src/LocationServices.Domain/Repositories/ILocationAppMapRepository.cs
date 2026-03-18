using LocationServices.Domain.Entities;

namespace LocationServices.Domain.Repositories;

/// <summary>Domain repository interface (not EF-specific)</summary>
public interface ILocationAppMapRepository
{
    Task<IEnumerable<LocationAppMapAggregate>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<LocationAppMapAggregate>> GetByLocationIdAsync(decimal locationId, CancellationToken ct = default);
    Task<IEnumerable<LocationAppMapAggregate>> GetByAppNameAsync(string appName, CancellationToken ct = default);
    Task<LocationAppMapAggregate?> GetMappingAsync(decimal locationId, string appName, CancellationToken ct = default);
    Task<IEnumerable<LocationAppMapAggregate>> GetActiveMappingsAsync(CancellationToken ct = default);
    Task<bool> ExistsAsync(decimal locationId, string appName, CancellationToken ct = default);
    Task AddAsync(LocationAppMapAggregate mapping, CancellationToken ct = default);
    void Update(LocationAppMapAggregate mapping);
    void Delete(LocationAppMapAggregate mapping);
}

/// <summary>Read-only Dapper-based query interface</summary>
public interface ILocationAppMapReadRepository
{
    Task<IEnumerable<LocationAppMapReadModel>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<LocationAppMapReadModel>> GetByLocationIdAsync(decimal locationId, CancellationToken ct = default);
    Task<LocationAppMapReadModel?> GetMappingAsync(decimal locationId, string appName, CancellationToken ct = default);
    Task<IEnumerable<LocationAppMapReadModel>> GetActiveMappingsAsync(CancellationToken ct = default);
    Task<int> GetTotalCountAsync(CancellationToken ct = default);
}

/// <summary>Read model for Dapper queries</summary>
public class LocationAppMapReadModel
{
    public decimal LocationId        { get; init; }
    public string  AppName           { get; init; } = null!;
    public long?   SiteCategoryCode  { get; init; }
    public string? SelfAccess        { get; init; }
    public string? DeemedApproval    { get; init; }
    public DateTime CreatedDate      { get; init; }
    public string?  CreatedBy        { get; init; }
    public DateTime? ModifiedDate    { get; init; }
    public string?  ModifiedBy       { get; init; }
    public bool     IsActive         { get; init; }
}
