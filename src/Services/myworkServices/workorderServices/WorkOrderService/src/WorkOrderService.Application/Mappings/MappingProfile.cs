using AutoMapper;
using WorkOrderService.Application.DTOs;
using WorkOrderService.Domain.Entities;

namespace WorkOrderService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<WorkOrder, WorkOrderDto>()
            .ForMember(d => d.WorkOrderStatus, opt => opt.MapFrom(s => s.WorkOrderStatus.Name))
            .ForMember(d => d.WorkOrderStatusCode, opt => opt.MapFrom(s => s.WorkOrderStatus.Code))
            .ForMember(d => d.CompletionPercentage, opt => opt.MapFrom(s => s.GetCompletionPercentage()))
            .ForMember(d => d.Tasks, opt => opt.MapFrom(s => s.Tasks));

        CreateMap<WorkTask, WorkTaskDto>()
            .ForMember(d => d.TaskStatus, opt => opt.MapFrom(s => s.TaskStatus.Name))
            .ForMember(d => d.TaskStatusCode, opt => opt.MapFrom(s => s.TaskStatus.Code));
    }
}
