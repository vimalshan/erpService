using MediatR;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Application.Features.Institutes.Queries;
using TrainingDevelopment.Application.Features.ProgramLov.Queries;
using TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetail;
using TrainingDevelopment.Application.Features.TrainingDetails.Queries.GetTrainingDetailList;

namespace TrainingDevelopment.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<TrainingDetailDto>> GetTrainingDetails(
        [Service] ISender sender,
        decimal? employeeSysId = null,
        decimal? financialYear = null,
        string? status = null,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(new GetTrainingDetailListQuery(employeeSysId, financialYear, status), cancellationToken);
    }

    public async Task<TrainingDetailDto> GetTrainingDetailById(
        [Service] ISender sender,
        decimal id,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(new GetTrainingDetailQuery(id), cancellationToken);
    }

    public async Task<IEnumerable<InstituteMasterDto>> GetInstitutes(
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(new GetInstituteListQuery(), cancellationToken);
    }

    public async Task<IEnumerable<ProgramLovDto>> GetProgramLovs(
        [Service] ISender sender,
        CancellationToken cancellationToken = default)
    {
        return await sender.Send(new GetProgramLovListQuery(), cancellationToken);
    }
}
