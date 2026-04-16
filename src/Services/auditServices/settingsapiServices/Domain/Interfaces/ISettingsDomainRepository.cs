using SettingsService.Domain.Entities;

namespace SettingsService.Domain.Interfaces;

public interface ISettingsDomainRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<Role?> GetRoleByIdAsync(int id);
    Task<IEnumerable<Role>> GetAllRolesAsync();
    Task<Role> AddRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
    Task<IEnumerable<UserPreference>> GetUserPreferencesAsync(int userId);
    Task<UserPreference> AddPreferenceAsync(UserPreference pref);
    Task UpdatePreferenceAsync(UserPreference pref);
}
