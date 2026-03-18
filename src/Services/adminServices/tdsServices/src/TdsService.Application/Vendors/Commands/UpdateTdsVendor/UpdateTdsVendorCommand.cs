using MediatR;

namespace TdsService.Application.Vendors.Commands.UpdateTdsVendor;

public sealed record UpdateTdsVendorCommand(
    long VendorId,
    string VendorName,
    string? EmailAddress,
    string? PanNo) : IRequest;
