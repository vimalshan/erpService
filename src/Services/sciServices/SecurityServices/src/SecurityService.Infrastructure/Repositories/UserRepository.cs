using Microsoft.EntityFrameworkCore;
using SecurityService.Application.Interfaces;
using SecurityService.Domain.Entities;
using SecurityService.Infrastructure.Data;

namespace SecurityService.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly SecurityDbContext _db;

    public UserRepository(SecurityDbContext db) => _db = db;

    public Task<User?> GetByIdAsync(long userId, CancellationToken ct = default)
        => _db.Users
              .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
              .FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public Task<User?> GetByCodeAsync(string userCode, CancellationToken ct = default)
        => _db.Users
              .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
              .FirstOrDefaultAsync(u => u.UserCode.Value == userCode, ct);

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
        => await _db.Users.AsNoTracking().ToListAsync(ct);

    public async Task<(IEnumerable<User> Items, int TotalCount)> SearchAsync(
        string? searchTerm, bool activeOnly, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _db.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(u =>
                u.UserCode.Value.ToLower().Contains(term) ||
                (u.UserName != null && u.UserName.ToLower().Contains(term)));
        }

        if (activeOnly)
            query = query.Where(u => u.EndDate == null || u.EndDate >= DateTime.UtcNow);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(u => u.UserId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<long> AddAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return user.UserId;
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsAsync(long userId, CancellationToken ct = default)
        => _db.Users.AnyAsync(u => u.UserId == userId, ct);
}
