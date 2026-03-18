namespace HRService.Domain.Exceptions;

public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(Guid employeeId)
        : base($"Employee with id {employeeId} not found")
    {
    }
}

public class DepartmentNotFoundException : Exception
{
    public DepartmentNotFoundException(Guid departmentId)
        : base($"Department with id {departmentId} not found")
    {
    }
}

public class InvalidEmployeeStateException : Exception
{
    public InvalidEmployeeStateException(string message)
        : base(message)
    {
    }
}

public class InvalidLeaveRequestException : Exception
{
    public InvalidLeaveRequestException(string message)
        : base(message)
    {
    }
}
