using HRDocumentService.Domain.Common;

namespace HRDocumentService.Domain.Entities;

public class HRDocumentCounter : BaseEntity
{
    public long DocNo { get; private set; }

    private HRDocumentCounter() { }

    public static HRDocumentCounter Create(long docNo)
    {
        return new HRDocumentCounter { DocNo = docNo };
    }

    public long GetNextNumber()
    {
        DocNo++;
        return DocNo;
    }
}
