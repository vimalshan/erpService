using MediatR;

namespace AuthorizationService.Application.Commands;

public class CreateSpecialInputCommand : IRequest<long>
{
    public decimal SpecialInputId { get; set; }
    public decimal YearId { get; set; }
    public string RoleType { get; set; } = string.Empty;
    public decimal EmployeeSysId { get; set; }
    public decimal AppraisalSysId { get; set; }
    public string Inputs { get; set; } = string.Empty;
    public char Status { get; set; }

    public class Handler : IRequestHandler<CreateSpecialInputCommand, long>
    {
        private readonly AuthorizationService.Domain.Interfaces.IUnitOfWork _unitOfWork;

        public Handler(AuthorizationService.Domain.Interfaces.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(CreateSpecialInputCommand request, CancellationToken cancellationToken)
        {
            var specialInput = new AuthorizationService.Domain.Entities.SpecialInput(
                request.SpecialInputId,
                request.YearId,
                request.RoleType,
                request.EmployeeSysId,
                request.AppraisalSysId)
            {
                Inputs = request.Inputs,
                Status = request.Status,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.SpecialInputs.AddAsync(specialInput, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return specialInput.Id;
        }
    }
}
