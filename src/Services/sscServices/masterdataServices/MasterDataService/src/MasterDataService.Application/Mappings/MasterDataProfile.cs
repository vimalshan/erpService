using AutoMapper;
using MasterDataService.Domain.Entities;
using MasterDataService.Application.DTOs;

namespace MasterDataService.Application.Mappings;

public class MasterDataProfile : Profile
{
    public MasterDataProfile()
    {
        CreateMap<LovMaster, LovMasterDto>()
            .ForCtorParam("LovId", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("LovType", opt => opt.MapFrom(s => s.LovType))
            .ForCtorParam("LovName", opt => opt.MapFrom(s => s.LovName));

        CreateMap<LovTypeMaster, LovTypeMasterDto>()
            .ForCtorParam("TypeCode", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("TypeName", opt => opt.MapFrom(s => s.LovTypeName));

        CreateMap<HoldTypeMaster, HoldTypeMasterDto>()
            .ForCtorParam("HoldId", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("HoldName", opt => opt.MapFrom(s => s.HoldName))
            .ForCtorParam("HoldCategory", opt => opt.MapFrom(s => s.HoldCategory));

        CreateMap<LocationScanParam, LocationScanParamDto>()
            .ForCtorParam("ParamId", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("LocationId", opt => opt.MapFrom(s => s.LocationId))
            .ForCtorParam("EffectiveDate", opt => opt.MapFrom(s => s.EffectivePeriod.EffectiveDate))
            .ForCtorParam("ClosingDate", opt => opt.MapFrom(s => s.EffectivePeriod.ClosingDate));

        CreateMap<ScannerMaster, ScannerMasterDto>()
            .ForCtorParam("DeviceId", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("DeviceName", opt => opt.MapFrom(s => s.DeviceName))
            .ForCtorParam("DeviceLocationId", opt => opt.MapFrom(s => s.DeviceLocationId))
            .ForCtorParam("DevicePath", opt => opt.MapFrom(s => s.DevicePath.Value));
    }
}
