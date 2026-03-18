namespace CourseService.Domain.Enums;

public enum CourseType
{
    Internal = 'I',
    External = 'E',
    Online = 'O',
    Blended = 'B'
}

public enum TrainingType
{
    Classroom = 'C',
    Online = 'O',
    OnJob = 'J',
    Workshop = 'W'
}

public enum NominationStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3,
    Waitlisted = 4
}

public enum AttendanceStatus
{
    NotMarked = 'N',
    Present = 'P',
    Absent = 'A',
    Partial = 'L'
}
