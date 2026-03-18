using MediatR;
using VendorService.Domain.Interfaces;

namespace VendorService.Application.Commands;

public sealed class DeactivateVendorCommandHandler : IRequestHandler<DeactivateVendorCommand, bool>
{
    private readonly IVendorRepository _repository;

    public DeactivateVendorCommandHandler(IVendorRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeactivateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _repository.GetByIdAsync(request.VendorId, cancellationToken);
        if (vendor is null) return false;

        vendor.Deactivate(request.UpdatedBy);
        _repository.Update(vendor);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
