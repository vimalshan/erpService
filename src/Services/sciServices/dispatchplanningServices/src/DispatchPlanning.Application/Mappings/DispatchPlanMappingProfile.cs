using AutoMapper;
using DispatchPlanning.Application.DTOs;
using DispatchPlanning.Domain.Aggregates;
using DispatchPlanning.Domain.Entities;

namespace DispatchPlanning.Application.Mappings;

public class DispatchPlanMappingProfile : Profile
{
    public DispatchPlanMappingProfile()
    {
        CreateMap<DispatchPlanAggregate, DispatchPlanHeaderDto>()
            .ConstructUsing(src => new DispatchPlanHeaderDto(
                src.DispatchPlanHeaderId,
                src.PlanType.Value,
                src.PlanMonth,
                src.PlanMPlus1,
                src.PlanMPlus2,
                src.PlanMPlus3,
                src.PlanMPlus4,
                src.EntryDate,
                src.CompanyUnitId,
                src.SciUserIdModified,
                src.ModifiedDate));

        CreateMap<DispatchPlanMainGroup, MainGroupDto>()
            .ConstructUsing(src => new MainGroupDto(
                src.MainGroupId,
                src.MainGroupName,
                src.GroupType,
                src.ProductSummary,
                src.TotalDisplayName,
                src.MgDisplayOrder,
                src.CompanyUnitId));

        CreateMap<DispatchPlanSubGroup, SubGroupDto>()
            .ConstructUsing(src => new SubGroupDto(
                src.SubGroupId,
                src.MainGroupId,
                src.SubGroupName,
                src.ProductId,
                src.SgDisplayOrder,
                src.CaptureTotalDirectly));

        CreateMap<DispatchPlanBreakupItem, BreakupItemDto>()
            .ConstructUsing(src => new BreakupItemDto(
                src.BreakupItemId,
                src.SubGroupId,
                src.ProductId,
                src.BreakupItemDesc,
                src.UnitId,
                src.MainProductUnitsConFactor,
                src.BiDisplayOrder,
                src.EffectiveDate,
                src.ClosureDate,
                src.PackageId));
    }
}
