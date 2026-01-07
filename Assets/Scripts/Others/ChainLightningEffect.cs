using UnityEngine;

public class ChainLightningEffect : MonoBehaviour, IPoolable,IGameSessionObject
{
    [Header("Visual")]
    [SerializeField] private int _segments = 8;
    [SerializeField] private float _offset = 0.4f;
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private Color _startColor = Color.cyan;
    [SerializeField] private Color _endColor = Color.white;
    [SerializeField] private Color _color = Color.white;

    private LineRenderer _lr;
    private Vector3 _from;
    private Vector3 _to;
    private string _name;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    public void Init(Vector3 from, Vector3 to,string name)
    {
        _from = from;
        _to = to;
        _name = name;
    }

    public void OnSpawn()
    {
        Draw();
        Invoke(nameof(ReturnToPool), _duration);
    }

    public void OnDespawn()
    {
        CancelInvoke();
    }

    private void Draw()
    {
        _lr.positionCount = _segments + 1;
        _lr.startColor = _startColor;
        _lr.endColor = _endColor;
        _lr.material.color = _color;

        Vector3 dir = _to - _from;
        Vector3 normal = Vector3.Cross(dir.normalized, Vector3.forward);

        for (int i = 0; i <= _segments; i++)
        {
            float t = i / (float)_segments;
            Vector3 pos = Vector3.Lerp(_from, _to, t);

            if (i != 0 && i != _segments)
            {
                float offset = Random.Range(-_offset, _offset);
                pos += normal * offset;
            }

            _lr.SetPosition(i, pos);
        }
    }

    private void ReturnToPool()
    {
        //PoolManager.Instance.Despawn(PoolGroup.Character,_name, gameObject);
        Return();
    }

    public void Return()
    {
        GameSession.instance.Despawn(PoolGroup.Character, _name, this,gameObject);
    }
}
