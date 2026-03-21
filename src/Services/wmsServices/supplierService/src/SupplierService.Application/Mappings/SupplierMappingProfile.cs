using AutoMapper;
using SupplierService.Application.DTOs;
using SupplierService.Domain.Entities;

namespace SupplierService.Application.Mappings;

public class SupplierMappingProfile : Profile
{
    public SupplierMappingProfile()
    {
        CreateMap<Supplier, SupplierDto>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, opt => opt.MapFrom(s => s.Address.City))
            .ForMember(d => d.State, opt => opt.MapFrom(s => s.Address.State))
            .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Address.Country))
            .ForMember(d => d.PostalCode, opt => opt.MapFrom(s => s.Address.PostalCode));
    }
}
