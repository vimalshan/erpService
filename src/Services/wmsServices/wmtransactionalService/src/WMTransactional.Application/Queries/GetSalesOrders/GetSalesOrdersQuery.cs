using AutoMapper;
using MediatR;
using WMTransactional.Application.DTOs;
using WMTransactional.Domain.Interfaces;

namespace WMTransactional.Application.Queries.GetSalesOrders;

public record GetSalesOrdersQuery : IRequest<IEnumerable<SalesOrderDto>>
{
    public int? CustomerId { get; init; }
    public string? Status { get; init; }
}

public class GetSalesOrdersQueryHandler : IRequestHandler<GetSalesOrdersQuery, IEnumerable<SalesOrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSalesOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SalesOrderDto>> Handle(GetSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.SalesOrder> orders;

        if (request.CustomerId.HasValue)
            orders = await _unitOfWork.SalesOrders.GetByCustomerAsync(request.CustomerId.Value, cancellationToken);
        else if (!string.IsNullOrEmpty(request.Status))
            orders = await _unitOfWork.SalesOrders.GetByStatusAsync(request.Status, cancellationToken);
        else
            orders = await _unitOfWork.SalesOrders.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<SalesOrderDto>>(orders);
    }
}
