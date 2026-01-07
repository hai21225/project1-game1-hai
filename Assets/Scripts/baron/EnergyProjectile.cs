using UnityEngine;

public class EnergyProjectile : MonoBehaviour, IPoolable,IGameSessionObject
{
    public System.Action<EnemyHealth> OnHitEnemy;

    private Transform _target;
    [SerializeField] private float _speed = 21f;

    public void Init(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        if (_target == null)
        {
            //PoolManager.Instance.Despawn(PoolGroup.Character, "EnergyProjectile", gameObject);
            Return();
            return;
        }

        Vector3 dir = _target.position - transform.position;
        transform.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        transform.position += dir.normalized * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") &&
            collision.TryGetComponent(out EnemyHealth enemy))
        {
            OnHitEnemy?.Invoke(enemy);
            //PoolManager.Instance.Despawn(PoolGroup.Character, "EnergyProjectile", gameObject);
            Return();
        }
    }

    public void OnSpawn()
    {
        
    }

    public void OnDespawn()
    {
        _target= null;
        OnHitEnemy= null;
    }

    public void Return()
    {
        GameSession.instance.Despawn(PoolGroup.Character, "EnergyProjectile", this, gameObject);
    }
}
