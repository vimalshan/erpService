using MediatR;

namespace EmployeePrideManagement.Application.Commands.DeletePrideMoment;

public record DeletePrideMomentCommand(decimal MomentPrideId) : IRequest<bool>;
