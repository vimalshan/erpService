using AutoMapper;
using Stationery.Application.DTOs;
using Stationery.Domain.Entities;

namespace Stationery.Application.Mappings;

public class StationeryMappingProfile : Profile
{
    public StationeryMappingProfile()
    {
        // Item mappings
        CreateMap<StationaryMaster, ItemDto>();
        CreateMap<StationaryMaster, ItemSummaryDto>();
        CreateMap<StationaryMaster, LowStockItemDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("Description", opt => opt.MapFrom(s => s.Description))
            .ForCtorParam("Stock", opt => opt.MapFrom(s => s.OpeningStock))
            .ForCtorParam("ReorderLevel", opt => opt.MapFrom(s => s.ReorderLevel ?? 0));

        // Request mappings
        CreateMap<RequestMain, RequestDto>()
            .ForMember(d => d.Details, opt => opt.MapFrom(s => s.Details));
        CreateMap<RequestMain, RequestSummaryDto>()
            .ForMember(d => d.TotalItems, opt => opt.MapFrom(s => s.Details.Count))
            .ForMember(d => d.PendingItems, opt => opt.MapFrom(s => s.Details.Count(d => d.Status == "P")))
            .ForMember(d => d.ApprovedItems, opt => opt.MapFrom(s => s.Details.Count(d => d.Status == "A")));
        CreateMap<RequestSub, RequestSubDto>();

        // Order mappings
        CreateMap<OrderMain, OrderDto>()
            .ForMember(d => d.Details, opt => opt.MapFrom(s => s.Details));
        CreateMap<OrderSub, OrderSubDto>();
    }
}
