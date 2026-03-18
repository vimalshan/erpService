using AutoMapper;
using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.QueryHandlers;

/// <summary>
/// Handler for GetEmailTypesByTypeQuery
/// </summary>
public class GetEmailTypesByTypeQueryHandler : IRequestHandler<Queries.GetEmailTypesByTypeQuery, IEnumerable<Dtos.EmailTypeDto>>
{
    private readonly IEmailTypeRepository _emailTypeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetEmailTypesByTypeQueryHandler class
    /// </summary>
    /// <param name="emailTypeRepository">Email type repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    public GetEmailTypesByTypeQueryHandler(
        IEmailTypeRepository emailTypeRepository,
        IMapper mapper)
    {
        _emailTypeRepository = emailTypeRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetEmailTypesByTypeQuery
    /// </summary>
    /// <param name="request">The query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of email types matching the type</returns>
    public async Task<IEnumerable<Dtos.EmailTypeDto>> Handle(
        Queries.GetEmailTypesByTypeQuery request,
        CancellationToken cancellationToken)
    {
        var emailType = request.EmailType == "D"
            ? Domain.ValueObjects.EmailTypeEnum.Daily
            : Domain.ValueObjects.EmailTypeEnum.Event;

        var emailTypes = await _emailTypeRepository.GetByTypeAsync(emailType, cancellationToken);
        return _mapper.Map<IEnumerable<Dtos.EmailTypeDto>>(emailTypes);
    }
}
