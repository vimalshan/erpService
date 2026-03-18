using AutoMapper;
using PayrollServices.Application.DTOs;
using PayrollServices.Domain.Entities;

namespace PayrollServices.Application.Mappings;

public class PayrollMappingProfile : Profile
{
    public PayrollMappingProfile()
    {
        CreateMap<PayrollBatch, PayrollBatchDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<PayrollTransaction, PayrollTransactionDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<PayrollAdjustment, PayrollAdjustmentDto>()
            .ForMember(dest => dest.AdjustmentType, opt => opt.MapFrom(src => src.AdjustmentType.ToString()));

        // Reverse mappings
        CreateMap<PayrollBatchDto, PayrollBatch>();
        CreateMap<PayrollTransactionDto, PayrollTransaction>();
        CreateMap<PayrollAdjustmentDto, PayrollAdjustment>();
    }
}
