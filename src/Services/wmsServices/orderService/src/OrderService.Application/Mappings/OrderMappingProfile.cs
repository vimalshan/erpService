using AutoMapper;
using OrderService.Domain.Aggregates;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, DTOs.OrderDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString().ToUpperInvariant()));

        CreateMap<OrderItem, DTOs.OrderItemDto>();
    }
}
