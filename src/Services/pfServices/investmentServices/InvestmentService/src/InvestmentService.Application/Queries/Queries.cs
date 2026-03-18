using InvestmentService.Application.DTOs;
using MediatR;

namespace InvestmentService.Application.Queries;

public record GetInvestmentByIdQuery(long InvNo) : IRequest<InvestmentDto?>;
public record GetAllInvestmentsQuery(string? Status = null, int? CategoryId = null) : IRequest<List<InvestmentDto>>;
public record GetActiveInvestmentsQuery : IRequest<List<InvestmentDto>>;
public record GetPortfolioSummaryQuery : IRequest<PortfolioSummaryDto>;
public record GetSalesByInvestmentQuery(long InvNo) : IRequest<List<SaleDetailDto>>;
public record GetSchedulesByInvestmentQuery(long InvNo) : IRequest<List<ScheduleDetailDto>>;
public record GetPendingSchedulesQuery(DateTime AsOfDate) : IRequest<List<ScheduleDetailDto>>;
public record GetAllCategoriesQuery : IRequest<List<CategoryDto>>;
public record GetSubCategoriesByCategoryQuery(int CategoryId) : IRequest<List<SubCategoryDto>>;
public record GetAllBrokersQuery : IRequest<List<BrokerDto>>;
public record GetAllCreditAgenciesQuery : IRequest<List<CreditAgencyDto>>;
public record GetAllCreditRatingsQuery : IRequest<List<CreditRatingDto>>;
public record GetMaturedInvestmentsQuery(DateTime AsOfDate) : IRequest<List<InvestmentDto>>;
