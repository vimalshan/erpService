using SettingsService.Domain.Entities;
using SettingsService.Domain.Interfaces;
using SettingsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace SettingsService.Infrastructure.Repositories;

public class EfSettingsDomainRepository : ISettingsDomainRepository
{
    private readonly SettingsDomainDbContext _ctx;
    public EfSettingsDomainRepository(SettingsDomainDbContext ctx) { _ctx = ctx; }

    public async Task<User?> GetUserByIdAsync(int id) =>
        await _ctx.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Preferences).FirstOrDefaultAsync(u => u.UserId == id);

    public async Task<IEnumerable<User>> GetAllUsersAsync() =>
        await _ctx.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .OrderBy(u => u.Username).ToListAsync();

    public async Task<User> AddUserAsync(User user)
    {
        _ctx.Users.Add(user); await _ctx.SaveChangesAsync(); return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        _ctx.Users.Update(user); await _ctx.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var entity = await _ctx.Users.FindAsync(id);
        if (entity != null) { _ctx.Users.Remove(entity); await _ctx.SaveChangesAsync(); }
    }

    public async Task<Role?> GetRoleByIdAsync(int id) =>
        await _ctx.Roles.FirstOrDefaultAsync(r => r.RoleId == id);

    public async Task<IEnumerable<Role>> GetAllRolesAsync() =>
        await _ctx.Roles.Where(r => r.IsActive).OrderBy(r => r.RoleName).ToListAsync();

    public async Task<Role> AddRoleAsync(Role role)
    {
        _ctx.Roles.Add(role); await _ctx.SaveChangesAsync(); return role;
    }

    public async Task UpdateRoleAsync(Role role)
    {
        _ctx.Roles.Update(role); await _ctx.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserPreference>> GetUserPreferencesAsync(int userId) =>
        await _ctx.UserPreferences.Where(p => p.UserId == userId && p.IsActive).ToListAsync();

    public async Task<UserPreference> AddPreferenceAsync(UserPreference pref)
    {
        _ctx.UserPreferences.Add(pref); await _ctx.SaveChangesAsync(); return pref;
    }

    public async Task UpdatePreferenceAsync(UserPreference pref)
    {
        _ctx.UserPreferences.Update(pref); await _ctx.SaveChangesAsync();
    }

    public async Task<bool> DeactivateUserAsync(int userId, int? modifiedBy)
    {
        var userIdParameter = new SqlParameter("@userId", userId);
        var rowsAffected = await _ctx.Database.ExecuteSqlRawAsync(
            "UPDATE [Users] SET [IsActive] = 0, [ModifiedDate] = GETUTCDATE() WHERE [UserId] = @userId AND [IsActive] = 1",
            userIdParameter);

        return rowsAffected > 0;
    }
}
