using MediatR;

namespace SupplierService.Application.Features.Suppliers.Commands;

public record DeactivateSupplierCommand(int SupplierId) : IRequest<Unit>;
