namespace CheckupManagementService.Application.Commands;

using MediatR;
using Microsoft.Extensions.Logging;
using CheckupManagementService.Domain.Entities;
using CheckupManagementService.Infrastructure.Persistence;
using Shared.Core.Repositories;
using Shared.Events;
using Shared.Infrastructure.Utilities;

/// <summary>
/// Command handler for creating checkups
/// </summary>
public class CreateCheckupCommandHandler : IRequestHandler<CreateCheckupCommand, CreateCheckupResponse>
{
    private readonly IRepository<CheckupMaster, long> _checkupRepository;
    private readonly IRepository<CheckupTestLink, long> _testLinkRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CreateCheckupCommandHandler> _logger;

    public CreateCheckupCommandHandler(
        IRepository<CheckupMaster, long> checkupRepository,
        IRepository<CheckupTestLink, long> testLinkRepository,
        IEventPublisher eventPublisher,
        ILogger<CreateCheckupCommandHandler> logger)
    {
        _checkupRepository = checkupRepository;
        _testLinkRepository = testLinkRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<CreateCheckupResponse> Handle(
        CreateCheckupCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Creating checkup for employee: {EmployeeNumber}",
            request.EmployeeNumber);

        try
        {
            var checkupId = $"CHK{DateTime.UtcNow:yyyyMMddHHmmss}";

            var checkup = new CheckupMaster
            {
                CheckupMasterId = checkupId,
                EmployeeNumber = request.EmployeeNumber,
                CheckupType = request.CheckupType,
                CheckupDate = request.CheckupDate,
                DoctorCode = request.DoctorCode,
                Status = "Pending",
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM"
            };

            await _checkupRepository.AddAsync(checkup, ct);

            // Create test links
            if (request.TestIds.Any())
            {
                var testLinks = request.TestIds.Select(testId => new CheckupTestLink
                {
                    LinkId = Guid.NewGuid().ToString(),
                    CheckupMasterId = checkupId,
                    TestId = testId,
                    IsRequired = true,
                    CreatedOn = DateTime.UtcNow
                }).ToList();

                foreach (var link in testLinks)
                {
                    await _testLinkRepository.AddAsync(link, ct);
                }
            }

            // Publish event
            await _eventPublisher.PublishAsync(
                new CheckupScheduledEvent(
                    checkupId,
                    request.EmployeeNumber,
                    request.CheckupDate,
                    request.CheckupType), ct);

            _logger.LogInformation(
                "Checkup created successfully: {CheckupMasterId}",
                checkupId);

            return new CreateCheckupResponse
            {
                CheckupMasterId = checkupId,
                EmployeeNumber = request.EmployeeNumber,
                CheckupType = request.CheckupType,
                CheckupDate = request.CheckupDate,
                Status = "Pending"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating checkup for employee: {EmployeeNumber}", request.EmployeeNumber);
            throw;
        }
    }
}

/// <summary>
/// Command handler for updating checkup status
/// </summary>
public class UpdateCheckupStatusCommandHandler : IRequestHandler<UpdateCheckupStatusCommand, UpdateCheckupResponse>
{
    private readonly IRepository<CheckupMaster, long> _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<UpdateCheckupStatusCommandHandler> _logger;

    public UpdateCheckupStatusCommandHandler(
        IRepository<CheckupMaster, long> repository,
        IEventPublisher eventPublisher,
        ILogger<UpdateCheckupStatusCommandHandler> logger)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<UpdateCheckupResponse> Handle(
        UpdateCheckupStatusCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Updating checkup status: {CheckupMasterId}",
            request.CheckupMasterId);

        if (!long.TryParse(request.CheckupMasterId, out var checkupId))
        {
            _logger.LogWarning("Invalid checkup ID format: {CheckupMasterId}", request.CheckupMasterId);
            throw new InvalidOperationException($"Invalid checkup ID format: '{request.CheckupMasterId}'");
        }

        var checkup = await _repository.GetByIdAsync(checkupId, ct);
        if (checkup == null)
        {
            _logger.LogWarning("Checkup not found: {CheckupMasterId}", request.CheckupMasterId);
            throw new InvalidOperationException($"Checkup '{request.CheckupMasterId}' not found");
        }

