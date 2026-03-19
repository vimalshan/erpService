using AutoMapper;
using IntegrationService.Application.DTOs;
using IntegrationService.Domain.Entities;
using IntegrationService.Domain.Exceptions;
using IntegrationService.Domain.Interfaces;
using MediatR;

namespace IntegrationService.Application.Vendors.Commands;

public class CreateVendorHandler(
    IVendorRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateVendorCommand, VendorDto>
{
    public async Task<VendorDto> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = Vendor.Create(request.VendorId, request.VendorName, request.VendorCode);
        await repository.AddAsync(vendor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<VendorDto>(vendor);
    }
}

public class UpdateVendorHandler(
    IVendorRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateVendorCommand, VendorDto>
{
    public async Task<VendorDto> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await repository.GetByIdAsync(request.VendorId, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Vendor), request.VendorId);

        vendor.UpdateDetails(request.VendorName, request.VendorCode);
        await repository.UpdateAsync(vendor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<VendorDto>(vendor);
    }
}

public class DeleteVendorHandler(
    IVendorRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteVendorCommand, bool>
{
    public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.VendorId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
