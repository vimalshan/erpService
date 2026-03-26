using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Commands.UpdateLovTypeMaster;

public record UpdateLovTypeMasterCommand(string LovTypeCode, string? LovTypeName) : IRequest<LovTypeMasterDto>;
