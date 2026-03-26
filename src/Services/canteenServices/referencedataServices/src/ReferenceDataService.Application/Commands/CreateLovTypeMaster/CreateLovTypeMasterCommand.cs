using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Commands.CreateLovTypeMaster;

public record CreateLovTypeMasterCommand(string LovTypeCode, string? LovTypeName) : IRequest<LovTypeMasterDto>;
