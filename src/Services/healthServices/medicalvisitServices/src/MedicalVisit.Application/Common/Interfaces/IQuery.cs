using MediatR;

namespace MedicalVisit.Application.Common.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
