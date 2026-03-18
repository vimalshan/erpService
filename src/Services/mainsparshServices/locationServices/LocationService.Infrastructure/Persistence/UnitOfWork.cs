using LocationService.Domain.Entities;

namespace LocationService.Infrastructure.Persistence
{
    /// <summary>
    /// Unit of Work implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LocationServiceDbContext _context;
        private ILocationRepository? _locationRepository;
        private IRoomRepository? _roomRepository;
        private IRoomResourceRepository? _roomResourceRepository;

        public UnitOfWork(LocationServiceDbContext context)
        {
            _context = context;
        }

        public ILocationRepository Locations
        {
            get
            {
                _locationRepository ??= new Persistence.Repositories.LocationRepository(_context);
                return _locationRepository;
            }
        }

        public IRoomRepository Rooms
        {
            get
            {
                _roomRepository ??= new Persistence.Repositories.RoomRepository(_context);
                return _roomRepository;
            }
        }

        public IRoomResourceRepository RoomResources
        {
            get
            {
                _roomResourceRepository ??= new Persistence.Repositories.RoomResourceRepository(_context);
                return _roomResourceRepository;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task TransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await action();
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
