using AuditService.Domain.Entities;
using AuditService.Infrastructure.Data;
using AuditService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AuditDomainDbContext _db;

        public AuditRepository(AuditDomainDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<AuditListResponse>> GetAuditListAsync()
        {
            var audits = await _db.Audits
                .Include(a => a.AuditSites)
                .Include(a => a.AuditServices)
                .AsNoTracking()
                .ToListAsync();

            return audits.Select(a => new AuditListResponse
            {
                AuditId    = a.AuditId,
                CompanyId  = a.CompanyId ?? 0,
                Status     = a.Status,
                StartDate  = a.StartDate,
                EndDate    = a.EndDate,
                LeadAuditor= a.LeadAuditor,
                Type       = a.Type,
                Sites      = a.AuditSites.Select(s => s.SiteId).Distinct().ToList(),
                Services   = a.AuditServices.Select(s => s.ServiceId).Distinct().ToList()
            }).ToList();
        }

        public async Task<AuditDetailResponse?> GetAuditDetailsAsync(int auditId)
        {
            var audit = await _db.Audits
                .Include(a => a.AuditSites)
                .Include(a => a.AuditTeamMembers)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuditId == auditId);

            if (audit == null) return null;

            // Resolve real site names from the Sites table
            var siteIds = audit.AuditSites.Select(s => s.SiteId).Distinct().ToList();
            var sites = siteIds.Any()
                ? await _db.Sites.Where(s => siteIds.Contains(s.SiteId)).AsNoTracking().ToListAsync()
                : new List<SiteInfo>();

            var siteNames     = sites.Any()
                ? string.Join(", ", sites.Select(s => s.SiteName))
                : (audit.Sites ?? "N/A");
            var siteAddresses = sites.Any()
                ? string.Join(", ", sites.Select(s => s.Location ?? s.SiteName))
                : (audit.Sites ?? "N/A");

            // Service names come from the plain-text Services column ("ISO 9001, ISO 14001")
            var serviceNames = string.IsNullOrWhiteSpace(audit.Services)
                ? new List<string>()
                : audit.Services.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            // Auditor team: lead + active team-member roles
            var team = new List<string>();
            if (!string.IsNullOrWhiteSpace(audit.LeadAuditor))
                team.Add(audit.LeadAuditor);
            foreach (var tm in audit.AuditTeamMembers.Where(m => m.IsActive))
            {
                var label = string.IsNullOrWhiteSpace(tm.Role) ? $"User {tm.UserId}" : tm.Role;
                if (!team.Contains(label)) team.Add(label);
            }

            return new AuditDetailResponse
            {
                AuditId     = audit.AuditId,
                EndDate     = audit.EndDate?.ToString("yyyy-MM-dd"),
                LeadAuditor = audit.LeadAuditor,
                SiteAddress = siteAddresses,
                SiteName    = siteNames,
                StartDate   = audit.StartDate?.ToString("yyyy-MM-dd"),
                Status      = audit.Status,
                Services    = serviceNames,
                AuditorTeam = team
            };
        }

        public async Task<IReadOnlyList<AuditFindingListResponse>> GetAuditFindingsAsync(int auditId)
        {
            var audit = await _db.Audits.Include(a => a.AuditSites)
                .AsNoTracking().FirstOrDefaultAsync(a => a.AuditId == auditId);
            if (audit == null) return new List<AuditFindingListResponse>();

            var siteIds   = audit.AuditSites.Select(s => s.SiteId).Distinct().ToList();
            var companyId = audit.CompanyId ?? 0;
            if (!siteIds.Any() || companyId == 0) return new List<AuditFindingListResponse>();

            var siteIdCsv = string.Join(",", siteIds.Select(id => id.ToString()));
            var sql = $@"SELECT FindingId, FindingNumber, ISNULL(Title,'') AS Title,
                         ISNULL(Status,'') AS Status, ISNULL(Category,'') AS Category,
                         ISNULL(CompanyId,0) AS CompanyId, ISNULL(SiteId,0) AS SiteId,
                         OpenDate, DueDate, AcceptedDate, ClosedDate
                         FROM Findings
                         WHERE CompanyId = {companyId} AND SiteId IN ({siteIdCsv})";

            var conn    = _db.Database.GetDbConnection();
            var wasOpen = conn.State == System.Data.ConnectionState.Open;
            if (!wasOpen) await conn.OpenAsync();
            try
            {
                using var cmd    = conn.CreateCommand();
                cmd.CommandText  = sql;
                using var reader = await cmd.ExecuteReaderAsync();
                var results = new List<AuditFindingListResponse>();
                while (await reader.ReadAsync())
                {
                    results.Add(new AuditFindingListResponse
                    {
                        FindingsId    = reader.GetInt32(0),
                        FindingNumber = reader.IsDBNull(1)  ? "" : reader.GetString(1),
                        Title         = reader.IsDBNull(2)  ? "" : reader.GetString(2),
                        Status        = reader.IsDBNull(3)  ? "" : reader.GetString(3),
                        Category      = reader.IsDBNull(4)  ? "" : reader.GetString(4),
                        CompanyId     = reader.IsDBNull(5)  ? 0  : reader.GetInt32(5),
                        SiteId        = reader.IsDBNull(6)  ? 0  : reader.GetInt32(6),
                        OpenDate      = reader.IsDBNull(7)  ? null : reader.GetDateTime(7).ToString("yyyy-MM-dd"),
                        DueDate       = reader.IsDBNull(8)  ? null : reader.GetDateTime(8).ToString("yyyy-MM-dd"),
                        AcceptedDate  = reader.IsDBNull(9)  ? null : reader.GetDateTime(9).ToString("yyyy-MM-dd"),
                        ClosedDate    = reader.IsDBNull(10) ? null : reader.GetDateTime(10).ToString("yyyy-MM-dd"),
                        AuditId       = auditId,
                        Services      = new List<string>()
                    });
                }
                return results;
            }
            finally { if (!wasOpen) await conn.CloseAsync(); }
        }

        public async Task<IReadOnlyList<AuditSiteResponse>> GetAuditSitesAsync(int auditId)
        {
            var siteIds = await _db.AuditSites
                .Where(s => s.AuditId == auditId)
                .Select(s => s.SiteId)
                .Distinct()
                .ToListAsync();

            var sites = siteIds.Any()
                ? await _db.Sites.Where(s => siteIds.Contains(s.SiteId)).AsNoTracking().ToListAsync()
                : new List<SiteInfo>();

            return sites.Select(s =>
            {
                var parts   = (s.Location ?? "").Split(',', 2);
                var city    = parts.Length > 0 ? parts[0].Trim() : "";
                var country = parts.Length > 1 ? parts[1].Trim() : "";
                return new AuditSiteResponse
                {
                    SiteName    = s.SiteName,
                    AddressLine = s.Location ?? "N/A",
                    City        = city,
                    Country     = country,
                    PostCode    = string.Empty
                };
            }).ToList();
        }

        public async Task<IReadOnlyList<SubAuditResponse>> GetSubAuditsAsync(int auditId)
        {
            var siteAudits = await _db.AuditSiteAudits
                .Where(sa => sa.AuditId == auditId)
                .AsNoTracking()
                .ToListAsync();

            var parentAudit = await _db.Audits.AsNoTracking().FirstOrDefaultAsync(a => a.AuditId == auditId);

            return siteAudits.Select(sa => new SubAuditResponse
            {
                AuditId     = sa.AuditId,
                Sites       = new List<int> { sa.SiteId },
                Services    = new List<int> { sa.AuditTypeId },
                Status      = sa.Status,
                StartDate   = sa.StartDate?.ToString("yyyy-MM-dd"),
                EndDate     = sa.EndDate?.ToString("yyyy-MM-dd"),
                AuditorTeam = sa.LeadAuditorId.HasValue && parentAudit != null
                    ? new List<string> { parentAudit.LeadAuditor ?? $"Auditor {sa.LeadAuditorId}" }
                    : new List<string>()
            }).ToList();
        }

        public async Task<ApiResponse<AuditDaysGridResponse>> GetAuditDaysGridAsync(
            string startDate, string endDate, List<int> companies, List<string> services, List<int> sites)
        {
            DateTime.TryParse(startDate, out var start);
            DateTime.TryParse(endDate, out var end);

            var query = _db.AuditSiteAudits
                .Where(sa => sa.StartDate >= start && sa.EndDate <= end);

            if (companies.Any())
                query = query.Where(sa => _db.Audits.Any(a => a.AuditId == sa.AuditId && companies.Contains(a.CompanyId ?? 0)));

            if (sites.Any())
                query = query.Where(sa => sites.Contains(sa.SiteId));

            var rows = await query.AsNoTracking().ToListAsync();

            var gridSiteIds = rows.Select(r => r.SiteId).Distinct().ToList();
            var siteNameMap = gridSiteIds.Any()
                ? (await _db.Sites.Where(s => gridSiteIds.Contains(s.SiteId)).AsNoTracking().ToListAsync())
                  .ToDictionary(s => s.SiteId, s => s.SiteName)
                : new Dictionary<int, string>();

            var nodes = rows
                .GroupBy(sa => sa.SiteId)
                .Select(g => new AuditDaysGridNode
                {
                    Data = new AuditDaysGridNodeData
                    {
                        Id       = g.Key,
                        Name     = siteNameMap.TryGetValue(g.Key, out var sn) ? sn : $"Site {g.Key}",
                        AuditDays= g.Sum(sa => sa.StartDate.HasValue && sa.EndDate.HasValue
                                            ? (decimal)(sa.EndDate.Value - sa.StartDate.Value).TotalDays
                                            : 0),
                        DataType = "site"
                    }
                }).ToList();

            return new ApiResponse<AuditDaysGridResponse>
            {
                Data      = new AuditDaysGridResponse { Data = nodes },
                IsSuccess = true,
                Message   = "Success",
                ErrorCode = string.Empty
            };
        }

        public async Task<ApiResponse<AuditDaysByServiceResponse>> GetAuditDaysByServiceAsync(AuditDaysFilter filters)
        {
            DateTime.TryParse(filters.StartDate, out var start);
            DateTime.TryParse(filters.EndDate, out var end);

            var siteAudits = await _db.AuditSiteAudits
                .Include(sa => sa.AuditTypeNavigation)
                .Where(sa => (!sa.StartDate.HasValue || sa.StartDate >= start)
                          && (!sa.EndDate.HasValue   || sa.EndDate   <= end))
                .AsNoTracking()
                .ToListAsync();

            var total = siteAudits.Sum(sa => sa.StartDate.HasValue && sa.EndDate.HasValue
                                           ? (decimal)(sa.EndDate.Value - sa.StartDate.Value).TotalDays : 0);

            var items = siteAudits
                .GroupBy(sa => sa.AuditTypeId)
                .Select(g =>
                {
                    var days     = g.Sum(sa => sa.StartDate.HasValue && sa.EndDate.HasValue
                                        ? (decimal)(sa.EndDate.Value - sa.StartDate.Value).TotalDays : 0);
                    var typeName = g.First().AuditTypeNavigation?.AuditTypeName ?? $"AuditType {g.Key}";
                    return new AuditDaysByServiceItem
                    {
                        ServiceName      = typeName,
                        AuditDays        = days,
                        AuditPercentage  = total > 0 ? (int)Math.Round(days / total * 100) : 0
                    };
                }).ToList();

            return new ApiResponse<AuditDaysByServiceResponse>
            {
                Data      = new AuditDaysByServiceResponse { PieChartData = items, TotalServiceAuditsDayCount = total },
                IsSuccess = true,
                Message   = "Success",
                ErrorCode = string.Empty
            };
        }

        public async Task<ApiResponse<AuditDaysByMonthAndServiceResponse>> GetAuditDaysByMonthAndServiceAsync(AuditDaysByMonthFilter filters)
        {
            DateTime.TryParse(filters.StartDate, out var start);
            DateTime.TryParse(filters.EndDate, out var end);

            var siteAudits = await _db.AuditSiteAudits
                .Include(sa => sa.AuditTypeNavigation)
                .Where(sa => (!sa.StartDate.HasValue || sa.StartDate >= start)
                          && (!sa.EndDate.HasValue   || sa.EndDate   <= end))
                .AsNoTracking()
                .ToListAsync();

            var chartData = siteAudits
                .Where(sa => sa.StartDate.HasValue)
                .GroupBy(sa => new { sa.StartDate!.Value.Year, sa.StartDate.Value.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g =>
                {
                    var monthCount = g.Sum(sa => sa.StartDate.HasValue && sa.EndDate.HasValue
                                              ? (decimal)(sa.EndDate.Value - sa.StartDate.Value).TotalDays : 0);
                    var serviceData = g.GroupBy(sa => sa.AuditTypeId)
                        .Select(sg => new AuditDaysServiceData
                        {
                            ServiceName = sg.First().AuditTypeNavigation?.AuditTypeName ?? $"AuditType {sg.Key}",
                            AuditDays   = sg.Sum(sa => sa.StartDate.HasValue && sa.EndDate.HasValue
                                                     ? (decimal)(sa.EndDate.Value - sa.StartDate.Value).TotalDays : 0)
                        }).ToList();

                    return new AuditDaysMonthData
                    {
                        Month       = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                        MonthCount  = monthCount,
                        ServiceData = serviceData
                    };
                }).ToList();

            return new ApiResponse<AuditDaysByMonthAndServiceResponse>
            {
                Data      = new AuditDaysByMonthAndServiceResponse { ChartData = chartData },
                IsSuccess = true,
                Message   = "Success",
                ErrorCode = string.Empty
            };
        }

    }
}
