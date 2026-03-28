using MediatR;
using Document.Application.DTOs;
using Document.Application.Features.Signatories.Commands;

namespace Document.API.GraphQL;

public class DocumentMutation
{
    public async Task<SignatoryDto> CreateSignatory(
        [Service] IMediator mediator,
        CreateSignatoryRequest input,
        CancellationToken ct)
    {
        return await mediator.Send(new CreateSignatoryCommand(
            input.SignatoryNumber, input.Name, input.Designation,
            input.EmployeeSysId, input.ImageFileName), ct);
    }
}
