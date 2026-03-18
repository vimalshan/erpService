using FluentAssertions;
using FluentValidation;
using Stationery.Application.Features.Requests.Validators;
using Stationery.Application.Features.Requests.Commands;
using Xunit;

namespace Stationery.UnitTests.Application.Requests;

public class CreateRequestValidatorTests
{
    private readonly CreateRequestCommandValidator _validator = new();

    [Fact]
    public async Task ValidCommand_ShouldPassValidation()
    {
        var command = new CreateRequestCommand(
            RequestedBy: 1,
            LocationId: 1,
            UnitCode: "HO",
            Details: new List<RequestDetailDto>
            {
                new(StationaryId: 1, DeptId: 100, ExpectedDate: DateTime.UtcNow.AddDays(5), RequestedQty: 10)
            });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task EmptyDetails_ShouldFailValidation()
    {
        var command = new CreateRequestCommand(
            RequestedBy: 1,
            LocationId: 1,
            UnitCode: "HO",
            Details: new List<RequestDetailDto>());

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Details");
    }

    [Fact]
    public async Task ZeroRequestedBy_ShouldFailValidation()
    {
        var command = new CreateRequestCommand(
            RequestedBy: 0,
            LocationId: 1,
            UnitCode: "HO",
            Details: new List<RequestDetailDto>
            {
                new(StationaryId: 1, DeptId: 100, ExpectedDate: DateTime.UtcNow.AddDays(5), RequestedQty: 10)
            });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "RequestedBy");
    }

    [Fact]
    public async Task UnitCodeTooLong_ShouldFailValidation()
    {
        var command = new CreateRequestCommand(
            RequestedBy: 1,
            LocationId: 1,
            UnitCode: "TOOLONG",
            Details: new List<RequestDetailDto>
            {
                new(StationaryId: 1, DeptId: 100, ExpectedDate: DateTime.UtcNow.AddDays(5), RequestedQty: 10)
            });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UnitCode");
    }

    [Fact]
    public async Task ZeroRequestedQty_ShouldFailValidation()
    {
        var command = new CreateRequestCommand(
            RequestedBy: 1,
            LocationId: 1,
            UnitCode: "HO",
            Details: new List<RequestDetailDto>
            {
                new(StationaryId: 1, DeptId: 100, ExpectedDate: DateTime.UtcNow.AddDays(5), RequestedQty: 0)
            });

        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeFalse();
    }
}
