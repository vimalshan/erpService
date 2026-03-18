using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Questions.Queries.GetQuestionById;

public record GetQuestionByIdQuery(string Id) : IRequest<FaqQuestionDto?>;
