using ConfigService.Application.DTOs;
using MediatR;

namespace ConfigService.Application.Features.Vendors.Commands;

public record CreateVendorCommand(string VendorId, string VendorName, string ActiveStatus, string VendorCode,
    string ContactPerson, string Address1, string Address2, string Address3, string Address4,
    string PinCode, string EmailId, string CcEmailId, string SrfTriggerId,
    string MobileNo, string PhoneNos, string VendorType, string SubType) : IRequest<VendorDto>;

public record UpdateVendorCommand(string VendorId, string VendorName, string ActiveStatus,
    string ContactPerson, string EmailId, string MobileNo) : IRequest<VendorDto>;

public record DeleteVendorCommand(string VendorId) : IRequest<bool>;
