using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.Trainings.Queries;

public sealed class GetTrainingsQueryHandler(ITrainingRepository repository, IMapper mapper)
    : IRequestHandler<GetTrainingsQuery, IEnumerable<TrainingProviderDto>>
{
    public async Task<IEnumerable<TrainingProviderDto>> Handle(GetTrainingsQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetAllActiveAsync(cancellationToken);
        return mapper.Map<IEnumerable<TrainingProviderDto>>(list);
    }
}

public sealed class GetTrainingByCodeQueryHandler(ITrainingRepository repository, IMapper mapper)
    : IRequestHandler<GetTrainingByCodeQuery, TrainingProviderDto?>
{
    public async Task<TrainingProviderDto?> Handle(GetTrainingByCodeQuery request, CancellationToken cancellationToken)
    {
        var provider = await repository.GetByCodeAsync(request.TrainingCode, cancellationToken);
        return provider is null ? null : mapper.Map<TrainingProviderDto>(provider);
    }
}
