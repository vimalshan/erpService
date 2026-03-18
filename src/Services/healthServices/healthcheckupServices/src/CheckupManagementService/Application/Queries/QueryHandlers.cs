namespace CheckupManagementService.Application.Queries;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CheckupManagementService.Domain.Entities;
using CheckupManagementService.DTOs;
using CheckupManagementService.Infrastructure.Persistence;
using Shared.Core.Repositories;
using Shared.Infrastructure.Caching;
using Shared.Infrastructure.Utilities;

/// <summary>
/// Query handler for getting checkups
/// </summary>
public class GetCheckupsQueryHandler : IRequestHandler<GetCheckupsQuery, GetCheckupsResponse>
{
    private readonly IRepository<CheckupMaster, long> _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<GetCheckupsQueryHandler> _logger;

    public GetCheckupsQueryHandler(
        IRepository<CheckupMaster, long> repository,
        ICacheService cache,
        ILogger<GetCheckupsQueryHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetCheckupsResponse> Handle(
        GetCheckupsQuery request,
        CancellationToken ct)
    {
        var cacheKey = $"checkups_{request.PageNumber}_{request.PageSize}_{request.Status}";

        // Try cache first
        var cached = await _cache.GetAsync<GetCheckupsResponse>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cached;
        }

        var (skip, take) = PaginationUtilities.GetPaginationValues(
            request.PageNumber,
            request.PageSize);

        var query = (await _repository.GetAllAsync(ct)).AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(request.Status))
            query = query.Where(c => c.Status == request.Status);

        if (!string.IsNullOrEmpty(request.EmployeeNumber))
            query = query.Where(c => c.EmployeeNumber == request.EmployeeNumber);

        if (!string.IsNullOrEmpty(request.CheckupType))
            query = query.Where(c => c.CheckupType == request.CheckupType);

        if (request.FromDate.HasValue)
            query = query.Where(c => c.CheckupDate >= request.FromDate);

        if (request.ToDate.HasValue)
            query = query.Where(c => c.CheckupDate <= request.ToDate);

        var total = query.Count();
        var checkups = query
            .OrderByDescending(c => c.CheckupDate)
            .Skip(skip)
            .Take(take)
            .Select(c => new CheckupMasterDto
            {
                CheckupMasterId = c.CheckupMasterId,
                EmployeeNumber = c.EmployeeNumber,
                CheckupType = c.CheckupType,
                CheckupDate = c.CheckupDate,
                DoctorCode = c.DoctorCode,
                DoctorRemarks = c.DoctorRemarks,
                Status = c.Status,
                ApprovedBy = c.ApprovedBy,
                ApprovedDate = c.ApprovedDate,
                CreatedOn = c.CreatedOn
            })
            .ToList();

        var result = new GetCheckupsResponse
        {
            Data = checkups,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        // Cache for 5 minutes
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        _logger.LogInformation("Retrieved {Count} checkups", checkups.Count);
        return result;
    }
}

/// <summary>
/// Query handler for getting checkup by ID
/// </summary>
public class GetCheckupByIdQueryHandler : IRequestHandler<GetCheckupByIdQuery, CheckupMasterDto?>
{
    private readonly IRepository<CheckupMaster, long> _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<GetCheckupByIdQueryHandler> _logger;

    public GetCheckupByIdQueryHandler(
        IRepository<CheckupMaster, long> repository,
        ICacheService cache,
        ILogger<GetCheckupByIdQueryHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<CheckupMasterDto?> Handle(
        GetCheckupByIdQuery request,
        CancellationToken ct)
    {
        if (!long.TryParse(request.CheckupMasterId, out var checkupId))
        {
            _logger.LogWarning("Invalid checkup ID format: {CheckupMasterId}", request.CheckupMasterId);
            return null;
        }

        var cacheKey = CacheKeyBuilder.CheckupKey(checkupId);

        // Try cache first
        var cached = await _cache.GetAsync<CheckupMasterDto>(cacheKey);
        if (cached != null)
        {
            return cached;
        }

        var checkup = await _repository.GetByIdAsync(checkupId, ct);
        if (checkup == null)
        {
            _logger.LogWarning("Checkup not found: {CheckupMasterId}", request.CheckupMasterId);
            return null;
        }

        var dto = new CheckupMasterDto
        {
            CheckupMasterId = checkup.CheckupMasterId,
            EmployeeNumber = checkup.EmployeeNumber,
            CheckupType = checkup.CheckupType,
            CheckupDate = checkup.CheckupDate,
            DoctorCode = checkup.DoctorCode,
            DoctorRemarks = checkup.DoctorRemarks,
            Status = checkup.Status,
            ApprovedBy = checkup.ApprovedBy,
            ApprovedDate = checkup.ApprovedDate,
            CreatedOn = checkup.CreatedOn
        };

        // Cache for 1 hour
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromHours(1));

        return dto;
    }
}

