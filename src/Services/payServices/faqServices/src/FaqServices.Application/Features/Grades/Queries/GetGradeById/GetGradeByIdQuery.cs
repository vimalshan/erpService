using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Grades.Queries.GetGradeById;

public record GetGradeByIdQuery(string Id) : IRequest<FaqGradeDto?>;
