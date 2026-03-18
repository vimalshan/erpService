using AutoMapper;
using OtherService.Application.DTOs;
using OtherService.Domain.Entities;

namespace OtherService.Application.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LogDdCatDevDetail, LogDdCatDevDetailDto>()
            .ConstructUsing(src => new LogDdCatDevDetailDto(
                src.ReqNum,
                src.QtnNum,
                src.AnsSrl,
                src.AppId,
                src.AppNum,
                src.EntDat,
                src.Desc,
                src.Need));
    }
}
