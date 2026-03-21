using AutoMapper;
using SalesOrderService.Application.SalesOrders.DTOs;
using SalesOrderService.Domain.Entities;

namespace SalesOrderService.Application.Common.Mappings;

public sealed class SalesOrderMappingProfile : Profile
{
    public SalesOrderMappingProfile()
    {
        CreateMap<SalesOrder, SalesOrderDto>()
            .ForMember(d => d.SoId,          o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Status,        o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.TotalAmount,   o => o.MapFrom(s => s.TotalAmount != null ? s.TotalAmount.Amount : (decimal?)null))
            .ForMember(d => d.Lines,         o => o.MapFrom(s => s.Lines));

        CreateMap<SalesOrderLine, SalesOrderLineDto>()
            .ForMember(d => d.LineTotal, o => o.MapFrom(s => s.LineTotal));

        CreateMap<SalesOrder, SalesOrderSummaryDto>()
            .ForMember(d => d.SoId,        o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Status,      o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount != null ? s.TotalAmount.Amount : (decimal?)null));
    }
}
