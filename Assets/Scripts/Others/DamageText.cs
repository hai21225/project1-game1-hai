using TMPro;
using UnityEngine;

public class DamageText: MonoBehaviour,IPoolable,IGameSessionObject
{
    [SerializeField] private string _namePool;
    [SerializeField] private float _moveUpSpeed= 1f;
    [SerializeField] private float _lifeTime = 0.5f;

    [SerializeField] private TextMeshProUGUI _textMesh;
    private  Color _color;
    public void SetDamage(float damage)
    {
        _textMesh.text = (damage).ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * _moveUpSpeed * Time.deltaTime;

        _color.a -= Time.deltaTime / _lifeTime;
        _textMesh.color = _color;

        if (_color.a <= 0)
        {
            ReturnToPool();
        }
    }

    public void OnSpawn()
    {
        _color = _textMesh.color;
    }

    public void OnDespawn()
    {
       
    }
    private void ReturnToPool()
    {
        //PoolManager.Instance.Despawn(PoolGroup.Common, _namePool, gameObject);
        Return();
    }

    public void Return()
    {
        GameSession.instance.Despawn(PoolGroup.Common,_namePool,this,gameObject);
    }
}