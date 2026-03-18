using AutoMapper;
using ObjectiveService.Domain.Entities;
using ObjectiveService.Application.DTOs;

namespace ObjectiveService.Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ControlPoint mappings
        CreateMap<ControlPoint, ControlPointDto>().ReverseMap();

        // Goal mappings
        CreateMap<Goal, GoalDto>()
            .ForMember(dest => dest.SubGoals, opt => opt.MapFrom(src => src.SubGoals));
        
        CreateMap<GoalSubGoal, GoalSubGoalDto>().ReverseMap();
    }
}
