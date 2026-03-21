using AutoMapper;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Mappings;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<StockLevel, StockLevelDto>();
        CreateMap<InventoryTransaction, InventoryTransactionDto>();
    }
}
