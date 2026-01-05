using UnityEngine;

public class LinearPrjtile : MonoBehaviour,IPoolable
{
    [Header("base")]
    [SerializeField] private string _name;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _range = 5f;

    [Header("knockback")]
    [SerializeField] private float _force = 0f;
    [SerializeField] private float _durationKnockback = 0.12f;

    private bool _canMove;
    private Vector2 _direction;
    private Vector3 _startPosition;

    public event System.Action<EnemyHealth> OnEnemy;

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        if (!_canMove) return;
        transform.Translate(_direction * _speed *Time.deltaTime,Space.World);
        float traveled = (transform.position - _startPosition).sqrMagnitude;
        if(traveled>= _range*_range)
        {
            ReturnToPool();
        }
    }
    public void SetDirection(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            dir= Vector2.right;
        }
        _direction=dir;
        float angle = Mathf.Atan2(_direction.y,_direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        _startPosition= transform.position;
        _canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
                if(collision.TryGetComponent(out EnemyHealth enemy))
                {
                    //knockback
                    // take dame
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 knockDir = (collision.transform.position - transform.position).normalized;
                        //enemy.KnockBack(knockDir, _force, _durationKnockback);
                        //enemy.TakeDamage(10);
                        OnEnemy?.Invoke(enemy);
                    }
                }
        }
    }

    public void OnSpawn()
    {
        return;
    }

    public void OnDespawn()
    {
        _canMove=false;
        _startPosition= Vector2.zero;
        _direction=Vector2.zero;
        OnEnemy = null;
    }
    private void ReturnToPool()
    {
        PoolManager.Instance.Despawn(PoolGroup.Character, _name, gameObject);
    }
}