using MediatR;

namespace ReferenceDataService.Application.Commands.DeleteLovMaster;

public record DeleteLovMasterCommand(string LovId) : IRequest<bool>;
