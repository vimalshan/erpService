namespace FilingAndArchiveService.Domain.Exceptions;

public class FilingDomainException : Exception
{
    public FilingDomainException(string message) : base(message) { }

    public FilingDomainException(string message, Exception inner) : base(message, inner) { }
}

public class FileNotFoundException : FilingDomainException
{
    public FileNotFoundException(long fileId)
        : base($"File with ID {fileId} was not found.") { }
}

public class FileAlreadyExistsException : FilingDomainException
{
    public FileAlreadyExistsException(string fileNo, string orgId)
        : base($"File '{fileNo}' for organization '{orgId}' already exists.") { }
}

public class InvalidFileStatusTransitionException : FilingDomainException
{
    public InvalidFileStatusTransitionException(string fromStatus, string toStatus)
        : base($"Cannot transition file from status '{fromStatus}' to '{toStatus}'.") { }
}
