using MassTransit;
using Microsoft.Extensions.Logging;

namespace VendorService.Infrastructure.Messaging;

public sealed record VendorCreatedMessage
{
    public long VendorId { get; init; }
    public string VendorName { get; init; } = default!;
    public string Address { get; init; } = default!;
    public long LocationId { get; init; }
    public long CategoryId { get; init; }
    public DateTime OccurredOn { get; init; }
}

public sealed record VendorUpdatedMessage
{
    public long VendorId { get; init; }
    public string VendorName { get; init; } = default!;
    public DateTime OccurredOn { get; init; }
}

public sealed record VendorStatusChangedMessage
{
    public long VendorId { get; init; }
    public char NewStatus { get; init; }
    public DateTime OccurredOn { get; init; }
}
