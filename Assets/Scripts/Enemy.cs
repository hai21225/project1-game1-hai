using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Image _hpImage;
    [SerializeField] private float _hp = 100f;
    [SerializeField] private GameObject _damageTextPrefab;
    [SerializeField] private Transform _damageTextPos;
    [SerializeField] private float  _seperationRadius = 1f;
    [SerializeField] private float _separationForce = 1.5f;
    [SerializeField] private LayerMask _enemyLayer;
    private float _currentHp;
    private BaseCharacter _player;
    [SerializeField] private float _speed = 2f;
    private bool _isKnockback = false;
    private Rigidbody2D _rb;
    private EnemySpawner _spawner;
    public void Init(EnemySpawner spawner)
    {
        _spawner = spawner;
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = FindAnyObjectByType<BaseCharacter>();
        _currentHp = _hp;
    }
    private void Dead()
    {
        if (_spawner != null)
        {
            _spawner.NotifyEnemyDead(this);
        }
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        ShowDamage(damage);
        if (_currentHp <= 0)
        {
            Dead();
        }
    }
    private void ShowDamage(float damage)
    {
        GameObject dmgText = Instantiate(
            _damageTextPrefab,
            _damageTextPos.position,
            Quaternion.identity
        );

        dmgText.GetComponent<DamageText>().SetDamage(damage);
    }
    private void Update()
    {
        if (_player != null)
        {

            Vector2 moveDir = (_player.transform.position - transform.position).normalized;

            Vector2 separation = GetSeparation();

            Vector2 finalDir = (moveDir + separation * _separationForce).normalized;

            _rb.transform.position += (Vector3)(finalDir * _speed * Time.deltaTime);
        }
        FillHp();
    }

    private Vector2 GetSeparation()
    {
        Vector2 force = Vector2.zero;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _seperationRadius, _enemyLayer); 
        foreach( Collider2D hit in hits)
        {
           if(hit.gameObject== gameObject)
           {
               continue;
           }

           Vector2 dir = (Vector2)(transform.position - hit.transform.position);
           float distance = dir.magnitude;
           if (distance < _seperationRadius)
           {
               force+= (dir.normalized/distance);
           }
        }
        return force;
    }

    public void KnockBack(Vector2 dir, float force, float duration)
    {
        if (_isKnockback) return;
        StartCoroutine(KnockBackRoutine(dir, force, duration));
    }

    private IEnumerator KnockBackRoutine(Vector2 dir, float force, float duration)
    {
        _isKnockback = true;

        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Dynamic;

        float timer = 0f;
        while (timer < duration)
        {
            _rb.linearVelocity = dir * force;
            timer += Time.deltaTime;
            yield return null;
        }

        _rb.linearVelocity = Vector2.zero;
        _isKnockback = false;
    }

    private void FillHp()
    {
        _hpImage.fillAmount = (float)Mathf.Clamp01((_currentHp / _hp));
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _seperationRadius);
    }
}