/// <summary>
/// Query handler for getting checkups by employee
/// </summary>
public class GetCheckupsByEmployeeQueryHandler : IRequestHandler<GetCheckupsByEmployeeQuery, GetCheckupsResponse>
{
    private readonly IRepository<CheckupMaster, long> _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<GetCheckupsByEmployeeQueryHandler> _logger;

    public GetCheckupsByEmployeeQueryHandler(
        IRepository<CheckupMaster, long> repository,
        ICacheService cache,
        ILogger<GetCheckupsByEmployeeQueryHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetCheckupsResponse> Handle(
        GetCheckupsByEmployeeQuery request,
        CancellationToken ct)
    {
        var (skip, take) = PaginationUtilities.GetPaginationValues(
            request.PageNumber,
            request.PageSize);

        var allCheckups = await _repository.GetAllAsync(ct);
        var query = allCheckups
            .Where(c => c.EmployeeNumber == request.EmployeeNumber)
            .AsQueryable();

        var total = query.Count();
        var checkups = query
            .OrderByDescending(c => c.CheckupDate)
            .Skip(skip)
            .Take(take)
            .Select(c => new CheckupMasterDto
            {
                CheckupMasterId = c.CheckupMasterId,
                EmployeeNumber = c.EmployeeNumber,
                CheckupType = c.CheckupType,
                CheckupDate = c.CheckupDate,
                DoctorCode = c.DoctorCode,
                DoctorRemarks = c.DoctorRemarks,
                Status = c.Status,
                ApprovedBy = c.ApprovedBy,
                ApprovedDate = c.ApprovedDate,
                CreatedOn = c.CreatedOn
            })
            .ToList();

        _logger.LogInformation(
            "Retrieved {Count} checkups for employee: {EmployeeNumber}",
            checkups.Count,
            request.EmployeeNumber);

        return new GetCheckupsResponse
        {
            Data = checkups,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

/// <summary>
/// Query handler for getting test masters
/// </summary>
public class GetTestMastersQueryHandler : IRequestHandler<GetTestMastersQuery, GetTestMastersResponse>
{
    private readonly IRepository<TestMaster, long> _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<GetTestMastersQueryHandler> _logger;

    public GetTestMastersQueryHandler(
        IRepository<TestMaster, long> repository,
        ICacheService cache,
        ILogger<GetTestMastersQueryHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GetTestMastersResponse> Handle(
        GetTestMastersQuery request,
        CancellationToken ct)
    {
        var cacheKey = $"tests_{request.PageNumber}_{request.PageSize}_{request.IsActive}";

        // Try cache first
        var cached = await _cache.GetAsync<GetTestMastersResponse>(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cached;
        }

        var (skip, take) = PaginationUtilities.GetPaginationValues(
            request.PageNumber,
            request.PageSize);

        var allTests = await _repository.GetAllAsync(ct);
        var query = allTests.AsQueryable();

        if (request.IsActive.HasValue)
            query = query.Where(t => t.IsActive == request.IsActive.Value);

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(t => t.TestCategory == request.Category);

        var total = query.Count();
        var tests = query
            .OrderBy(t => t.TestName)
            .Skip(skip)
            .Take(take)
            .Select(t => new TestMasterDto
            {
                TestId = t.TestId,
                TestName = t.TestName,
                TestCategory = t.TestCategory,
                NormalRange = t.NormalRange,
                Unit = t.Unit,
                Cost = t.Cost,
                IsActive = t.IsActive
            })
            .ToList();

        var result = new GetTestMastersResponse
        {
            Data = tests,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        // Cache for 24 hours (master data)
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromHours(24));

        _logger.LogInformation("Retrieved {Count} tests", tests.Count);
        return result;
    }
}

/// <summary>
/// Query handler for getting health examination
/// </summary>
public class GetHealthExaminationQueryHandler : IRequestHandler<GetHealthExaminationQuery, HealthMainDto?>
{
    private readonly IRepository<HealthMain, long> _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<GetHealthExaminationQueryHandler> _logger;

    public GetHealthExaminationQueryHandler(
        IRepository<HealthMain, long> repository,
        ICacheService cache,
        ILogger<GetHealthExaminationQueryHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<HealthMainDto?> Handle(
        GetHealthExaminationQuery request,
        CancellationToken ct)
    {
        if (!long.TryParse(request.HealthId, out var healthId))
        {
            _logger.LogWarning("Invalid health ID format: {HealthId}", request.HealthId);
            return null;
        }

        var cacheKey = CacheKeyBuilder.HealthKey(healthId);

        // Try cache first
        var cached = await _cache.GetAsync<HealthMainDto>(cacheKey);
        if (cached != null)
        {
            return cached;
        }

        var health = await _repository.GetByIdAsync(healthId, ct);
        if (health == null)
        {
            return null;
        }

        var dto = new HealthMainDto
        {
            HealthId = health.HealthId,
            CheckupMasterId = health.CheckupMasterId,
            EmployeeNumber = health.EmployeeNumber,
            Height = health.Height,
            Weight = health.Weight,
            BMI = health.BMI,
            BloodPressure = health.BloodPressure,
            HeartRate = health.HeartRate,
            BloodGroup = health.BloodGroup,
            EyeVision = health.EyeVision,
            OverallFitness = health.OverallFitness,
            MedicalClearance = health.MedicalClearance,
            CreatedOn = health.CreatedOn
        };

        // Cache for 1 hour
        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromHours(1));

        return dto;
    }
}

/// <summary>
/// Query handler for getting checkup status report
/// </summary>
public class GetCheckupStatusReportQueryHandler : IRequestHandler<GetCheckupStatusReportQuery, CheckupStatusReportDto>
{
    private readonly IRepository<CheckupMaster, long> _repository;
    private readonly ILogger<GetCheckupStatusReportQueryHandler> _logger;

    public GetCheckupStatusReportQueryHandler(
        IRepository<CheckupMaster, long> repository,
        ILogger<GetCheckupStatusReportQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CheckupStatusReportDto> Handle(
        GetCheckupStatusReportQuery request,
        CancellationToken ct)
    {
        var allCheckups = await _repository.GetAllAsync(ct);
        var query = allCheckups.AsQueryable();

        if (request.FromDate.HasValue)
            query = query.Where(c => c.CheckupDate >= request.FromDate);

        if (request.ToDate.HasValue)
            query = query.Where(c => c.CheckupDate <= request.ToDate);

        var checkups = query.ToList();
        var total = checkups.Count;

        var pending = checkups.Count(c => c.Status == "Pending");
        var completed = checkups.Count(c => c.Status == "Completed");
        var approved = checkups.Count(c => c.Status == "Approved");

        var checkupsByType = checkups
            .GroupBy(c => c.CheckupType)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .Select(g => new CheckupStatusSummary
            {
                CheckupType = g.Key,
                Count = g.Count(),
                Percentage = g.Count() * 100.0m / (total == 0 ? 1 : total)
            })
            .ToList();

        _logger.LogInformation(
            "Report generated: {TotalCheckups} total, {CompletedCheckups} completed",
            total,
            completed);

        return new CheckupStatusReportDto
        {
            TotalCheckups = total,
            PendingCheckups = pending,
            CompletedCheckups = completed,
            ApprovedCheckups = approved,
            CompletionRate = total == 0 ? 0 : (completed + approved) * 100.0m / total,
            TopCheckupTypes = checkupsByType,
            ReportDate = DateTime.UtcNow
        };
    }
}
