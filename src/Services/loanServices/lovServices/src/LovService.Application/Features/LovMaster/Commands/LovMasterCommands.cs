using MediatR;
using LovService.Application.DTOs;

namespace LovService.Application.Features.LovMaster.Commands;

public record CreateLovMasterCommand(
    int LovTypeId,
    string LovName,
    long CreatedBy) : IRequest<LovMasterDto>;

public record UpdateLovMasterCommand(
    long LovId,
    string LovName,
    long UpdatedBy) : IRequest<LovMasterDto>;

public record DeleteLovMasterCommand(long LovId) : IRequest<bool>;
