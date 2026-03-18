using AutoMapper;
using RequestServices.Application.DTOs;
using RequestServices.Domain.Entities;

namespace RequestServices.Application.Mappings;

public class RequestMappingProfile : Profile
{
    public RequestMappingProfile()
    {
        CreateMap<RequestMain, RequestMainDto>()
            .ConstructUsing(src => new RequestMainDto(
                src.RequestId, src.EmployeeUser, src.RequestDate, src.SupervisorUser,
                new List<RequestSubDto>()))
            .ForMember(dest => dest.SubRequests,
                opt => opt.MapFrom(src => src.SubRequests));

        CreateMap<RequestSub, RequestSubDto>()
            .ConstructUsing(src => new RequestSubDto(
                src.SerialNumber, src.TrainingNeed, src.StatusCode, src.CourseId,
                src.CourseDescription, src.StartDate, src.EndDate,
                src.BusinessBenefit, src.ExpectedCompetency,
                src.CancellationDate, src.CancellationRemark));

        CreateMap<RequestApp, RequestAppDto>()
            .ConstructUsing(src => new RequestAppDto(
                src.RequestId, src.SerialNumber, src.ApprovalDate,
                src.ApprovalNumber, src.ApprovalRemark, src.ApprovalUser));
    }
}
