using AuditService.Models;
using AuditService.Services;

namespace AuditService.GraphQL.Queries
{
    public class Query
    {
        [GraphQLName("viewAudits")]
        public Task<ApiResponse<List<AuditListResponse>>> ViewAudits([Service] IAuditService service)
        {
            return service.GetAuditListAsync();
        }

        [GraphQLName("auditDetails")]
        public Task<ApiResponse<AuditDetailResponse>> AuditDetails([Service] IAuditService service, int auditId)
        {
            return service.GetAuditDetailsAsync(auditId);
        }

        [GraphQLName("viewFindings")]
        public Task<ApiResponse<List<AuditFindingListResponse>>> ViewFindings([Service] IAuditService service, int auditId)
        {
            return service.GetAuditFindingsAsync(auditId);
        }

        [GraphQLName("viewSitesForAudit")]
        public Task<ApiResponse<List<AuditSiteResponse>>> ViewSitesForAudit([Service] IAuditService service, int auditId)
        {
            return service.GetAuditSitesAsync(auditId);
        }

        [GraphQLName("viewSubAudits")]
        public Task<ApiResponse<List<SubAuditResponse>>> ViewSubAudits([Service] IAuditService service, int auditId)
        {
            return service.GetSubAuditsAsync(auditId);
        }

        [GraphQLName("getAuditDaysPerSite")]
        public Task<ApiResponse<AuditDaysGridResponse>> GetAuditDaysPerSite(
            [Service] IAuditService service,
            string startDate,
            string endDate,
            List<int>? companies,
            List<int>? services,
            List<int>? sites)
        {
            return service.GetAuditDaysGridAsync(
                startDate,
                endDate,
                companies ?? new List<int>(),
                services?.Select(id => id.ToString()).ToList() ?? new List<string>(),
                sites ?? new List<int>());
        }

        [GraphQLName("auditDaysbyServicePieChart")]
        public Task<ApiResponse<AuditDaysByServiceResponse>> AuditDaysByServicePieChart(
            [Service] IAuditService service, AuditDaysFilter filters)
        {
            return service.GetAuditDaysByServiceAsync(filters);
        }

        [GraphQLName("getAuditDaysByMonthAndService")]
        public Task<ApiResponse<AuditDaysByMonthAndServiceResponse>> GetAuditDaysByMonthAndService(
            [Service] IAuditService service,
            string startDate,
            string endDate,
            List<int>? companyFilter,
            List<int>? serviceFilter,
            List<int>? siteFilter)
        {
            return service.GetAuditDaysByMonthAndServiceAsync(new AuditDaysByMonthFilter
            {
                StartDate     = startDate,
                EndDate       = endDate,
                CompanyFilter = companyFilter ?? new List<int>(),
                ServiceFilter = serviceFilter ?? new List<int>(),
                SiteFilter    = siteFilter    ?? new List<int>()
            });
        }
    }
}
