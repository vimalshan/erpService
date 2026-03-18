using HotChocolate;
using MediatR;
using Recruitment.API.GraphQL.Types;
using Recruitment.Application.CQRS.Commands;
using Recruitment.Application.CQRS.Queries;
using Recruitment.Application.DTOs;

namespace Recruitment.API.GraphQL;

public class Query
{
    private readonly IMediator _mediator;

    public Query(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IList<JobType>> GetAllJobs()
    {
        var query = new GetAllJobsQuery();
        var result = await _mediator.Send(query);
        return result.Select(MapToJobType).ToList();
    }

    public async Task<JobType?> GetJobById(decimal jobId)
    {
        var query = new GetJobByIdQuery { JobId = jobId };
        var result = await _mediator.Send(query);
        return result != null ? MapToJobType(result) : null;
    }

    public async Task<IList<ApplicationType>> GetAllApplications()
    {
        var query = new GetAllApplicationsQuery();
        var result = await _mediator.Send(query);
        return result.Select(MapToApplicationType).ToList();
    }

    public async Task<ApplicationType?> GetApplicationById(decimal applicationNumber)
    {
        var query = new GetApplicationByIdQuery { ApplicationNumber = applicationNumber };
        var result = await _mediator.Send(query);
        return result != null ? MapToApplicationType(result) : null;
    }

    public async Task<IList<ApplicationType>> GetApplicationsByJobId(decimal jobId)
    {
        var query = new GetApplicationsByJobIdQuery { JobId = jobId };
        var result = await _mediator.Send(query);
        return result.Select(MapToApplicationType).ToList();
    }

    private JobType MapToJobType(JobDto dto)
    {
        return new JobType
        {
            JobId = dto.JobId,
            JobDescription = dto.JobDescription,
            RoleDetails = dto.RoleDetails,
            CadreCode = dto.CadreCode,
            EffectiveDate = dto.EffectiveDate,
            PrincipalAccount = dto.PrincipalAccount,
            Type = dto.JobType,
            BusinessCode = dto.BusinessCode,
            UnitCode = dto.UnitCode,
            IsActive = dto.IsActive
        };
    }

    private ApplicationType MapToApplicationType(ApplicationDto dto)
    {
        return new ApplicationType
        {
            ApplicationNumber = dto.ApplicationNumber,
            JobId = dto.JobId,
            SparshId = dto.SparshId,
            Status = dto.Status,
            Achievements = dto.Achievements,
            ReasonForJoining = dto.ReasonForJoining,
            Strength = dto.Strength,
            CrtMarks = dto.CrtMarks,
            DomainMarks = dto.DomainMarks,
            CreatedDate = dto.CreatedDate,
            CourseDetails = dto.CourseDetails?.Select(c => new CourseDetailType
            {
                CourseTitle = c.CourseTitle,
                Duration = c.Duration,
                Institute = c.Institute
            }).ToList() ?? new()
        };
    }
}

public class Mutation
{
    private readonly IMediator _mediator;

    public Mutation(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<decimal> CreateJob(CreateJobInput jobData)
    {
        var command = new CreateJobCommand
        {
            JobData = new CreateJobDto
            {
                JobId = jobData.JobId,
                RecruitmentCycleNo = jobData.RecruitmentCycleNo,
                JobDescription = jobData.JobDescription,
                RoleDetails = jobData.RoleDetails,
                CadreCode = jobData.CadreCode,
                EffectiveDate = jobData.EffectiveDate,
                PrincipalAccount = jobData.PrincipalAccount,
                JobType = jobData.Type,
                BusinessCode = jobData.BusinessCode,
                UnitCode = jobData.UnitCode
            }
        };

        return await _mediator.Send(command);
    }

    public async Task<decimal> CreateApplication(CreateApplicationInput applicationData)
    {
        var command = new CreateApplicationCommand
        {
            ApplicationData = new CreateApplicationDto
            {
                ApplicationNumber = applicationData.ApplicationNumber,
                JobId = applicationData.JobId,
                SparshId = applicationData.SparshId,
                SparshPin = applicationData.SparshPin
            }
        };

        return await _mediator.Send(command);
    }
}

public class CreateJobInput
{
    public decimal JobId { get; set; }
    public decimal RecruitmentCycleNo { get; set; }
    public string JobDescription { get; set; }
    public string RoleDetails { get; set; }
    public string CadreCode { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string PrincipalAccount { get; set; }
    public string Type { get; set; }
    public string BusinessCode { get; set; }
    public string UnitCode { get; set; }
}

public class CreateApplicationInput
{
    public decimal ApplicationNumber { get; set; }
    public decimal JobId { get; set; }
    public string SparshId { get; set; }
    public decimal SparshPin { get; set; }
}
