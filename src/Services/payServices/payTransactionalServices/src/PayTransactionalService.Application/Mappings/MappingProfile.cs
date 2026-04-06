using AutoMapper;
using PayTransactionalService.Application.DTOs;
using PayTransactionalService.Domain.Entities;

namespace PayTransactionalService.Application.Mappings;

public class PayTransactionalMappingProfile : Profile
{
    public PayTransactionalMappingProfile()
    {
        CreateMap<PayTransaction, PayTransactionDto>()
            .ForMember(d => d.GrossAmount, o => o.MapFrom(s => s.GrossAmount.Amount))
            .ForMember(d => d.Deductions, o => o.MapFrom(s => s.Deductions.Amount))
            .ForMember(d => d.NetAmount, o => o.MapFrom(s => s.NetAmount.Amount));

        CreateMap<PayArrear, PayArrearDto>()
            .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.Amount));

        CreateMap<PayAdjustment, PayAdjustmentDto>()
            .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.Amount));

        CreateMap<PayrollBatch, PayrollBatchDto>();
    }
}
