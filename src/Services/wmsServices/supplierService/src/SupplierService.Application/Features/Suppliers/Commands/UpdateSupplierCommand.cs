using MediatR;
using SupplierService.Application.DTOs;

namespace SupplierService.Application.Features.Suppliers.Commands;

public record UpdateSupplierCommand(int SupplierId, UpdateSupplierDto Supplier) : IRequest<SupplierDto>;
