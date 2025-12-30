
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    [SerializeField] private float _speed = 21f;
    [SerializeField] private float _damage = 21f;
    [SerializeField] private int _maxChain = 4;
    [SerializeField] private float _chainRange = 3f;
    [SerializeField] private float _chainDamageMultiplier = 0.7f;
    [SerializeField] private GameObject _strikePrefab;
    [SerializeField] private GameObject _chainLightning;

    private Transform _target;
    private bool  _isGod=false;
    private Coroutine _chainCoroutine;
    public void Init(Transform target)
    {
        _target= target;
    }
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    void Update()
    {
        // if no enemy, destroy gameobject
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir= _target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        transform.rotation= Quaternion.Euler(0,0,angle);    
        transform.position += dir.normalized * _speed *Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (_isGod)
                {
                    if (collision.TryGetComponent(out Enemy enemy))
                    {
                        StartChainLightning(enemy);
                        GetComponent<SpriteRenderer>().enabled = false;
                        GetComponent<Collider2D>().enabled = false;
                    }
                }
                else
                {
                    collision.TryGetComponent(out Enemy enemy);
                    enemy.TakeDamage(_damage);
                    Destroy(gameObject);
                }
            }
        }
    }
    
    private void StartChainLightning(Enemy firstTarget)
    {
        if(_chainCoroutine != null)
        {
            StopCoroutine(_chainCoroutine);
        } 
        _chainCoroutine=StartCoroutine(ChainLightningCoroutine(firstTarget));
    }

    private IEnumerator  ChainLightningCoroutine(Enemy firstTarget)
    {
        float currentDamage = _damage;
        Enemy currentTarget = firstTarget;

        var hitEnemies = new HashSet<Enemy>();

        Vector3 lastHitPos = currentTarget.transform.position;
        for (int i = 0; i < _maxChain; i++)
        {
            if (currentTarget == null) yield break;
            lastHitPos = currentTarget.transform.position;

            currentTarget.TakeDamage(currentDamage);
            Instantiate(_strikePrefab, currentTarget.transform.position, Quaternion.identity);
            hitEnemies.Add(currentTarget);

            Enemy nextTarget = FindNextEnemy(lastHitPos, hitEnemies);
            if (nextTarget == null) yield break;

            
            SpawnLightning(
                currentTarget.transform.position,
                nextTarget.transform.position
            );

            yield return new WaitForSeconds(0.04f);

            currentTarget = nextTarget;
            currentDamage *= _chainDamageMultiplier;
        }
        Destroy(gameObject);
    }
    private Enemy FindNextEnemy(Vector3 fromPos, HashSet<Enemy> hitEnemies)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(fromPos, _chainRange);

        Enemy nearestEnemy = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            if (hit.TryGetComponent(out Enemy enemy))
            {
                if (hitEnemies.Contains(enemy)) continue;
                float sqrDist = (enemy.transform.position - fromPos).sqrMagnitude;
                if (sqrDist < 0.05f) continue;


                float dist = Vector2.Distance(fromPos, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestEnemy = enemy;
                }
            }
        }
        return nearestEnemy;
    }
    private void SpawnLightning(Vector3 from, Vector3 to)
    {
        GameObject go = Instantiate(_chainLightning);
        LineRenderer lr = go.GetComponent<LineRenderer>();
        lr.material.color = Color.yellow;
        int segments = 8;            // càng nhiều càng zig-zag
        float offsetAmount = 0.3f;   // độ lệch sét
        lr.positionCount = segments + 1;
        lr.startColor = Color.cyan;
        lr.endColor = Color.white;
        Vector3 dir = to - from;
        Vector3 normal = Vector3.Cross(dir.normalized, Vector3.forward);

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(from, to, t);

            if (i != 0 && i != segments)
            {
                float offset = Random.Range(-offsetAmount, offsetAmount);
                pos += normal * offset;
            }

            lr.SetPosition(i, pos);
        }
        Destroy(go, 0.3f); // sét nháy nhanh
    }
    public void SetGodState(bool value)
    {
        //Debug.Log("set state:  " + value);
        _isGod= value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _chainRange);
    }
}
