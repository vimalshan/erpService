using MediatR;

namespace VendorService.Application.Commands;

public sealed record DeactivateVendorCommand(long VendorId, long UpdatedBy) : IRequest<bool>;
