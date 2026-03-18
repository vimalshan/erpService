using MediatR;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Application.Features.Institutes.Commands;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.CompleteTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.CreateTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.DeleteTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.DropTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Commands.UpdateTrainingDetail;

namespace TrainingDevelopment.API.GraphQL;

public class Mutation
{
    public async Task<TrainingDetailDto> CreateTrainingDetail(
        [Service] ISender sender,
        CreateTrainingDetailCommand input,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(input, cancellationToken);
    }

    public async Task<TrainingDetailDto> UpdateTrainingDetail(
        [Service] ISender sender,
        UpdateTrainingDetailCommand input,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(input, cancellationToken);
    }

    public async Task<bool> CompleteTrainingDetail(
        [Service] ISender sender,
        CompleteTrainingDetailCommand input,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(input, cancellationToken);
    }

    public async Task<bool> DropTrainingDetail(
        [Service] ISender sender,
        decimal id,
        string remarks,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(new DropTrainingDetailCommand(id, remarks), cancellationToken);
    }

    public async Task<bool> DeleteTrainingDetail(
        [Service] ISender sender,
        decimal id,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(new DeleteTrainingDetailCommand(id), cancellationToken);
    }

    public async Task<InstituteMasterDto> CreateInstitute(
        [Service] ISender sender,
        CreateInstituteCommand input,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(input, cancellationToken);
    }
}
