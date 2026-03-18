using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Commands.BankMasters;

public record CreateBankMasterCommand : IRequest<BankMasterDto>
{
    public string BankTrustCode { get; init; } = null!;
    public string BankCode { get; init; } = null!;
    public string BankName { get; init; } = null!;
    public string MicrCode { get; init; } = null!;
    public string BranchName { get; init; } = null!;
    public string BranchAddressLine1 { get; init; } = null!;
    public string? BranchAddressLine2 { get; init; }
    public string? BranchAddressLine3 { get; init; }
    public string? BranchAddressLine4 { get; init; }
    public string? BranchPhoneNo { get; init; }
    public string? BranchFaxNo { get; init; }
    public DateTime BranchEffDate { get; init; }
}
