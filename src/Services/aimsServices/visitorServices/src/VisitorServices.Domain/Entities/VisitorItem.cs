using VisitorServices.Domain.Common;
using VisitorServices.Domain.Enums;
using VisitorServices.Domain.Events;
using VisitorServices.Domain.ValueObjects;

namespace VisitorServices.Domain.Entities;

public sealed class VisitorItem : Entity
{
    public long VisitorId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public string? MaterialType { get; private set; }
    public string? Notes { get; private set; }
    public char Status { get; private set; }
    public DateTime EnteredOn { get; private set; }
    public long EnteredBy { get; private set; }

    private VisitorItem() { }

    public static VisitorItem Create(
        long id,
        long visitorId,
        string description,
        int quantity,
        string? materialType,
        string? notes,
        long enteredBy)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Item description is required.", nameof(description));
        if (quantity <= 0)
            throw new ArgumentException("Item quantity must be greater than zero.", nameof(quantity));

        return new VisitorItem
        {
            Id = id,
            VisitorId = visitorId,
            Description = description.Trim(),
            Quantity = quantity,
            MaterialType = materialType?.Trim(),
            Notes = notes?.Trim(),
            Status = 'A',
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy
        };
    }
}
