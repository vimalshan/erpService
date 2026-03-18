namespace CourseService.Domain.Exceptions;

public class CourseDomainException : Exception
{
    public CourseDomainException(string message) : base(message) { }
    public CourseDomainException(string message, Exception inner) : base(message, inner) { }
}
