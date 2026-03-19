using EmployeePrideManagement.Application.Commands.CreatePrideMoment;
using EmployeePrideManagement.Application.Commands.DeletePrideMoment;
using EmployeePrideManagement.Application.Commands.UpdatePrideMoment;
using EmployeePrideManagement.Application.DTOs;
using MediatR;

namespace EmployeePrideManagement.API.GraphQL;

public class PrideMomentMutation
{
    public async Task<PrideMomentDto> CreatePrideMoment(
        [Service] IMediator mediator,
        string title,
        string? body,
        decimal employeeSysId,
        string footer,
        string location,
        string imagePath,
        long modifiedBy)
    {
        return await mediator.Send(new CreatePrideMomentCommand(
            title, body, employeeSysId, footer, location, imagePath, modifiedBy));
    }

    public async Task<PrideMomentDto> UpdatePrideMoment(
        [Service] IMediator mediator,
        decimal momentPrideId,
        string title,
        string? body,
        string footer,
        string location,
        string imagePath,
        long modifiedBy)
    {
        return await mediator.Send(new UpdatePrideMomentCommand(
            momentPrideId, title, body, footer, location, imagePath, modifiedBy));
    }

    public async Task<bool> DeletePrideMoment(
        [Service] IMediator mediator,
        decimal momentPrideId)
    {
        return await mediator.Send(new DeletePrideMomentCommand(momentPrideId));
    }
}
