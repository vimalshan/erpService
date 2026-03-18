using Moq;
using Xunit;
using AccidentManagementService.Application.Commands;
using AccidentManagementService.Infrastructure.Persistence;
using AccidentManagementService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AccidentManagementService.Tests.CommandHandlers;

/// <summary>
/// Unit tests for Create Accident command handler
/// Tests: accident creation, severity classification, initial status
/// </summary>
public class CreateAccidentCommandHandlerTests
{
    private readonly Mock<Repository<AccidentMain>> _mockRepository;
    private readonly Mock<ILogger<CreateAccidentCommandHandler>> _mockLogger;
    private readonly CreateAccidentCommandHandler _handler;

    public CreateAccidentCommandHandlerTests()
    {
        _mockRepository = new Mock<Repository<AccidentMain>>();
        _mockLogger = new Mock<ILogger<CreateAccidentCommandHandler>>();
        _handler = new CreateAccidentCommandHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidAccidentReport_CreatesAccidentSuccessfully()
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Minor Injury",
            Severity = "Low",
            Description = "Cut on hand during work",
            Location = "Assembly Area"
        };

        AccidentMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<AccidentMain>()))
            .Callback<AccidentMain>(a => capturedEntity = a)
            .ReturnsAsync((AccidentMain a) => a);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("EMP001", result.EmployeeNumber);
        Assert.StartsWith("ACC", result.AccidentId);  // Accident ID pattern: ACC + timestamp
        Assert.Equal("Open", result.Status);  // Initial status
        Assert.Equal("Low", result.Severity);

        // Verify entity was created
        Assert.NotNull(capturedEntity);
        Assert.Equal("Open", capturedEntity.AccidentStatus);
    }

    [Fact]
    public async Task Handle_HighSeverityAccident_FlagsForImmediateAttention()
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Serious Injury",
            Severity = "High",
            Description = "Serious injury requiring immediate medical attention",
            Location = "Factory floor"
        };

        AccidentMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<AccidentMain>()))
            .Callback<AccidentMain>(a => capturedEntity = a)
            .ReturnsAsync((AccidentMain a) => a);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("High", result.Severity);
        Assert.True(capturedEntity.RequiresImmediateAttention);  // High severity flag
    }

    [Fact]
    public async Task Handle_ValidAccident_GeneratesCorrectIdFormat()
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Minor Injury",
            Severity = "Low",
            Description = "Test accident"
        };

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<AccidentMain>()))
            .ReturnsAsync((AccidentMain a) => a);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: ID should be ACC + timestamp (yyyyMMddHHmmss) + suffix
        Assert.StartsWith("ACC", result.AccidentId);
        Assert.True(result.AccidentId.Length > 15);  // ACC + timestamp min
    }

    [Fact]
    public async Task Handle_ValidAccident_SetsCreatedTimestamp()
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Minor Injury",
            Severity = "Low",
            Description = "Test accident"
        };

        var beforeCreation = DateTime.UtcNow;
        AccidentMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<AccidentMain>()))
            .Callback<AccidentMain>(a => capturedEntity = a)
            .ReturnsAsync((AccidentMain a) => a);

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
        Assert.Equal(capturedEntity.CreatedOn, capturedEntity.UpdatedOn);  // Same on create
    }
}

/// <summary>
/// Unit tests for Update Accident Status command handler
/// Tests: status transitions, investigation workflow
/// </summary>
public class UpdateAccidentStatusCommandHandlerTests
{
    private readonly Mock<Repository<AccidentMain>> _mockRepository;
    private readonly Mock<ILogger<UpdateAccidentStatusCommandHandler>> _mockLogger;
    private readonly UpdateAccidentStatusCommandHandler _handler;

    public UpdateAccidentStatusCommandHandlerTests()
    {
        _mockRepository = new Mock<Repository<AccidentMain>>();
        _mockLogger = new Mock<ILogger<UpdateAccidentStatusCommandHandler>>();
        _handler = new UpdateAccidentStatusCommandHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidStatusTransition_UpdatesAccidentSuccessfully()
    {
        // Arrange
        var accident = new AccidentMain
        {
            AccidentId = "ACC001",
            EmployeeNumber = "EMP001",
            AccidentStatus = "Open",
            Severity = "Low"
        };

        var command = new UpdateAccidentStatusCommand
        {
            AccidentId = "ACC001",
            NewStatus = "UnderInvestigation",
            InvestigatorName = "John Doe",
            Notes = "Investigation started"
        };

        AccidentMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.GetByIdAsync("ACC001"))
            .ReturnsAsync(accident);

        _mockRepository
            .Setup(r => r.Update(It.IsAny<AccidentMain>()))
            .Callback<AccidentMain>(a => capturedEntity = a)
            .Returns((AccidentMain a) => a);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.Equal("UnderInvestigation", result.Status);
        Assert.Equal("John Doe", result.InvestigatorName);
    }

