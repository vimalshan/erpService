using MediatR;
using LovService.Application.DTOs;

namespace LovService.Application.Features.LovTypeMast.Commands;

public record CreateLovTypeCommand(
    int LovTypeId,
    string LovTypeName,
    string LovCategory,
    int LovOrgId) : IRequest<LovTypeMastDto>;

public record UpdateLovTypeCommand(
    int LovTypeId,
    string LovTypeName,
    string LovCategory,
    int LovOrgId) : IRequest<LovTypeMastDto>;

public record DeleteLovTypeCommand(int LovTypeId) : IRequest<bool>;
