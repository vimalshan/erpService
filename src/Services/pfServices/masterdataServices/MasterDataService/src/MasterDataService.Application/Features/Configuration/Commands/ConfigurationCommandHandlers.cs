using MasterDataService.Application.DTOs;
using MasterDataService.Application.Interfaces;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Features.Configuration.Commands;

public class CreateConfigurationCommandHandler : IRequestHandler<CreateConfigurationCommand, ConfigurationDto>
{
    private readonly IConfigurationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateConfigurationCommandHandler(IConfigurationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ConfigurationDto> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Configuration
        {
            ConfigKey = request.ConfigKey,
            ConfigValue = request.ConfigValue,
            ConfigType = request.ConfigType,
            ConfigDescription = request.Description,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = request.CreatedBy
        };
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ConfigurationDto(entity.ConfigId, entity.ConfigKey, entity.ConfigValue, entity.ConfigType, entity.ConfigDescription, entity.CreatedDate, entity.UpdatedDate, entity.CreatedBy);
    }
}

public class UpdateConfigurationCommandHandler : IRequestHandler<UpdateConfigurationCommand, bool>
{
    private readonly IConfigurationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateConfigurationCommandHandler(IConfigurationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.ConfigId, cancellationToken);
        if (entity is null) return false;
        entity.UpdateValue(request.ConfigValue);
        entity.ConfigType = request.ConfigType;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
