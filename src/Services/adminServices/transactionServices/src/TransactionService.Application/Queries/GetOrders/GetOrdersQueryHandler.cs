namespace TransactionService.Application.Queries.GetOrders;

using AutoMapper;
using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

public sealed class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderSummaryDto>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetAllOrdersQueryHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderSummaryDto>> Handle(
        GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = request.LocationId.HasValue
            ? await _repository.GetByLocationAsync(request.LocationId.Value, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
    }
}

public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderMainDto?>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OrderMainDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdWithDetailsAsync(request.OrderMainId, cancellationToken);
        return order is null ? null : _mapper.Map<OrderMainDto>(order);
    }
}

public sealed class GetOrdersByVendorQueryHandler : IRequestHandler<GetOrdersByVendorQuery, IEnumerable<OrderSummaryDto>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetOrdersByVendorQueryHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderSummaryDto>> Handle(
        GetOrdersByVendorQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetByVendorAsync(request.VendorId, cancellationToken);
        return _mapper.Map<IEnumerable<OrderSummaryDto>>(orders);
    }
}
