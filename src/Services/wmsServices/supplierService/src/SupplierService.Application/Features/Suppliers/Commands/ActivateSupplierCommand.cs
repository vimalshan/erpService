using MediatR;

namespace SupplierService.Application.Features.Suppliers.Commands;

public record ActivateSupplierCommand(int SupplierId) : IRequest<Unit>;
