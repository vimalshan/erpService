using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Features.Configuration.Commands;

public record CreateConfigurationCommand(string ConfigKey, string ConfigValue, string ConfigType, string? Description, long CreatedBy) : IRequest<ConfigurationDto>;
public record UpdateConfigurationCommand(int ConfigId, string ConfigValue, string ConfigType) : IRequest<bool>;
public record UpdateConfigurationByKeyCommand(string ConfigKey, string ConfigValue, string ConfigType, long UpdatedBy) : IRequest<bool>;
public record DeleteConfigurationCommand(int ConfigId) : IRequest<bool>;
