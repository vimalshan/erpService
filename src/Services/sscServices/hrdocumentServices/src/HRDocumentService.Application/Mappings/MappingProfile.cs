using AutoMapper;
using HRDocumentService.Application.DTOs;
using HRDocumentService.Domain.Entities;

namespace HRDocumentService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<HRDocument, HRDocumentDto>();
        CreateMap<HRDocumentFile, HRDocumentFileDto>();
        CreateMap<HRDocumentReceipt, HRDocumentReceiptDto>();
    }
}
