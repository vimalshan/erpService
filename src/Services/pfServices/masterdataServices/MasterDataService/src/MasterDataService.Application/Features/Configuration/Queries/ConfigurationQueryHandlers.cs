using MasterDataService.Application.DTOs;
using MasterDataService.Application.Interfaces;
using MediatR;

namespace MasterDataService.Application.Features.Configuration.Queries;

public class GetAllConfigurationsQueryHandler : IRequestHandler<GetAllConfigurationsQuery, IEnumerable<ConfigurationDto>>
{
    private readonly IConfigurationRepository _repository;

    public GetAllConfigurationsQueryHandler(IConfigurationRepository repository) => _repository = repository;

    public async Task<IEnumerable<ConfigurationDto>> Handle(GetAllConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(ConfigurationMapper.MapToDto);
    }
}

public class GetConfigurationByKeyQueryHandler : IRequestHandler<GetConfigurationByKeyQuery, ConfigurationDto?>
{
    private readonly IConfigurationRepository _repository;

    public GetConfigurationByKeyQueryHandler(IConfigurationRepository repository) => _repository = repository;

    public async Task<ConfigurationDto?> Handle(GetConfigurationByKeyQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByKeyAsync(request.Key, cancellationToken);
        return entity is null ? null : ConfigurationMapper.MapToDto(entity);
    }
}

public class GetConfigurationByIdQueryHandler : IRequestHandler<GetConfigurationByIdQuery, ConfigurationDto?>
{
    private readonly IConfigurationRepository _repository;

    public GetConfigurationByIdQueryHandler(IConfigurationRepository repository) => _repository = repository;

    public async Task<ConfigurationDto?> Handle(GetConfigurationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : ConfigurationMapper.MapToDto(entity);
    }
}

file static class ConfigurationMapper
{
    public static ConfigurationDto MapToDto(Domain.Entities.Configuration e) =>
        new(e.ConfigId, e.ConfigKey, e.ConfigValue, e.ConfigType, e.ConfigDescription, e.CreatedDate, e.UpdatedDate, e.CreatedBy);
}
