using AutoMapper;
using ExpenseService.Application.DTOs;
using ExpenseService.Domain.Entities;

namespace ExpenseService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TravelExpense, TravelExpenseDto>()
            .ForMember(d => d.Allocations, opt => opt.MapFrom(s => s.Allocations))
            .ForMember(d => d.SubDetails, opt => opt.MapFrom(s => s.SubDetails));

        CreateMap<TravelExpenseAllocation, ExpenseAllocationDto>();
        CreateMap<TravelExpenseSub, ExpenseSubDetailDto>();
        CreateMap<TravelConveyance, ConveyanceDto>();
        CreateMap<TravelCurrency, CurrencyDto>();
        CreateMap<DaSummary, DaSummaryDto>();
        CreateMap<ExpSettlement, SettlementDto>();
        CreateMap<ExpSettlementReport, SettlementDto>();
    }
}
