using MediatR;
using SupplierService.Application.DTOs;

namespace SupplierService.Application.Features.Suppliers.Queries;

public record GetAllSuppliersQuery : IRequest<IReadOnlyList<SupplierDto>>;