        if (!string.IsNullOrEmpty(request.Status))
            checkup.Status = request.Status;

        if (!string.IsNullOrEmpty(request.DoctorRemarks))
            checkup.DoctorRemarks = request.DoctorRemarks;

        if (!string.IsNullOrEmpty(request.ApprovedBy))
        {
            checkup.ApprovedBy = request.ApprovedBy;
            checkup.ApprovedDate = DateTime.UtcNow;
        }

        checkup.UpdatedOn = DateTime.UtcNow;
        checkup.UpdatedBy = request.ApprovedBy ?? "SYSTEM";

        await _repository.UpdateAsync(checkup, ct);

        _logger.LogInformation(
            "Checkup updated successfully: {CheckupMasterId}",
            request.CheckupMasterId);

        return new UpdateCheckupResponse
        {
            CheckupMasterId = checkup.CheckupMasterId,
            Status = checkup.Status,
            UpdatedOn = checkup.UpdatedOn
        };
    }
}

/// <summary>
/// Command handler for recording health examination
/// </summary>
public class RecordHealthExaminationCommandHandler : IRequestHandler<RecordHealthExaminationCommand, RecordHealthExaminationResponse>
{
    private readonly IRepository<HealthMain, long> _healthRepository;
    private readonly IRepository<HealthSub, long> _healthSubRepository;
    private readonly ILogger<RecordHealthExaminationCommandHandler> _logger;

    public RecordHealthExaminationCommandHandler(
        IRepository<HealthMain, long> healthRepository,
        IRepository<HealthSub, long> healthSubRepository,
        ILogger<RecordHealthExaminationCommandHandler> logger)
    {
        _healthRepository = healthRepository;
        _healthSubRepository = healthSubRepository;
        _logger = logger;
    }

    public async Task<RecordHealthExaminationResponse> Handle(
        RecordHealthExaminationCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Recording health examination for checkup: {CheckupMasterId}",
            request.CheckupMasterId);

        var healthId = $"HLT{DateTime.UtcNow:yyyyMMddHHmmss}";

        // Calculate BMI
        decimal? bmi = null;
        if (request.Height.HasValue && request.Weight.HasValue && request.Height > 0)
        {
            var heightInMeters = request.Height.Value / 100;
            bmi = request.Weight.Value / (heightInMeters * heightInMeters);
        }

        var health = new HealthMain
        {
            HealthId = healthId,
            CheckupMasterId = request.CheckupMasterId,
            EmployeeNumber = request.EmployeeNumber,
            Height = request.Height,
            Weight = request.Weight,
            BMI = bmi,
            BloodPressure = request.BloodPressure,
            HeartRate = request.HeartRate,
            BloodGroup = request.BloodGroup,
            EyeVision = request.EyeVision,
            CreatedOn = DateTime.UtcNow
        };

        await _healthRepository.AddAsync(health, ct);

        // Record test results
        if (request.TestResults.Any())
        {
            var healthSubs = request.TestResults.Select(tr => new HealthSub
            {
                HealthSubId = Guid.NewGuid().ToString(),
                HealthId = healthId,
                TestName = tr.TestName,
                TestValue = tr.TestValue,
                Result = tr.Result,
                Remarks = tr.Remarks,
                CreatedOn = DateTime.UtcNow
            }).ToList();

            foreach (var sub in healthSubs)
            {
                await _healthSubRepository.AddAsync(sub, ct);
            }
        }

        _logger.LogInformation(
            "Health examination recorded: {HealthId}",
            healthId);

        return new RecordHealthExaminationResponse
        {
            HealthId = healthId,
            CheckupMasterId = request.CheckupMasterId,
            BMI = bmi,
            Message = "Health examination recorded successfully"
        };
    }
}

/// <summary>
/// Command handler for creating test master
/// </summary>
public class CreateTestMasterCommandHandler : IRequestHandler<CreateTestMasterCommand, CreateTestMasterResponse>
{
    private readonly IRepository<TestMaster, long> _repository;
    private readonly ILogger<CreateTestMasterCommandHandler> _logger;

