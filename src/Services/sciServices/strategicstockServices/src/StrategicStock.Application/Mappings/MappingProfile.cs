using AutoMapper;
using StrategicStock.Application.DTOs;
using StrategicStock.Domain.Entities;

namespace StrategicStock.Application.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StrategicStockEntity, StrategicStockDto>()
            .ForMember(d => d.StrategicStockId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.StrategicStockType, o => o.MapFrom(s => s.StockType != null ? s.StockType.Code : null))
            .ForMember(d => d.MaxQty, o => o.MapFrom(s => s.MaxQty != null ? (long?)s.MaxQty.Value : null))
            .ForMember(d => d.FilledQty, o => o.MapFrom(s => s.FilledQty != null ? (long?)s.FilledQty.Value : null));
    }
}
