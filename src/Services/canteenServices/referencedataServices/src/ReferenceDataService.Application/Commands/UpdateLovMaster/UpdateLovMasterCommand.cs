using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Commands.UpdateLovMaster;

public record UpdateLovMasterCommand(string LovId, string? LovType, string? LovName) : IRequest<LovMasterDto>;
