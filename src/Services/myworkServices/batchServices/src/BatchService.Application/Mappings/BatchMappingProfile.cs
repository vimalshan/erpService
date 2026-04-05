using AutoMapper;
using BatchService.Application.DTOs;
using BatchService.Domain.Entities;

namespace BatchService.Application.Mappings;

public sealed class BatchMappingProfile : Profile
{
    public BatchMappingProfile()
    {
        CreateMap<BatchMaster, BatchDto>()
            .ForMember(d => d.BatchStatus,      o => o.MapFrom(s => s.BatchStatusChar.ToString()))
            .ForMember(d => d.BatchStatusLabel, o => o.MapFrom(s => StatusLabel(s.BatchStatusChar)));
    }

    private static string StatusLabel(char c) => c switch
    {
        'O' => "Open",
        'C' => "Closed",
        'L' => "Locked",
        _   => "Unknown"
    };
}
