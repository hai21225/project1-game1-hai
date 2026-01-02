using UnityEngine;

public class EnergyProjectile : MonoBehaviour, IPoolable
{
    public System.Action<Enemy> OnHitEnemy;

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
            PoolManager.Instance.Despawn("EnergyProjectile", gameObject);
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
            collision.TryGetComponent(out Enemy enemy))
        {
            OnHitEnemy?.Invoke(enemy);
            PoolManager.Instance.Despawn("EnergyProjectile", gameObject);
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
}
