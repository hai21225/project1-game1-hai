using UnityEngine;

public interface ISkillUser
{
    bool CanUseSkill(int index);
    void UseSkill(int index, Vector2 dir);
    float GetCooldownPercent(int index);
    CharacterSkillSet SkillSet { get; }
}
