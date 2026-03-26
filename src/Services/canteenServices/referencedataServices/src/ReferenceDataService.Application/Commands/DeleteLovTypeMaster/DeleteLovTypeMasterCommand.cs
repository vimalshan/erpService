using MediatR;

namespace ReferenceDataService.Application.Commands.DeleteLovTypeMaster;

public record DeleteLovTypeMasterCommand(string LovTypeCode) : IRequest<bool>;
