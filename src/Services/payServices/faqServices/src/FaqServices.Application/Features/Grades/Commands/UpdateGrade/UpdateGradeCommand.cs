using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Grades.Commands.UpdateGrade;

public record UpdateGradeCommand(
    string Id,
    string GradeName,
    string? Description,
    int SortOrder
) : IRequest<FaqGradeDto>;
