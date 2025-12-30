using System.Collections.Generic;
using UnityEngine;

public class EnemyListDetected : MonoBehaviour
{
    [SerializeField] private float _attackRadius = 1.5f;
    [SerializeField] private LayerMask _enemyLayer;
    
    public Enemy Detected()
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _attackRadius,_enemyLayer);

        if (hits.Length == 0) return null;
        Enemy closestenemy = null;
        float mindistance = 99999f;
        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out Enemy enemy )) continue;
            float distance = (enemy.transform.position - this.transform.position).sqrMagnitude; 
            if( distance < mindistance)
            {
                mindistance= distance;
                closestenemy= enemy;
            }
        }
        return closestenemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }



}