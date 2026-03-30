using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Application.Mappings;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Domain.Exceptions;

namespace EmployeeRelations.Application.Queries.Survey;

public record GetSurveyByIdQuery(long Id) : IRequest<SurveyMasterDto>;
public record GetAllSurveysQuery : IRequest<IEnumerable<SurveyMasterDto>>;

public class GetSurveyByIdHandler : IRequestHandler<GetSurveyByIdQuery, SurveyMasterDto>
{
    private readonly ISurveyRepository _repo;

    public GetSurveyByIdHandler(ISurveyRepository repo) { _repo = repo; }

    public async Task<SurveyMasterDto> Handle(GetSurveyByIdQuery req, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(req.Id, ct)
            ?? throw new EntityNotFoundException("SurveyMaster", req.Id);
        return entity.ToDto();
    }
}

public class GetAllSurveysHandler : IRequestHandler<GetAllSurveysQuery, IEnumerable<SurveyMasterDto>>
{
    private readonly ISurveyRepository _repo;

    public GetAllSurveysHandler(ISurveyRepository repo) { _repo = repo; }

    public async Task<IEnumerable<SurveyMasterDto>> Handle(GetAllSurveysQuery req, CancellationToken ct)
    {
        var all = await _repo.GetAllAsync(ct);
        return all.Select(e => e.ToDto());
    }
}
