using MediatR;

namespace TdsService.Application.Vendors.Commands.CreateTdsVendor;

public sealed record CreateTdsVendorCommand(
    long VendorId,
    string VendorName,
    string? EmailAddress,
    string? PanNo) : IRequest<long>;
