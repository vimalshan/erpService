using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Commands;

public class CreateDemandCommand : IRequest<long>
{
    public CreateDemandRequest Request { get; set; }

    public CreateDemandCommand(CreateDemandRequest request)
    {
        Request = request;
    }
}
