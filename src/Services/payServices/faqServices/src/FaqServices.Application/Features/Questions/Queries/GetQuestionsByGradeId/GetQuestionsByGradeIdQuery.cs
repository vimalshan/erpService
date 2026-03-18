using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Questions.Queries.GetQuestionsByGradeId;

public record GetQuestionsByGradeIdQuery(string GradeId) : IRequest<IEnumerable<FaqQuestionDto>>;
