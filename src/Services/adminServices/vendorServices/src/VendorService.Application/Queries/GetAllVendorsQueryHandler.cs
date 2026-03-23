using AutoMapper;
using MediatR;
using VendorService.Application.DTOs;
using VendorService.Domain.Interfaces;

namespace VendorService.Application.Queries;

public sealed class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQuery, IEnumerable<VendorDto>>
{
    private readonly IVendorRepository _repository;
    private readonly IMapper _mapper;

    public GetAllVendorsQueryHandler(IVendorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VendorDto>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
    {
        var vendors = !string.IsNullOrEmpty(request.Status)
            ? await _repository.GetByStatusAsync(request.Status[0], cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<VendorDto>>(vendors);
    }
}
