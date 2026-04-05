using Microsoft.EntityFrameworkCore;
using LocationService.Domain.Aggregates;
using LocationService.Domain.Entities;

namespace LocationService.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for Location Aggregate
    /// </summary>
    public class LocationRepository : ILocationRepository
    {
        private readonly LocationServiceDbContext _context;

        public LocationRepository(LocationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<LocationAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .Include(l => l.Rooms)
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<LocationAggregate?> GetByCodeAsync(string locationCode, CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .Include(l => l.Rooms)
                .FirstOrDefaultAsync(l => l.LocationCode == locationCode, cancellationToken);
        }

        public async Task<IReadOnlyList<LocationAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Locations
                .Include(l => l.Rooms)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<LocationAggregate>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            var all = await _context.Locations
                .Include(l => l.Rooms)
                .ToListAsync(cancellationToken);
            return all.Where(l => l.LocationStatus.IsActive).ToList();
        }

        public async Task AddAsync(LocationAggregate location, CancellationToken cancellationToken = default)
        {
            await _context.Locations.AddAsync(location, cancellationToken);
        }

        public async Task UpdateAsync(LocationAggregate location, CancellationToken cancellationToken = default)
        {
            _context.Locations.Update(location);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var location = await GetByIdAsync(id, cancellationToken);
            if (location != null)
            {
                _context.Locations.Remove(location);
            }
        }
    }

    /// <summary>
    /// Repository implementation for Room Aggregate
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly LocationServiceDbContext _context;

        public RoomRepository(LocationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<RoomAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Rooms
                .Include(r => r.Resources)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<RoomAggregate?> GetByCodeAsync(long locationId, string roomCode, CancellationToken cancellationToken = default)
        {
            return await _context.Rooms
                .Include(r => r.Resources)
                .FirstOrDefaultAsync(r => r.LocationId == locationId && r.RoomCode == roomCode, cancellationToken);
        }

        public async Task<IReadOnlyList<RoomAggregate>> GetByLocationIdAsync(long locationId, CancellationToken cancellationToken = default)
        {
            return await _context.Rooms
                .Include(r => r.Resources)
                .Where(r => r.LocationId == locationId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<RoomAggregate>> GetByLocationIdAndStatusAsync(long locationId, string status, CancellationToken cancellationToken = default)
        {
            var rooms = await _context.Rooms
                .Include(r => r.Resources)
                .Where(r => r.LocationId == locationId)
                .ToListAsync(cancellationToken);
            return rooms.Where(r => r.RoomStatus.Value == status).ToList();
        }

        public async Task AddAsync(RoomAggregate room, CancellationToken cancellationToken = default)
        {
            await _context.Rooms.AddAsync(room, cancellationToken);
        }

        public async Task UpdateAsync(RoomAggregate room, CancellationToken cancellationToken = default)
        {
            _context.Rooms.Update(room);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var room = await GetByIdAsync(id, cancellationToken);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }
        }
    }

    /// <summary>
    /// Repository implementation for Room Resource Aggregate
    /// </summary>
    public class RoomResourceRepository : IRoomResourceRepository
    {
        private readonly LocationServiceDbContext _context;

        public RoomResourceRepository(LocationServiceDbContext context)
        {
            _context = context;
        }

        public async Task<RoomResourceAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.RoomResources
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<RoomResourceAggregate?> GetByCodeAsync(long roomId, string resourceCode, CancellationToken cancellationToken = default)
        {
            return await _context.RoomResources
                .FirstOrDefaultAsync(r => r.RoomId == roomId && r.ResourceCode == resourceCode, cancellationToken);
        }

        public async Task<IReadOnlyList<RoomResourceAggregate>> GetByRoomIdAsync(long roomId, CancellationToken cancellationToken = default)
        {
            return await _context.RoomResources
                .Where(r => r.RoomId == roomId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<RoomResourceAggregate>> GetByLocationIdAsync(long locationId, CancellationToken cancellationToken = default)
        {
            return await _context.RoomResources
                .Where(r => r.LocationId == locationId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<RoomResourceAggregate>> GetByResourceTypeAsync(string resourceType, CancellationToken cancellationToken = default)
        {
            return await _context.RoomResources
                .Where(r => r.ResourceType == resourceType)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(RoomResourceAggregate resource, CancellationToken cancellationToken = default)
        {
            await _context.RoomResources.AddAsync(resource, cancellationToken);
        }

        public async Task UpdateAsync(RoomResourceAggregate resource, CancellationToken cancellationToken = default)
        {
            _context.RoomResources.Update(resource);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var resource = await GetByIdAsync(id, cancellationToken);
            if (resource != null)
            {
                _context.RoomResources.Remove(resource);
            }
        }
    }
}
