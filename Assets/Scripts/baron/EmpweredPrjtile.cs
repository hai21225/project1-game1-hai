using UnityEngine;

public class EmpweredPrjtile : MonoBehaviour,IPoolable,IGameSessionObject
{
    [SerializeField] private float _lifeTime = 1f;
    private Transform _target;
    public void Init(Transform target)
    {
        _target = target;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                if(collision.TryGetComponent(out EnemyHealth enemy))
                {
                    enemy.TakeDamage(35f);
                    
                }
            }
        }
    }

    public void OnSpawn()
    {
        if (_target == null)
        {
            Debug.Log("nguuuu");
            ReturnToPool();
            return;
        }
        if (_target.position.x > transform.position.x)
        {
            transform.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_target.position.x < transform.position.x)
        {
            transform.transform.localScale = new Vector3(1, -1, 1);
        }

        Vector3 dir = _target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //auto return
        Invoke(nameof(ReturnToPool), _lifeTime);
    }

    public void OnDespawn()
    {
        CancelInvoke();
        _target = null;
    }
    private void ReturnToPool()
    {
        //PoolManager.Instance.Despawn(PoolGroup.Character,"EmpoweredProjectile", gameObject);
        Return();
    }

    public void Return()
    {
        GameSession.instance.Despawn(PoolGroup.Character, "EmpoweredProjectile", this, gameObject);
    }
}