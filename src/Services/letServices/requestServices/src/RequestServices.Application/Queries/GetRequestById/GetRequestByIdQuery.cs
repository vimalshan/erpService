using AutoMapper;
using MediatR;
using RequestServices.Application.DTOs;
using RequestServices.Domain.Exceptions;
using RequestServices.Domain.Interfaces;

namespace RequestServices.Application.Queries.GetRequestById;

public record GetRequestByIdQuery(long RequestId) : IRequest<RequestMainDto>;

public class GetRequestByIdQueryHandler(IRequestRepository repository, IMapper mapper)
    : IRequestHandler<GetRequestByIdQuery, RequestMainDto>
{
    public async Task<RequestMainDto> Handle(GetRequestByIdQuery query, CancellationToken ct)
    {
        var main = await repository.GetByIdAsync(query.RequestId, ct)
            ?? throw new RequestNotFoundException(query.RequestId);

        return mapper.Map<RequestMainDto>(main);
    }
}
