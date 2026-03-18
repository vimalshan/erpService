using AutoMapper;
using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.QueryHandlers;

/// <summary>
/// Handler for GetAllEmailTypesQuery
/// </summary>
public class GetAllEmailTypesQueryHandler : IRequestHandler<Queries.GetAllEmailTypesQuery, IEnumerable<Dtos.EmailTypeDto>>
{
    private readonly IEmailTypeRepository _emailTypeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetAllEmailTypesQueryHandler class
    /// </summary>
    /// <param name="emailTypeRepository">Email type repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    public GetAllEmailTypesQueryHandler(
        IEmailTypeRepository emailTypeRepository,
        IMapper mapper)
    {
        _emailTypeRepository = emailTypeRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllEmailTypesQuery
    /// </summary>
    /// <param name="request">The query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of all email types</returns>
    public async Task<IEnumerable<Dtos.EmailTypeDto>> Handle(
        Queries.GetAllEmailTypesQuery request,
        CancellationToken cancellationToken)
    {
        var emailTypes = await _emailTypeRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Dtos.EmailTypeDto>>(emailTypes);
    }
}
