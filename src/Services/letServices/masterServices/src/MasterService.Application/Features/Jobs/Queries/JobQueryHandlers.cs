using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.Jobs.Queries;

public sealed class GetJobsQueryHandler(IJobRepository repository, IMapper mapper)
    : IRequestHandler<GetJobsQuery, IEnumerable<JobMasterDto>>
{
    public async Task<IEnumerable<JobMasterDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var jobs = await repository.GetByCategoryAsync(request.CategoryCode, cancellationToken);
        return mapper.Map<IEnumerable<JobMasterDto>>(jobs);
    }
}

public sealed class GetJobByCodeQueryHandler(IJobRepository repository, IMapper mapper)
    : IRequestHandler<GetJobByCodeQuery, JobMasterDto?>
{
    public async Task<JobMasterDto?> Handle(GetJobByCodeQuery request, CancellationToken cancellationToken)
    {
        var job = await repository.GetByCodeAsync(request.JobCode, cancellationToken);
        return job is null ? null : mapper.Map<JobMasterDto>(job);
    }
}
