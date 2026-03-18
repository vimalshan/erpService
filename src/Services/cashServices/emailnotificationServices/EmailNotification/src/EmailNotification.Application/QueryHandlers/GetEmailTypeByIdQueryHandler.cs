using AutoMapper;
using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.QueryHandlers;

/// <summary>
/// Handler for GetEmailTypeByIdQuery
/// </summary>
public class GetEmailTypeByIdQueryHandler : IRequestHandler<Queries.GetEmailTypeByIdQuery, Dtos.EmailTypeDto?>
{
    private readonly IEmailTypeRepository _emailTypeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetEmailTypeByIdQueryHandler class
    /// </summary>
    /// <param name="emailTypeRepository">Email type repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    public GetEmailTypeByIdQueryHandler(
        IEmailTypeRepository emailTypeRepository,
        IMapper mapper)
    {
        _emailTypeRepository = emailTypeRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetEmailTypeByIdQuery
    /// </summary>
    /// <param name="request">The query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The email type DTO or null if not found</returns>
    public async Task<Dtos.EmailTypeDto?> Handle(
        Queries.GetEmailTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var emailType = await _emailTypeRepository.GetByIdAsync(request.Id, cancellationToken);
        if (emailType == null)
            return null;

        return _mapper.Map<Dtos.EmailTypeDto>(emailType);
    }
}
