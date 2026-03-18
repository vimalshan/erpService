using AutoMapper;
using InvestmentService.Application.DTOs;
using InvestmentService.Domain.Interfaces;
using MediatR;

namespace InvestmentService.Application.Queries.Handlers;

public class GetInvestmentByIdHandler : IRequestHandler<GetInvestmentByIdQuery, InvestmentDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetInvestmentByIdHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<InvestmentDto?> Handle(GetInvestmentByIdQuery query, CancellationToken ct)
    {
        var investment = await _uow.Investments.GetByIdAsync(query.InvNo, ct);
        return investment == null ? null : _mapper.Map<InvestmentDto>(investment);
    }
}

public class GetAllInvestmentsHandler : IRequestHandler<GetAllInvestmentsQuery, List<InvestmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllInvestmentsHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<InvestmentDto>> Handle(GetAllInvestmentsQuery query, CancellationToken ct)
    {
        var investments = await _uow.Investments.GetAllAsync(ct);

        if (query.Status != null)
            investments = investments.Where(i => i.Status == query.Status);
        if (query.CategoryId.HasValue)
            investments = investments.Where(i => i.CategoryId == query.CategoryId);

        return _mapper.Map<List<InvestmentDto>>(investments.ToList());
    }
}

public class GetActiveInvestmentsHandler : IRequestHandler<GetActiveInvestmentsQuery, List<InvestmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetActiveInvestmentsHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<InvestmentDto>> Handle(GetActiveInvestmentsQuery query, CancellationToken ct)
    {
        var investments = await _uow.Investments.GetActiveInvestmentsAsync(ct);
        return _mapper.Map<List<InvestmentDto>>(investments.ToList());
    }
}

public class GetPortfolioSummaryHandler : IRequestHandler<GetPortfolioSummaryQuery, PortfolioSummaryDto>
{
    private readonly IDapperQueryService _dapper;

    public GetPortfolioSummaryHandler(IDapperQueryService dapper) { _dapper = dapper; }

    public async Task<PortfolioSummaryDto> Handle(GetPortfolioSummaryQuery query, CancellationToken ct)
    {
        const string sql = @"
            SELECT COUNT(*) as TotalInvestments,
                   SUM(CASE WHEN INV_STATUS = 'A' THEN 1 ELSE 0 END) as ActiveInvestments,
                   SUM(CASE WHEN INV_STATUS = 'M' THEN 1 ELSE 0 END) as MaturedInvestments,
                   SUM(CASE WHEN INV_STATUS = 'R' THEN 1 ELSE 0 END) as RedeemedInvestments,
                   ISNULL(SUM(INV_PURVALUE), 0) as TotalPurchaseValue,
                   ISNULL(SUM(CASE WHEN INV_STATUS = 'A' THEN INV_PURVALUE ELSE 0 END), 0) as TotalActiveValue
            FROM INV_MAIN";

        var summary = await _dapper.QueryFirstOrDefaultAsync<PortfolioSummaryDto>(sql, ct: ct);

        const string categorySql = @"
            SELECT ic.INVCAT_NAME as CategoryName, COUNT(*) as [Count],
                   ISNULL(SUM(im.INV_PURVALUE), 0) as TotalValue
            FROM INV_MAIN im
            LEFT JOIN INVCAT_MAST ic ON im.INV_CATID = ic.INVCAT_CODE
            WHERE im.INV_STATUS = 'A'
            GROUP BY ic.INVCAT_NAME";

        var categories = await _dapper.QueryAsync<CategorySummaryDto>(categorySql, ct: ct);

        summary ??= new PortfolioSummaryDto();
        summary.CategorySummaries = categories.ToList();
        return summary;
    }
}

public class GetSalesByInvestmentHandler : IRequestHandler<GetSalesByInvestmentQuery, List<SaleDetailDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetSalesByInvestmentHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<SaleDetailDto>> Handle(GetSalesByInvestmentQuery query, CancellationToken ct)
    {
        var sales = await _uow.SaleDetails.GetByInvestmentAsync(query.InvNo, ct);
        return _mapper.Map<List<SaleDetailDto>>(sales.ToList());
    }
}

public class GetSchedulesByInvestmentHandler : IRequestHandler<GetSchedulesByInvestmentQuery, List<ScheduleDetailDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetSchedulesByInvestmentHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<ScheduleDetailDto>> Handle(GetSchedulesByInvestmentQuery query, CancellationToken ct)
    {
        var schedules = await _uow.ScheduleDetails.GetByInvestmentAsync(query.InvNo, ct);
        return _mapper.Map<List<ScheduleDetailDto>>(schedules.ToList());
    }
}

public class GetPendingSchedulesHandler : IRequestHandler<GetPendingSchedulesQuery, List<ScheduleDetailDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetPendingSchedulesHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<ScheduleDetailDto>> Handle(GetPendingSchedulesQuery query, CancellationToken ct)
    {
        var schedules = await _uow.ScheduleDetails.GetPendingSchedulesAsync(query.AsOfDate, ct);
        return _mapper.Map<List<ScheduleDetailDto>>(schedules.ToList());
    }
}

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery query, CancellationToken ct)
    {
        var categories = await _uow.Categories.GetAllAsync(ct);
        return _mapper.Map<List<CategoryDto>>(categories.ToList());
    }
}

public class GetSubCategoriesByCategoryHandler : IRequestHandler<GetSubCategoriesByCategoryQuery, List<SubCategoryDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetSubCategoriesByCategoryHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<SubCategoryDto>> Handle(GetSubCategoriesByCategoryQuery query, CancellationToken ct)
    {
        var subs = await _uow.SubCategories.GetByCategoryAsync(query.CategoryId, ct);
        return _mapper.Map<List<SubCategoryDto>>(subs.ToList());
    }
}

public class GetAllBrokersHandler : IRequestHandler<GetAllBrokersQuery, List<BrokerDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllBrokersHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<BrokerDto>> Handle(GetAllBrokersQuery query, CancellationToken ct)
    {
        var brokers = await _uow.Brokers.GetAllActiveAsync(ct);
        return _mapper.Map<List<BrokerDto>>(brokers.ToList());
    }
}

public class GetAllCreditAgenciesHandler : IRequestHandler<GetAllCreditAgenciesQuery, List<CreditAgencyDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllCreditAgenciesHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<CreditAgencyDto>> Handle(GetAllCreditAgenciesQuery query, CancellationToken ct)
    {
        var agencies = await _uow.CreditAgencies.GetAllAsync(ct);
        return _mapper.Map<List<CreditAgencyDto>>(agencies.ToList());
    }
}

public class GetAllCreditRatingsHandler : IRequestHandler<GetAllCreditRatingsQuery, List<CreditRatingDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAllCreditRatingsHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<CreditRatingDto>> Handle(GetAllCreditRatingsQuery query, CancellationToken ct)
    {
        var ratings = await _uow.CreditRatings.GetAllAsync(ct);
        return _mapper.Map<List<CreditRatingDto>>(ratings.ToList());
    }
}

public class GetMaturedInvestmentsHandler : IRequestHandler<GetMaturedInvestmentsQuery, List<InvestmentDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetMaturedInvestmentsHandler(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

    public async Task<List<InvestmentDto>> Handle(GetMaturedInvestmentsQuery query, CancellationToken ct)
    {
        var investments = await _uow.Investments.GetMaturedInvestmentsAsync(query.AsOfDate, ct);
        return _mapper.Map<List<InvestmentDto>>(investments.ToList());
    }
}
