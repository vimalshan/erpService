namespace RecruitmentService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class VacancyNotFoundException : DomainException
{
    public VacancyNotFoundException(decimal vacancyId)
        : base($"Vacancy with ID '{vacancyId}' was not found.") { }
}

public class ApplicationNotFoundException : DomainException
{
    public ApplicationNotFoundException(decimal appId)
        : base($"Application with ID '{appId}' was not found.") { }
}

public class ProspectNotFoundException : DomainException
{
    public ProspectNotFoundException(decimal userId)
        : base($"Prospect with ID '{userId}' was not found.") { }
}

public class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string email)
        : base($"A prospect with email '{email}' is already registered.") { }
}

public class VacancyClosedException : DomainException
{
    public VacancyClosedException(decimal vacancyId)
        : base($"Vacancy '{vacancyId}' is closed and no longer accepts applications.") { }
}
