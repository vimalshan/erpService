using AutoMapper;
using IntegrationService.Application.DTOs;
using IntegrationService.Domain.Entities;

namespace IntegrationService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PurchaseOrder, PurchaseOrderDto>()
            .ForMember(d => d.PoSeqId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.DueDays, o => o.MapFrom(s => s.PaymentTerms.DueDays))
            .ForMember(d => d.DueDayMonthOffset, o => o.MapFrom(s => s.PaymentTerms.DueDayMonthOffset))
            .ForMember(d => d.MonthForward, o => o.MapFrom(s => s.PaymentTerms.MonthForward));

        CreateMap<MaterialReceiptCertificate, MaterialReceiptDto>()
            .ForMember(d => d.MrcSeqId, o => o.MapFrom(s => s.Id));

        CreateMap<Vendor, VendorDto>()
            .ForMember(d => d.VendorId, o => o.MapFrom(s => s.Id));

        CreateMap<VendorSite, VendorSiteDto>()
            .ForMember(d => d.VendorSiteId, o => o.MapFrom(s => s.Id));

        CreateMap<VendorSiteBuMapping, VendorSiteBuMappingDto>();

        CreateMap<OrganizationUnit, OrganizationUnitDto>()
            .ForMember(d => d.OuId, o => o.MapFrom(s => s.Id));

        CreateMap<OuBuMapping, OuBuMappingDto>();
    }
}
