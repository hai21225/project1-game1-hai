using UnityEngine;

public class EmpoweredAttack: MonoBehaviour
{
    private Transform _target;
    public void Init(Transform target)
    {
        _target = target;
    }
    private void Start()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }
        if(_target.position.x> transform.position.x)
        {
            transform.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_target.position.x < transform.position.x)
        {
            transform.transform.localScale= new Vector3(1, -1, 1);
        }

        Vector3 dir = _target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                if(collision.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(35f);
                    
                }
            }
        }
    }


}