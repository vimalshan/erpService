namespace OrderScheduleService.Application.QueryHandlers;

using MediatR;
using AutoMapper;
using OrderScheduleService.Application.Queries;
using OrderScheduleService.Application.DTOs;
using OrderScheduleService.Domain.Interfaces;

public class GetTiedOrderByIdQueryHandler : IRequestHandler<GetTiedOrderByIdQuery, TiedOrderDto?>
{
    private readonly ITiedOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetTiedOrderByIdQueryHandler(ITiedOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TiedOrderDto?> Handle(GetTiedOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId);
        return _mapper.Map<TiedOrderDto>(order);
    }
}

public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<TiedOrderDto>>
{
    private readonly ITiedOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerQueryHandler(ITiedOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TiedOrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetByCustomerAsync(request.CustomerCode);
        return _mapper.Map<IEnumerable<TiedOrderDto>>(orders);
    }
}

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<TiedOrderDto>>
{
    private readonly ITiedOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetAllOrdersQueryHandler(ITiedOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TiedOrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TiedOrderDto>>(orders);
    }
}

public class GetOrderDetailsQueryHandler : IRequestHandler<GetOrderDetailsQuery, TiedOrderDetailDto?>
{
    private readonly ITiedOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetOrderDetailsQueryHandler(ITiedOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TiedOrderDetailDto?> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId);
        if (order == null) return null;

        var detail = order.Details.FirstOrDefault(d => d.Id == request.DetailId);
        return _mapper.Map<TiedOrderDetailDto>(detail);
    }
}

public class GetAllOrderDetailsQueryHandler : IRequestHandler<GetAllOrderDetailsQuery, IEnumerable<TiedOrderDetailDto>>
{
    private readonly ITiedOrderRepository _repository;
    private readonly IMapper _mapper;

    public GetAllOrderDetailsQueryHandler(ITiedOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TiedOrderDetailDto>> Handle(GetAllOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.OrderId);
        if (order == null) return Enumerable.Empty<TiedOrderDetailDto>();

        return _mapper.Map<IEnumerable<TiedOrderDetailDto>>(order.Details);
    }
}
