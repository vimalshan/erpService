using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class PfMainSubMapping : BaseEntity
{
    public decimal MainAccountCode { get; set; }
    public decimal SubAccountCode { get; set; }

    public PfMainAccount? MainAccount { get; set; }
}