    public CreateTestMasterCommandHandler(
        IRepository<TestMaster, long> repository,
        ILogger<CreateTestMasterCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CreateTestMasterResponse> Handle(
        CreateTestMasterCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Creating test master: {TestName}",
            request.TestName);

        var testId = $"TST{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

        var test = new TestMaster
        {
            TestId = testId,
            TestName = request.TestName,
            TestCategory = request.TestCategory,
            NormalRange = request.NormalRange,
            Unit = request.Unit,
            Cost = request.Cost,
            IsActive = true,
            CreatedOn = DateTime.UtcNow
        };

        await _repository.AddAsync(test, ct);

        _logger.LogInformation(
            "Test master created: {TestId}",
            testId);

        return new CreateTestMasterResponse
        {
            TestId = testId,
            TestName = request.TestName,
            TestCategory = request.TestCategory
        };
    }
}

/// <summary>
/// Command handler for recording checkup other details
/// </summary>
public class RecordCheckupOthersCommandHandler : IRequestHandler<RecordCheckupOthersCommand, RecordCheckupOthersResponse>
{
    private readonly IRepository<CheckupOthers, long> _repository;
    private readonly ILogger<RecordCheckupOthersCommandHandler> _logger;

    public RecordCheckupOthersCommandHandler(
        IRepository<CheckupOthers, long> repository,
        ILogger<RecordCheckupOthersCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<RecordCheckupOthersResponse> Handle(
        RecordCheckupOthersCommand request,
        CancellationToken ct)
    {
        var othersId = $"CHO{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

        var others = new CheckupOthers
        {
            CheckupOthersId = othersId,
            CheckupMasterId = request.CheckupMasterId,
            MedicineAllergy = request.MedicineAllergy,
            FamilyHistory = request.FamilyHistory,
            PastSurgery = request.PastSurgery,
            CurrentMedicines = request.CurrentMedicines,
            LifestyleHabits = request.LifestyleHabits,
            OtherComments = request.OtherComments,
            CreatedOn = DateTime.UtcNow
        };

        await _repository.AddAsync(others, ct);

        _logger.LogInformation(
            "Checkup others recorded: {CheckupOthersId}",
            othersId);

        return new RecordCheckupOthersResponse
        {
            CheckupOthersId = othersId,
            CheckupMasterId = request.CheckupMasterId
        };
    }
}

/// <summary>
/// Command handler for issuing health check card
/// </summary>
public class IssueHealthCheckCardCommandHandler : IRequestHandler<IssueHealthCheckCardCommand, IssueHealthCheckCardResponse>
{
    private readonly IRepository<HealthCheckCard, long> _repository;
    private readonly ILogger<IssueHealthCheckCardCommandHandler> _logger;

    public IssueHealthCheckCardCommandHandler(
        IRepository<HealthCheckCard, long> repository,
        ILogger<IssueHealthCheckCardCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IssueHealthCheckCardResponse> Handle(
        IssueHealthCheckCardCommand request,
        CancellationToken ct)
    {
        var cardNumber = $"HCC{DateTime.UtcNow:yyyyMMddHHmm}{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";

        var card = new HealthCheckCard
        {
            CardNumber = cardNumber,
            CheckupMasterId = request.CheckupMasterId,
            EmployeeNumber = decimal.TryParse(request.EmployeeNumber, out var empNum) ? empNum : null,
            IssueDate = DateTime.UtcNow,
            ExpiryDate = request.ExpiryDate ?? DateTime.UtcNow.AddYears(1),
            CardStatus = "Valid",
            IssuedBy = request.IssuedBy ?? "SYSTEM",
            CreatedOn = DateTime.UtcNow
        };

        await _repository.AddAsync(card, ct);

        _logger.LogInformation(
            "Health check card issued: {CardNumber}",
            cardNumber);

        return new IssueHealthCheckCardResponse
        {
            CardNumber = cardNumber,
            CheckupMasterId = request.CheckupMasterId,
            IssueDate = card.IssueDate,
            ExpiryDate = card.ExpiryDate,
            CardStatus = "Valid"
        };
    }
}
