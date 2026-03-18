using MediatR;
using TdsService.Domain.Entities;
using TdsService.Domain.Repositories;

namespace TdsService.Application.Vendors.Commands.CreateTdsVendor;

public sealed class CreateTdsVendorCommandHandler
    : IRequestHandler<CreateTdsVendorCommand, long>
{
    private readonly ITdsVendorRepository _repository;

    public CreateTdsVendorCommandHandler(ITdsVendorRepository repository)
        => _repository = repository;

    public async Task<long> Handle(
        CreateTdsVendorCommand request,
        CancellationToken cancellationToken)
    {
        var vendor = TdsVendor.Create(
            request.VendorId,
            request.VendorName,
            request.EmailAddress,
            request.PanNo);

        await _repository.AddAsync(vendor, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return vendor.Id;
    }
}
