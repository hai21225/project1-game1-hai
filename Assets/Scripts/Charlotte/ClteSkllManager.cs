using UnityEngine;

public class ClteSkllManager : MonoBehaviour,ISkillUser
{
    [SerializeField] private SkillBase[] skills;
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private CharacterSkillSet _skillset;
    [SerializeField] private CharlotteAttack _attack;
    public CharacterSkillSet SkillSet => _skillset;

    private bool _empowered = false;
    private Rigidbody2D _rb;
    private RigidbodyType2D _originType;
    private void Start()
    {
        _rb= GetComponent<Rigidbody2D>();
        _originType = _rb.bodyType;
        for (int i = 0; i < 3; i++)
        {
            skills[i].cooldown=_skillset.GetSkill(i).cooldown;
        }
        skills[0].OnSkillUsed += SkillUsed;
        skills[1].OnSkillUsed += SkillUsed;
        skills[2].OnSkillUsed += LockMoveUti;

        skills[0].OnHitEnemy += Skill1HandleEnemyHit;
        skills[1].OnHitEnemy += Skill2HandleEnemyHit;
        skills[2].OnHitEnemy += Skill3HandleEnemyHit;
    }

    private void SkillUsed()
    {
        _empowered = false;
        LockMovement();
        Invoke(nameof(UnlockMovement), 0.2f);
    }
    private void LockMoveUti()
    {
        _empowered=false;
        LockMovement();
        Invoke(nameof(UnlockMovement), 0.6f);
    }
    private void Skill1HandleEnemyHit(Enemy enemy)
    {
        enemy.TakeDamage(_stats.damageSkill1);
        if(_empowered) { return; }

        _empowered=true;
        _attack.SetEmpoweredAttack();
    }
    private void Skill2HandleEnemyHit(Enemy enemy)
    {
        enemy.TakeDamage(_stats.damageSkill2);
        if (_empowered) { return; }

        _empowered = true;
        _attack.SetEmpoweredAttack();

    }
    private void Skill3HandleEnemyHit(Enemy enemy)
    {
        enemy.TakeDamage(_stats.damageSkill3);
        if (_empowered) { return; }

        _empowered = true;
        _attack.SetEmpoweredAttack();
    }
    private void LockMovement()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Static;
    }

    private void UnlockMovement()
    {
        _rb.bodyType = _originType;
    }

    public bool CanUseSkill(int index)
    {
        return skills[index].CanUse();
    }

    public void UseSkill(int index, Vector2 dir)
    {
        ////////////////////////
        skills[index].TryUse(dir);
    }

    public float GetCooldownPercent(int index)
    {
        return skills[index].CooldownPercent();
    }


}