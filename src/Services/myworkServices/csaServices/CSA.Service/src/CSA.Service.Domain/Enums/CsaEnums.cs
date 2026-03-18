namespace CSA.Service.Domain.Enums;

public enum ControlType
{
    Preventative = 'P',
    Detective = 'D'
}

public enum ControlMethod
{
    Manual = 'M',
    Automated = 'A'
}

public enum Priority
{
    High = 'H',
    Medium = 'M',
    Low = 'L'
}

public enum Periodicity
{
    Monthly = 'M',
    Quarterly = 'Q',
    Annual = 'A'
}

public enum YesNo
{
    Yes = 'Y',
    No = 'N'
}

public enum SurveyFeedbackStatus
{
    Pass = 'P',
    Fail = 'F',
    NotApplicable = 'N'
}

public enum FeedbackType
{
    ControlOwner = 'C',
    Approver = 'A'
}

public enum ApprovalFlag
{
    Pending = 'P',
    Approved = 'Y',
    Rejected = 'N'
}

public enum AssessmentStatus
{
    Pending = 'P',
    ControlAssessed = 'C',
    ApproverReviewed = 'A'
}

public enum RemedialStatus
{
    NotApplicable = 'N',
    Pending = 'P',
    ControlAssessed = 'C',
    ApproverReviewed = 'A'
}

public enum ApproverDueStatus
{
    NotDueForApproval = 'N',
    AwaitingApproval = 'P',
    Approved = 'Y'
}
