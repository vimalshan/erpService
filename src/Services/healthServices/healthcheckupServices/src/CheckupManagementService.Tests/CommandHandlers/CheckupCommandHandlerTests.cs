using Moq;
using Xunit;
using CheckupManagementService.Application.Commands;
using CheckupManagementService.Infrastructure.Persistence;
using CheckupManagementService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CheckupManagementService.Tests.CommandHandlers;

/// <summary>
/// Unit tests for Schedule Checkup command handler
/// Tests: checkup creation, scheduling, media attachment
/// </summary>
public class ScheduleCheckupCommandHandlerTests
{
    private readonly Mock<Repository<CheckupMain>> _mockRepository;
    private readonly Mock<ILogger<ScheduleCheckupCommandHandler>> _mockLogger;
    private readonly ScheduleCheckupCommandHandler _handler;

    public ScheduleCheckupCommandHandlerTests()
    {
        _mockRepository = new Mock<Repository<CheckupMain>>();
        _mockLogger = new Mock<ILogger<ScheduleCheckupCommandHandler>>();
        _handler = new ScheduleCheckupCommandHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidCheckupSchedule_CreatesCheckupSuccessfully()
    {
        // Arrange
        var scheduledDate = DateTime.UtcNow.AddDays(7);  // Schedule for next week
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = "Annual",
            ScheduledDate = scheduledDate,
            Department = "Engineering",
            SpecialRequirements = "None"
        };

        CheckupMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<CheckupMain>()))
            .Callback<CheckupMain>(c => capturedEntity = c)
            .ReturnsAsync((CheckupMain c) => c);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("EMP001", result.EmployeeNumber);
        Assert.StartsWith("CHK", result.CheckupId);  // Checkup ID pattern: CHK + timestamp
        Assert.Equal("Scheduled", result.Status);
        Assert.Equal(scheduledDate, result.ScheduledDate);

        // Verify entity was created
        Assert.NotNull(capturedEntity);
        Assert.Equal("Scheduled", capturedEntity.CheckupStatus);
    }

    [Fact]
    public async Task Handle_ValidCheckupSchedule_GeneratesCorrectIdFormat()
    {
        // Arrange
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = "Annual",
            ScheduledDate = DateTime.UtcNow.AddDays(7),
            Department = "Engineering"
        };

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<CheckupMain>()))
            .ReturnsAsync((CheckupMain c) => c);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: ID should be CHK + timestamp
        Assert.StartsWith("CHK", result.CheckupId);
        Assert.True(result.CheckupId.Length > 15);  // CHK + timestamp min
    }

    [Fact]
    public async Task Handle_ValidCheckup_SetsCreatedTimestamp()
    {
        // Arrange
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = "Annual",
            ScheduledDate = DateTime.UtcNow.AddDays(7),
            Department = "Engineering"
        };

        var beforeCreation = DateTime.UtcNow;
        CheckupMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<CheckupMain>()))
            .Callback<CheckupMain>(c => capturedEntity = c)
            .ReturnsAsync((CheckupMain c) => c);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.True(capturedEntity.CreatedOn >= beforeCreation);
        Assert.True(capturedEntity.CreatedOn <= afterCreation);
        Assert.Equal(capturedEntity.CreatedOn, capturedEntity.UpdatedOn);
    }

    [Fact]
    public async Task Handle_ScheduledDateInPast_ThrowsException()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = "Annual",
            ScheduledDate = pastDate,
            Department = "Engineering"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("past", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}

/// <summary>
/// Unit tests for Conduct Checkup command handler
/// Tests: checkup execution, findings recording, media attachment
/// </summary>
public class ConductCheckupCommandHandlerTests
{
    private readonly Mock<Repository<CheckupMain>> _mockRepository;
    private readonly Mock<ILogger<ConductCheckupCommandHandler>> _mockLogger;
    private readonly ConductCheckupCommandHandler _handler;

    public ConductCheckupCommandHandlerTests()
    {
        _mockRepository = new Mock<Repository<CheckupMain>>();
        _mockLogger = new Mock<ILogger<ConductCheckupCommandHandler>>();
        _handler = new ConductCheckupCommandHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidCheckupConduct_UpdatesCheckupSuccessfully()
    {
        // Arrange
        var checkup = new CheckupMain
        {
            CheckupId = "CHK001",
            EmployeeNumber = "EMP001",
            CheckupStatus = "Scheduled",
            ScheduledDate = DateTime.UtcNow.AddDays(-1),
            ConductedDate = null
        };

        var command = new ConductCheckupCommand
        {
            CheckupId = "CHK001",
            CheckupFindings = "Employee is in good health",
            FollowUpRequired = false,
            ConductingDoctor = "Dr. Smith"
        };

        CheckupMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.GetByIdAsync("CHK001"))
            .ReturnsAsync(checkup);

        _mockRepository
            .Setup(r => r.Update(It.IsAny<CheckupMain>()))
            .Callback<CheckupMain>(c => capturedEntity = c)
            .Returns((CheckupMain c) => c);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.Equal("Completed", result.Status);
        Assert.NotNull(result.ConductedDate);
        Assert.Equal("Dr. Smith", result.ConductingDoctor);
        Assert.False(result.FollowUpRequired);
    }

