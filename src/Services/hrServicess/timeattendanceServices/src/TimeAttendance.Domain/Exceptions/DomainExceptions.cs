namespace TimeAttendance.Domain.Exceptions;

public class AbsenteeismNotFoundException : Exception
{
    public AbsenteeismNotFoundException(long id)
        : base($"Absenteeism record with ID '{id}' was not found.") { }
}

public class AbsenteeismMisNotFoundException : Exception
{
    public AbsenteeismMisNotFoundException(long id)
        : base($"Absenteeism MIS record with ID '{id}' was not found.") { }
}

public class InvalidPeriodException : Exception
{
    public InvalidPeriodException(int year, int month)
        : base($"Period '{year}-{month:D2}' is invalid.") { }
}

public class DomainValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public DomainValidationException(IEnumerable<string> errors)
        : base("One or more domain validation errors occurred.")
    {
        Errors = errors;
    }

    public DomainValidationException(string error)
        : this([error]) { }
}
