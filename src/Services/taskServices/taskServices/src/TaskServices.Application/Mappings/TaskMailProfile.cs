using AutoMapper;
using TaskServices.Application.DTOs;
using TaskServices.Domain.Entities;

namespace TaskServices.Application.Mappings;

public class TaskMailProfile : Profile
{
    public TaskMailProfile()
    {
        CreateMap<TaskMail, TaskMailDto>();
    }
}
