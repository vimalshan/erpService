using AutoMapper;
using StipendService.Application.DTOs;
using StipendService.Domain.Entities;

namespace StipendService.Application.Mappings;

public class StipendMappingProfile : Profile
{
    public StipendMappingProfile()
    {
        CreateMap<StipendMaster, StipendMasterDto>()
            .ConstructUsing(src => new StipendMasterDto(
                src.Id,
                src.ResearchCategoryId,
                src.SrfRankId,
                src.SrfMonthlyStipend,
                src.AdditionalAllowance,
                src.EffectiveFrom,
                src.EffectiveTo,
                src.Status,
                src.CreatedBy,
                src.CreatedOn,
                src.UpdatedBy,
                src.UpdatedOn));

        CreateMap<StipendDisbursement, StipendDisbursementDto>()
            .ConstructUsing(src => new StipendDisbursementDto(
                src.Id,
                src.SrfId,
                src.StipendId,
                src.DisbursementDate,
                src.DisbursementAmount,
                src.DisbursementStatus,
                src.MonthYear,
                src.BankReference,
                src.ReferenceNo,
                src.CreatedBy,
                src.CreatedOn));
    }
}
