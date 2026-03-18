using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Entities;

public class PfMainAccount : BaseEntity
{
    public decimal MainAccountCode { get; set; }
    public string MainAccountName { get; set; } = string.Empty;

    public ICollection<PfMainSubMapping> SubMappings { get; set; } = [];
}
