using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Commands.CreateLovMaster;

public record CreateLovMasterCommand(string LovId, string? LovType, string? LovName) : IRequest<LovMasterDto>;
