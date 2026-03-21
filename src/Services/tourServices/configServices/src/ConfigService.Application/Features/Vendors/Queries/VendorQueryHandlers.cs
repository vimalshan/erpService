using ConfigService.Application.DTOs;
using ConfigService.Domain.Repositories;
using MediatR;

namespace ConfigService.Application.Features.Vendors.Queries;

public class GetAllVendorsHandler(IVendorRepository repo) : IRequestHandler<GetAllVendorsQuery, IReadOnlyList<VendorDto>>
{
    public async Task<IReadOnlyList<VendorDto>> Handle(GetAllVendorsQuery request, CancellationToken ct)
    {
        var vendors = await repo.GetAllAsync(ct);
        return vendors.Select(MapToDto).ToList();
    }

    private static VendorDto MapToDto(Domain.Entities.Vendor v) =>
        new(v.Id, v.VendorName, v.ActiveStatus, v.VendorCode, v.ContactPerson,
            v.Address1, v.Address2, v.Address3, v.Address4, v.PinCode,
            v.EmailId, v.CcEmailId, v.SrfTriggerId, v.MobileNo, v.PhoneNos,
            v.VendorType, v.SubType, v.DirectMail, v.UserId, v.GstNo);
}

public class GetVendorByIdHandler(IVendorRepository repo) : IRequestHandler<GetVendorByIdQuery, VendorDto?>
{
    public async Task<VendorDto?> Handle(GetVendorByIdQuery request, CancellationToken ct)
    {
        var v = await repo.GetByIdAsync(request.Id, ct);
        return v is null ? null : new VendorDto(v.Id, v.VendorName, v.ActiveStatus, v.VendorCode, v.ContactPerson,
            v.Address1, v.Address2, v.Address3, v.Address4, v.PinCode,
            v.EmailId, v.CcEmailId, v.SrfTriggerId, v.MobileNo, v.PhoneNos,
            v.VendorType, v.SubType, v.DirectMail, v.UserId, v.GstNo);
    }
}

public class GetActiveVendorsHandler(IVendorRepository repo) : IRequestHandler<GetActiveVendorsQuery, IReadOnlyList<VendorDto>>
{
    public async Task<IReadOnlyList<VendorDto>> Handle(GetActiveVendorsQuery request, CancellationToken ct)
    {
        var vendors = await repo.GetActiveVendorsAsync(ct);
        return vendors.Select(v => new VendorDto(v.Id, v.VendorName, v.ActiveStatus, v.VendorCode, v.ContactPerson,
            v.Address1, v.Address2, v.Address3, v.Address4, v.PinCode,
            v.EmailId, v.CcEmailId, v.SrfTriggerId, v.MobileNo, v.PhoneNos,
            v.VendorType, v.SubType, v.DirectMail, v.UserId, v.GstNo)).ToList();
    }
}
