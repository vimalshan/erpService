using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class MedicineDebitCreditFlag : BaseEntity
{
    public char? Flag { get; private set; }
    public int? DebitCredit { get; private set; }

    private MedicineDebitCreditFlag() { }

    public static MedicineDebitCreditFlag Create(char? flag, int? debitCredit)
    {
        return new MedicineDebitCreditFlag
        {
            Flag = flag,
            DebitCredit = debitCredit
        };
    }
}
