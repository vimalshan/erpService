using AutoMapper;
using travelTransactionService.Application.DTOs;
using travelTransactionService.Domain.Entities;

namespace travelTransactionService.Application.Mappings;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<VendorMaster, VendorMasterDto>();
        CreateMap<AccountMaster, AccountMasterDto>();
        CreateMap<GlCodeCombination, GlCodeCombinationDto>();
        CreateMap<TaxMaster, TaxMasterDto>();
        CreateMap<TaxComponent, TaxComponentDto>();
        CreateMap<JvInterface, JvInterfaceDto>();
        CreateMap<JvMissingCombiCode, JvMissingCombiCodeDto>();
        CreateMap<JaiInterfaceLine, JaiInterfaceLineDto>();
        CreateMap<JaiInterfaceTaxLine, JaiInterfaceTaxLineDto>();
        CreateMap<BatchSubBreakup, BatchSubBreakupDto>();
        CreateMap<TravelApParams, TravelApParamsDto>();
        CreateMap<SourceHistory, SourceHistoryDto>();
    }
}
