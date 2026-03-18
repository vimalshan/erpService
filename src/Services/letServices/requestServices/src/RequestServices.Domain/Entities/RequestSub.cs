namespace RequestServices.Domain.Entities;

/// <summary>Represents REQUEST_SUB — detailed line item of a training request.</summary>
public class RequestSub
{
    public long   RequestId         { get; private set; }
    public long   SerialNumber      { get; private set; }
    public DateTime RequestDate     { get; private set; }
    public DateTime ModifiedDate    { get; private set; }
    public char   ModifiedUser      { get; private set; }
    public char   RequestSource     { get; private set; }
    public char   ModuleTraining    { get; private set; }
    public char   GoalDesignation   { get; private set; }
    public char   StatusCode        { get; private set; }
    public string TrainingNeed      { get; private set; } = default!;
    public DateTime? CancellationDate   { get; private set; }
    public string? CancellationRemark   { get; private set; }
    public string MentorUser        { get; private set; } = default!;
    public string MentorRemark      { get; private set; } = default!;
    public long   CourseId          { get; private set; }
    public long   ApprovalNumber    { get; private set; }
    public long   ReviewDays        { get; private set; }
    public string ReviewUser        { get; private set; } = default!;
    public string ReviewModule      { get; private set; } = default!;
    public DateTime StartDate       { get; private set; }
    public DateTime EndDate         { get; private set; }
    public long   RefRequestId      { get; private set; }
    public long   RefSerialNumber   { get; private set; }
    public string SupervisorUser    { get; private set; } = default!;
    public string EnteredUser       { get; private set; } = default!;
    public char   EnteredMode       { get; private set; }
    public long   ApprovalTime      { get; private set; }
    public string BusinessBenefit   { get; private set; } = default!;
    public string ExpectedCompetency{ get; private set; } = default!;
    public string CourseDescription { get; private set; } = default!;
    public char   CourseAvailability{ get; private set; }

    // Navigation
    public RequestMain? RequestMain { get; private set; }

    private RequestSub() { }

    public static RequestSub Create(
        long requestId, long serialNumber, DateTime requestDate,
        char statusCode, string trainingNeed, long courseId,
        DateTime startDate, DateTime endDate,
        string supervisorUser, string enteredUser,
        string businessBenefit, string expectedCompetency, string courseDescription)
    {
        return new RequestSub
        {
            RequestId          = requestId,
            SerialNumber       = serialNumber,
            RequestDate        = requestDate,
            ModifiedDate       = requestDate,
            ModifiedUser       = 'E',
            RequestSource      = 'E',
            ModuleTraining     = 'N',
            GoalDesignation    = 'I',
            StatusCode         = statusCode,
            TrainingNeed       = trainingNeed,
            CourseId           = courseId,
            ApprovalNumber     = 0,
            ReviewDays         = 0,
            ReviewUser         = string.Empty,
            ReviewModule       = string.Empty,
            StartDate          = startDate,
            EndDate            = endDate,
            RefRequestId       = 0,
            RefSerialNumber    = 0,
            SupervisorUser     = supervisorUser,
            EnteredUser        = enteredUser,
            EnteredMode        = 'N',
            ApprovalTime       = 0,
            BusinessBenefit    = businessBenefit,
            ExpectedCompetency = expectedCompetency,
            CourseDescription  = courseDescription,
            CourseAvailability = 'N',
            MentorUser         = string.Empty,
            MentorRemark       = string.Empty
        };
    }

    public void Cancel(DateTime cancellationDate, string remark)
    {
        CancellationDate   = cancellationDate;
        CancellationRemark = remark;
        StatusCode         = 'C';
    }

    public void Approve(long approvalNumber)
    {
        ApprovalNumber = approvalNumber;
        StatusCode     = 'A';
    }
}
