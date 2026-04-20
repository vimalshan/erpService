using OverviewService.GraphQL.Types;
using OverviewService.Services;

namespace OverviewService.GraphQL.Queries;

public class Query
{
    private readonly IOverviewService _overviewService;

    public Query(IOverviewService overviewService)
    {
        _overviewService = overviewService;
    }

    public async Task<CertificationQuicklinkCardDataType?> ViewCertificationQuicklinkCard(
        int? pageNumber = null, 
        int? pageSize = null)
    {
        return await _overviewService.GetCertificationQuicklinkCardDataAsync(pageNumber, pageSize);
    }

    public async Task<List<FinancialStatusItemType>> GetWidgetForFinancials()
    {
        return await _overviewService.GetFinancialStatusWidgetAsync();
    }

    public async Task<UpcomingAuditDataType?> GetWidgetForUpcomingAudit(int? month = null, int? year = null)
    {
        return await _overviewService.GetUpcomingAuditWidgetAsync(month, year);
    }

    public async Task<List<TrainingStatusDataType>> GetWidgetForTrainingStatus()
    {
        return await _overviewService.GetTrainingStatusWidgetAsync();
    }
}
