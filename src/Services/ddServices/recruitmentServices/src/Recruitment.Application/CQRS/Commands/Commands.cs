using MediatR;
using Recruitment.Application.DTOs;

namespace Recruitment.Application.CQRS.Commands;

#region Job Commands

public class CreateJobCommand : IRequest<decimal>
{
    public CreateJobDto JobData { get; set; }
}

public class UpdateJobCommand : IRequest<bool>
{
    public UpdateJobDto JobData { get; set; }
}

public class DeleteJobCommand : IRequest<bool>
{
    public decimal JobId { get; set; }
}

public class DeactivateJobCommand : IRequest<bool>
{
    public decimal JobId { get; set; }
}

#endregion

#region Application Commands

public class CreateApplicationCommand : IRequest<decimal>
{
    public CreateApplicationDto ApplicationData { get; set; }
}

public class UpdateApplicationCommand : IRequest<bool>
{
    public UpdateApplicationDto ApplicationData { get; set; }
}

public class DeleteApplicationCommand : IRequest<bool>
{
    public decimal ApplicationNumber { get; set; }
}

public class ChangeApplicationStatusCommand : IRequest<bool>
{
    public decimal ApplicationNumber { get; set; }
    public string Status { get; set; }
    public string Remark { get; set; }
    public string UpdatedBy { get; set; }
}

public class SetApplicationMarksCommand : IRequest<bool>
{
    public decimal ApplicationNumber { get; set; }
    public decimal CrtMarks { get; set; }
    public decimal DomainMarks { get; set; }
}

public class SetApplicationDocumentsCommand : IRequest<bool>
{
    public decimal ApplicationNumber { get; set; }
    public string CrtDocumentPath { get; set; }
    public string DomainDocumentPath { get; set; }
}

public class AddCourseDetailCommand : IRequest<bool>
{
    public decimal ApplicationNumber { get; set; }
    public CourseDetailDto CourseDetail { get; set; }
}

#endregion

#region RecruitmentCycle Commands

public class CreateRecruitmentCycleCommand : IRequest<decimal>
{
    public decimal RecruitmentCycleNo { get; set; }
    public DateTime EffectiveFromDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class ExtendRecruitmentCycleCommand : IRequest<bool>
{
    public decimal RecruitmentCycleNo { get; set; }
    public DateTime NewEndDate { get; set; }
}

public class DeactivateRecruitmentCycleCommand : IRequest<bool>
{
    public decimal RecruitmentCycleNo { get; set; }
}

#endregion
