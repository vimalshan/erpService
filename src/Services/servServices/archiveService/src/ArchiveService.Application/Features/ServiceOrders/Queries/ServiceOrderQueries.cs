using ArchiveService.Application.DTOs;
using MediatR;

namespace ArchiveService.Application.Features.ServiceOrders.Queries;

public record GetServiceOrderByIdQuery(string SernoDell) : IRequest<ServiceOrderDto?>;

public record GetServiceOrdersPagedQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<ServiceOrderDto>>;

public record SearchServiceOrdersQuery(
    string? Branch,
    string? EngineerId,
    string? CallStatus,
    DateTime? FromDate,
    DateTime? ToDate) : IRequest<IReadOnlyList<ServiceOrderDto>>;
