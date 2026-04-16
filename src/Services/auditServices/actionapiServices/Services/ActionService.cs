using ActionService.Infrastructure.Data;
using ActionService.Models;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Services
{
    public class ActionService : IActionService
    {
        private readonly ActionDbContext _dbContext;
        private readonly ILogger<ActionService> _logger;

        public ActionService(ActionDbContext dbContext, ILogger<ActionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ApiResponse<ActionsPaginationResponse>> GetActionsAsync(
            IEnumerable<int> category, IEnumerable<int> company, IEnumerable<int> service,
            IEnumerable<int> site, bool isHighPriority, int pageNumber, int pageSize)
        {
            try
            {
                var query = _dbContext.Actions.AsQueryable();

                if (isHighPriority)
                    query = query.Where(a => a.HighPriority);

                var categoryList = category.ToList();
                if (categoryList.Count > 0)
                {
                    var entityTypes = new List<string>();
                    if (categoryList.Contains(2)) entityTypes.AddRange(new[] { "Certificate", "certificates" });
                    if (categoryList.Contains(3)) entityTypes.AddRange(new[] { "Finding", "findings" });
                    if (categoryList.Contains(4)) entityTypes.AddRange(new[] { "Schedule", "schedule" });
                    if (entityTypes.Count > 0)
                        query = query.Where(a => entityTypes.Contains(a.EntityType ?? ""));
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var entities = await query
                    .OrderBy(a => a.DueDate == null ? 1 : 0)
                    .ThenBy(a => a.DueDate)
                    .ThenByDescending(a => a.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Success(new ActionsPaginationResponse
                {
                    CurrentPage = pageNumber,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    Items = entities.Select(MapToModel).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load actions list");
                return Failure<ActionsPaginationResponse>("Failed to load actions");
            }
        }

        public async Task<ApiResponse<ActionItem>> CreateActionAsync(CreateActionRequest request)
        {
            try
            {
                var entity = Domain.Entities.ActionItem.Create(
                    request.Action, request.DueDate, request.HighPriority,
                    request.Message, request.Language, request.Service,
                    request.Site, request.EntityType, request.EntityId,
                    request.Subject, request.SnowLink);

                _dbContext.Actions.Add(entity);
                await _dbContext.SaveChangesAsync();

                return Success(MapToModel(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create action");
                return Failure<ActionItem>("Failed to create action");
            }
        }

        public Task<ApiResponse<List<ActionFilterItem>>> GetActionCategoriesAsync(
            IEnumerable<int> companies, IEnumerable<int> services, IEnumerable<int> sites)
        {
            // Categories are a static system-level list: 2=Certificates, 3=Findings, 4=Schedule
            var categories = new List<ActionFilterItem>
            {
                new ActionFilterItem { Id = 2, Label = "Certificates" },
                new ActionFilterItem { Id = 3, Label = "Findings" },
                new ActionFilterItem { Id = 4, Label = "Schedule" }
            };
            return Task.FromResult(Success(categories));
        }

        public async Task<ApiResponse<List<ActionFilterItem>>> GetActionCompaniesAsync(
            IEnumerable<int> categories, IEnumerable<int> services, IEnumerable<int> sites)
        {
            try
            {
                // Derive distinct companies from Actions; assign sequential IDs for local dev
                var rows = await _dbContext.Actions
                    .Where(a => a.Site != null)
                    .Select(a => a.Site!)
                    .Distinct()
                    .OrderBy(s => s)
                    .ToListAsync();

                var items = rows
                    .Select((label, idx) => new ActionFilterItem { Id = idx + 1, Label = label })
                    .ToList();

                return Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load action companies");
                return Failure<List<ActionFilterItem>>("Failed to load action companies");
            }
        }

        public async Task<ApiResponse<List<ActionFilterItem>>> GetActionServicesAsync(
            IEnumerable<int> companies, IEnumerable<int> categories, IEnumerable<int> sites)
        {
            try
            {
                var rows = await _dbContext.Actions
                    .Where(a => a.Service != null)
                    .Select(a => a.Service!)
                    .Distinct()
                    .OrderBy(s => s)
                    .ToListAsync();

                var items = rows
                    .Select((label, idx) => new ActionFilterItem { Id = idx + 1, Label = label })
                    .ToList();

                return Success(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load action services");
                return Failure<List<ActionFilterItem>>("Failed to load action services");
            }
        }

        public async Task<ApiResponse<List<ActionSiteNode>>> GetActionSitesAsync(
            IEnumerable<int> companies, IEnumerable<int> categories, IEnumerable<int> services)
        {
            try
            {
                var sites = await _dbContext.Actions
                    .Where(a => a.Site != null)
                    .Select(a => a.Site!)
                    .Distinct()
                    .OrderBy(s => s)
                    .ToListAsync();

                var nodes = sites
                    .Select((label, idx) => new ActionSiteNode { Id = idx + 1, Label = label })
                    .ToList();

                return Success(nodes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load action sites");
                return Failure<List<ActionSiteNode>>("Failed to load action sites");
            }
        }

        private static ActionItem MapToModel(Domain.Entities.ActionItem e) => new ActionItem
        {
            Id = e.Id,
            Action = e.Action,
            DueDate = e.DueDate,
            HighPriority = e.HighPriority ? 1 : 0,
            Message = e.Message,
            Language = e.Language,
            Service = e.Service,
            Site = e.Site,
            EntityType = e.EntityType,
            EntityId = e.EntityId?.ToString(),
            Subject = e.Subject,
            SnowLink = e.SnowLink
        };

        private static ApiResponse<T> Success<T>(T data) => new ApiResponse<T>
        {
            Data = data,
            IsSuccess = true,
            Message = "Success",
            ErrorCode = string.Empty
        };

        private static ApiResponse<T> Failure<T>(string message) => new ApiResponse<T>
        {
            Data = default,
            IsSuccess = false,
            Message = message,
            ErrorCode = "ERR_ACTIONS"
        };
    }
}
