using ArchiveService.Application.DTOs;
using AutoMapper;
using ArchiveService.Domain.Entities;

namespace ArchiveService.Application.Mappings;

public class ArchiveMappingProfile : Profile
{
    public ArchiveMappingProfile()
    {
        CreateMap<ArchivedServiceOrder, ServiceOrderDto>()
            .ForMember(d => d.EngineerId, o => o.MapFrom(s => s.Engineer.EngineerId))
            .ForMember(d => d.EngineerName, o => o.MapFrom(s => s.Engineer.EngineerName))
            .ForMember(d => d.EngMobNo, o => o.MapFrom(s => s.Engineer.MobileNo))
            .ForMember(d => d.ContactNo, o => o.MapFrom(s => s.Contact.ContactNo))
            .ForMember(d => d.AltContactNo, o => o.MapFrom(s => s.Contact.AltContactNo))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address != null ? s.Address.FullAddress : null));

        CreateMap<ArchivedServiceOrderDetail, ServiceOrderDetailDto>();

        CreateMap<ArchivedToolKit, ToolKitDto>();
        CreateMap<ArchivedToolKitTransaction, ToolKitTransactionDto>();
    }
}
