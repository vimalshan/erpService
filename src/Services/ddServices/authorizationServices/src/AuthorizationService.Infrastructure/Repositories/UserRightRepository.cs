using Microsoft.EntityFrameworkCore;
using AuthorizationService.Domain.Entities;
using AuthorizationService.Domain.Interfaces;

namespace AuthorizationService.Infrastructure.Repositories;

public class UserRightRepository : IUserRightRepository
{
    private readonly Data.AuthorizationDbContext _context;

    public UserRightRepository(Data.AuthorizationDbContext context)
    {
        _context = context;
    }

    public async Task<UserRight?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.UserRights
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<UserRight>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.UserRights
            .Where(a => !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserRight>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRights
            .Where(a => a.UserId == userId && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserRight>> GetByPinNumberAsync(decimal pinNumber, CancellationToken cancellationToken = default)
    {
        return await _context.UserRights
            .Where(a => a.PinNumber == pinNumber && !a.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(UserRight entity, CancellationToken cancellationToken = default)
    {
        await _context.UserRights.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(UserRight entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.UserRights.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            _context.UserRights.Update(entity);
        }
    }
}
