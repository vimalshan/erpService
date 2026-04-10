using AutoMapper;
using TaskTransactional.Application.DTOs;
using TaskTransactional.Domain.Entities;

namespace TaskTransactional.Application.Mappings;

public class ComplaintMappingProfile : Profile
{
    public ComplaintMappingProfile()
    {
        CreateMap<ComplaintMain, ComplaintMainDto>();
        CreateMap<ComplaintDetail, ComplaintDetailDto>();
        CreateMap<ComplaintTask, ComplaintTaskDto>();
        CreateMap<ComplaintAction, ComplaintActionDto>();
        CreateMap<ComplaintHistory, ComplaintHistoryDto>();
        CreateMap<ComplaintEscalation, ComplaintEscalationDto>();
    }
}
