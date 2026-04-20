using OverviewService.GraphQL.Types;

namespace OverviewService.Services;

public interface IOverviewService
{
    Task<CertificationQuicklinkCardDataType?> GetCertificationQuicklinkCardDataAsync(int? pageNumber, int? pageSize);
    Task<List<FinancialStatusItemType>> GetFinancialStatusWidgetAsync();
    Task<UpcomingAuditDataType?> GetUpcomingAuditWidgetAsync(int? month, int? year);
    Task<List<TrainingStatusDataType>> GetTrainingStatusWidgetAsync();
}

public class OverviewService : IOverviewService
{
    private readonly ILogger<OverviewService> _logger;

    public OverviewService(ILogger<OverviewService> logger)
    {
        _logger = logger;
    }

    public async Task<CertificationQuicklinkCardDataType?> GetCertificationQuicklinkCardDataAsync(int? pageNumber, int? pageSize)
    {
        _logger.LogInformation("Getting certification quicklink card data - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
        
        return new CertificationQuicklinkCardDataType
        {
            CurrentPage = pageNumber ?? 1,
            TotalItems = 0,
            TotalPages = 0,
            Data = new List<CertificationServiceDataType>()
        };
    }

    public async Task<List<FinancialStatusItemType>> GetFinancialStatusWidgetAsync()
    {
        _logger.LogInformation("Getting financial status widget data");
        
        return new List<FinancialStatusItemType>
        {
            new() { FinancialStatus = "Approved", FinancialCount = 0, FinancialPercentage = 0 },
            new() { FinancialStatus = "Pending", FinancialCount = 0, FinancialPercentage = 0 }
        };
    }

    public async Task<UpcomingAuditDataType?> GetUpcomingAuditWidgetAsync(int? month, int? year)
    {
        _logger.LogInformation("Getting upcoming audit widget data - Month: {Month}, Year: {Year}", month, year);
        
        return new UpcomingAuditDataType
        {
            Confirmed = 0,
            ToBeConfirmed = 0,
            ToBeConfirmedBySuaadhya = 0
        };
    }

    public async Task<List<TrainingStatusDataType>> GetTrainingStatusWidgetAsync()
    {
        _logger.LogInformation("Getting training status widget data");
        
        return new List<TrainingStatusDataType>
        {
            new() { Completed = 0, Pending = 0, InProgress = 0 }
        };
    }
}
