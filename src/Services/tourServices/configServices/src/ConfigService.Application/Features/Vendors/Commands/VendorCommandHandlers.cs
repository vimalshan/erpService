using ConfigService.Application.DTOs;
using ConfigService.Domain.Common;
using ConfigService.Domain.Entities;
using ConfigService.Domain.Repositories;
using MediatR;

namespace ConfigService.Application.Features.Vendors.Commands;

public class CreateVendorHandler(IVendorRepository repo, IUnitOfWork uow) : IRequestHandler<CreateVendorCommand, VendorDto>
{
    public async Task<VendorDto> Handle(CreateVendorCommand r, CancellationToken ct)
    {
        var vendor = Vendor.Create(r.VendorId, r.VendorName, r.ActiveStatus, r.VendorCode,
            r.ContactPerson, r.Address1, r.Address2, r.Address3, r.Address4,
            r.PinCode, r.EmailId, r.CcEmailId, r.SrfTriggerId,
            r.MobileNo, r.PhoneNos, r.VendorType, r.SubType);
        await repo.AddAsync(vendor, ct);
        await uow.SaveChangesAsync(ct);
        return new VendorDto(vendor.Id, vendor.VendorName, vendor.ActiveStatus, vendor.VendorCode,
            vendor.ContactPerson, vendor.Address1, vendor.Address2, vendor.Address3, vendor.Address4,
            vendor.PinCode, vendor.EmailId, vendor.CcEmailId, vendor.SrfTriggerId,
            vendor.MobileNo, vendor.PhoneNos, vendor.VendorType, vendor.SubType,
            vendor.DirectMail, vendor.UserId, vendor.GstNo);
    }
}

public class UpdateVendorHandler(IVendorRepository repo, IUnitOfWork uow) : IRequestHandler<UpdateVendorCommand, VendorDto>
{
    public async Task<VendorDto> Handle(UpdateVendorCommand r, CancellationToken ct)
    {
        var vendor = await repo.GetByIdAsync(r.VendorId, ct) ?? throw new KeyNotFoundException($"Vendor {r.VendorId} not found.");
        vendor.Update(r.VendorName, r.ActiveStatus, r.ContactPerson, r.EmailId, r.MobileNo);
        await repo.UpdateAsync(vendor, ct);
        await uow.SaveChangesAsync(ct);
        return new VendorDto(vendor.Id, vendor.VendorName, vendor.ActiveStatus, vendor.VendorCode,
            vendor.ContactPerson, vendor.Address1, vendor.Address2, vendor.Address3, vendor.Address4,
            vendor.PinCode, vendor.EmailId, vendor.CcEmailId, vendor.SrfTriggerId,
            vendor.MobileNo, vendor.PhoneNos, vendor.VendorType, vendor.SubType,
            vendor.DirectMail, vendor.UserId, vendor.GstNo);
    }
}

public class DeleteVendorHandler(IVendorRepository repo, IUnitOfWork uow) : IRequestHandler<DeleteVendorCommand, bool>
{
    public async Task<bool> Handle(DeleteVendorCommand r, CancellationToken ct)
    {
        var vendor = await repo.GetByIdAsync(r.VendorId, ct);
        if (vendor is null) return false;
        await repo.DeleteAsync(vendor, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
