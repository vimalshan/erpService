using FaqServices.Application.Common.DTOs;
using MediatR;

namespace FaqServices.Application.Features.Grades.Queries.GetAllGrades;

public record GetAllGradesQuery : IRequest<List<FaqGradeDto>>;
