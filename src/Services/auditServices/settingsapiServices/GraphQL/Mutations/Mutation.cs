using SettingsService.Application.Commands;
using SettingsService.Application.DTOs;
using SettingsService.Models;
using SettingsService.Services;
using MediatR;

namespace SettingsService.GraphQL.Mutations
{
    public class Mutation
    {
        private readonly ISettingsService _service;

        public Mutation(ISettingsService service)
        {
            _service = service;
        }

        [GraphQLName("updateCompanyDetails")]
        public Task<ApiResponse<CompanyDetailsUpdateResponse>> UpdateCompanyDetails(CompanyDetailsUpdateRequest input)
        {
            return _service.UpdateCompanyDetailsAsync(input);
        }

        [GraphQLName("updateUserPreferences")]
        public Task<ApiResponse<UserPreferencesUpdateResponse>> UpdateUserPreferences(int userId, UserPreferencesUpdateRequest input)
        {
            input.UserId = userId;
            return _service.UpdateUserPreferencesAsync(userId, input);
        }

        [GraphQLName("updateSystemPreferences")]
        public Task<ApiResponse<SystemPreferencesUpdateResponse>> UpdateSystemPreferences(SystemPreferencesUpdateRequest input)
        {
            return _service.UpdateSystemPreferencesAsync(input);
        }

        [GraphQLName("updateNotificationTemplate")]
        public Task<ApiResponse<NotificationTemplateUpdateResponse>> UpdateNotificationTemplate(NotificationTemplateUpdateRequest input)
        {
            return _service.UpdateNotificationTemplateAsync(input);
        }

        [GraphQLName("createUser")]
        public async Task<UserDto> CreateUser([Service] IMediator mediator, CreateUserDto input)
            => await mediator.Send(new CreateUserCommand(input));

        [GraphQLName("updateUser")]
        public async Task<UserDto> UpdateUser([Service] IMediator mediator, UpdateUserDto input)
            => await mediator.Send(new UpdateUserCommand(input));

        [GraphQLName("deactivateUser")]
        public async Task<bool> DeactivateUser([Service] IMediator mediator, int userId, int? modifiedBy)
            => await mediator.Send(new DeactivateUserCommand(userId, modifiedBy));

        [GraphQLName("createRole")]
        public async Task<RoleDto> CreateRole([Service] IMediator mediator, CreateRoleDto input)
            => await mediator.Send(new CreateRoleCommand(input));

        [GraphQLName("setUserPreference")]
        public async Task<UserPreferenceDto> SetUserPreference([Service] IMediator mediator, SetUserPreferenceDto input)
            => await mediator.Send(new SetUserPreferenceCommand(input));
    }
}
