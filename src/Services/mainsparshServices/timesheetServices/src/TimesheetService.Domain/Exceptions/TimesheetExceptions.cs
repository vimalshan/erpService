namespace TimesheetService.Domain.Exceptions;

public sealed class TimesheetDomainException : Exception
{
    public TimesheetDomainException(string message) : base(message) { }
    public TimesheetDomainException(string message, Exception inner) : base(message, inner) { }
}

public sealed class TimesheetNotFoundException : Exception
{
    public TimesheetNotFoundException(long timesheetId)
        : base($"Timesheet with ID {timesheetId} was not found.") { }
}
