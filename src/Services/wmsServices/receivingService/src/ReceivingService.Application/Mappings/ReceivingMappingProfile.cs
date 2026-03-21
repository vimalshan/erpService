using AutoMapper;
using ReceivingService.Application.DTOs;
using ReceivingService.Domain.Entities;

namespace ReceivingService.Application.Mappings;

public sealed class ReceivingMappingProfile : Profile
{
    public ReceivingMappingProfile()
    {
        CreateMap<Domain.Entities.Receiving, ReceivingDto>()
            .ConstructUsing((src, ctx) => new ReceivingDto(
                src.Id,
                src.ReceivingNumber,
                src.PoId,
                src.WarehouseId,
                src.ReceivedDate,
                src.Status,
                src.Notes,
                src.CreatedBy,
                src.CreatedDate,
                ctx.Mapper.Map<List<ReceivingLineDto>>(src.Lines)
            ));

        CreateMap<ReceivingLine, ReceivingLineDto>()
            .ConstructUsing(src => new ReceivingLineDto(
                src.Id,
                src.ReceivingId,
                src.PoLineId,
                src.ProductId,
                src.BinId,
                src.QuantityReceived,
                src.LotNumber,
                src.ExpiryDate,
                src.Notes
            ));
    }
}
