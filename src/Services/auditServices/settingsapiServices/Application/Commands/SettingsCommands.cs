using SettingsService.Application.DTOs;
using MediatR;

namespace SettingsService.Application.Commands;

public record CreateUserCommand(CreateUserDto Dto) : IRequest<UserDto>;
public record UpdateUserCommand(UpdateUserDto Dto) : IRequest<UserDto>;
public record DeactivateUserCommand(int UserId, int? ModifiedBy) : IRequest<bool>;
public record CreateRoleCommand(CreateRoleDto Dto) : IRequest<RoleDto>;
public record SetUserPreferenceCommand(SetUserPreferenceDto Dto) : IRequest<UserPreferenceDto>;
