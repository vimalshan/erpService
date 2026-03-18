using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Exceptions;
using AutoMapper;

namespace EmployeeRelations.Application.Queries.Survey;

public record GetSurveyByIdQuery(long Id) : IRequest<SurveyMasterDto>;
public record GetAllSurveysQuery : IRequest<IEnumerable<SurveyMasterDto>>;

public class GetSurveyByIdHandler : IRequestHandler<GetSurveyByIdQuery, SurveyMasterDto>
{
    private readonly ISurveyRepository _repo;
    private readonly IMapper _mapper;

    public GetSurveyByIdHandler(ISurveyRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<SurveyMasterDto> Handle(GetSurveyByIdQuery req, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new EntityNotFoundException("SurveyMaster", req.Id);
        return _mapper.Map<SurveyMasterDto>(entity);
    }
}

public class GetAllSurveysHandler : IRequestHandler<GetAllSurveysQuery, IEnumerable<SurveyMasterDto>>
{
    private readonly ISurveyRepository _repo;
    private readonly IMapper _mapper;

    public GetAllSurveysHandler(ISurveyRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<SurveyMasterDto>> Handle(GetAllSurveysQuery req, CancellationToken ct)
    {
        var all = await _repo.GetAllAsync(ct);
        return all.Select(_mapper.Map<SurveyMasterDto>);
    }
}
