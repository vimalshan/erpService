using AutoMapper;
using CategoryAndVendorService.Application.DTOs;
using CategoryAndVendorService.Domain.Entities;

namespace CategoryAndVendorService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MainCategory, MainCategoryDto>();
        CreateMap<SubCategory, SubCategoryDto>();
        CreateMap<VendorDocument, VendorDocumentDto>()
            .ForMember(d => d.DocFlag, o => o.MapFrom(s => s.DocFlag.ToString()))
            .ForMember(d => d.ActiveStatus, o => o.MapFrom(s => s.ActiveStatus.ToString()))
            .ForMember(d => d.ApprovalStatusCode, o => o.MapFrom(s => s.ApprovalStatus.Code.ToString()))
            .ForMember(d => d.ApprovalStatusDescription, o => o.MapFrom(s => s.ApprovalStatus.Description))
            .ForMember(d => d.Files, o => o.MapFrom(s => s.Files));
        CreateMap<VendorDocumentFile, VendorDocumentFileDto>();
        CreateMap<SupportDocument, SupportDocumentDto>()
            .ForMember(d => d.Attachments, o => o.MapFrom(s => s.Attachments));
        CreateMap<SupportDocumentAttachment, SupportDocumentAttachmentDto>()
            .ForMember(d => d.RefFlag, o => o.MapFrom(s => s.RefFlag.ToString()));
    }
}
