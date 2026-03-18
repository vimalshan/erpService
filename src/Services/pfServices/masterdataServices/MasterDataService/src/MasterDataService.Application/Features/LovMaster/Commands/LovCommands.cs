using MasterDataService.Application.DTOs;
using MediatR;

namespace MasterDataService.Application.Features.LovMaster.Commands;

public record CreateLovCommand(string LovCode, string LovDescription, string LovValue, string LovCategory) : IRequest<LovMasterDto>;
public record UpdateLovCommand(decimal LovId, string LovCode, string LovDescription, string LovValue, string LovCategory, string LovStatus) : IRequest<LovMasterDto>;
public record ActivateLovCommand(decimal LovId) : IRequest<bool>;
public record DeactivateLovCommand(decimal LovId) : IRequest<bool>;
public record DeleteLovCommand(decimal LovId) : IRequest<bool>;
