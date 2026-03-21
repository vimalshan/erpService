using AutoMapper;
using EnergyService.Domain.Entities;
using EnergyService.Application.DTOs;

namespace EnergyService.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<EcProcess, EcProcessDto>();
        CreateMap<EcReading, EcReadingDto>();
        CreateMap<EcProcessAccess, EcProcessAccessDto>();
        CreateMap<EcProcessMailId, EcProcessMailIdDto>();
    }
}
