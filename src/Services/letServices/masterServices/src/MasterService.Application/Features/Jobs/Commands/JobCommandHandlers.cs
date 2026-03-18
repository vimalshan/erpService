using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Entities;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.Jobs.Commands;

public sealed class CreateJobCommandHandler(IJobRepository repository, IMapper mapper)
    : IRequestHandler<CreateJobCommand, JobMasterDto>
{
    public async Task<JobMasterDto> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = JobMaster.Create(request.JobCode, request.JobName, request.CategoryCode, request.SerialNumber);
        await repository.AddAsync(job, cancellationToken);
        return mapper.Map<JobMasterDto>(job);
    }
}

public sealed class UpdateJobCommandHandler(IJobRepository repository, IMapper mapper)
    : IRequestHandler<UpdateJobCommand, JobMasterDto>
{
    public async Task<JobMasterDto> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await repository.GetByCodeAsync(request.JobCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Job {request.JobCode} not found.");
        job.Update(request.JobName, request.CategoryCode);
        await repository.UpdateAsync(job, cancellationToken);
        return mapper.Map<JobMasterDto>(job);
    }
}
