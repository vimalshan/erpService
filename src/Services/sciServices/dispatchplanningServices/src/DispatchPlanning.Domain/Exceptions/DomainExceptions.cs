namespace DispatchPlanning.Domain.Exceptions;

public sealed class DispatchPlanNotFoundException : Exception
{
    public DispatchPlanNotFoundException(int id)
        : base($"Dispatch plan with header ID '{id}' was not found.") { }
}

public sealed class DispatchPlanItemNotFoundException : Exception
{
    public DispatchPlanItemNotFoundException(int breakupItemId)
        : base($"Dispatch plan item with breakup item ID '{breakupItemId}' was not found.") { }
}

public sealed class DuplicateDispatchPlanItemException : Exception
{
    public DuplicateDispatchPlanItemException(int itemId, int headerId)
        : base($"Item '{itemId}' already exists in dispatch plan header '{headerId}'.") { }
}

public sealed class InvalidPlanTypeException : Exception
{
    public InvalidPlanTypeException(char planType)
        : base($"Plan type '{planType}' is not valid. Expected 'I' or 'S'.") { }
}
