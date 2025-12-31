using UnityEngine;

public class LightningLogic : MonoBehaviour,IPoolable
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _range = 5f;

    private bool _canMove;
    private Vector2 _direction;
    private Vector3 _startPosition;

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        if (!_canMove) return;
        transform.Translate(_direction * _speed *Time.deltaTime,Space.World);
        float traveled = (transform.position - _startPosition).sqrMagnitude;
        if(traveled>= _range)
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
            if (collision.CompareTag("Enemy"))
            {
                if(collision.TryGetComponent(out Enemy enemy))
                {
                    //knockback
                    // take dame
                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 knockDir = (collision.transform.position - transform.position).normalized;
                        enemy.KnockBack(knockDir, 6f, 0.12f);
                         
                        enemy.TakeDamage(10);
                    }
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
    }
    private void ReturnToPool()
    {
        PoolManager.Instance.Despawn("Lightning", gameObject);
    }
}