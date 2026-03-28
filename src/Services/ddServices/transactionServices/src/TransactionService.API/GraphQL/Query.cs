using HotChocolate.Types;
using TransactionService.Application.DTOs;
using TransactionService.Application.Queries;
using MediatR;

namespace TransactionService.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<DemandMasterDto>> GetDemandsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllDemandsQuery());
    }

    public async Task<DemandMasterDto?> GetDemandAsync([Service] IMediator mediator, long id)
    {
        return await mediator.Send(new GetDemandByIdQuery { Id = id });
    }

    public async Task<IEnumerable<SaaBudgetDto>> GetBudgetsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllBudgetsQuery());
    }

    public async Task<IEnumerable<SaaBudgetDto>> GetBudgetsByYearAsync([Service] IMediator mediator, long yearId)
    {
        return await mediator.Send(new GetBudgetsByYearQuery { YearId = yearId });
    }

    public async Task<IEnumerable<SaaPeriodDto>> GetPeriodsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllPeriodsQuery());
    }

    public async Task<IEnumerable<SaaLevelDto>> GetLevelsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllLevelsQuery());
    }

    public async Task<IEnumerable<SaaRecommendDto>> GetRecommendsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllRecommendsQuery());
    }

    public async Task<SaaRecommendDto?> GetRecommendAsync([Service] IMediator mediator, long id)
    {
        return await mediator.Send(new GetRecommendByIdQuery { Id = id });
    }

    public async Task<IEnumerable<SaaRecommendDto>> GetRecommendsByPeriodAsync([Service] IMediator mediator, long periodId)
    {
        return await mediator.Send(new GetRecommendsByPeriodQuery { PeriodId = periodId });
    }

    public async Task<IEnumerable<SaaRecommendDto>> GetRecommendsByEmployeeAsync([Service] IMediator mediator, long empSysId)
    {
        return await mediator.Send(new GetRecommendsByEmployeeQuery { EmpSysId = empSysId });
    }

    public async Task<IEnumerable<SaaSubmitDto>> GetSubmitsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllSubmitsQuery());
    }

    public async Task<IEnumerable<SaaMailTriggerDto>> GetMailTriggersAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllMailTriggersQuery());
    }
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor.Name("Query");

        descriptor
            .Field(q => q.GetDemandsAsync(default!))
            .Name("getDemands")
            .Type<NonNullType<ListType<DemandMasterType>>>();

        descriptor
            .Field(q => q.GetDemandAsync(default!, default))
            .Name("getDemand")
            .Type<DemandMasterType>();

        descriptor
            .Field(q => q.GetBudgetsAsync(default!))
            .Name("getBudgets")
            .Type<NonNullType<ListType<SaaBudgetType>>>();

        descriptor
            .Field(q => q.GetBudgetsByYearAsync(default!, default))
            .Name("getBudgetsByYear")
            .Type<NonNullType<ListType<SaaBudgetType>>>();

        descriptor
            .Field(q => q.GetPeriodsAsync(default!))
            .Name("getPeriods")
            .Type<NonNullType<ListType<SaaPeriodType>>>();

        descriptor
            .Field(q => q.GetLevelsAsync(default!))
            .Name("getLevels")
            .Type<NonNullType<ListType<SaaLevelType>>>();

        descriptor
            .Field(q => q.GetRecommendsAsync(default!))
            .Name("getRecommends")
            .Type<NonNullType<ListType<SaaRecommendType>>>();

        descriptor
            .Field(q => q.GetRecommendAsync(default!, default))
            .Name("getRecommend")
            .Type<SaaRecommendType>();

        descriptor
            .Field(q => q.GetRecommendsByPeriodAsync(default!, default))
            .Name("getRecommendsByPeriod")
            .Type<NonNullType<ListType<SaaRecommendType>>>();

        descriptor
            .Field(q => q.GetRecommendsByEmployeeAsync(default!, default))
            .Name("getRecommendsByEmployee")
            .Type<NonNullType<ListType<SaaRecommendType>>>();

        descriptor
            .Field(q => q.GetSubmitsAsync(default!))
            .Name("getSubmits")
            .Type<NonNullType<ListType<SaaSubmitType>>>();

        descriptor
            .Field(q => q.GetMailTriggersAsync(default!))
            .Name("getMailTriggers")
            .Type<NonNullType<ListType<SaaMailTriggerType>>>();
    }
}
