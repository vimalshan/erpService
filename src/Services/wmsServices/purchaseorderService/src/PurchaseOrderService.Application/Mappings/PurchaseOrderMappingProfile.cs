using AutoMapper;
using PurchaseOrderService.Application.DTOs;
using PurchaseOrderService.Domain.Entities;
using PurchaseOrderService.Domain.Enums;

namespace PurchaseOrderService.Application.Mappings;

public class PurchaseOrderMappingProfile : Profile
{
    public PurchaseOrderMappingProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderDto>()
            .ForMember(d => d.PoId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToDbString()))
            .ForMember(d => d.TotalAmount, opt => opt.MapFrom(s => s.TotalAmount));

        CreateMap<PurchaseOrderLine, PurchaseOrderLineDto>()
            .ForMember(d => d.PoLineId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.LineTotal, opt => opt.MapFrom(s => s.LineTotal))
            .ForMember(d => d.IsFullyReceived, opt => opt.MapFrom(s => s.IsFullyReceived));
    }
}
