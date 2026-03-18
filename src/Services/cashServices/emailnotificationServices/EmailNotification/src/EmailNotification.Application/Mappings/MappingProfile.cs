using AutoMapper;

namespace EmailNotification.Application.Mappings;

/// <summary>
/// AutoMapper profile for EmailNotification mappings
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the MappingProfile class
    /// </summary>
    public MappingProfile()
    {
        // Domain to DTO mappings
        CreateMap<Domain.Aggregates.EmailTypeAggregate, Dtos.EmailTypeDto>()
            .ForMember(dest => dest.EmailType, opt => opt.MapFrom(src => src.EmailType.ToString()))
            .ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.MailAccessList));

        CreateMap<Domain.Entities.MailAccess, Dtos.MailAccessDto>()
            .ForMember(dest => dest.MailEmail, opt => opt.MapFrom(src => src.MailEmail.Value))
            .ForMember(dest => dest.RecipientType, opt => opt.MapFrom(src => src.MailEmpSysId.HasValue ? "Employee" : "External"))
            .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.ModifiedAt));

        CreateMap<Domain.ValueObjects.EmailAddress, Dtos.EmailAddressDto>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));

        // DTO to Domain mappings
        CreateMap<Dtos.EmailAddressDto, Domain.ValueObjects.EmailAddress>()
            .ConstructUsing(src => new Domain.ValueObjects.EmailAddress(src.Value));
    }
}
