using UnityEngine;

public class Effect : MonoBehaviour, IPoolable,IGameSessionObject
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

    public void Return()
    {
        GameSession.instance.Despawn(PoolGroup.Character, _name,this,gameObject);
    }

    private void ReturnToPool()
    {
        //PoolManager.Instance.Despawn(PoolGroup.Character,_name, gameObject);
        Return();
    }
}