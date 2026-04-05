using Microsoft.EntityFrameworkCore;
using GroupManagementService.Domain.Entities;
using GroupManagementService.Domain.Repositories;
using GroupManagementService.Domain.ValueObjects;
using GroupManagementService.Infrastructure.Persistence;

namespace GroupManagementService.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly GroupManagementDbContext _context;

        public GroupRepository(GroupManagementDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Group?> GetByIdAsync(long groupId, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .Include(g => g.MenuMaps)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
        }

        public async Task<Group?> GetByCodeAsync(string groupCode, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Code == groupCode, cancellationToken);
        }

        public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Group>> GetByStatusAsync(GroupStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .AsNoTracking()
                .Where(g => g.Status == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(long groupId, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .AnyAsync(g => g.Id == groupId, cancellationToken);
        }

        public async Task<bool> CodeExistsAsync(string groupCode, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .AnyAsync(g => g.Code == groupCode, cancellationToken);
        }

        public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            await _context.Groups.AddAsync(group, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Group group, CancellationToken cancellationToken = default)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            _context.Groups.Update(group);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(long groupId, CancellationToken cancellationToken = default)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
