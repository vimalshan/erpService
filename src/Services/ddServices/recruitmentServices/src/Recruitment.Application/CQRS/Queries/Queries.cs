using MediatR;
using Recruitment.Application.DTOs;

namespace Recruitment.Application.CQRS.Queries;

#region Job Queries

public class GetJobByIdQuery : IRequest<JobDto>
{
    public decimal JobId { get; set; }
}

public class GetAllJobsQuery : IRequest<IEnumerable<JobDto>>
{
}

public class GetJobsByRecruitmentCycleQuery : IRequest<IEnumerable<JobDto>>
{
    public decimal RecruitmentCycleNo { get; set; }
}

public class GetActiveJobsQuery : IRequest<IEnumerable<JobDto>>
{
}

#endregion

#region Application Queries

public class GetApplicationByIdQuery : IRequest<ApplicationDto>
{
    public decimal ApplicationNumber { get; set; }
}

public class GetAllApplicationsQuery : IRequest<IEnumerable<ApplicationDto>>
{
}

public class GetApplicationsByJobIdQuery : IRequest<IEnumerable<ApplicationDto>>
{
    public decimal JobId { get; set; }
}

public class GetApplicationsBySparshIdQuery : IRequest<IEnumerable<ApplicationDto>>
{
    public string SparshId { get; set; }
}

public class GetApplicationsByStatusQuery : IRequest<IEnumerable<ApplicationDto>>
{
    public string Status { get; set; }
}

public class GetApplicationsByRecruitmentCycleQuery : IRequest<IEnumerable<ApplicationDto>>
{
    public decimal RecruitmentCycleNo { get; set; }
}

#endregion

#region RecruitmentCycle Queries

public class GetRecruitmentCycleByIdQuery : IRequest<object>
{
    public decimal RecruitmentCycleNo { get; set; }
}

public class GetAllRecruitmentCyclesQuery : IRequest<IEnumerable<object>>
{
}

public class GetActiveRecruitmentCyclesQuery : IRequest<IEnumerable<object>>
{
}

#endregion
