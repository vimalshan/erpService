using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Features.Configuration.Queries;

public record GetAllConfigurationsQuery : IRequest<IEnumerable<ConfigurationDto>>;
public record GetConfigurationByKeyQuery(string Key) : IRequest<ConfigurationDto?>;
public record GetConfigurationByIdQuery(int Id) : IRequest<ConfigurationDto?>;
