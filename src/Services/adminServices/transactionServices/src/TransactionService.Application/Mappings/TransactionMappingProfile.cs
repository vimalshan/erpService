namespace TransactionService.Application.Mappings;

using AutoMapper;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;

public sealed class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        // Request mappings
        CreateMap<RequestMain, RequestMainDto>()
            .ForCtorParam("UnitCode", opt => opt.MapFrom(s => s.UnitCode == null ? null : s.UnitCode.Value))
            .ForCtorParam("Details", opt => opt.MapFrom(s => s.Details));

        CreateMap<RequestSub, RequestSubDto>()
            .ForCtorParam("Status", opt => opt.MapFrom(s => s.Status.Value));

        CreateMap<RequestMain, RequestSummaryDto>()
            .ForCtorParam("TotalItems", opt => opt.MapFrom(s => s.Details.Count))
            .ForCtorParam("PendingItems", opt => opt.MapFrom(s => s.Details.Count(d => d.Status.IsPending)))
            .ForCtorParam("ApprovedItems", opt => opt.MapFrom(s => s.Details.Count(d => d.Status.IsApproved)));

        // Order mappings
        CreateMap<OrderMain, OrderMainDto>()
            .ForCtorParam("Details", opt => opt.MapFrom(s => s.Details));

        CreateMap<OrderSub, OrderSubDto>();

        CreateMap<OrderMain, OrderSummaryDto>()
            .ForCtorParam("TotalItems", opt => opt.MapFrom(s => s.Details.Count))
            .ForCtorParam("ReceivedItems", opt => opt.MapFrom(s => s.Details.Count(d => d.ReceivedOn.HasValue)));

        // Budget mappings
        CreateMap<DeptBudget, DeptBudgetDto>()
            .ForCtorParam("UnitCode", opt => opt.MapFrom(s => s.UnitCode.Value))
            .ForCtorParam("BudgetAmount", opt => opt.MapFrom(s => s.BudgetAmount.Amount));

        CreateMap<UnitBudget, UnitBudgetDto>()
            .ForCtorParam("UnitCode", opt => opt.MapFrom(s => s.UnitCode.Value))
            .ForCtorParam("BudgetAmount", opt => opt.MapFrom(s => s.BudgetAmount.Amount));
    }
}
