namespace InventoryManagement.Domain.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entity, object key) : base($"{entity} ({key}) was not found.") { }
}
