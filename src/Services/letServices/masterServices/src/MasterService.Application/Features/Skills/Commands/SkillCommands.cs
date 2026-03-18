using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.Skills.Commands;

public record CreateSkillCommand(
    long SkillCode,
    string SkillName,
    char SkillType,
    decimal? WeightNum,
    string? Remark,
    DateTime? EffectiveDate) : IRequest<SkillDto>;

public record UpdateSkillCommand(
    long SkillCode,
    string SkillName,
    char SkillType,
    decimal? WeightNum,
    string? Remark) : IRequest<SkillDto>;

public record CloseSkillCommand(long SkillCode) : IRequest<Unit>;
