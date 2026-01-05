using UnityEngine;

public class CharlotteTrigger : MonoBehaviour
{
    public event System.Action<EnemyHealth> OnEnemy;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out EnemyHealth enemy))
        {
            // slow, stun,... 
            OnEnemy?.Invoke(enemy); 
        }
    }
}