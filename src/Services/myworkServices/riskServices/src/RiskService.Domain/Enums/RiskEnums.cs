namespace RiskService.Domain.Enums;

public enum ApplicableTo
{
    Organization = 'O',
    Business = 'B',
    SubDivision = 'S',
    Unit = 'U'
}

public enum ApprovalStatus
{
    Entry = 'E',
    Pending = 'P',
    Approved = 'A'
}

public enum MitigationStatus
{
    Mitigated = 'M',
    Live = 'L',
    Dropped = 'D'
}

public enum ActionApprovalStatus
{
    Entry = 'E',
    Pending = 'P',
    Approved = 'A'
}

public enum ActionStatus
{
    NotCompleted = 'N',
    Completed = 'C',
    PartiallyCompleted = 'P',
    Dropped = 'D'
}

public enum MeetingStatus
{
    Pending = 'P',
    Conducted = 'Y',
    Skipped = 'N'
}

public enum SelfAssessmentStatus
{
    Entry = 'E',
    Pending = 'P',
    Completed = 'C',
    Skipped = 'S'
}

public enum MonitoredBy
{
    BRD,
    CLT,
    BLT,
    ULT
}

public enum ReviewFrequency
{
    Monthly = 'M',
    HalfYearly = 'H',
    Annually = 'A',
    Quarterly = 'Q'
}

public enum ChampionType
{
    Organization = 'O',
    Business = 'B',
    SubDivision = 'S',
    Unit = 'U',
    SuperUser = 'A'
}
