using AutoMapper;
using InvoiceProcessing.Domain.Entities;

namespace InvoiceProcessing.Application.Mappings;

public class DocumentMappingProfile : Profile
{
    public DocumentMappingProfile()
    {
        CreateMap<DocumentDetail, DTOs.DocumentDetailDto>()
            .ForMember(d => d.DocId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.OracleInvoices, o => o.MapFrom(s => s.OracleInvoiceDetails))
            .ForMember(d => d.OraclePayments, o => o.MapFrom(s => s.OraclePaymentDetails))
            .ForMember(d => d.PoList, o => o.MapFrom(s => s.PoList))
            .ForMember(d => d.CostCenters, o => o.MapFrom(s => s.CostCenters));

        CreateMap<OracleInvoiceDetail, DTOs.OracleInvoiceDetailDto>();
        CreateMap<OraclePaymentDetail, DTOs.OraclePaymentDetailDto>();
        CreateMap<DocumentPoList, DTOs.DocumentPoListDto>();
        CreateMap<DocumentCostCenter, DTOs.DocumentCostCenterDto>();
    }
}
