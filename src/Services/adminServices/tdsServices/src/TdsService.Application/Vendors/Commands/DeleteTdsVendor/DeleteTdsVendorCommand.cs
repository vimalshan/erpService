using MediatR;

namespace TdsService.Application.Vendors.Commands.DeleteTdsVendor;

public sealed record DeleteTdsVendorCommand(long VendorId) : IRequest;
