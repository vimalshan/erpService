using MediatR;
using SupplierService.Application.DTOs;

namespace SupplierService.Application.Features.Suppliers.Commands;

public record CreateSupplierCommand(CreateSupplierDto Supplier) : IRequest<SupplierDto>;
