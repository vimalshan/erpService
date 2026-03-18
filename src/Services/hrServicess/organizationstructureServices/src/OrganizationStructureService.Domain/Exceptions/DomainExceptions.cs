namespace OrganizationStructureService.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class BusinessNotFoundException : DomainException
{
    public BusinessNotFoundException(decimal businessId)
        : base($"Business with ID {businessId} was not found.") { }
}

public class UnitNotFoundException : DomainException
{
    public UnitNotFoundException(decimal unitId)
        : base($"Unit with ID {unitId} was not found.") { }
}

public class GradeNotFoundException : DomainException
{
    public GradeNotFoundException(decimal gradeId)
        : base($"Grade with ID {gradeId} was not found.") { }
}

public class PositionNotFoundException : DomainException
{
    public PositionNotFoundException(decimal positionId)
        : base($"Position with ID {positionId} was not found.") { }
}

public class DepartmentNotFoundException : DomainException
{
    public DepartmentNotFoundException(decimal departmentId)
        : base($"Department with ID {departmentId} was not found.") { }
}

public class InvalidLiveFlagException : DomainException
{
    public InvalidLiveFlagException(string value)
        : base($"Invalid live flag value: '{value}'. Must be 'Y' or 'N'.") { }
}
