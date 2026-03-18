namespace MasterDataService.Application.DTOs;

public record ConfigurationDto(
    int ConfigId,
    string ConfigKey,
    string ConfigValue,
    string ConfigType,
    string? ConfigDescription,
    DateTime CreatedDate,
    DateTime? UpdatedDate,
    long CreatedBy);

public record CreateConfigurationDto(
    string ConfigKey,
    string ConfigValue,
    string ConfigType,
    string? ConfigDescription,
    long CreatedBy);

public record UpdateConfigurationDto(
    int ConfigId,
    string ConfigValue,
    string ConfigType);
