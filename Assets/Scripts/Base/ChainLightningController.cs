using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningController : MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private int _maxChain = 4;
    [SerializeField] private float _range = 3f;
    private float _damage;
    [SerializeField] private float _damageMultiplier = 0.36f;

    public void Execute(EnemyHealth firstTarget)
    {
        StartCoroutine(Chain(firstTarget));
    }

    private IEnumerator Chain(EnemyHealth first)
    {
        _damage=_stats.maxDamage;
        float damage = _damage;
        EnemyHealth current = first;
        HashSet<EnemyHealth> hit = new();
        Vector3 lastHitPos = current.transform.position;
        for (int i = 0; i < _maxChain; i++)
        {
            if (current == null) yield break;
            lastHitPos = current.transform.position;

            current.TakeDamage(damage);
            hit.Add(current);

            EnemyHealth next = FindNext(lastHitPos, hit);
            if (next == null) yield break;

            //var fx = PoolManager.Instance.Spawn(PoolGroup.Character,
            //            "ChainLightning",
            //            Vector3.zero,
            //            Quaternion.identity
            //            ).GetComponent<ChainLightningEffect>();
            var fx = GameSession.instance.Spawn(PoolGroup.Character,
            "ChainLightning",
            Vector3.zero,
            Quaternion.identity
            ).GetComponent<ChainLightningEffect>();
            if ( fx == null)
            {
                yield break;
            }

            fx.Init(
                current.transform.position,
                next.transform.position, "ChainLightning"
            );
            fx.OnSpawn();

            yield return new WaitForSeconds(0.04f);
            damage *= _damageMultiplier;
            current = next;
        }
    }

    private EnemyHealth FindNext(Vector3 fromPos, HashSet<EnemyHealth> hitEnemies)
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(fromPos, _range);

        EnemyHealth nearestEnemy = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            if (hit.TryGetComponent(out EnemyHealth enemy))
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
}
