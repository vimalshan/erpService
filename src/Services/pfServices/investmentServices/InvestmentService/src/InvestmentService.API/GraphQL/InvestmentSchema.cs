using InvestmentService.Application.DTOs;
using InvestmentService.Application.Queries;
using InvestmentService.Application.Commands;
using MediatR;

namespace InvestmentService.API.GraphQL;

public class Query
{
    public async Task<List<InvestmentDto>> GetInvestments(
        [Service] IMediator mediator,
        string? status = null,
        int? categoryId = null) =>
        await mediator.Send(new GetAllInvestmentsQuery(status, categoryId));

    public async Task<InvestmentDto?> GetInvestment(
        [Service] IMediator mediator, long invNo) =>
        await mediator.Send(new GetInvestmentByIdQuery(invNo));

    public async Task<List<InvestmentDto>> GetActiveInvestments(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetActiveInvestmentsQuery());

    public async Task<PortfolioSummaryDto> GetPortfolioSummary(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetPortfolioSummaryQuery());

    public async Task<List<ScheduleDetailDto>> GetSchedules(
        [Service] IMediator mediator, long invNo) =>
        await mediator.Send(new GetSchedulesByInvestmentQuery(invNo));

    public async Task<List<ScheduleDetailDto>> GetPendingSchedules(
        [Service] IMediator mediator, DateTime? asOfDate = null) =>
        await mediator.Send(new GetPendingSchedulesQuery(asOfDate ?? DateTime.UtcNow));

    public async Task<List<SaleDetailDto>> GetSales(
        [Service] IMediator mediator, long invNo) =>
        await mediator.Send(new GetSalesByInvestmentQuery(invNo));

    public async Task<List<CategoryDto>> GetCategories(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetAllCategoriesQuery());

    public async Task<List<BrokerDto>> GetBrokers(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetAllBrokersQuery());

    public async Task<List<CreditAgencyDto>> GetCreditAgencies(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetAllCreditAgenciesQuery());

    public async Task<List<CreditRatingDto>> GetCreditRatings(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetAllCreditRatingsQuery());
}

public class Mutation
{
    public async Task<InvestmentDto> CreateInvestment(
        [Service] IMediator mediator, CreateInvestmentCommand input) =>
        await mediator.Send(input);

    public async Task<InvestmentDto> UpdateInvestment(
        [Service] IMediator mediator, UpdateInvestmentCommand input) =>
        await mediator.Send(input);

    public async Task<SaleDetailDto> RedeemInvestment(
        [Service] IMediator mediator, RedeemInvestmentCommand input) =>
        await mediator.Send(input);

    public async Task<CategoryDto> CreateCategory(
        [Service] IMediator mediator, CreateCategoryCommand input) =>
        await mediator.Send(input);

    public async Task<BrokerDto> CreateBroker(
        [Service] IMediator mediator, CreateBrokerCommand input) =>
        await mediator.Send(input);

    public async Task<List<ScheduleDetailDto>> GenerateSchedule(
        [Service] IMediator mediator, long invNo, long year) =>
        await mediator.Send(new GenerateInterestScheduleCommand(invNo, year));

    public async Task<ScheduleDetailDto> RecordInterestReceipt(
        [Service] IMediator mediator, RecordInterestReceiptCommand input) =>
        await mediator.Send(input);

    public async Task<bool> ApproveInvestment(
        [Service] IMediator mediator, ApproveInvestmentCommand input) =>
        await mediator.Send(input);
}
