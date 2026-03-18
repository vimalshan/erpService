namespace ItemMasterService.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}

public class ItemNotFoundException : DomainException
{
    public ItemNotFoundException(long canteenUnitCode, long itemCode)
        : base($"Item with code {itemCode} in canteen unit {canteenUnitCode} was not found.") { }
}

public class DuplicateItemException : DomainException
{
    public DuplicateItemException(long canteenUnitCode, long itemCode)
        : base($"Item with code {itemCode} in canteen unit {canteenUnitCode} already exists.") { }
}
