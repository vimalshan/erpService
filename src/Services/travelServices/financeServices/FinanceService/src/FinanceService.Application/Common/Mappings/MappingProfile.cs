using AutoMapper;
using FinanceService.Application.DTOs;
using FinanceService.Domain.Entities;

namespace FinanceService.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApInvoice, InvoiceDto>()
            .ForMember(d => d.Lines, opt => opt.MapFrom(s => s.InvoiceLines));
        CreateMap<ApInvoiceLine, InvoiceLineDto>();
        CreateMap<TravelBatchMain, BatchDto>()
            .ForMember(d => d.Lines, opt => opt.MapFrom(s => s.BatchLines));
        CreateMap<TravelBatchSub, BatchLineDto>();
        CreateMap<TravelAccount, PaymentDto>();
        CreateMap<JvPostingDetail, JvPostingDto>();
    }
}
