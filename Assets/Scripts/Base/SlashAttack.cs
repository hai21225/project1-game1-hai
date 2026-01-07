using UnityEngine;

public class SlashAttack : MonoBehaviour, IPoolable,IGameSessionObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _lifeTime;

    public System.Action<EnemyHealth> OnHitEnemy;
    private Transform _target;
    private Transform _player;
    public void Init(Transform target,Transform player)
    {
        _target = target;
        _player = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(collision.TryGetComponent(out EnemyHealth enemy))
                OnHitEnemy.Invoke(enemy);
        }
    }
    public void OnDespawn()
    {
        _target = null;
        _player = null;
        OnHitEnemy = null;
    }

    public void OnSpawn()
    {
        if (_target == null) {
            ReturnToPool(); 
            return; 
        }

        //if (_target.position.x > transform.position.x)
        //{
        //    transform.transform.localScale = new Vector3(1, 1, 1);
        //}
        //else if (_target.position.x < transform.position.x)
        //{
        //    transform.transform.localScale = new Vector3(-1, 1, 1);
        //}

        Vector3 dir = _target.position - _player.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //auto return
        Invoke(nameof(ReturnToPool), _lifeTime);
    }

    private void ReturnToPool()
    {
        //PoolManager.Instance.Despawn(PoolGroup.Character, _name,gameObject);
        Return();
    }

    public void Return()
    {
        GameSession.instance.Despawn(PoolGroup.Character,_name,this,gameObject);
    }
}