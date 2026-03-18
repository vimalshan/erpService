using AutoMapper;
using LocationService.Application.DTOs;
using LocationService.Domain.Aggregates;

namespace LocationService.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for entity mappings
    /// </summary>
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            // Location mappings
            CreateMap<LocationAggregate, LocationDto>()
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LocationStatus, opt => opt.MapFrom(src => src.LocationStatus.Value))
                .ForMember(dest => dest.StreetAddress, opt => opt.MapFrom(src => src.Address.StreetAddress))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Address.State))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Contact.Phone))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contact.Email))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.Contact.ContactPerson))
                .ReverseMap();

            // Room mappings
            CreateMap<RoomAggregate, RoomDto>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.RoomStatus.Value))
                .ReverseMap();

            // Room Resource mappings
            CreateMap<RoomResourceAggregate, RoomResourceDto>()
                .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ResourceStatus, opt => opt.MapFrom(src => src.ResourceStatus.Value))
                .ReverseMap();
        }
    }
}
