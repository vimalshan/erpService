using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Grades.Commands.CreateGrade;

public record CreateGradeCommand(
    string GradeName,
    string? Description,
    int SortOrder
) : IRequest<FaqGradeDto>;
