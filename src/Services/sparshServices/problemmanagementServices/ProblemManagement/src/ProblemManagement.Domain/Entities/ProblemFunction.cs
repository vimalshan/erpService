using ProblemManagement.Domain.Common;

namespace ProblemManagement.Domain.Entities;

public class ProblemFunction : BaseEntity
{
    public long FuncId { get; set; }
    public string FuncName { get; set; } = string.Empty;
}
