using AutoMapper;
using VendorService.Application.DTOs;
using VendorService.Domain.Entities;

namespace VendorService.Application.Mappings;

public sealed class VendorMappingProfile : Profile
{
    public VendorMappingProfile()
    {
        CreateMap<VendorMaster, VendorDto>()
            .ConstructUsing(src => new VendorDto(
                src.Id,
                src.CategoryId,
                src.LocationId,
                src.Name.Value,
                src.Email != null ? src.Email.Value : null,
                src.Address.Value,
                src.UpdatedBy,
                src.UpdatedOn,
                src.LiveStatus.Value.ToString()));

        CreateMap<TdsVendor, TdsVendorDto>()
            .ConstructUsing(src => new TdsVendorDto(
                src.VendorId,
                src.VendorName,
                src.EmailAddress,
                src.PanNo));

        CreateMap<TdsFileDetail, TdsFileDetailDto>()
            .ConstructUsing(src => new TdsFileDetailDto(
                src.FileId,
                src.FileName,
                src.PanNo,
                src.EmailStatus,
                src.FileType));
    }
}
