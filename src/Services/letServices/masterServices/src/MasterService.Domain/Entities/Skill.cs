using MasterService.Domain.Common;
using MasterService.Domain.Events;

namespace MasterService.Domain.Entities;

/// <summary>Aggregate: SKILL_MAST</summary>
public sealed class Skill : AggregateRoot
{
    public long SkillCode { get; private set; }
    public string SkillName { get; private set; } = string.Empty;
    public char SkillType { get; private set; }
    public decimal? WeightNum { get; private set; }
    public string? Remark { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? CloseDate { get; private set; }

    private Skill() { }

    public static Skill Create(long skillCode, string skillName, char skillType,
        decimal? weightNum = null, string? remark = null, DateTime? effectiveDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(skillName);
        if (skillCode <= 0) throw new ArgumentException("SkillCode must be positive.", nameof(skillCode));

        var skill = new Skill
        {
            SkillCode = skillCode,
            SkillName = skillName.Trim(),
            SkillType = char.ToUpper(skillType),
            WeightNum = weightNum,
            Remark = remark,
            EffectiveDate = effectiveDate ?? DateTime.UtcNow
        };

        skill.AddDomainEvent(new SkillCreatedEvent(skill.SkillCode, skill.SkillName, skill.SkillType));
        return skill;
    }

    public void Update(string skillName, char skillType, decimal? weightNum, string? remark)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(skillName);
        SkillName = skillName.Trim();
        SkillType = char.ToUpper(skillType);
        WeightNum = weightNum;
        Remark = remark;
    }

    public void Close(DateTime? closeDate = null)
    {
        CloseDate = closeDate ?? DateTime.UtcNow;
        AddDomainEvent(new SkillClosedEvent(SkillCode));
    }

    public bool IsActive => CloseDate is null;
}
