using UnityEngine;

public class Effect : MonoBehaviour, IPoolable
{
    [SerializeField] private string _name;
    [SerializeField] private float _lifeTime=0.15f;
    public void OnDespawn()
    {
        CancelInvoke();
    }

    public void OnSpawn()
    {

        Invoke(nameof(ReturnToPool), _lifeTime); 
    }

    private void ReturnToPool()
    {
        PoolManager.Instance.Despawn(_name, gameObject);
    }
}