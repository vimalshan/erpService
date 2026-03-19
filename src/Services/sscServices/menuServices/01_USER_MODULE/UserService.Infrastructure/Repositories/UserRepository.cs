using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of User repository
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly Data.UserServiceDbContext _context;

    public UserRepository(Data.UserServiceDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.RoleMappings)
            .Include(u => u.OrganizationMappings)
            .Include(u => u.LocationMappings)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.RoleMappings)
            .Include(u => u.OrganizationMappings)
            .Include(u => u.LocationMappings)
            .FirstOrDefaultAsync(u => u.EmailId == email, cancellationToken);
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.RoleMappings)
            .Include(u => u.OrganizationMappings)
            .Include(u => u.LocationMappings)
            .FirstOrDefaultAsync(u => u.Name == userName, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.RoleMappings)
            .Include(u => u.OrganizationMappings)
            .Include(u => u.LocationMappings)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .Include(u => u.RoleMappings)
            .Include(u => u.OrganizationMappings)
            .Include(u => u.LocationMappings)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }
}
