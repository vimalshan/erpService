using AutoMapper;
using TravelRequestService.Domain.Entities;
using TravelRequestService.Domain.Enums;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Mappings;

public class TravelRequestMappingProfile : Profile
{
    public TravelRequestMappingProfile()
    {
        CreateMap<TravelMain, TravelRequestDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.TravelType, opt => opt.MapFrom(s => s.TravelType.ToString()));

        CreateMap<TravelSub, TravelSubDto>();
        CreateMap<TravelAgenda, TravelAgendaDto>();
        CreateMap<TravelAdvance, TravelAdvanceDto>();
        CreateMap<TravelApprovalRemark, TravelApprovalRemarkDto>();
        CreateMap<DashTourPlan, DashTourPlanDto>();
    }
}
