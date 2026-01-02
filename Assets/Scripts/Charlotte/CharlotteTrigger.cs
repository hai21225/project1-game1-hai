using UnityEngine;

public class CharlotteTrigger : MonoBehaviour
{
    public event System.Action<Enemy> OnEnemy;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            // slow, stun,... 
            OnEnemy?.Invoke(enemy); 
        }
    }
}