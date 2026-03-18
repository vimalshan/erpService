using DevelopmentService.Application.DTOs;
using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Application.Mappings;

public static class MappingExtensions
{
    public static LetPlanDto ToDto(this LetPlan src) => new(
        src.ReqNum, src.Sno, src.UserId, src.PinNum,
        src.DevSource, src.DevNeed, src.DevIndicator, src.DevMode,
        src.RecProg, src.TrainingProgram, src.InternalTraining,
        src.RevDate, src.Priority, src.EntDate, src.AppStatus, src.BhrStatus);

    public static IEnumerable<LetPlanDto> ToDtos(this IEnumerable<LetPlan> plans) =>
        plans.Select(p => p.ToDto());

    public static LetBhrPlanDto ToDto(this LetBhrPlan src) => new(
        src.ReqNum, src.Sno, src.UserId, src.TrainingProgram,
        src.TrainingCode, src.Priority, src.PiNum, src.FinalAccept, src.BhrAccept);

    public static CompetencyIndDto ToDto(this CompetencyInd src) => new(
        src.SrlNo, src.Band, src.CompNum, src.IndFlag, src.IndDefn);

    public static IEnumerable<CompetencyIndDto> ToDtos(this IEnumerable<CompetencyInd> list) =>
        list.Select(x => x.ToDto());
}

