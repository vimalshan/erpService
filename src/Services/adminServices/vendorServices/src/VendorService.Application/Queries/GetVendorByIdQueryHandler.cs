using AutoMapper;
using MediatR;
using VendorService.Application.DTOs;
using VendorService.Domain.Interfaces;

namespace VendorService.Application.Queries;

public sealed class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorDto?>
{
    private readonly IVendorRepository _repository;
    private readonly IMapper _mapper;

    public GetVendorByIdQueryHandler(IVendorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VendorDto?> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        var vendor = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return vendor is null ? null : _mapper.Map<VendorDto>(vendor);
    }
}
