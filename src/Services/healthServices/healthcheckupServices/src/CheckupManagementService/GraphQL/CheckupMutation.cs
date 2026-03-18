using HotChocolate;
using MediatR;
using CheckupManagementService.Application.Commands;
using CheckupManagementService.DTOs;

namespace CheckupManagementService.GraphQL;

/// <summary>
/// GraphQL Mutation types for Checkup Management
/// </summary>
[MutationType]
public class CheckupMutation
{
    /// <summary>
    /// Create a new checkup
    /// </summary>
    public async Task<CreateCheckupResponse> CreateCheckup(
        [Service] IMediator mediator,
        CreateCheckupCommand command)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Update checkup status
    /// </summary>
    public async Task<UpdateCheckupResponse> UpdateCheckupStatus(
        [Service] IMediator mediator,
        UpdateCheckupStatusCommand command)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Record health examination
    /// </summary>
    public async Task<RecordHealthExaminationResponse> RecordHealthExamination(
        [Service] IMediator mediator,
        RecordHealthExaminationCommand command)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Create a new test master
    /// </summary>
    public async Task<CreateTestMasterResponse> CreateTestMaster(
        [Service] IMediator mediator,
        CreateTestMasterCommand command)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Update test master
    /// </summary>
    public async Task<UpdateTestMasterResponse> UpdateTestMaster(
        [Service] IMediator mediator,
        UpdateTestMasterCommand command)
    {
        return await mediator.Send(command);
    }
}
