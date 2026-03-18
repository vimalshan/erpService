namespace EmployeeManagement.Domain.Exceptions;

public class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(long id) : base($"Employee with ID {id} was not found.") { }
    public EmployeeNotFoundException(string employeeNo) : base($"Employee with number '{employeeNo}' was not found.") { }
}

public class DuplicateEmployeeException : Exception
{
    public DuplicateEmployeeException(string employeeNo) : base($"An employee with number '{employeeNo}' already exists.") { }
}

public class InvalidGradeException : Exception
{
    public InvalidGradeException(long gradeId) : base($"Grade with ID {gradeId} is not valid.") { }
}

public class ProbationAlreadyCompletedException : Exception
{
    public ProbationAlreadyCompletedException(long employeeId) : base($"Probation for employee {employeeId} is already completed.") { }
}
