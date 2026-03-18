using MediatR;

namespace VendorService.Application.Commands;

public sealed record UpdateVendorCommand(
    long VendorId,
    long CategoryId,
    long LocationId,
    string Name,
    string? Email,
    string Address,
    long UpdatedBy,
    char LiveStatus) : IRequest<bool>;
