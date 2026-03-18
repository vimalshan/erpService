using AutoMapper;
using CardManagement.Application.Common.DTOs;
using CardManagement.Domain.Entities;

namespace CardManagement.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GuestCardMaster, GuestCardDto>()
            .ConstructUsing(src => new GuestCardDto(
                src.CanteenUnit, src.CardSequence, src.CardNumber, src.CardName,
                src.ReportingUnit, src.ReportingDepartment, src.CardType,
                src.EffectiveDate, src.ClosingDate, src.IsActive));

        CreateMap<CanteenCardMap, CanteenCardMapDto>()
            .ConstructUsing(src => new CanteenCardMapDto(
                src.SysId, src.CanteenUnit, src.CardNumber,
                src.EffectiveDate, src.ClosingDate, src.UpdatedDate));

        CreateMap<CardSettlement, CardSettlementDto>()
            .ConstructUsing(src => new CardSettlementDto(
                src.SysId, src.CanteenUnit, src.CardNumber,
                src.SettlementDate, src.UpdatedDate));
    }
}
