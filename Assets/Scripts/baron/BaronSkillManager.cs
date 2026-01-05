using UnityEngine;

public class BaronSkillManager : MonoBehaviour, ISkillUser
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private CharacterSkillSet _skillSet;
    [SerializeField] private SkillBase[] _skills;
    [SerializeField] private BaronAttack _attack;
    public CharacterSkillSet SkillSet => _skillSet;

    private bool _isCooldownReset = false;

    private void Start()
    {
        for(int i=0;i<3;i++)
        {
            _skills[i].cooldown = _skillSet.GetSkill(i).cooldown;


            _skills[i].OnHitEnemy += OnHitEnemy;
        }
        _skills[0].OnSkillUsed += OnSkillUsed;
        _skills[1].OnSkillUsed += OnSkillUsed;
        //_skills[2].OnSkillUsed += OnSkillUsed;
    }

    private void Update()
    {
        if (_attack.GetAmountAttack())
        {
            _isCooldownReset = true;
        }
    }

    private void OnSkillUsed()
    {
        _attack.SetEmpoweredAttack(true);
        _isCooldownReset=false;
        _attack.ResetAmountAttack();
    }


    private void OnHitEnemy(EnemyHealth enemy)
    {

    }






    public bool CanUseSkill(int index)
    {
        if (_isCooldownReset && index!=2)
        {
            return true;
        }
        else
        {
            return _skills[index].CanUse();
        }
    }

    public float GetCooldownPercent(int index)
    {
        if (_isCooldownReset && index!=2)
        {
            return 1f;
        }else
        {
            return _skills[index].CooldownPercent();
        }

    }

    public void UseSkill(int index, Vector2 dir)
    {
        if (_isCooldownReset && index != 2)
        {
            _skills[index].TryUse(dir, true);
            return;
        }
        _skills[index].TryUse(dir);

    }

}