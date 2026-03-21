using MediatR;

namespace SupplierService.Application.Features.Suppliers.Commands;

public record DeleteSupplierCommand(int SupplierId) : IRequest<Unit>;
