using MediatR;
using VendorService.Domain.Interfaces;

namespace VendorService.Application.Commands;

public sealed class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand, bool>
{
    private readonly IVendorRepository _repository;

    public UpdateVendorCommandHandler(IVendorRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _repository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor is null) return false;

        vendor.Update(
            categoryId: request.CategoryId,
            locationId: request.LocationId,
            name: request.Name,
            email: request.Email,
            address: request.Address,
            updatedBy: request.UpdatedBy,
            liveStatus: request.LiveStatus);

        _repository.Update(vendor);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
