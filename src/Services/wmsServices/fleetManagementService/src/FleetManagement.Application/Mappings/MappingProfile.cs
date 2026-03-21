using AutoMapper;
using FleetManagement.Application.DTOs;
using FleetManagement.Domain.Entities;

namespace FleetManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(d => d.VehicleType, o => o.MapFrom(s => s.VehicleType.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<Driver, DriverDto>();

        CreateMap<Route, RouteDto>();

        CreateMap<Trip, TripDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Stops, o => o.MapFrom(s => s.Stops));

        CreateMap<TripStop, TripStopDto>();

        CreateMap<MaintenanceLog, MaintenanceLogDto>();

        CreateMap<FuelLog, FuelLogDto>();
    }
}
