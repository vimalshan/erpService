using MediatR;
using VendorService.Domain.Entities;
using VendorService.Domain.Interfaces;

namespace VendorService.Application.Commands;

public sealed class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, long>
{
    private readonly IVendorRepository _repository;

    public CreateVendorCommandHandler(IVendorRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        // Use stored procedure to generate ID and persist via Dapper
        var vendorId = await _repository.AddUpdateVendorSpAsync(
            vendorId: null,
            categoryId: request.CategoryId,
            locationId: request.LocationId,
            name: request.Name,
            email: request.Email,
            address: request.Address,
            updatedBy: request.UpdatedBy,
            liveStatus: request.LiveStatus,
            ct: cancellationToken);

        return vendorId;
    }
}
