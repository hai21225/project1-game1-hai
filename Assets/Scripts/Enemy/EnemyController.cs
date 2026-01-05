using UnityEngine;

public class EnemyController : MonoBehaviour, IPoolable
{
    [SerializeField] private string _nameEnemy;

    [SerializeField] private EnemyAi _ai;
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyAttack _enemyAttack;
    [SerializeField] private EnemyHealth _enemyHealth;

    private EnemySpawner _spawner;
    public void Init(EnemySpawner spawner)
    {
        _spawner = spawner;
    }

    public void OnDespawn()
    {
        _enemyMovement.OnDespawn();
        _enemyHealth.OnDespawn();
        _enemyAttack.OnDespawn();
        _enemyHealth.OnDead -= ReturnToPool;
    }

    public void OnSpawn()
    {
        _ai.ResetState();

        _enemyMovement.OnSpawn();
        _enemyHealth.Onspawn();
        _enemyHealth.OnDead += ReturnToPool;
        _enemyAttack.OnSpawn();
    }
    public void ReturnToPool()
    {
        _spawner.NotifyEnemyDead(this);
        PoolManager.Instance.Despawn(PoolGroup.Common,_nameEnemy, gameObject);
    }

}