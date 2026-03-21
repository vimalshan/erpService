using AutoMapper;
using MediatR;
using OrderService.Application.DTOs;
using OrderService.Domain.Aggregates;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Commands.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.Request.CustomerId, request.Request.CreatedBy, request.Request.RequiredDate);

        foreach (var item in request.Request.Items)
        {
            order.AddItem(item.ProductId, item.Quantity, item.UnitPrice, item.Discount, item.Notes);
        }

        var created = await _repository.AddAsync(order, cancellationToken);
        return _mapper.Map<OrderDto>(created);
    }
}
