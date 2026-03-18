using AutoMapper;
using MediatR;
using EmailNotification.Domain.Repositories;

namespace EmailNotification.Application.QueryHandlers;

/// <summary>
/// Handler for GetRecipientsByOrgAndBusinessQuery
/// </summary>
public class GetRecipientsByOrgAndBusinessQueryHandler : IRequestHandler<Queries.GetRecipientsByOrgAndBusinessQuery, IEnumerable<Dtos.MailAccessDto>>
{
    private readonly IMailAccessRepository _mailAccessRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetRecipientsByOrgAndBusinessQueryHandler class
    /// </summary>
    /// <param name="mailAccessRepository">Mail access repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    public GetRecipientsByOrgAndBusinessQueryHandler(
        IMailAccessRepository mailAccessRepository,
        IMapper mapper)
    {
        _mailAccessRepository = mailAccessRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetRecipientsByOrgAndBusinessQuery
    /// </summary>
    /// <param name="request">The query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Enumerable of recipients matching the criteria</returns>
    public async Task<IEnumerable<Dtos.MailAccessDto>> Handle(
        Queries.GetRecipientsByOrgAndBusinessQuery request,
        CancellationToken cancellationToken)
    {
        var mailAccesses = await _mailAccessRepository.GetByOrgAndBusinessAsync(
            request.EmailTypeId,
            request.OrgId,
            request.BusinessId,
            cancellationToken);

        return _mapper.Map<IEnumerable<Dtos.MailAccessDto>>(mailAccesses);
    }
}
