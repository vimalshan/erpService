using HotChocolate;
using MediatR;
using Recruitment.API.GraphQL.Types;
using Recruitment.Application.CQRS.Commands;
using Recruitment.Application.CQRS.Queries;
using Recruitment.Application.DTOs;

namespace Recruitment.API.GraphQL;

public class Query
{
    public async Task<IList<JobType>> GetAllJobs([Service] IMediator mediator)
    {
        var query = new GetAllJobsQuery();
        var result = await mediator.Send(query);
        return result.Select(MapToJobType).ToList();
    }

    public async Task<JobType?> GetJobById([Service] IMediator mediator, decimal jobId)
    {
        var query = new GetJobByIdQuery { JobId = jobId };
        var result = await mediator.Send(query);
        return result != null ? MapToJobType(result) : null;
    }

    public async Task<IList<ApplicationType>> GetAllApplications([Service] IMediator mediator)
    {
        var query = new GetAllApplicationsQuery();
        var result = await mediator.Send(query);
        return result.Select(MapToApplicationType).ToList();
    }

    public async Task<ApplicationType?> GetApplicationById([Service] IMediator mediator, decimal applicationNumber)
    {
        var query = new GetApplicationByIdQuery { ApplicationNumber = applicationNumber };
        var result = await mediator.Send(query);
        return result != null ? MapToApplicationType(result) : null;
    }

    public async Task<IList<ApplicationType>> GetApplicationsByJobId([Service] IMediator mediator, decimal jobId)
    {
        var query = new GetApplicationsByJobIdQuery { JobId = jobId };
        var result = await mediator.Send(query);
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
    public async Task<decimal> CreateJob([Service] IMediator mediator, CreateJobInput jobData)
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

        return await mediator.Send(command);
    }

    public async Task<decimal> CreateApplication([Service] IMediator mediator, CreateApplicationInput applicationData)
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

        return await mediator.Send(command);
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
