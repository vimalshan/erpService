using MediatR;

namespace FaqServices.Application.Features.Grades.Commands.DeleteGrade;

public record DeleteGradeCommand(string Id) : IRequest<bool>;
