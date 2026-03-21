using AutoMapper;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Entities;

namespace CustomerService.Application.Mappings;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.ContactPerson, opt => opt.MapFrom(s => s.Contact.ContactPerson))
            .ForMember(d => d.ContactTitle, opt => opt.MapFrom(s => s.Contact.ContactTitle))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Contact.Email))
            .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Contact.Phone))
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, opt => opt.MapFrom(s => s.Address.City))
            .ForMember(d => d.State, opt => opt.MapFrom(s => s.Address.State))
            .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Address.Country))
            .ForMember(d => d.PostalCode, opt => opt.MapFrom(s => s.Address.PostalCode));
    }
}
