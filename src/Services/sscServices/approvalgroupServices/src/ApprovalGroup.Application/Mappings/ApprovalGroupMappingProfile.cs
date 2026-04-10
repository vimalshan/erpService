using AutoMapper;
using ApprovalGroup.Domain.Entities;
using ApprovalGroup.Application.DTOs;

namespace ApprovalGroup.Application.Mappings;

public class ApprovalGroupMappingProfile : Profile
{
    public ApprovalGroupMappingProfile()
    {
        CreateMap<ApprovalGroupMaster, ApprovalGroupDto>()
            .ForMember(d => d.GroupMaps, opt => opt.MapFrom(s => s.GroupMaps))
            .ForMember(d => d.UserMaps, opt => opt.MapFrom(s => s.UserMaps));

        CreateMap<ApprovalGroupMap, ApprovalGroupMapDto>()
            .ForMember(d => d.MapCurrency, opt => opt.MapFrom(s => s.MapCurrency.HasValue ? s.MapCurrency.Value.ToString() : null))
            .ForMember(d => d.UnitMaps, opt => opt.MapFrom(s => s.UnitMaps))
            .ForMember(d => d.PayByMaps, opt => opt.MapFrom(s => s.PayByMaps))
            .ForMember(d => d.MainCatMaps, opt => opt.MapFrom(s => s.MainCatMaps));

        CreateMap<ApprovalGroupUnitMap, ApprovalGroupUnitMapDto>();
        CreateMap<ApprovalGroupPayBy, ApprovalGroupPayByDto>();
        CreateMap<ApprovalGroupMainCatMap, ApprovalGroupMainCatMapDto>();
        CreateMap<ApprovalGroupUserMap, ApprovalGroupUserMapDto>();
        CreateMap<PullMatrixDetail, PullMatrixDetailDto>()
            .ForMember(d => d.MatFlag, opt => opt.MapFrom(s => s.MatFlag.ToString()));
    }
}
