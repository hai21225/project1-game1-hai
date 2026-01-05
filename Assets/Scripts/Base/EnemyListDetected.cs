using UnityEngine;

public class EnemyListDetected : MonoBehaviour
{
    [SerializeField] private float _attackRadius = 1.5f;
    [SerializeField] private LayerMask _enemyLayer;

    private readonly Collider2D[] _hits = new Collider2D[8];

    public BaseEnemy Detected()
    {
        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            _attackRadius,
            _hits,
            _enemyLayer
        );

        if (count == 0) return null;

        BaseEnemy closestEnemy = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            if (!_hits[i].TryGetComponent(out BaseEnemy enemy)) continue;

            float dist = (enemy.transform.position - transform.position).sqrMagnitude;
            if (dist < minDistance)
            {
                minDistance = dist;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
