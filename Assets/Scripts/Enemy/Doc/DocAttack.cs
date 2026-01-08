using System.Collections;
using UnityEngine;

public class DocAttack : MonoBehaviour, IEnemyAttack
{

    [SerializeField] private EnemyMovement _move;
    [SerializeField] private string _nameObjectPool = "Knife";
    [SerializeField] private Transform _pos;
    [SerializeField] private float _damage = 21f;
    [SerializeField] private float _delay = 1f;

    private float _timer = 0f;

    private void Update()
    {
        _timer -= Time.deltaTime;
    }

    public void Attack(Transform target)
    {
        if (_timer > 0f) return;
        _timer= _delay;
        var knife = GameSession.instance.Spawn(PoolGroup.Common, _nameObjectPool,_pos.position,Quaternion.identity)
            .GetComponent<ComonLinearPrjtile>();
        if (knife != null )
        {
            knife.SetDirection((target.position - _pos.transform.position).normalized);
            knife.OnPlayer += (player) =>
            {
                player.TakeDamage(_damage);
            };
        }
    }

    public void OnDespawn()
    {
        _move.OnAttack -= Attack;

        _timer = 0f;
    }

    public void OnSpawn()
    {
        _move.OnAttack += Attack;
       
    }


}
