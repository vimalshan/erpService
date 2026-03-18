using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CardManagement.Application.Common.DTOs;
using CardManagement.Application.Common.Interfaces;
using CardManagement.Application.Common.Models;

namespace CardManagement.Application.Cards.Queries.GetGuestCards;

public class GetGuestCardsQueryHandler : IRequestHandler<GetGuestCardsQuery, PagedResult<GuestCardDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetGuestCardsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<GuestCardDto>> Handle(GetGuestCardsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.GuestCardMasters.AsQueryable();

        if (request.CanteenUnit.HasValue)
            query = query.Where(x => x.CanteenUnit == request.CanteenUnit.Value);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => x.CardSequence)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<GuestCardDto>
        {
            Items = _mapper.Map<IEnumerable<GuestCardDto>>(items),
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
