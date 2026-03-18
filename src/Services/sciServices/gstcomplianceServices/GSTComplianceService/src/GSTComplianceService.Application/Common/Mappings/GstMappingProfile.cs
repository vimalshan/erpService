using AutoMapper;
using GSTComplianceService.Application.Common.DTOs;
using GSTComplianceService.Domain.Entities;

namespace GSTComplianceService.Application.Common.Mappings;

public class GstMappingProfile : Profile
{
    public GstMappingProfile()
    {
        CreateMap<GstMain, GstMainDto>()
            .ConstructUsing((src, ctx) => new GstMainDto(
                src.GstId, src.GstType, src.GstPanNo, src.GstEmailId, src.GstMobileNo,
                src.GstCreatedOn, src.GstModifiedOn, src.GstVendorName, src.GstVendAddLine1,
                src.GstVendCity, src.GstVendState, src.GstVendPincode, src.GstRegistrationType,
                src.GstContactName, src.GstContactEmailId, src.GstContactMobileNo, src.GstRemarks,
                src.GstStatus, src.GstDigitalFlag, src.GstGstnCopy,
                ctx.Mapper.Map<List<GstHsnDetailDto>>(src.HsnDetails),
                ctx.Mapper.Map<List<GstServiceDetailDto>>(src.ServiceDetails),
                ctx.Mapper.Map<List<GstStateRegDetailDto>>(src.StateRegDetails)
            ));

        CreateMap<GstHsnDetail, GstHsnDetailDto>()
            .ConstructUsing(src => new GstHsnDetailDto(
                src.GstHsnId, src.GstHsnGstId, src.GstHsnProductName,
                src.GstHsnCode, src.GstHsnRemarks));

        CreateMap<GstServiceDetail, GstServiceDetailDto>()
            .ConstructUsing(src => new GstServiceDetailDto(
                src.GstSacId, src.GstSacGstId, src.GstSacServiceName,
                src.GstSacCode, src.GstSacRemarks));

        CreateMap<GstStateRegDetail, GstStateRegDetailDto>()
            .ConstructUsing(src => new GstStateRegDetailDto(
                src.GstTinId, src.GstId, src.GstState, src.GstAddress,
                src.GstGstinNo, src.GstTinNo, src.GstContactPerson,
                src.GstEmailId, src.GstMobileNo, src.GstRemarks));
    }
}
