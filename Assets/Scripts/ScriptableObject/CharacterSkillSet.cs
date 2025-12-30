using UnityEngine;

[CreateAssetMenu(menuName ="Skill/Character Skill Set")]
public class CharacterSkillSet : ScriptableObject
{
    public SkillData skill1;
    public SkillData skill2;
    public SkillData skill3;

    public SkillData GetSkill(int index)
    {
        switch (index)
        {
            case 0:
                return skill1;
            case 1:
                return skill2;
            case 2:
                return skill3;
            default: return null;
        }
    }
}