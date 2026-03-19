using IntegrationService.Application.DTOs;
using MediatR;

namespace IntegrationService.Application.Vendors.Commands;

public record CreateVendorCommand(int VendorId, string VendorName, string VendorCode) : IRequest<VendorDto>;
public record UpdateVendorCommand(int VendorId, string VendorName, string VendorCode) : IRequest<VendorDto>;
public record DeleteVendorCommand(int VendorId) : IRequest<bool>;
