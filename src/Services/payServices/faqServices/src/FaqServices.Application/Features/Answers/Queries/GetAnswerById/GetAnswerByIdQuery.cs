using MediatR;
using FaqServices.Application.Common.DTOs;

namespace FaqServices.Application.Features.Answers.Queries.GetAnswerById;

public record GetAnswerByIdQuery(string Id) : IRequest<FaqAnswerDto?>;
