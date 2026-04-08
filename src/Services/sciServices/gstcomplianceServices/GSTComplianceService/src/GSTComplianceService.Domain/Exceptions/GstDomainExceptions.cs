namespace GSTComplianceService.Domain.Exceptions;

public class GstNotFoundException : Exception
{
    public GstNotFoundException(long gstId)
        : base($"GST registration with ID {gstId} was not found.") { }
}

public class DuplicatePanException : Exception
{
    public DuplicatePanException(string panNo)
        : base($"A GST registration with PAN {panNo} already exists.") { }
}

public class InvalidGstStatusTransitionException : Exception
{
    public InvalidGstStatusTransitionException(string from, string to)
        : base($"Cannot transition GST status from '{from}' to '{to}'.") { }
}
