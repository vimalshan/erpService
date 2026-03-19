using AutoMapper;
using MamAllocationService.Application.DTOs;
using MamAllocationService.Domain.Entities;

namespace MamAllocationService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AllocationDetail, AllocationDetailDto>().ReverseMap();
        CreateMap<AllocationDetail, AllocationSummaryDto>();
        CreateMap<AllocationProdDetail, AllocationProdDetailDto>().ReverseMap();
        CreateMap<AllocationFg, AllocationFgDto>().ReverseMap();
        CreateMap<ArrivalDetail, ArrivalDetailDto>().ReverseMap();
        CreateMap<ConsumptionDetail, ConsumptionDetailDto>().ReverseMap();
        CreateMap<DispatchDetail, DispatchDetailDto>().ReverseMap();
        CreateMap<FgAllocation, FgAllocationDto>().ReverseMap();
        CreateMap<ProductAllocation, ProductAllocationDto>().ReverseMap();
    }
}