    [Fact]
    public async Task Handle_TransitionToResolved_UpdatesResolutionDate()
    {
        // Arrange
        var accident = new AccidentMain
        {
            AccidentId = "ACC001",
            EmployeeNumber = "EMP001",
            AccidentStatus = "UnderInvestigation",
            ResolutionDate = null
        };

        var command = new UpdateAccidentStatusCommand
        {
            AccidentId = "ACC001",
            NewStatus = "Resolved",
            ResolutionNotes = "Investigation completed, corrective actions identified"
        };

        AccidentMain capturedEntity = null;
        _mockRepository
            .Setup(r => r.GetByIdAsync("ACC001"))
            .ReturnsAsync(accident);

        _mockRepository
            .Setup(r => r.Update(It.IsAny<AccidentMain>()))
            .Callback<AccidentMain>(a => capturedEntity = a)
            .Returns((AccidentMain a) => a);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Resolved", result.Status);
        Assert.NotNull(capturedEntity.ResolutionDate);
        Assert.True(capturedEntity.ResolutionDate <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_InvalidAccident_ThrowsException()
    {
        // Arrange
        var command = new UpdateAccidentStatusCommand
        {
            AccidentId = "INVALID_ID",
            NewStatus = "Resolved"
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync("INVALID_ID"))
            .ReturnsAsync((AccidentMain)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Contains("not found", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Handle_ValidStatusUpdate_UpdatesTimeStamp()
    {
        // Arrange
        var accident = new AccidentMain
        {
            AccidentId = "ACC001",
            EmployeeNumber = "EMP001",
            AccidentStatus = "Open",
            UpdatedOn = DateTime.UtcNow.AddHours(-1)
        };

        var command = new UpdateAccidentStatusCommand
        {
            AccidentId = "ACC001",
            NewStatus = "UnderInvestigation"
        };

        AccidentMain capturedEntity = null;
        var beforeUpdate = DateTime.UtcNow;
        _mockRepository
            .Setup(r => r.GetByIdAsync("ACC001"))
            .ReturnsAsync(accident);

        _mockRepository
            .Setup(r => r.Update(It.IsAny<AccidentMain>()))
            .Callback<AccidentMain>(a => capturedEntity = a)
            .Returns((AccidentMain a) => a);

        _mockRepository
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);
        var afterUpdate = DateTime.UtcNow;

        // Assert
        Assert.NotNull(capturedEntity);
        Assert.True(capturedEntity.UpdatedOn >= beforeUpdate);
        Assert.True(capturedEntity.UpdatedOn <= afterUpdate);
        Assert.True(capturedEntity.UpdatedOn > accident.UpdatedOn);  // Updated time changed
    }
}

/// <summary>
/// Validator unit tests for Accident commands
/// </summary>
public class CreateAccidentCommandValidatorTests
{
    private readonly CreateAccidentCommandValidator _validator;

    public CreateAccidentCommandValidatorTests()
    {
        _validator = new CreateAccidentCommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_EmptyEmployeeNumber_Failed(string employeeNumber)
    {
        // Arrange
        var command = new CreateAccidentCommand { EmployeeNumber = employeeNumber };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("Invalid")]  // Not a valid severity level
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_InvalidSeverity_Failed(string severity)
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Minor Injury",
            Severity = severity
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("Low")]
    [InlineData("Medium")]
    [InlineData("High")]
    [InlineData("Critical")]
    public async Task Validate_ValidSeverityLevels_Passed(string severity)
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Minor Injury",
            Severity = severity,
            Description = "Test accident"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_ValidAccidentReport_Passed()
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Minor Injury",
            Severity = "Low",
            Description = "Cut on hand during work",
            Location = "Assembly Area"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Validate_EmptyDescription_Failed()
    {
        // Arrange
        var command = new CreateAccidentCommand
        {
            EmployeeNumber = "EMP001",
            SiteCode = "SITE001",
            AccidentType = "Minor Injury",
            Severity = "Low",
            Description = ""  // Empty
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }
}

/// <summary>
/// Validator unit tests for Update Accident Status
/// </summary>
public class UpdateAccidentStatusCommandValidatorTests
{
    private readonly UpdateAccidentStatusCommandValidator _validator;

    public UpdateAccidentStatusCommandValidatorTests()
    {
        _validator = new UpdateAccidentStatusCommandValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_EmptyAccidentId_Failed(string accidentId)
    {
        // Arrange
        var command = new UpdateAccidentStatusCommand
        {
            AccidentId = accidentId,
            NewStatus = "UnderInvestigation"
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("InvalidStatus")]  // Not a valid status
    [InlineData("")]
    [InlineData(null)]
    public async Task Validate_InvalidStatus_Failed(string newStatus)
    {
        // Arrange
        var command = new UpdateAccidentStatusCommand
        {
            AccidentId = "ACC001",
            NewStatus = newStatus
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("Open")]
    [InlineData("UnderInvestigation")]
    [InlineData("Resolved")]
    [InlineData("Closed")]
    public async Task Validate_ValidStatusValues_Passed(string newStatus)
    {
        // Arrange
        var command = new UpdateAccidentStatusCommand
        {
            AccidentId = "ACC001",
            NewStatus = newStatus
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
    }
}
