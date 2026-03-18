using AutoMapper;
using MediatR;
using Recruitment.Application.CQRS.Commands;
using Recruitment.Application.DTOs;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Repositories;
using Recruitment.Domain.ValueObjects;

namespace Recruitment.Application.CQRS.Handlers;

/// <summary>
/// Handler for CreateJobCommand
/// </summary>
public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, decimal>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateJobCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<decimal> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var job = new Job(
            request.JobData.JobId,
            request.JobData.RecruitmentCycleNo,
            request.JobData.JobDescription,
            request.JobData.RoleDetails,
            request.JobData.CadreCode,
            request.JobData.EffectiveDate,
            request.JobData.PrincipalAccount,
            request.JobData.JobType,
            request.JobData.BusinessCode,
            request.JobData.UnitCode);

        await _unitOfWork.Jobs.AddAsync(job);
        await _unitOfWork.SaveChangesAsync();

        return job.JobId;
    }
}

/// <summary>
/// Handler for UpdateJobCommand
/// </summary>
public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateJobCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobData.JobId);
        if (job == null)
            return false;

        job.UpdateJobDetails(
            request.JobData.JobDescription,
            request.JobData.RoleDetails,
            request.JobData.JobType);

        await _unitOfWork.Jobs.UpdateAsync(job);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

/// <summary>
/// Handler for DeleteJobCommand
/// </summary>
public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteJobCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Jobs.DeleteAsync(request.JobId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

/// <summary>
/// Handler for DeactivateJobCommand
/// </summary>
public class DeactivateJobCommandHandler : IRequestHandler<DeactivateJobCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateJobCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeactivateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(request.JobId);
        if (job == null)
            return false;

        job.Deactivate();
        await _unitOfWork.Jobs.UpdateAsync(job);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
