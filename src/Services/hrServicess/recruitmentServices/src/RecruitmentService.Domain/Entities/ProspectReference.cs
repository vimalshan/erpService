namespace RecruitmentService.Domain.Entities;

public class ProspectReference
{
    public decimal EmpSysId { get; private set; }
    public decimal RefId { get; private set; }
    public string? Name { get; private set; }
    public string? Designation { get; private set; }
    public string? Address1 { get; private set; }
    public string? Address2 { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }

    private ProspectReference() { }

    public static ProspectReference Create(
        decimal empSysId, decimal refId, string? name, string? designation,
        string? addr1, string? addr2, string? phone, string? email) =>
        new()
        {
            EmpSysId = empSysId,
            RefId = refId,
            Name = name,
            Designation = designation,
            Address1 = addr1,
            Address2 = addr2,
            Phone = phone,
            Email = email
        };
}
