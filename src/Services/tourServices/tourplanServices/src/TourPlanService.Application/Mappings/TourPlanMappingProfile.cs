using AutoMapper;
using TourPlanService.Application.DTOs;
using TourPlanService.Domain.Entities;

namespace TourPlanService.Application.Mappings;

public sealed class TourPlanMappingProfile : Profile
{
    public TourPlanMappingProfile()
    {
        CreateMap<TourPlan, TourPlanDto>()
            .ForMember(d => d.Advances, opt => opt.MapFrom(s => s.Advances))
            .ForMember(d => d.Agendas, opt => opt.MapFrom(s => s.Agendas))
            .ForMember(d => d.Expenses, opt => opt.MapFrom(s => s.Expenses));

        CreateMap<TourPlan, TourPlanSummaryDto>();
        CreateMap<TourAdvance, TourAdvanceDto>();
        CreateMap<TourAgenda, TourAgendaDto>();
        CreateMap<TourExpense, TourExpenseDto>();
        CreateMap<ForexRequisition, ForexRequisitionDto>();
    }
}