    [Fact]
    public async Task Handle_CheckupConduct_SetsConductedDate()
    {
        // Arrange
        var checkup = new CheckupMain
        {
            CheckupId = "CHK001",
            EmployeeNumber = "EMP001",
            CheckupStatus = "Scheduled",
            ConductedDate = null
        };

        var command = new ConductCheckupCommand
        {
            CheckupId = "CHK001",
            CheckupFindings = "All tests normal",
            FollowUpRequired = false,
            ConductingDoctor = "Dr. Smith"
        };

        var beforeConduct = DateTime.UtcNow;
        CheckupMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.GetByIdAsync("CHK001"))
            .ReturnsAsync(checkup);

        _mockRepository
            .Setup(r => r.Update(It.IsAny<CheckupMain>()))
            .Callback<CheckupMain>(c => capturedEntity = c)
            .Returns((CheckupMain c) => c);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);
        var afterConduct = DateTime.UtcNow;

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.NotNull(capturedEntity.ConductedDate);
        Assert.True(capturedEntity.ConductedDate >= beforeConduct);
        Assert.True(capturedEntity.ConductedDate <= afterConduct);
    }

    [Fact]
    public async Task Handle_InvalidCheckup_ThrowsException()
    {
        // Arrange
        var command = new ConductCheckupCommand
        {
            CheckupId = "INVALID_ID",
            CheckupFindings = "Test"
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync("INVALID_ID"))
            .ReturnsAsync((CheckupMain)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("not found", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Handle_CheckupWithFollowUp_FlagsAppropriately()
    {
        // Arrange
        var checkup = new CheckupMain
        {
            CheckupId = "CHK001",
            EmployeeNumber = "EMP001",
            CheckupStatus = "Scheduled"
        };

        var command = new ConductCheckupCommand
        {
            CheckupId = "CHK001",
            CheckupFindings = "Abnormalities detected",
            FollowUpRequired = true,
            ConductingDoctor = "Dr. Smith"
        };

        CheckupMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.GetByIdAsync("CHK001"))
            .ReturnsAsync(checkup);

        _mockRepository
            .Setup(r => r.Update(It.IsAny<CheckupMain>()))
            .Callback<CheckupMain>(c => capturedEntity = c)
            .Returns((CheckupMain c) => c);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.FollowUpRequired);
        Assert.NotNull(capturedEntity);
        Assert.True(capturedEntity.RequiresFollowUp);
    }
}

/// <summary>
/// Validator unit tests for Checkup commands
/// </summary>
public class ScheduleCheckupCommandValidatorTests
{
    private readonly ScheduleCheckupCommandValidator _validator;

    public ScheduleCheckupCommandValidatorTests()
    {
        _validator = new ScheduleCheckupCommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_EmptyEmployeeNumber_Failed(string employeeNumber)
    {
        // Arrange
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = employeeNumber,
            CheckupType = "Annual",
            ScheduledDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task Validate_ScheduledDateInPast_Failed()
    {
        // Arrange
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = "Annual",
            ScheduledDate = DateTime.UtcNow.AddDays(-1)  // Past date
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("ScheduledDate"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Validate_EmptyCheckupType_Failed(string checkupType)
    {
        // Arrange
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = checkupType,
            ScheduledDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("Annual")]
    [InlineData("Periodic")]
    [InlineData("Specific")]
    [InlineData("Return to Work")]
    public async Task Validate_ValidCheckupTypes_Passed(string checkupType)
    {
        // Arrange
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = checkupType,
            ScheduledDate = DateTime.UtcNow.AddDays(7),
            Department = "Engineering"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_ValidCheckupSchedule_Passed()
    {
        // Arrange
        var command = new ScheduleCheckupCommand
        {
            EmployeeNumber = "EMP001",
            CheckupType = "Annual",
            ScheduledDate = DateTime.UtcNow.AddDays(7),
            Department = "Engineering",
            SpecialRequirements = "None"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}

/// <summary>
/// Validator unit tests for Conduct Checkup
/// </summary>
public class ConductCheckupCommandValidatorTests
{
    private readonly ConductCheckupCommandValidator _validator;

    public ConductCheckupCommandValidatorTests()
    {
        _validator = new ConductCheckupCommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_EmptyCheckupId_Failed(string checkupId)
    {
        // Arrange
        var command = new ConductCheckupCommand
        {
            CheckupId = checkupId,
            CheckupFindings = "Test findings"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public async Task Validate_EmptyFindings_Failed(string findings)
    {
        // Arrange
        var command = new ConductCheckupCommand
        {
            CheckupId = "CHK001",
            CheckupFindings = findings
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_EmptyDoctorName_Failed(string doctorName)
    {
        // Arrange
        var command = new ConductCheckupCommand
        {
            CheckupId = "CHK001",
            CheckupFindings = "Employee healthy",
            ConductingDoctor = doctorName
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task Validate_ValidConductCheckup_Passed()
    {
        // Arrange
        var command = new ConductCheckupCommand
        {
            CheckupId = "CHK001",
            CheckupFindings = "All tests normal",
            ConductingDoctor = "Dr. Smith",
            FollowUpRequired = false
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
