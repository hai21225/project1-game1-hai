using UnityEngine;

public class ShootSkill : SkillBase
{
    [SerializeField] private string _name;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    protected override void Execute(Vector2 dir)
    {
        //var obj = PoolManager.Instance.Spawn(PoolGroup.Character, _name, transform.position, Quaternion.identity)
        //    .GetComponent<LinearPrjtile>();
        if (Mathf.Abs(dir.x) > 0.01f)
            _spriteRenderer.flipX = dir.x < 0f;
        var obj = GameSession.instance.Spawn(PoolGroup.Character, _name,transform.position,Quaternion.identity)
            .GetComponent<LinearPrjtile>();
        if (obj == null) return;
        obj.SetDirection(dir);

        obj.OnEnemy += RaiseHitEnemy;
    }
}