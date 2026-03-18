using AutoMapper;
using DealTicketing.Application.DTOs;
using DealTicketing.Domain.Entities;

namespace DealTicketing.Application.Mappings;

public class DealMappingProfile : Profile
{
    public DealMappingProfile()
    {
        CreateMap<Bank, BankDto>().ReverseMap();

        CreateMap<LovMaster, LovMasterDto>().ReverseMap();

        CreateMap<DealBatch, DealBatchDto>()
            .ForCtorParam("BankName", opt => opt.MapFrom(s => s.Bank != null ? s.Bank.BankName : null))
            .ForCtorParam("DealCount", opt => opt.MapFrom(s => s.DealDetails.Count));

        CreateMap<DealDetail, DealDetailDto>()
            .ForCtorParam("BankName", opt => opt.MapFrom(s => s.Bank != null ? s.Bank.BankName : null));

        CreateMap<DealSettlement, DealSettlementDto>();
    }
}
