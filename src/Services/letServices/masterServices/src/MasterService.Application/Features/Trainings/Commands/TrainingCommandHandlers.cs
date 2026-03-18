using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Entities;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.Trainings.Commands;

public sealed class CreateTrainingCommandHandler(ITrainingRepository repository, IMapper mapper)
    : IRequestHandler<CreateTrainingCommand, TrainingProviderDto>
{
    public async Task<TrainingProviderDto> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
    {
        var provider = TrainingProvider.Create(
            request.TrainingCode, request.TrainingName,
            request.Address1, request.ContactName, request.PhoneNum, request.GroupCode);

        await repository.AddAsync(provider, cancellationToken);
        return mapper.Map<TrainingProviderDto>(provider);
    }
}

public sealed class CancelTrainingCommandHandler(ITrainingRepository repository)
    : IRequestHandler<CancelTrainingCommand, Unit>
{
    public async Task<Unit> Handle(CancelTrainingCommand request, CancellationToken cancellationToken)
    {
        var provider = await repository.GetByCodeAsync(request.TrainingCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Training provider {request.TrainingCode} not found.");

        provider.Cancel(request.CancelRemark);
        await repository.UpdateAsync(provider, cancellationToken);
        return Unit.Value;
    }
}

public sealed class UpdateTrainingBrochureCommandHandler(ITrainingRepository repository)
    : IRequestHandler<UpdateTrainingBrochureCommand, Unit>
{
    public async Task<Unit> Handle(UpdateTrainingBrochureCommand request, CancellationToken cancellationToken)
    {
        var provider = await repository.GetByCodeAsync(request.TrainingCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Training provider {request.TrainingCode} not found.");

        provider.UpdateBrochure(request.FilePath);
        await repository.UpdateAsync(provider, cancellationToken);
        return Unit.Value;
    }
}
