using UnityEngine;

public class CharlotteSkill: MonoBehaviour,ISkillUser
{
    [SerializeField] private SkillBase[] skills;
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private CharacterSkillSet _skillset;

    public CharacterSkillSet SkillSet => _skillset;

    private void Start()
    {
        for(int i = 0; i < 1; i++)
        {
            skills[i].cooldown=_skillset.GetSkill(i).cooldown;
        }
        skills[0].OnSkillUsed += SkillUsed;
        //skills[1].OnSkillUsed += SkillUsed;
    }

    private void SkillUsed()
    {
        Debug.Log("skiill su dung, noi tai +1");
    }

    public bool CanUseSkill(int index)
    {
        return skills[index].CanUse();
    }

    public void UseSkill(int index, Vector2 dir)
    {
        skills[index].TryUse(dir);
    }

    public float GetCooldownPercent(int index)
    {
        return skills[index].CooldownPercent();
    }


}