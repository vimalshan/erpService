using ArchiveService.Application.DTOs;
using AutoMapper;
using ArchiveService.Domain.Interfaces;
using MediatR;

namespace ArchiveService.Application.Features.ServiceOrders.Queries;

public class GetServiceOrderByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetServiceOrderByIdQuery, ServiceOrderDto?>
{
    public async Task<ServiceOrderDto?> Handle(GetServiceOrderByIdQuery request, CancellationToken ct)
    {
        var order = await unitOfWork.ServiceOrders.GetByIdAsync(request.SernoDell, ct);
        return order is null ? null : mapper.Map<ServiceOrderDto>(order);
    }
}

public class GetServiceOrdersPagedHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetServiceOrdersPagedQuery, PagedResult<ServiceOrderDto>>
{
    public async Task<PagedResult<ServiceOrderDto>> Handle(GetServiceOrdersPagedQuery request, CancellationToken ct)
    {
        var orders = await unitOfWork.ServiceOrders.GetAllAsync(request.Page, request.PageSize, ct);
        var count = await unitOfWork.ServiceOrders.GetCountAsync(ct);

        return new PagedResult<ServiceOrderDto>
        {
            Items = mapper.Map<IReadOnlyList<ServiceOrderDto>>(orders),
            TotalCount = count,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}

public class SearchServiceOrdersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<SearchServiceOrdersQuery, IReadOnlyList<ServiceOrderDto>>
{
    public async Task<IReadOnlyList<ServiceOrderDto>> Handle(SearchServiceOrdersQuery request, CancellationToken ct)
    {
        var orders = await unitOfWork.ServiceOrders.SearchAsync(
            request.Branch, request.EngineerId, request.CallStatus,
            request.FromDate, request.ToDate, ct);
        return mapper.Map<IReadOnlyList<ServiceOrderDto>>(orders);
    }
}
