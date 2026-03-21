using MediatR;
using SupplierService.Application.DTOs;

namespace SupplierService.Application.Features.Suppliers.Queries;

public record GetSupplierByIdQuery(int SupplierId) : IRequest<SupplierDto?>;
