using MediatR;

namespace VendorService.Application.Commands;

public sealed record CreateVendorCommand(
    long CategoryId,
    long LocationId,
    string Name,
    string? Email,
    string Address,
    long UpdatedBy,
    char LiveStatus = 'A') : IRequest<long>;
