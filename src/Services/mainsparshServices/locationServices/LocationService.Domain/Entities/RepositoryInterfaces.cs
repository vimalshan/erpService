namespace LocationService.Domain.Entities
{
    /// <summary>
    /// Repository interface for Location Aggregate
    /// </summary>
    public interface ILocationRepository
    {
        Task<Aggregates.LocationAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Aggregates.LocationAggregate?> GetByCodeAsync(string locationCode, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Aggregates.LocationAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Aggregates.LocationAggregate>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Aggregates.LocationAggregate location, CancellationToken cancellationToken = default);
        Task UpdateAsync(Aggregates.LocationAggregate location, CancellationToken cancellationToken = default);
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Repository interface for Room Aggregate
    /// </summary>
    public interface IRoomRepository
    {
        Task<Aggregates.RoomAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Aggregates.RoomAggregate?> GetByCodeAsync(long locationId, string roomCode, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Aggregates.RoomAggregate>> GetByLocationIdAsync(long locationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Aggregates.RoomAggregate>> GetByLocationIdAndStatusAsync(long locationId, string status, CancellationToken cancellationToken = default);
        Task AddAsync(Aggregates.RoomAggregate room, CancellationToken cancellationToken = default);
        Task UpdateAsync(Aggregates.RoomAggregate room, CancellationToken cancellationToken = default);
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Repository interface for Room Resource Aggregate
    /// </summary>
    public interface IRoomResourceRepository
    {
        Task<Aggregates.RoomResourceAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Aggregates.RoomResourceAggregate?> GetByCodeAsync(long roomId, string resourceCode, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Aggregates.RoomResourceAggregate>> GetByRoomIdAsync(long roomId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Aggregates.RoomResourceAggregate>> GetByLocationIdAsync(long locationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Aggregates.RoomResourceAggregate>> GetByResourceTypeAsync(string resourceType, CancellationToken cancellationToken = default);
        Task AddAsync(Aggregates.RoomResourceAggregate resource, CancellationToken cancellationToken = default);
        Task UpdateAsync(Aggregates.RoomResourceAggregate resource, CancellationToken cancellationToken = default);
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Unit of Work pattern interface
    /// </summary>
    public interface IUnitOfWork
    {
        ILocationRepository Locations { get; }
        IRoomRepository Rooms { get; }
        IRoomResourceRepository RoomResources { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task TransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
    }
}
