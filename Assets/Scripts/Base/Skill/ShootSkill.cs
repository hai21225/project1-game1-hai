using UnityEngine;

public class ShootSkill : SkillBase
{
    [SerializeField] private string _name;


    protected override void Execute(Vector2 dir)
    {
        var obj = PoolManager.Instance.Spawn(PoolGroup.Character, _name, transform.position, Quaternion.identity)
            .GetComponent<LinearPrjtile>();
        obj.SetDirection(dir);

        obj.OnEnemy += RaiseHitEnemy;
    }
}