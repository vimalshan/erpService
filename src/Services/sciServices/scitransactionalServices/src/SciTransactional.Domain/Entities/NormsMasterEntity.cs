using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.Entities;

public sealed class NormsMasterEntity : Entity<long>
{
    public int? InputCode { get; private set; }
    public int? OutputCode { get; private set; }
    public int? Rate { get; private set; }
    public long? NormNo { get; private set; }

    private NormsMasterEntity() { }

    public static NormsMasterEntity Create(long normId, int? inputCode, int? outputCode, int? rate, long? normNo)
    {
        return new NormsMasterEntity
        {
            InputCode = inputCode,
            OutputCode = outputCode,
            Rate = rate,
            NormNo = normNo
        };
    }

    public void Update(int? inputCode, int? outputCode, int? rate)
    {
        InputCode = inputCode ?? InputCode;
        OutputCode = outputCode ?? OutputCode;
        Rate = rate ?? Rate;
    }
}
